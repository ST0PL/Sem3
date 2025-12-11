using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class MainCommanderVM : BaseVM
    {
        private IUserService _userService;
        private IWindowService _windowService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private Unit? _currentUnit;
        private SupplyResponseWrap[] _supplyResponses;
        private Staff[]? _personnel;
        private object[]? _warehouseItems;
        public Unit? CurrentUnit
        {
            get => _currentUnit;
            set
            {
                _currentUnit = value;
                OnPropertyChanged();
            }
        }
        public SupplyResponseWrap[] SupplyResponses
        {
            get => _supplyResponses;
            set
            {
                _supplyResponses = value;
                OnPropertyChanged();
            }
        }
        public Staff[]? Personnel
        {
            get => _personnel;
            set
            {
                _personnel = value;
                OnPropertyChanged();
            }
        }
        public object[]? WarehouseItems
        {
            get => _warehouseItems;
            set
            {
                _warehouseItems = value;
                OnPropertyChanged();
            }
        }

        public bool HasSupplyResponses => SupplyResponses?.Length > 0;
        public bool HasPersonnel => Personnel?.Length > 0;
        public bool HasWarehouseItems => WarehouseItems?.Length > 0;

        public ICommand OpenSupplyRequestWindowCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public MainCommanderVM(IUserService userService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _userService = userService;
            _windowService = windowService;
            _dbFactory = dbFactory;
            OpenSupplyRequestWindowCommand = new RelayCommand(_=> _windowService.OpenSupplyRequestWindow());
            RefreshCommand = new RelayCommand(async _=> await LoadData());
            _ = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            CurrentUnit = await context.Units
                .Where(u => u.CommanderId == _userService.GetUser()!.StaffId)
                .Include(u => u.Children)
                .Include(u => u.Commander)
                .Include(u => u.AssignedWarehouse)
                .ThenInclude(w => w!.Resources)
                .Include(u => u.AssignedWarehouse)
                .ThenInclude(w => w!.Equipments)
                .Include(u => u.SupplyRequests)
                .FirstOrDefaultAsync();

            if (CurrentUnit == null)
                return;

            SupplyResponses = await context.SupplyResponses
                .Include(sr => sr.Request)
                .ThenInclude(r => r!.RequestUnit)
                .Where(sr => sr.Request != null && sr.Request.RequestUnitId == CurrentUnit.Id)
                .Select(sr => new SupplyResponseWrap(sr)).ToArrayAsync();
            SupplyResponses =
                [
                    new SupplyResponseWrap(new Models.Core.Requests.SupplyResponse() { Id = 10, RequestId = 20, Request = new Models.Core.Requests.SupplyRequest() { Id = 20, CreationTime = DateTime.Now, RequestUnit = CurrentUnit } }),
                    new SupplyResponseWrap(new Models.Core.Requests.SupplyResponse() { Id = 10, RequestId = 20, Request = new Models.Core.Requests.SupplyRequest() { Id = 20, CreationTime = DateTime.Now, RequestUnit = CurrentUnit } }),
                    new SupplyResponseWrap(new Models.Core.Requests.SupplyResponse() { Id = 10, RequestId = 20, Request = new Models.Core.Requests.SupplyRequest() { Id = 20, CreationTime = DateTime.Now, RequestUnit = CurrentUnit } })
                ];

            LoadPersonnel(context, CurrentUnit.Id);

            if (CurrentUnit.AssignedWarehouse != null)
                WarehouseItems = [.. CurrentUnit.AssignedWarehouse.Resources, .. CurrentUnit.AssignedWarehouse.Equipments];
            
            OnPropertiesChanged(nameof(HasSupplyResponses), nameof(HasPersonnel), nameof(HasWarehouseItems));
        }

        void LoadPersonnel(ILSContext context, int unitId)
        {
            int currentId;

            Queue<int> unitQueue = new();
            IQueryable<Unit> children;
            List<Staff> personnel = new();
            unitQueue.Enqueue(unitId);

            while(unitQueue.Count > 0)
            {
                currentId = unitQueue.Dequeue();
                children = context.Units
                    .Where(u=>u.ParentId == currentId)
                    .Include(u=>u.Personnel);

                foreach(var child in children)
                {
                    if(child.Type == UnitType.Battalion)
                    {
                        personnel.AddRange(child.Personnel);
                        continue;
                    }
                    unitQueue.Enqueue(child.Id);
                }
            }
            Personnel = [.. personnel];
        }

        void OnPropertiesChanged(params string[] properties)
        {
            foreach (var property in properties)
                OnPropertyChanged(property);
        }
    }
}
