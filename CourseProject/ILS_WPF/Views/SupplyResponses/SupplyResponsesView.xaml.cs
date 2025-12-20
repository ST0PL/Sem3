using ILS_WPF.ViewModels;
using System.Windows.Controls;

namespace ILS_WPF.Views.SupplyResponses
{
    /// <summary>
    /// Логика взаимодействия для SupplyResponsesView.xaml
    /// </summary>
    public partial class SupplyResponsesView : UserControl
    {
        public SupplyResponsesView(SupplyResponsesVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
