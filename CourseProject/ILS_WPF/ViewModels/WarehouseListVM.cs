using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class WarehouseListVM : BaseVM
    {
        private IDbContextFactory<ILSContext> _dbFactory;
        private string _query;
        private WarehouseType _selectedWarehouseType;
        private Warehouse[] _warehouses;

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                _ = LoadData();
            }
        }

        public WarehouseType[] WarehouseTypes { get; set; }

        public WarehouseType SelectedWarehouseType
        {
            get => _selectedWarehouseType;
            set
            {
                _selectedWarehouseType = value;
                _ = LoadData();
            }
        }

        public Warehouse[] Warehouses
        {
            get => _warehouses;
            set
            {
                _warehouses = value;
                OnPropertyChanged();
            }
        }

        public bool HasItems => _warehouses?.Length > 0;

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand NavigateToWarehouseViewCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }

        public WarehouseListVM(
            IViewModelUpdaterService viewUpdaterService,
            IWindowService windowService,
            IDbContextFactory<ILSContext> dbFactory,
            ICommand navigateToWarehouseView,
            ICommand navigateBack)
        {
            _dbFactory = dbFactory;
            WarehouseTypes = [.. Enum.GetValues<WarehouseType>().Order()];
            _selectedWarehouseType = WarehouseTypes[0];
            NavigateToWarehouseViewCommand = navigateToWarehouseView;
            NavigateBackCommand = navigateBack;
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            OpenRegisterWindowCommand = new RelayCommand(_ => windowService.OpenWarehouseRegisterWindow());
            viewUpdaterService.SetUpdateCommand<WarehouseListVM>(RefreshCommand);
            _ = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            Warehouses = await (context.Warehouses.Where(
                w =>
                (string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(w.Name, $"%{Query}%")) &&
                (SelectedWarehouseType == WarehouseType.AnyType || w.Type == SelectedWarehouseType))
                .ToArrayAsync());
            OnPropertyChanged(nameof(HasItems));
        }
    }
}
