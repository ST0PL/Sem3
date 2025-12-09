using ILS_WPF.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ILS_WPF.Views.Warehouses
{
    /// <summary>
    /// Логика взаимодействия для EditEntryWindow.xaml
    /// </summary>
    public partial class EditEntryWindow : Window
    {
        public EditEntryWindow(EditWarehouseEntryVM viewModel)
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
