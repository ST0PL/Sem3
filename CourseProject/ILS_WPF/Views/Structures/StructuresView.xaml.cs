using ILS_WPF.ViewModels;
using System.Windows.Controls;

namespace ILS_WPF.Views.Structures
{
    /// <summary>
    /// Логика взаимодействия для StructuresView.xaml
    /// </summary>
    public partial class StructuresView : UserControl
    {
        public StructuresView(StructuresVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
