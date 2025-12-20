using ILS_WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ILS_WPF.Views.Accounts
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow(AddAccountVM viewModel)
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
            => ((AddAccountVM)DataContext).Password = ((PasswordBox)sender).Password;
    }
}
