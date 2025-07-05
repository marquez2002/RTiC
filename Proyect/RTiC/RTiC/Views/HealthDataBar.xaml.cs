using RTIC.ViewModels;
using System.Windows.Controls;

namespace RTIC.Views
{
    /// <summary>
    /// Lógica de interacción para HealthDataBar.xaml
    /// </summary>
    public partial class HealthDataBar : UserControl
    {
        public HealthDataBar()
        {
            InitializeComponent();
            this.DataContext = new HealthDataBarViewModel();
        }
    }
}


