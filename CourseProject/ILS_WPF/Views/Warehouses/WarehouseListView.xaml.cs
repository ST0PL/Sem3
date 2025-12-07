using ILS_WPF.ViewModels;
using System.Windows.Controls;

namespace ILS_WPF.Views.Warehouses
{
    /// <summary>
    /// Логика взаимодействия для WarehouseListView.xaml
    /// </summary>
    public partial class WarehouseListView : UserControl
    {
        public WarehouseListView(WarehouseListVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
