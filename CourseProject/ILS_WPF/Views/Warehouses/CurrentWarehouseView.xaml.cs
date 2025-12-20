using ILS_WPF.ViewModels;
using System.Windows.Controls;

namespace ILS_WPF.Views.Warehouses
{
    /// <summary>
    /// Логика взаимодействия для CurrentWarehouseView.xaml
    /// </summary>
    public partial class CurrentWarehouseView : UserControl
    {
        public CurrentWarehouseView(CurrentWarehouseVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
