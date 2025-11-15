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
        public LoginWindow(
            IConfigurationService<Configuration?> configurationService,
            IUserService userService,
            IAccountService accountService)
        {
            var vm = new LoginVM(configurationService, userService, accountService);
            vm.LoginPerformed += (_, isSuccess) => CloseIfSuccess(isSuccess);
            DataContext = vm;
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
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
            => ((LoginVM)DataContext).Password = ((PasswordBox)sender).Password;

        private void CloseIfSuccess(bool loginSuccess)
        {
            if (loginSuccess)
                DialogResult = true;
        }
    }
}
