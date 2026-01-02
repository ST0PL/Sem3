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
        private IUserService _userService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private string _query;
        private WarehouseType _selectedWarehouseType;
        private Warehouse[]? _warehouses;

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

        public Warehouse[]? Warehouses
        {
            get => _warehouses;
            set
            {
                _warehouses = value;
                OnPropertyChanged();
            }
        }

        public bool HasItems => _warehouses?.Length > 0;

        public bool IsAdmin { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand NavigateToWarehouseViewCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }

        public WarehouseListVM(
            IViewModelUpdaterService viewUpdaterService,
            IUserService userService,
            IWindowService windowService,
            IDbContextFactory<ILSContext> dbFactory,
            ICommand navigateToWarehouseView,
            ICommand navigateBack,
            bool isAdmin)
        {
            _userService = userService;
            _dbFactory = dbFactory;
            WarehouseTypes = [.. Enum.GetValues<WarehouseType>().Order()];
            _selectedWarehouseType = WarehouseTypes[0];
            NavigateToWarehouseViewCommand = navigateToWarehouseView;
            NavigateBackCommand = navigateBack;
            IsAdmin = isAdmin;
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            OpenRegisterWindowCommand = new RelayCommand(_ => windowService.OpenWarehouseRegisterWindow(), _=> isAdmin);
            viewUpdaterService.SetUpdateCommand<WarehouseListVM>(RefreshCommand);
            _ = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var commanderId = _userService.GetUser()?.StaffId;


            if (IsAdmin)
            {
                Warehouses = await (context.Warehouses.Where(
                    w =>
                    (string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(w.Name, $"%{Query}%")) &&
                    (SelectedWarehouseType == WarehouseType.AnyType || w.Type == SelectedWarehouseType))
                    .ToArrayAsync());
            }
            else
            {
                var allUnits = await context.Units.ToListAsync();
                await LoadAllUnitsParentsAndWarehouses(context, allUnits);
                var commandedUnits = allUnits.Where(u => HasCommanderInTree(u, commanderId));
                Warehouses = [.. commandedUnits.Where(u=>u.AssignedWarehouse != null).Select(u => u.AssignedWarehouse!)];
            }
            OnPropertyChanged(nameof(HasItems));
        }

        async Task LoadAllUnitsParentsAndWarehouses(ILSContext context, List<Unit> units)
        {
            foreach (var unit in units)
            {
                unit.Parent = await context.Units.Where(u => u.Id == unit.ParentId).FirstOrDefaultAsync();
                unit.AssignedWarehouse = await context.Warehouses.Where(w => w.Id == unit.AssignedWarehouseId).FirstOrDefaultAsync();
            }
        }

        bool HasCommanderInTree(Unit unit, int? commanderId)
        {
            if (commanderId == null)
                return false;
            Unit currentUnit = unit;
            while (currentUnit != null)
            {
                if (currentUnit.CommanderId == commanderId)
                    return true;
                currentUnit = currentUnit.Parent!;
            }
            return false;
        }
    }
}
