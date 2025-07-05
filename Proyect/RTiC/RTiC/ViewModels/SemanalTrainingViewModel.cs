using GalaSoft.MvvmLight.Command;
using RTIC.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RTIC.ViewModels
{
    public class SemanalTrainingViewModel : INotifyPropertyChanged
    {
        private static readonly Random random = new();
        public TrainingPreferences Preferencias { get; set; } = new();
        public DaySchedule ExercisesSchedule { get; set; } = new();
        private HealthDataBarViewModel _healthDataBarViewModel;
        public List<string> WeekDays { get; } = new() { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };
        public List<string> TimeUsers { get; set; } = new List<string> { "", "", "", "", "", "", "" };
        public List<string> BestTimeUsers { get; set; } = new List<string> { "", "", "", "", "", "", "" };
        public ICommand GenerateWeeklyScheduleCommand { get; }
        public ICommand SaveTimeUser0Command { get; }
        public ICommand SaveTimeUser1Command { get; }
        public ICommand SaveTimeUser2Command { get; }
        public ICommand SaveTimeUser3Command { get; }
        public ICommand SaveTimeUser4Command { get; }
        public ICommand SaveTimeUser5Command { get; }
        public ICommand SaveTimeUser6Command { get; }

        private string _timeUser;
        public string TimeUser
        {
            get => _timeUser;
            set
            {
                _timeUser = value;
                OnPropertyChanged(nameof(TimeUser));
            }
        }


        public string? BreakDay1
        {
            get => Preferencias.BreakDay1;
            set
            {
                if (Preferencias.BreakDay2 != value && value != null)
                {
                    Preferencias.BreakDay1 = value;
                    SavePreferences();
                    OnPropertyChanged();
                }
            }
        }

        public string? BreakDay2
        {
            get => Preferencias.BreakDay2;
            set
            {
                if (Preferencias.BreakDay1 != value && value != null)
                {
                    Preferencias.BreakDay2 = value;
                    SavePreferences();
                    OnPropertyChanged();
                }
            }
        }

        public String? AvailableDay
        {
            get => Preferencias.AvailableDay;
            set
            {
                if (Preferencias.AvailableDay != value && value != null && value != Preferencias.BreakDay1 && value != Preferencias.BreakDay2)
                {
                    Preferencias.AvailableDay = value;
                    SavePreferences();
                    OnPropertyChanged();
                }
                value = Preferencias.AvailableDay;
            }
        } 

        // Constructor de SemanalTrainingViewModel
        public SemanalTrainingViewModel()
        {
            Preferencias = LoadPreferences();
            _healthDataBarViewModel = new HealthDataBarViewModel();
            ExercisesSchedule = new DaySchedule();
            ExercisesSchedule = LoadExercises();
            GetBestTimeWeekExercise(ExercisesSchedule.Exercises);
            GenerateWeeklyScheduleCommand = new RelayCommand(() =>
            {
                var result = GenerateWeeklySchedule(_healthDataBarViewModel, true);

                ExercisesSchedule.Exercises.Clear();
                foreach (var item in result)
                    ExercisesSchedule.Exercises.Add(item);
            });

            SaveTimeUser0Command = new RelayCommand(() => SaveTimeForExercise(0));
            SaveTimeUser1Command = new RelayCommand(() => SaveTimeForExercise(1));
            SaveTimeUser2Command = new RelayCommand(() => SaveTimeForExercise(2));
            SaveTimeUser3Command = new RelayCommand(() => SaveTimeForExercise(3));
            SaveTimeUser4Command = new RelayCommand(() => SaveTimeForExercise(4));
            SaveTimeUser5Command = new RelayCommand(() => SaveTimeForExercise(5));
            SaveTimeUser6Command = new RelayCommand(() => SaveTimeForExercise(6));

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Preferencias = LoadPreferences();
        }

        /* PREFERENCIAS */
        // Método para cargar las preferencias del usuario
        private TrainingPreferences LoadPreferences()
        {
            try
            {
                if (File.Exists("runner_preferences.xml"))
                {
                    var serializer = new XmlSerializer(typeof(TrainingPreferences));
                    using (var reader = new StreamReader("runner_preferences.xml"))
                    {
                        return (TrainingPreferences)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return new TrainingPreferences();
        }

        // Método para cargar los ejercicios semanales
        private DaySchedule LoadExercises()
        {
            try
            {
                if (File.Exists("exercises_preferences.xml"))
                {
                    var serializer = new XmlSerializer(typeof(DaySchedule));
                    using (var reader = new StreamReader("exercises_preferences.xml"))
                    {
                        var loaded = (DaySchedule)serializer.Deserialize(reader);
                        ExercisesSchedule.Exercises = new ObservableCollection<string>(loaded.Exercises.TakeLast(7));
                        ExercisesSchedule.Time = new ObservableCollection<string>(loaded.Time.TakeLast(7));
                        ExercisesSchedule.TimeUser = new ObservableCollection<string>(loaded.TimeUser.TakeLast(7));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return ExercisesSchedule;
        }

        // Método para guardar las preferencias del usuario
        private void SavePreferences()
        {
            var doc = new XDocument(
                new XElement("TrainingPreferences",
                    new XElement("BreakDay1", Preferencias.BreakDay1?.ToString() ?? ""),
                    new XElement("BreakDay2", Preferencias.BreakDay2?.ToString() ?? ""),
                    new XElement("AvailableDay", Preferencias.AvailableDay?.ToString() ?? "")
                )
            );
            doc.Save("runner_preferences.xml");
            HealthDataBarViewModel healthDataBarViewModel = new HealthDataBarViewModel();
            GenerateWeeklySchedule(healthDataBarViewModel, true);
        }

        /* PLANNING EJERCICIOS */
        // Método para guardar los ejercicios semanales en un XML
        private void SaveExercises(ObservableCollection<string> exercises)
        {
            var runner = _healthDataBarViewModel.LoadRunnerData();
            ObservableCollection<string> time = SearchObjectiveTime(exercises, runner.Level);
            ObservableCollection<string> timeUser = new() { "-", "-", "-", "-", "-", "-", "-", "-" };

            var filteredExercises = new ObservableCollection<string>();
            var filteredTime = new ObservableCollection<string>();
            var filteredTimeUser = new ObservableCollection<string>();

            for (int i = 0; i < exercises.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(exercises[i]))
                {
                    filteredExercises.Add(exercises[i]);
                    if (i < time.Count) filteredTime.Add(time[i]);
                    else filteredTime.Add("-");

                    if (i < timeUser.Count) filteredTimeUser.Add(timeUser[i]);
                    else filteredTimeUser.Add("-");
                }
            }

            var schedule = new DaySchedule
            {
                Exercises = filteredExercises,
                Time = filteredTime,
                TimeUser = filteredTimeUser
            };

            try
            {
                var serializer = new XmlSerializer(typeof(DaySchedule));
                using (var writer = new StreamWriter("exercises_preferences.xml"))
                {
                    serializer.Serialize(writer, schedule);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
         
        // Método para generar el planing semanal
        private ObservableCollection<string> GenerateWeeklySchedule(HealthDataBarViewModel healthDataBarViewModel, Boolean upd)
        {
            var exercises = new ObservableCollection<string>();
            var now = DateTime.Now;
            bool isSundayAfternoon = now.DayOfWeek == DayOfWeek.Sunday && now.TimeOfDay.Hours >= 20;

            if (upd == true || isSundayAfternoon)
            { var Preferences = LoadPreferences();
                var runner = healthDataBarViewModel.LoadRunnerData();

                string[] days = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };

                for (int i = 0; i < 7; i++)
                {
                    string day = days[i];

                    if (Preferences.BreakDay1 == day || Preferences.BreakDay2 == day)
                    {
                        exercises.Add("🛌 Descanso");
                    }
                    else if (Preferences.AvailableDay == day)
                    {
                        exercises.Add(SelectExercises(runner.Level, "RT"));
                    }
                    else
                    {
                        exercises.Add(SelectExercises(runner.Level, "EJ"));  
                    }
                }
                // Además de generar el planing, le asocia el tiempo y en caso de haberlo hecho antes toma el menor tiempo empleado en el ejercicio.
                ExercisesSchedule.Exercises = new ObservableCollection<string>(exercises);
                ExercisesSchedule.Time = SearchObjectiveTime(exercises, runner.Level);
                GetBestTimeWeekExercise(exercises);
                SaveExercises(exercises);
                return exercises;
            }
            ExercisesSchedule = LoadExercises();
            return ExercisesSchedule.Exercises;
        }

        // Método que permite seleccionar ejercicios del diccionario en función del nivel y del tipo de ejercicio.
        private string SelectExercises(string level, string modo)
        {
            var source = modo == "EJ" ? DaySchedule.ExercisesByLevel : DaySchedule.ChallengesByLevel;
            if (source.TryGetValue(level, out var exercises) && exercises.Count > 0)
            {
                var selected = exercises[random.Next(exercises.Count)];
                return selected.Item1;
            }
            return "-";
        }


        /* TIEMPOS */
        // Método para obtener el tiempo objetivo
        private ObservableCollection<string> SearchObjectiveTime(ObservableCollection<string> exercises, string level)
        {

            for (int i = 0; i < 7; i++)
            {
                if (exercises[i] == "🛌 Descanso")
                {
                    ExercisesSchedule.Time[i] = "//";
                }
                else if (exercises[i].StartsWith("🏆"))
                {
                    if (DaySchedule.ChallengesByLevel.TryGetValue(level, out var challenges))
                    {
                        var match = challenges.FirstOrDefault(e => e.Item1 == exercises[i]);
                        ExercisesSchedule.Time[i] = match != default ? match.Item2 : "--:--:--";
                    }
                }
                else
                {
                    if (DaySchedule.ExercisesByLevel.TryGetValue(level, out var normalExercises))
                    {
                        var match = normalExercises.FirstOrDefault(e => e.Item1 == exercises[i]);
                        ExercisesSchedule.Time[i] = match != default ? match.Item2 : "--:--:--";
                    }
                }
            }
            return ExercisesSchedule.Time;
        }

        // Método que introduce el tiempo en la posicion correspondiente
        private string GetTimeUserByIndex(int index)
        {
            return index switch
            {
                0 => TimeUsers[0],
                1 => TimeUsers[1],
                2 => TimeUsers[2],
                3 => TimeUsers[3],
                4 => TimeUsers[4],
                5 => TimeUsers[5],
                6 => TimeUsers[6],
                _ => null
            };
        }

        // Guarda el tiempo utilizado por ejercicio en un XML
        private void SaveTimeForExercise(int index)
        {
            if (index >= 0 && index < TimeUsers.Count &&
                !string.IsNullOrWhiteSpace(TimeUsers[index]) &&
                index < ExercisesSchedule.Exercises.Count &&
                index < ExercisesSchedule.Time.Count)
            {
                LoadTrainingHistory(
                    TimeUsers[index],
                    ExercisesSchedule.Exercises[index],
                    ExercisesSchedule.Time[index],
                    DateTime.Now.ToString("dd-MM-yyyy")
                );
            }
        }

        // Carga el historial de entrenamientos de un usuario
        private void LoadTrainingHistory(string userTime, string exercise, string defaultTime, string date)
        {
            string historyPath = "training_history.xml";
            TrainingUserHistory history;
            var serializer = new XmlSerializer(typeof(TrainingUserHistory));

            var newEntry = new TrainingHistory
            {
                ExerciseDefault = exercise,
                Fecha = date,
                TimeUser = userTime,
                TimeDefault = defaultTime
            };

            // Cargar historial existente, en caso de que no haya crear uno
            if (File.Exists(historyPath))
            {
                using (var reader = new StreamReader(historyPath))
                {
                    history = (TrainingUserHistory)serializer.Deserialize(reader);
                }
            }
            else
            {
                history = new TrainingUserHistory();
            }

            history.TrainingHistories.Add(newEntry);

            using (var writer = new StreamWriter(historyPath))
            {
                serializer.Serialize(writer, history);
            }
        }

        // Método que obtiene el mejor tiempo por ejercicio del usuario (record personal)
        private void GetBestTimeWeekExercise(ObservableCollection<string> exercises)
        {
            string path = "training_history.xml";

    // Verificar si el archivo existe y no está vacío
    if (!File.Exists(path) || new FileInfo(path).Length == 0)
    {
        // Crear XML vacío con nodo raíz si no existe o está vacío
        var newDoc = new XDocument(new XElement("TrainingHistory"));
        newDoc.Save(path);
    }

    // Ahora es seguro cargar el XML
    XDocument xml;
    try
    {
        xml = XDocument.Load(path);
    }
    catch (XmlException ex)
    {
        Debug.WriteLine("Error al leer el XML: " + ex.Message);
        // En caso de error de lectura, recrear estructura básica
        xml = new XDocument(new XElement("TrainingHistory"));
        xml.Save(path);
    }
            for (int i = 0; i < exercises.Count; i++)
            {
                string ejercicioBuscado = exercises[i];

                var tiempos = xml.Descendants("TrainingHistories")
                    .Where(e => e.Element("ExerciseDefault")?.Value.Trim() == ejercicioBuscado)
                    .Select(e => TimeSpan.TryParse(e.Element("TimeUser")?.Value, out var t) ? t : TimeSpan.MaxValue)
                    .ToList();

                if (tiempos.Any() && tiempos.Min() != TimeSpan.MaxValue)
                {
                    BestTimeUsers[i] = $"{tiempos.Min()}";
                }
                else
                {
                    BestTimeUsers[i] = "";
                }
            }
        }

        // Comprobación del día correspondiente y de que no es día de Descanso
        public bool IsMondayPast => DateTime.Now.DayOfWeek > DayOfWeek.Monday;
        public bool IsTuesdayPast => DateTime.Now.DayOfWeek > DayOfWeek.Tuesday;
        public bool IsWednesdayPast => DateTime.Now.DayOfWeek > DayOfWeek.Wednesday;
        public bool IsThursdayPast => DateTime.Now.DayOfWeek > DayOfWeek.Thursday;
        public bool IsFridayPast => DateTime.Now.DayOfWeek > DayOfWeek.Friday;
        public bool IsSaturdayPast => DateTime.Now.DayOfWeek > DayOfWeek.Saturday;
        public bool IsSundayPast => DateTime.Now.DayOfWeek == DayOfWeek.Monday; // Solo visible el domingo
        public bool HideMonday => ExercisesSchedule.Exercises[0] == "🛌 Descanso" || IsMondayPast;
        public bool HideTuesday => ExercisesSchedule.Exercises[1] == "🛌 Descanso" || IsTuesdayPast;
        public bool HideWednesday => ExercisesSchedule.Exercises[2] == "🛌 Descanso" || IsWednesdayPast;
        public bool HideThursday => ExercisesSchedule.Exercises[3] == "🛌 Descanso" || IsThursdayPast;
        public bool HideFriday => ExercisesSchedule.Exercises[4] == "🛌 Descanso" || IsFridayPast;
        public bool HideSaturday => ExercisesSchedule.Exercises[5] == "🛌 Descanso" || IsSaturdayPast;
        public bool HideSunday => ExercisesSchedule.Exercises[6] == "🛌 Descanso" || IsSundayPast;
    }
}

