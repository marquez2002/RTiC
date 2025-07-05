using RTIC.ViewModels;
using System.Windows.Controls;

namespace RTIC.Views
{
    /// <summary>
    /// Lógica de interacción para GraphicsExercises.xaml
    /// </summary>
    public partial class GraphicsExercises : UserControl
    {
        public GraphicsExercises()
        {
            InitializeComponent();
            this.DataContext = new GraphicsExercisesViewModel();
        }
    }
}