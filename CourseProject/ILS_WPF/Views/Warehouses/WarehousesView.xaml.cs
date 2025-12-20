using ILS_WPF.ViewModels;
using System.Windows.Controls;

namespace ILS_WPF.Views.Warehouses
{
    /// <summary>
    /// Логика взаимодействия для WarehousesView.xaml
    /// </summary>
    public partial class WarehousesView : UserControl
    {
        public WarehousesView(WarehousesVM viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
