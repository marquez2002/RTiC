using System.Windows.Controls;
using RTIC.ViewModels;

namespace RTIC.Views
{
    /// <summary>
    /// Lógica de interacción para SemanalTraining.xaml
    /// </summary>
    public partial class SemanalTraining : UserControl
    {
        public SemanalTraining()
        {
            InitializeComponent();
            this.DataContext = new SemanalTrainingViewModel();
        }
    }
}