using ILS_WPF.Services.Interfaces;
using ILS_WPF.ViewModels;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using ILS_WPF.Models;

namespace ILS_WPF
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
            => ((LoginVM)DataContext).Password = ((PasswordBox)sender).Password;
    }
}
