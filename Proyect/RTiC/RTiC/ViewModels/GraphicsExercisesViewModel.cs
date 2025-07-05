using GalaSoft.MvvmLight.Command;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using RTIC.Models;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using ClosedXML.Excel;
using System.Xml;

namespace RTIC.ViewModels
{
    /* Esta clase gestionará todo lo relacionado con el historial de entrenamiento del usuario de manera más visual (tabla de los últimos 10 entrenos y el grafico de tiempos). */
    public class GraphicsExercisesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TrainingHistory> Last10Trainings { get; set; }
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand DowloadCommand { get; }
      
        /* Constructor de GraphisExercisesViewModel. Carga el historial de entramiento, los botones de descargar historial y refrescar datos y obtiene los últimos 10 entrenamientos, además de cargar la grafica */
        public GraphicsExercisesViewModel()
        {
            var allTrainings = ReadTrainingHistory();
            RefreshCommand = new RelayCommand(RefreshData);
            DowloadCommand = new RelayCommand(DowloadHistory);

            var last10 = allTrainings
                .Where(t => DateTime.TryParse(t.Fecha, out _))
                .OrderByDescending(t => DateTime.Parse(t.Fecha)) 
                .Take(10)
                .ToList();

            Last10Trainings = new ObservableCollection<TrainingHistory>(last10);

            var labels = Last10Trainings
                .Select(t => DateTime.TryParse(t.Fecha, out var f) ? f.ToString("dd/MM") : "")
                .ToArray();

            var timeDefaults = Last10Trainings
                .Select(t => t.TimeDefaultParsed?.TotalSeconds ?? 0)
                .ToArray();

            var timeUsers = Last10Trainings
                .Select(t => t.TimeUserParsed?.TotalSeconds ?? 0)
                .ToArray();

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = timeDefaults,
                    Name = "Tiempo Objetivo",
                    Stroke = new SolidColorPaint(SKColors.Purple, 2),
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(SKColors.Purple),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    LineSmoothness = 0.5
                },
                new LineSeries<double>
                {
                    Values = timeUsers,
                    Name = "Tiempo de Usuario",
                    Stroke = new SolidColorPaint(SKColors.Turquoise, 2),
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(SKColors.Turquoise),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    LineSmoothness = 0.5
                }
            };

            XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Labels = labels,
                    LabelsRotation = 45,
                    Name = "Fecha",
                    TextSize = 14,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                }
            };

            YAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Name = "Tiempo",
                    Labeler = value =>
                    {
                        if (double.TryParse(value.ToString(), out double seconds))
                        {
                            return TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
                        }
                        else
                        {
                            return "00:00:00";
                        }
                    },
                    TextSize = 14,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /* Método que permite leer el historial de entrenamientos del usuario */
        private List<TrainingHistory> ReadTrainingHistory()
        {
            string historyPath = "training_history.xml";
            var serializer = new XmlSerializer(typeof(TrainingUserHistory));

            if (!File.Exists(historyPath))
                return new List<TrainingHistory>();

            using (var reader = new StreamReader(historyPath))
            {
                var history = (TrainingUserHistory)serializer.Deserialize(reader);
                return history.TrainingHistories;
            }
        }

        /* Método que refresca los datos del historial de entrenamiento */
        private void RefreshData()
        {
            var allTrainings = ReadTrainingHistory();

            var last10 = allTrainings
                .Where(t => DateTime.TryParse(t.Fecha, out _))
                .OrderByDescending(t => DateTime.Parse(t.Fecha))
                .Take(10)
                .ToList();

            Last10Trainings.Clear();
            foreach (var t in last10)
                Last10Trainings.Add(t);

            var labels = Last10Trainings
                .Select(t => DateTime.TryParse(t.Fecha, out var f) ? f.ToString("dd/MM") : "")
                .ToArray();

            var timeDefaults = Last10Trainings
                .Select(t => t.TimeDefaultParsed?.TotalSeconds ?? 0)
                .ToArray();

            var timeUsers = Last10Trainings
                .Select(t => t.TimeUserParsed?.TotalSeconds ?? 0)
                .ToArray();

            Series.Clear();
            Series.Add(new LineSeries<double>
            {
                Values = timeDefaults,
                Name = "Time Default",
                Stroke = new SolidColorPaint(SKColors.MediumPurple),
                Fill = null
            });
            Series.Add(new LineSeries<double>
            {
                Values = timeUsers,
                Name = "Time User",
                Stroke = new SolidColorPaint(SKColors.MediumTurquoise),
                Fill = null
            });

            XAxes.Clear();
            XAxes.Add(new Axis
            {
                Labels = labels,
                LabelsRotation = 45
            });

            YAxes.Clear();
            YAxes.Add(new Axis
            {
                Name = "Tiempo",
                Labeler = value => TimeSpan.FromSeconds(value).ToString(@"hh\:mm\:ss")
            });
        }

        /* Método que descarga en un excel todos los datos del historial de entrenamiento de un usaurio */
        private void DowloadHistory()
        {
            var xmlPath = "training_history.xml";
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine("Archivo XML no encontrado.");
                return;
            }

            var xml = XDocument.Load(xmlPath);

            var entrenamientos = xml.Descendants("TrainingHistories")
                .Select(e => new
                {
                    TimeDefault = e.Element("TimeDefault")?.Value ?? "",
                    TimeUser = e.Element("TimeUser")?.Value ?? "",
                    Fecha = e.Element("Fecha")?.Value ?? "",
                    Exercise = e.Element("ExerciseDefault")?.Value ?? ""
                })
                .ToList();


            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Entrenamientos");
            
            worksheet.Cell(1, 1).Value = "Fecha";
            worksheet.Cell(1, 2).Value = "Ejercicio";
            worksheet.Cell(1, 3).Value = "Tiempo Objetivo";
            worksheet.Cell(1, 4).Value = "Tiempo Usuario";

            for (int i = 0; i < entrenamientos.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = entrenamientos[i].Fecha;
                worksheet.Cell(row, 2).Value = entrenamientos[i].Exercise;
                worksheet.Cell(row, 3).Value = entrenamientos[i].TimeDefault;
                worksheet.Cell(row, 4).Value = entrenamientos[i].TimeUser;
            }

            // Se descargará por defecto el archivo en la carpeta de Descargas
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadsPath, "training_history.xlsx");

            workbook.SaveAs(filePath);
        }      
    }
}