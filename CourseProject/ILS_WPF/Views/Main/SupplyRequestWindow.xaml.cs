using ILS_WPF.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ILS_WPF.Views.Main
{
    /// <summary>
    /// Логика взаимодействия для SupplyRequestWindow.xaml
    /// </summary>
    public partial class SupplyRequestWindow : Window
    {
        public SupplyRequestWindow(SupplyRequestVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
