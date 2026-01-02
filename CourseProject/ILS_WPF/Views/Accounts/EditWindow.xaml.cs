using ILS_WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ILS_WPF.Views.Accounts
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow(EditAccountVM viewModel)
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
            => ((EditAccountVM)DataContext).Password = ((PasswordBox)sender).Password;

    }
}
