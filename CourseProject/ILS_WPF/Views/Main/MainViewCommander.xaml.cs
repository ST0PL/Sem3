using ILS_WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ILS_WPF.Views.Main
{
    /// <summary>
    /// Логика взаимодействия для MainViewCommander.xaml
    /// </summary>
    public partial class MainViewCommander : UserControl
    {
        public MainViewCommander(MainCommanderVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
