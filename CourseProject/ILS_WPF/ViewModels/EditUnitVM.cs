using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class EditUnitVM : BaseVM
    {
        private IViewModelUpdaterService _viewModelUpdaterService;
        private Unit _unit;
        private IDbContextFactory<ILSContext> _dbFactory;
        private IWindowService _windowService;
        private WarehouseType _selectedWarehouseType;
        private RelationType _selectedUnitRelationType;
        private RelationType _selectedStaffRelationType;
        private Speciality _selectedCommanderSpeciality;
        private Speciality _selectedSoldierSpeciality;
        private UnitType _selectedUnitType;
        private string? _name;
        private string _unitQuery;
        private string _warehouseQuery;
        private string _staffQuery;
        private string _commanderQuery;
        private Wrap<Warehouse>[] _warehouses;
        private Wrap<Unit>[] _units;
        private Wrap<Staff>[] _commanders;
        private Wrap<Staff>[] _personnel;
        private Warehouse? _selectedWarehouse;
        private List<Unit> _selectedUnits = [];
        private Staff? _selectedCommander;
        private List<Staff> _selectedPersonnel = [];

        public string? Name { get => _name; set { _name = value; OnPropertyChanged(); } }

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
                _selectedCommander = null;
                _selectedUnits = [];
                _selectedPersonnel = [];
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

        public bool HasWarehouses => Warehouses?.Length > 0;
        public bool HasUnits => Units?.Length > 0;
        public bool HasCommanders => Commanders?.Length > 0;
        public bool HasPersonnel => Personnel?.Length > 0;

        public ICommand WarehouseWrapCheckedCommand { get; set; }
        public ICommand UnitWrapCheckedCommand { get; set; }
        public ICommand CommanderWrapCheckedCommand { get; set; }
        public ICommand StaffWrapCheckedCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public EditUnitVM(Unit unit, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _unit = unit;
            _viewModelUpdaterService = viewUpdaterService;
            _dbFactory = dbFactory;
            _windowService = windowService;
            WarehouseTypes = Enum.GetValues<WarehouseType>().Order().ToArray();
            RelationTypes = Enum.GetValues<RelationType>().Order().ToArray();
            Specialities = Enum.GetValues<Speciality>().Order().ToArray();
            UnitTypes = Enum.GetValues<UnitType>().Order().Skip(1).ToArray();
            _selectedWarehouseType = WarehouseTypes[0];
            _selectedUnitRelationType = RelationTypes[0];
            _selectedStaffRelationType = RelationTypes[0];
            _selectedSoldierSpeciality = Specialities[0];
            _selectedCommanderSpeciality = Specialities[0];
            _selectedUnitType = UnitTypes[0];
            InitCommands();
            InitFormFields();
            _ = LoadData();
        }


        void InitFormFields()
        {
            _name = _unit.Name;
            _selectedUnitType = _unit.Type;
            _selectedWarehouse = _unit.AssignedWarehouse;
            _selectedUnits = _unit.Children;
            _selectedCommander = _unit.Commander;
            _selectedPersonnel = _unit.Personnel;
        }

        void InitCommands()
        {
            WarehouseWrapCheckedCommand = new RelayCommand(wrap => OnSingleSelectionChanged((wrap as Wrap<Warehouse>)!, Warehouses, w => _selectedWarehouse = w));
            UnitWrapCheckedCommand = new RelayCommand(wrap => OnMultipleSelectionChanged((wrap as Wrap<Unit>)!, _selectedUnits, () => _ = LoadUnits()));
            CommanderWrapCheckedCommand =
                new RelayCommand(wrap => OnSingleSelectionChanged((wrap as Wrap<Staff>)!, Commanders,
                cmd =>
                {
                    _selectedCommander = cmd;
                    _ = LoadCommanders();
                    _ = LoadPersonnel();

                }));
            StaffWrapCheckedCommand = new RelayCommand(wrap => OnMultipleSelectionChanged((wrap as Wrap<Staff>)!, _selectedPersonnel, () =>
            {
                _ = LoadPersonnel();
                _ = LoadCommanders();
            }));
            RegisterCommand = new RelayCommand(async _ => await SaveAsync(), _ => !string.IsNullOrWhiteSpace(Name));
            RemoveCommand = new RelayCommand(async _=> await RemoveAsync());
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
                (string.IsNullOrWhiteSpace(WarehouseQuery) || EF.Functions.Like(w.Name, $"%{WarehouseQuery}%")) &&
                (w.Id == _unit.AssignedWarehouseId || !context.Units.Any(u=>u.AssignedWarehouseId == w.Id)))
                .Select(w=>new Wrap<Warehouse>(w) { IsChecked = (_selectedWarehouse != null && _selectedWarehouse.Id == w.Id) })
                .OrderByDescending(w=>w.IsChecked)
                .ToArrayAsync();
            OnPropertyChanged(nameof(HasWarehouses));
        }

        async Task LoadUnits()
        {
            Units = [];
            using var context = await _dbFactory.CreateDbContextAsync();

            Units = (await context.Units
                .Include(u => u.Commander)
                .Where(u => (u.Type == (SelectedUnitType - 1)) && u.Id!=_unit.Id &&
                    (string.IsNullOrWhiteSpace(UnitQuery) || EF.Functions.Like(u.Name, $"%{UnitQuery}%") || (u.Commander != null && EF.Functions.Like(u.Commander.FullName, $"%{UnitQuery}%"))) &&
                    (u.ParentId == _unit.Id || u.ParentId == null))
                .ToArrayAsync())
                .Where(u => IsRelationMatches(u, SelectedUnitRelationType, _selectedUnits))
                .Select(u => new Wrap<Unit>(u) { IsChecked = _selectedUnits.Any(selected=>selected.Id == u.Id) })
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
                        (p.UnitId == _unit.Id || p.UnitId == null) &&
                        (_selectedCommander == null || p.Id != _selectedCommander.Id) &&
                        p.Rank <= UnitRankMatcher.MaxBattalionRank &&
                        (SelectedSoldierSpeciality == Speciality.AnySpeciality || p.Speciality == SelectedSoldierSpeciality) &&
                        (string.IsNullOrWhiteSpace(StaffQuery) || EF.Functions.Like(p.FullName, $"%{StaffQuery}%")))
                    .ToArrayAsync())
                    .Where(p =>
                        IsRelationMatches(p, SelectedStaffRelationType, _selectedPersonnel) &&
                        !context.Units.Any(u=>u.CommanderId == p.Id))
                    .Select(p => new Wrap<Staff>(p) { IsChecked = _selectedPersonnel.Any(selected => selected.Id == p.Id) })
                    .OrderByDescending(w=>w.IsChecked)
                    .ToArray();
            }
            OnPropertyChanged(nameof(HasPersonnel));
        }

        async Task LoadCommanders()
        {
            Commanders = [];
            using var context = await _dbFactory.CreateDbContextAsync();
            Commanders = (await context.Personnel
                .Where(c =>
                    (SelectedCommanderSpeciality == Speciality.AnySpeciality || c.Speciality == SelectedCommanderSpeciality) &&
                    (string.IsNullOrWhiteSpace(CommanderQuery) || EF.Functions.Like(c.FullName, $"%{CommanderQuery}%")) &&
                    !context.Units.Any(u=>u.Id != _unit.Id && u.CommanderId == c.Id) && c.UnitId == null)
                .ToArrayAsync())
                .Where(c => UnitRankMatcher.IsMatches(SelectedUnitType, c.Rank) && !_selectedPersonnel.Any(p => p.Id == c.Id))
                .Select(c =>new Wrap<Staff>(c) { IsChecked = _selectedCommander != null && _selectedCommander.Id == c.Id })
                .OrderByDescending(w=>w.IsChecked)
                .ToArray();

            OnPropertyChanged(nameof(HasCommanders));
        }

        async Task SaveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            var unitToUpdate = new Unit
            {
                Id = _unit.Id,
                Name = Name,
                Type = SelectedUnitType,
                CommanderId = _selectedCommander?.Id,
                AssignedWarehouseId = _selectedWarehouse?.Id
            };

            context.Update(unitToUpdate);

            // Сброс связей

            await context.Personnel
                .Where(p => p.UnitId == _unit.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.UnitId, (int?)null));
            await context.Units
                .Where(u => u.ParentId == _unit.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.ParentId, (int?)null));

            // Прикрепление выбранных подразделений
            await context.Units
                .Where(u => _selectedUnits.Select(selected => selected.Id).Contains(u.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.ParentId, _unit.Id));

            // Прикрепление выбранного персонала
            await context.Personnel
                .Where(p => _selectedPersonnel.Select(selected => selected.Id).Contains(p.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.UnitId, _unit.Id));

            await context.SaveChangesAsync();
            _viewModelUpdaterService.Update<StructuresVM>();
            _viewModelUpdaterService.Update<PersonnelVM>();
            _windowService.OpenMessageWindow("Изменение данных", "Данные о подразделении были успешно изменены.");
        }


        async Task RemoveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            context.Remove(_unit);
            await context.SaveChangesAsync();
            _viewModelUpdaterService.Update<StructuresVM>();
            _viewModelUpdaterService.Update<PersonnelVM>();
            _windowService.OpenMessageWindow("Удаление данных", "Данные о подразделении были успешно удалены.");
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
                collection.RemoveAll(i => i.Id == wrap.Value.Id);
            action?.Invoke();
        }
    }
}
