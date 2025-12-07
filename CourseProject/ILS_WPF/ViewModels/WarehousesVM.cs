using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using ILS_WPF.Views.Warehouses;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace ILS_WPF.ViewModels
{
    public class WarehousesVM : BaseVM
    {
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public WarehousesVM(IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            RelayCommand navigateBackCommand = null!;
            RelayCommand navigateToWarehouseViewCommand = null!;
            
            navigateBackCommand = new RelayCommand(_ =>
                CurrentView = new WarehouseListView(new WarehouseListVM(viewUpdaterService, windowService, dbFactory, navigateToWarehouseViewCommand, navigateBackCommand)));

            navigateToWarehouseViewCommand = new RelayCommand(w =>
                CurrentView = new CurrentWarehouseView(new CurrentWarehouseVM((w as Warehouse)!.Id, viewUpdaterService, windowService, dbFactory, navigateBackCommand)));

            CurrentView = new WarehouseListView(new WarehouseListVM(viewUpdaterService, windowService, dbFactory, navigateToWarehouseViewCommand, navigateBackCommand));
        }
    }
}
