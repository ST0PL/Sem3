using ILS_WPF.ViewModels;
using System.Windows.Controls;

namespace ILS_WPF.Views.Main
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView(StatVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
