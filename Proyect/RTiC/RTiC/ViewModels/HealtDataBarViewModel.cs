using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;
using RTIC.Models;

namespace RTIC.ViewModels
{
    public class HealthDataBarViewModel : INotifyPropertyChanged
    {
        private Runner _runner;
        private int adviseIndex = 0;
        private DispatcherTimer adviseTimer;
        private Brush _ImcColor;
        private string _temperatureText;
        private string _windText;
        private string _adviceText;

        public ICommand SaveProfileCommand { get; }
        public ObservableCollection<string> WeightHistory { get; set; } = new();
        public List<string> Niveles { get; } = new List<string> { "Inicial", "Aficionado", "Profesional", "Senior"};

        // Constructor de Runner
        public Runner Runner
        {
            get => _runner;
            set
            {
                _runner = value;
                OnPropertyChanged(nameof(Runner));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));       

        public Brush ImcColor
        {
            get => _ImcColor;
            set
            {
                _ImcColor = value;
                OnPropertyChanged(nameof(ImcColor));
            }
        }

        public string TemperatureText
        {
            get => _temperatureText;
            set
            {
                _temperatureText = value;
                OnPropertyChanged(nameof(TemperatureText));
            }
        }

        public string WindText
        {
            get => _windText;
            set
            {
                _windText = value;
                OnPropertyChanged(nameof(WindText));
            }
        }

        public string AdviceText
        {
            get => _adviceText;
            set
            {
                _adviceText = value;
                OnPropertyChanged(nameof(AdviceText));
            }
        }

        // Constructor HealtDataBarViewModel
        public HealthDataBarViewModel()
        {
            Runner = LoadRunnerData();
            UpdateImcBar();
            ViewDiferencesDate();
            InitializeAdvises();
            _ = LoadWeatherAsync();
            SaveProfileCommand = new RelayCommand(SaveProfile);
        }

        /*  PERFIL DEL CORREDOR */
        // Método que guarda el perfil del corredor
        public void SaveProfile()
        {
            UpdateImcBar();
            try
            {
                var serializer = new XmlSerializer(typeof(Runner));
                Runner.Date = DateTime.Now;
                using (var writer = new StreamWriter("runner_data.xml"))
                {
                    serializer.Serialize(writer, Runner);
                }
                SaveToHistory(Runner);
                ViewDiferencesDate();
                MessageBox.Show("Perfil guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método para cargar los datos del corredor
        public Runner LoadRunnerData()
        {
            try
            {
                if (File.Exists("runner_data.xml"))
                {
                    var serializer = new XmlSerializer(typeof(Runner));
                    using (var reader = new StreamReader("runner_data.xml"))
                    {
                        return (Runner)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return new Runner();
        }

        /* IMC */
        // Método para actualizar la barra del indice de masa corporal
        private void UpdateImcBar()
        {
            if (Runner.Height <= 0) return;
            double imc = Runner.Weight / (Runner.Height / 100 * Runner.Height / 100);
            double normalized = Math.Max(0, Math.Min(100, (imc - 16) / (40 - 16) * 100));
            Runner.Imc = imc;

            if (imc < 18.5)
                ImcColor = new SolidColorBrush(Colors.Yellow);
            else if (imc < 27)
                ImcColor = new SolidColorBrush(Colors.Green);
            else
                ImcColor = new SolidColorBrush(Colors.Red);
        }


        /*  HISTORIAL DEL CORREDOR */
        // Método para guardar el historial del corredor
        private void SaveToHistory(Runner newEntry)
        {
            string historyPath = "runner_history.xml";
            RunnerHistory history;

            var serializer = new XmlSerializer(typeof(RunnerHistory));

            if (File.Exists(historyPath))
            {
                newEntry.Date = DateTime.Now;
                double imc = newEntry.Weight / (newEntry.Height / 100 * newEntry.Height / 100);
                newEntry.Imc = imc;
                using (var reader = new StreamReader(historyPath))
                {
                    history = (RunnerHistory)serializer.Deserialize(reader);
                }
            }
            else
            {
                history = new RunnerHistory();
            }

            history.Runners.Add(new Runner
            {
                Name = newEntry.Name,
                Age = newEntry.Age,
                Height = newEntry.Height,
                Weight = newEntry.Weight,
                Date = newEntry.Date,
                Level = newEntry.Level
            });

            using (var writer = new StreamWriter(historyPath))
            {
                serializer.Serialize(writer, history);
            }
        }

        // Método para mostrar la diferencia de peso en los ultimos tres guardados del perfil
        private void ViewDiferencesDate()
        {
            string path = "runner_history.xml";
            if (!File.Exists(path)) return;

            var serializer = new XmlSerializer(typeof(RunnerHistory));
            RunnerHistory history;

            using (var reader = new StreamReader(path))
            {
                history = (RunnerHistory)serializer.Deserialize(reader);
            }

            var ultimos = history.Runners.TakeLast(4).ToList();
            if (ultimos.Count < 2) return;

            WeightHistory.Clear();

            for (int i = 1; i < ultimos.Count; i++)
            {
                var anterior = ultimos[i - 1];
                var actual = ultimos[i];

                double diferencia = actual.Weight - anterior.Weight;
                string color = diferencia < 0 ? "✅" : diferencia > 0 ? "❌" : "⚜️";
                string signo = diferencia > 0 ? "+" : "";

                string texto = $"{color} {actual.FormattedDate2}: {actual.Weight:F2}kg ({signo}{diferencia:F2}kg)";
                WeightHistory.Add(texto);
            }
        }

        /* TIEMPO */
        // Método que permite cargar el tiempo a través de una URL
        private async Task LoadWeatherAsync()
        {
            string url = "https://api.open-meteo.com/v1/forecast?latitude=37.8847&longitude=-4.7792&current=temperature_2m,wind_speed_10m";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    using JsonDocument doc = JsonDocument.Parse(response);
                    var root = doc.RootElement;

                    double temperature = root.GetProperty("current").GetProperty("temperature_2m").GetDouble();
                    double windSpeed = root.GetProperty("current").GetProperty("wind_speed_10m").GetDouble();

                    TemperatureText = $"🌡️ {temperature} °C";
                    WindText = $"🌬 {windSpeed} km/h";
                }
                catch (Exception ex)
                {
                    TemperatureText = "🌡️ --";
                    WindText = "🌬 --";
                }
            }
        }

        /* CONSEJOS */

        // Diccionario de consejos que iran rotando en la interfaz
        private readonly List<string> advises = new List<string>
        {
            "Hidrátate bien antes y después de ejercicio físico.",
            "Haz estiramientos para evitar lesiones.",
            "Los ejercicios son muy simples para ti, ¿qué tal aumentar el nivel?",
            "Corre con una postura erguida y mirada al frente.",
            "Escucha a tu cuerpo y descansa cuando lo necesites. No lo fuerces o no evitarás las lesiones.",
            "Ve modificando tu peso y altura, para ver tu progreso.",
            "Hoy es un buen día para salir a correr.",
            "Con RTiC llevarás un registro completo de tu proceso.",
            "Aunque haya una estimación de tiempo, es objetiva. No tengas prisas por pasar a la siguiente fase.",
            "No te vengas abajo porque no hayas superado tu mejor tiempo, sigue así, no todos los días puedes superarte.",
            "Comprueba el tiempo para salir a correr, ¡ten cuidado con el calor y el viento!",
            "Hazte un hueco y sal a correr, notarás un bienestar mental y físico.",
            "Cada paso cuenta, incluso los más lentos te acercan a tu meta.",
            "No corras para competir, corre para sentirte mejor contigo mismo.",
            "La constancia vale más que la velocidad.",
            "Un mal día corriendo sigue siendo mejor que un día sin moverse.",
            "No te compares con otros, compárate con tu versión de ayer.",
            "El descanso también es parte del entrenamiento.",
            "Recuerda calentar antes de correr y enfriar al terminar.",
            "Corre con ropa cómoda y adecuada al clima.",
            "No olvides revisar tus zapatillas, ¡son tu herramienta principal!",
            "Si hoy no puedes correr, camina. Lo importante es moverse.",
            "La motivación te inicia, el hábito te mantiene.",
            "No te rindas, cada kilómetro suma.",
            "Respira profundo, relaja los hombros y sigue adelante.",
            "Corre con música si te motiva, o en silencio si te conecta.",
            "Visualiza tu meta antes de empezar.",
            "No hay atajos para llegar lejos, solo pasos constantes.",
            "Hoy puede ser el día en que superes tu mejor versión.",
            "Tu cuerpo es más fuerte de lo que crees, confía en él.",
        };

        // Método para inicalizar la visualización de consejos
        private void InitializeAdvises()
        {
            adviseTimer = new DispatcherTimer();
            // Cada 10 seg. se va actualizando
            adviseTimer.Interval = TimeSpan.FromSeconds(10);
            adviseTimer.Tick += ShowNextAdvise;
            adviseTimer.Start();
        }

        // Método que muestra el siguiente consejo
        private void ShowNextAdvise(object sender, EventArgs e)
        {
            AdviceText = advises[adviseIndex];
            adviseIndex = (adviseIndex + 1) % advises.Count;
        }
    }
}
