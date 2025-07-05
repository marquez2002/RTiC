using RTIC.Models;
using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace RTIC
{
    /// <summary>
    /// Lógica de interacción para StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public string UserName { get; private set; }

        // Constructor de StartupWindow
        public StartupWindow()
        {
            InitializeComponent();
        }

        // Método que se activa cuando el usaurio hace click en el botón
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("Por favor, ingresa un nombre válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

             SaveProfile();
             File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "firstrun.flag"), "ok");
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        // Método que permite guardar en el perfil solo el nombre y la fecha
        private void SaveProfile()
        {
            try
            {
                var runnerToSave = new Runner
                {
                    Name = UserName,
                    Date = DateTime.Now
                };

                var serializer = new XmlSerializer(typeof(Runner));
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "runner_data.xml");

                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, runnerToSave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
