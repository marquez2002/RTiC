using RTIC;
using RTIC.Views;
using System.IO;
using System.Windows;

namespace RTIC
{
    /// <summary>
    /// Lógica de interación para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string firstRunFilePath = Path.Combine(AppContext.BaseDirectory, "firstrun.flag");

        // Método que se ejecuta cuando se inicia la aplicación
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string flagPath = Path.Combine(AppContext.BaseDirectory, "firstrun.flag");
            if (!File.Exists(flagPath))
            {
                var startupWindow = new StartupWindow();
                startupWindow.ShowDialog();

                if (!File.Exists(flagPath))
                {
                    Shutdown();
                    return;
                }
            }
            else
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
