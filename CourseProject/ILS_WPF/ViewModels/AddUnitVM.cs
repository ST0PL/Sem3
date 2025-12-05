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
        private RelationType _selectedUnitRelationType;
        private RelationType _selectedStaffRelationType;
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
        private Wrap<Staff>[] _commanders;
        private Wrap<Staff>[] _personnel;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        public WarehouseType[] WarehouseTypes { get; set; }
        public RelationType[] RelationTypes { get; set; }
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

        public RelationType SelectedUnitRelationType
        {
            get => _selectedUnitRelationType;
            set
            {
                _selectedUnitRelationType = value;
                _ = LoadUnits();
            }
        }

        public RelationType SelectedStaffRelationType
        {
            get => _selectedStaffRelationType;
            set
            {
                _selectedStaffRelationType = value;
                _ = LoadPersonnel();
            }
        }

        public Speciality SelectedCommanderSpeciality
        {
            get => _selectedCommanderSpeciality;
            set
            {
                _selectedCommanderSpeciality = value;
                _ = LoadCommanders();
            }
        }

        public Speciality SelectedSoldierSpeciality
        {
            get => _selectedSoldierSpeciality;
            set
            {
                _selectedSoldierSpeciality = value;
                _ = LoadPersonnel();
            }
        }

        public UnitType SelectedUnitType
        {
            get => _selectedUnitType;
            set
            {
                _selectedUnitType = value;
                SelectedUnits = [];
                SelectedPersonnel = [];
                _ = LoadUnits();
                _ = LoadPersonnel();
                _ = LoadCommanders();
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
                _ = LoadPersonnel();
            }
        }
        public string CommanderQuery
        {
            get => _commanderQuery;
            set
            {
                _commanderQuery = value;
                _ = LoadCommanders();
            }
        }

        public Wrap<Warehouse>[] Warehouses { get => _warehouses; set { _warehouses = value; OnPropertyChanged(); } }
        public Wrap<Unit>[] Units { get => _units; set { _units = value; OnPropertyChanged(); } }
        public Wrap<Staff>[] Commanders { get => _commanders; set { _commanders = value; OnPropertyChanged(); } }
        public Wrap<Staff>[] Personnel { get => _personnel; set { _personnel = value; OnPropertyChanged(); } }

        public Warehouse? SelectedWarehouse { get; set; }
        public List<Unit> SelectedUnits { get; set; } = new();
        public Staff? SelectedCommander { get; set; }
        public List<Staff> SelectedPersonnel { get; set; } = new();

        public bool HasWarehouses => Warehouses?.Length > 0;
        public bool HasUnits => Units?.Length > 0;
        public bool HasCommanders => Commanders?.Length > 0;
        public bool HasPersonnel => Personnel?.Length > 0;

        public ICommand WarehouseWrapCheckedCommand { get; set; }
        public ICommand UnitWrapCheckedCommand { get; set; }
        public ICommand CommanderWrapCheckedCommand { get; set; }
        public ICommand StaffWrapCheckedCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public AddUnitVM(IWindowService windowService, IDbContextFactory<ILSContext> dbFactory, ICommand dataRefreshCommand)
        {
            _dbFactory = dbFactory;
            _windowService = windowService;
            _dataRefreshCommand = dataRefreshCommand;
            WarehouseTypes = Enum.GetValues<WarehouseType>().Order().ToArray();
            RelationTypes = Enum.GetValues<RelationType>().Order().ToArray();
            Specialities = Enum.GetValues<Speciality>().Order().ToArray();
            UnitTypes = Enum.GetValues<UnitType>().Order().Skip(1).ToArray();
            SelectedWarehouseType = WarehouseTypes[0];
            SelectedUnitRelationType = RelationTypes[0];
            SelectedStaffRelationType = RelationTypes[0];
            SelectedSoldierSpeciality = Specialities[0];
            SelectedCommanderSpeciality = Specialities[0];
            SelectedUnitType = UnitTypes[0];
            InitCommands();
            _ = LoadData();
        }

        void InitCommands()
        {
            WarehouseWrapCheckedCommand = new RelayCommand(wrap => OnSingleSelectionChanged((wrap as Wrap<Warehouse>)!, Warehouses, w => SelectedWarehouse = w));
            UnitWrapCheckedCommand = new RelayCommand(wrap => OnMultipleSelectionChanged((wrap as Wrap<Unit>)!, SelectedUnits, () => _ = LoadUnits()));
            CommanderWrapCheckedCommand =
                new RelayCommand(wrap => OnSingleSelectionChanged((wrap as Wrap<Staff>)!, Commanders,
                cmd =>
                {
                    SelectedCommander = cmd;
                    _ = LoadCommanders();
                    _ = LoadPersonnel();

                }));
            StaffWrapCheckedCommand = new RelayCommand(wrap => OnMultipleSelectionChanged((wrap as Wrap<Staff>)!, SelectedPersonnel, () =>
            {
                _ = LoadPersonnel();
                _ = LoadCommanders();
            }));
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => !string.IsNullOrWhiteSpace(Name));
        }

        async Task LoadData()
        {
            await LoadWarehouses();
            await LoadUnits();
            await LoadCommanders();
            await LoadPersonnel();
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
                .Where(u => IsRelationMatches(u, SelectedUnitRelationType, SelectedUnits))
                .Select(u => new Wrap<Unit>(u) { IsChecked = SelectedUnits.Any(selected=>selected.Id == u.Id) })
                .OrderByDescending(u=>u.IsChecked).ToArray();

            OnPropertyChanged(nameof(HasUnits));
        }

        async Task LoadPersonnel()
        {
            Personnel = [];

            if (SelectedUnitType == UnitType.Battalion)
            {
                using var context = await _dbFactory.CreateDbContextAsync();
                Personnel = (await context.Personnel
                    .Include(p => p.Unit)
                    .Where(p =>
                        p.UnitId == null &&
                        (SelectedCommander == null || p.Id != SelectedCommander.Id) &&
                        p.Rank <= UnitRankMatcher.MaxBattalionRank &&
                        (SelectedSoldierSpeciality == Speciality.AnySpeciality || p.Speciality == SelectedSoldierSpeciality) &&
                        (string.IsNullOrWhiteSpace(StaffQuery) || EF.Functions.Like(p.FullName, $"%{StaffQuery}%")))
                    .ToArrayAsync())
                    .Where(p =>
                        IsRelationMatches(p, SelectedStaffRelationType, SelectedPersonnel) &&
                        !context.Units.Any(u=>u.CommanderId == p.Id))
                    .Select(p => new Wrap<Staff>(p) { IsChecked = SelectedPersonnel.Any(selected => selected.Id == p.Id) })
                    .OrderBy(w=>w.IsChecked)
                    .ToArray();
            }
            OnPropertyChanged(nameof(HasPersonnel));
        }

        async Task LoadCommanders()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            Commanders = (await context.Personnel
                .Where(c =>
                    (SelectedCommanderSpeciality == Speciality.AnySpeciality || c.Speciality == SelectedCommanderSpeciality) &&
                    (string.IsNullOrWhiteSpace(CommanderQuery) || EF.Functions.Like(c.FullName, $"%{CommanderQuery}%")) &&
                    !context.Units.Any(u=>u.CommanderId == c.Id) && c.UnitId == null)
                .ToArrayAsync())
                .Where(c => UnitRankMatcher.IsMatches(SelectedUnitType, c.Rank) && !SelectedPersonnel.Any(p => p.Id == c.Id))
                .Select(c =>new Wrap<Staff>(c) { IsChecked = SelectedCommander != null && SelectedCommander.Id == c.Id })
                .OrderBy(w=>w.IsChecked)
                .ToArray();

            OnPropertyChanged(nameof(HasCommanders));
        }

        async Task RegisterAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            foreach (var p in Personnel)
                context.Attach(p);
            foreach (var c in SelectedUnits)
                context.Attach(c);

            await context.Units.AddAsync(new Unit()
            {
                Name = Name,
                CommanderId = SelectedCommander?.Id,
                AssignedWarehouseId = SelectedWarehouse?.Id,
                Personnel = SelectedPersonnel,
                Children = SelectedUnits
            });
            await context.SaveChangesAsync();
            _dataRefreshCommand.Execute(null);
            _windowService.OpenMessageWindow("Регистрация данных", "Данные о подразделении были успешно зарегистрированы.");
        }

        void OnSingleSelectionChanged<T>(
            Wrap<T>? wrap,
            IEnumerable<Wrap<T>> collection,
            Action<T?> updatePropertyAction) where T : class?
        {
            if(wrap == null)
            {
                updatePropertyAction(null);
                return;
            }
            if (wrap.IsChecked)
            {
                foreach (var w in collection)
                {
                    if (w != wrap)
                        w.IsChecked = false;
                }
            }
            updatePropertyAction(wrap.IsChecked ? wrap.Value : null);
        }

        bool IsRelationMatches<T>(T item, RelationType relationType, IEnumerable<T> collection) where T : IDbEntry
        {
            if (relationType == RelationType.AnyType)
                return true;

            bool isAttached = collection.Any(i => i.Id == item.Id);

            return relationType switch
            {
                RelationType.Attached => isAttached,
                RelationType.NotAttached => !isAttached,
                _ => false
            };
        }

        void OnMultipleSelectionChanged<T>(Wrap<T> wrap, List<T> collection, Action action) where T : class, IDbEntry
        {
            if (wrap.IsChecked)
                collection.Add(wrap.Value);
            else
                collection.RemoveAll(u => u.Id == wrap.Value.Id);
            action?.Invoke();
        }
    }
}
