using ILS_WPF.ViewModels;
using System.Windows.Controls;


namespace ILS_WPF.Views.Accounts
{
    /// <summary>
    /// Логика взаимодействия для AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        public AccountsView(AccountsVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
