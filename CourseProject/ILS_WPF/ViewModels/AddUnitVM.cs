using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class AddUnitVM : BaseVM
    {
        private IDbContextFactory<ILSContext> _dbFactory;
        private IWindowService _windowService;
        private ICommand _dataRefreshCommand;
        private WarehouseType _selectedWarehouseType;
        private UnitRelationType _selectedUnitRelationType;
        private Speciality _selectedCommanderSpeciality;
        private Speciality _selectedSoldierSpeciality;
        private UnitType _selectedUnitType;
        private string _name;
        private string _unitQuery;
        private string _warehouseQuery;
        private string _staffQuery;
        private string _commanderQuery;
        private Wrap<Warehouse>[] _warehouses;
        private Wrap<Unit>[] _units;
        private Wrap<Staff>[] _personnel;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        public WarehouseType[] WarehouseTypes { get; set; }
        public UnitRelationType[] UnitRelationTypes { get; set; }
        public Speciality[] Specialities { get; set; }
        public UnitType[] UnitTypes { get; set; }

        public WarehouseType SelectedWarehouseType
        {
            get => _selectedWarehouseType;
            set
            {
                _selectedWarehouseType = value;
                _ = LoadWarehouses();
            }
        }

        public UnitRelationType SelectedUnitRelationType
        {
            get => _selectedUnitRelationType;
            set
            {
                _selectedUnitRelationType = value;
                _ = LoadUnits();
            }
        }

        public Speciality SelectedCommanderSpeciality
        {
            get => _selectedCommanderSpeciality;
            set
            {
                _selectedCommanderSpeciality = value;
                OnPropertyChanged();
            }
        }

        public Speciality SelectedSoldierSpeciality
        {
            get => _selectedSoldierSpeciality;
            set
            {
                _selectedSoldierSpeciality = value;
                OnPropertyChanged();
            }
        }

        public UnitType SelectedUnitType
        {
            get => _selectedUnitType;
            set
            {
                _selectedUnitType = value;
                SelectedUnits = [];
                _ = LoadUnits();
                OnPropertyChanged();
            }
        }

        public string UnitQuery
        {
            get => _unitQuery;
            set
            {
                _unitQuery = value;
                _ = LoadUnits();
            }
        }
        public string WarehouseQuery
        {
            get => _warehouseQuery;
            set
            {
                _warehouseQuery = value;
                _ = LoadWarehouses();
            }
        }
        public string StaffQuery
        {
            get => _staffQuery;
            set
            {
                _staffQuery = value;
            }
        }
        public string CommanderQuery
        {
            get => _commanderQuery;
            set
            {
                _commanderQuery = value;
            }
        }

        public Wrap<Warehouse>[] Warehouses { get => _warehouses; set { _warehouses = value; OnPropertyChanged(); } }
        public Wrap<Unit>[] Units { get => _units; set { _units = value; OnPropertyChanged(); } }
        public Wrap<Staff>[] Personnel { get => _personnel; set { _personnel = value; OnPropertyChanged(); } }

        public Warehouse? SelectedWarehouse { get; set; }
        public List<Unit> SelectedUnits { get; set; } = new();
        public List<Staff> SelectedPersonnel { get; set; } = new();
        public Staff? SelectedCommander { get; set; }

        public bool HasWarehouses => Warehouses?.Length > 0;
        public bool HasUnits => Units?.Length > 0;
        public bool HasPersonnel => Personnel?.Length > 0;

        public ICommand WarehouseWrapCheckedCommand { get; set; }
        public ICommand UnitWrapCheckedCommand { get; set; }
        public ICommand StaffWrapCheckedCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public AddUnitVM(IWindowService windowService, IDbContextFactory<ILSContext> dbFactory, ICommand dataRefreshCommand)
        {
            _dbFactory = dbFactory;
            _windowService = windowService;
            _dataRefreshCommand = dataRefreshCommand;
            WarehouseTypes = Enum.GetValues<WarehouseType>().Order().ToArray();
            UnitRelationTypes = Enum.GetValues<UnitRelationType>().Order().ToArray();
            Specialities = Enum.GetValues<Speciality>().Order().ToArray();
            UnitTypes = Enum.GetValues<UnitType>().Order().Skip(1).ToArray();
            SelectedWarehouseType = WarehouseTypes[0];
            SelectedUnitRelationType = UnitRelationTypes[0];
            SelectedSoldierSpeciality = Specialities[0];
            SelectedCommanderSpeciality = Specialities[0];
            SelectedUnitType = UnitTypes[0];
            InitCommands();
            _ = LoadWarehouses();
            _ = LoadUnits();
            _ = LoadPersonnel();
        }

        void InitCommands()
        {
            WarehouseWrapCheckedCommand = new RelayCommand(wrap => OnWarehouseWrapCheckChanged((wrap as Wrap<Warehouse>)!));
            UnitWrapCheckedCommand = new RelayCommand(wrap => OnMultipleSelectionChanged((wrap as Wrap<Unit>)!, SelectedUnits));
            StaffWrapCheckedCommand = new RelayCommand(wrap => OnMultipleSelectionChanged((wrap as Wrap<Staff>)!, SelectedPersonnel));
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => !string.IsNullOrWhiteSpace(Name));
        }

        async Task LoadWarehouses()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            Warehouses = await context.Warehouses.
                Where(
                w =>
                (SelectedWarehouseType == WarehouseType.AnyType || w.Type == SelectedWarehouseType) &&
                (string.IsNullOrWhiteSpace(WarehouseQuery) || EF.Functions.Like(w.Name, $"%{WarehouseQuery}%")))
                .Select(w=>new Wrap<Warehouse>(w) { IsChecked = (SelectedWarehouse != null && SelectedWarehouse.Id == w.Id) }).ToArrayAsync();
            OnPropertyChanged(nameof(HasWarehouses));
        }

        async Task LoadUnits()
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            Units = (await context.Units
                .Include(u => u.Commander)
                .Where(u => (u.Type == (SelectedUnitType - 1)) &&
                    (string.IsNullOrWhiteSpace(UnitQuery) || EF.Functions.Like(u.Name, $"%{UnitQuery}%") || (u.Commander != null && EF.Functions.Like(u.Commander.FullName, $"%{UnitQuery}%"))) &&
                    u.ParentId == null)
                .ToArrayAsync())
                .Where(IsUnitRelationMatches)
                .Select(u => new Wrap<Unit>(u) { IsChecked = SelectedUnits.Any(selected=>selected.Id == u.Id) })
                .OrderByDescending(u=>u.IsChecked).ToArray();

            OnPropertyChanged(nameof(HasUnits));
        }

        async Task LoadPersonnel()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            Personnel = await context.Personnel
                .Where(p => p.UnitId == null && p.Rank <= UnitRankMatcher.CommanderRanks[SelectedUnitType].Last())
                .Select(p=>new Wrap<Staff>(p)).ToArrayAsync();
        }
        
        bool IsUnitRelationMatches(Unit unit)
        {
            if (SelectedUnitRelationType == UnitRelationType.AnyType)
                return true;
            
            bool isAttached = SelectedUnits.Any(u => u.Id == unit.Id);

            return SelectedUnitRelationType switch
            {
                UnitRelationType.Attached => isAttached,
                UnitRelationType.NotAttached => !isAttached,
                _ => false
            };
        }

        async Task RegisterAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            await context.Units.AddAsync(new Unit());
            await context.SaveChangesAsync();
            _dataRefreshCommand.Execute(null);
            _windowService.OpenMessageWindow("Регистрация данных", "Данные о подразделении были успешно зарегистрированы.");
        }

        void OnWarehouseWrapCheckChanged(Wrap<Warehouse> wrap)
        {
            SelectedWarehouse = wrap.IsChecked ? wrap.Value : null;
            if (wrap.IsChecked)
            {
                foreach (var w in Warehouses)
                {
                    if (w != wrap)
                        w.IsChecked = false;
                }
            } 
        }

        void OnMultipleSelectionChanged<T>(Wrap<T> wrap, List<T> collection) where T : class, IDbEntry
        {
            if (wrap.IsChecked)
                collection.Add(wrap.Value);
            else
                collection.RemoveAll(u => u.Id == wrap.Value.Id);
            _ = LoadUnits();
        }
    }
}
