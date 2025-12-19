using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Equipments;
using ILS_WPF.Models.Core.Resources;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class AddWarehouseEntriesVM : BaseVM
    {
        private IViewModelUpdaterService _updaterService;
        private IWindowService _windowService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private int _warehouseId;
        private string _warehouseName;
        private bool _canRegister;

        public ObservableCollection<WarehouseEntryVM> WarehouseEntries { get; set; } = [];

        public string WarehouseName
        {
            get => _warehouseName;
            set
            {
                _warehouseName = value;
                OnPropertyChanged();
            }
        }

        public bool HasItems => WarehouseEntries.Count > 0;

        public bool CanRegister
        {
            get => _canRegister;
            set
            {
                _canRegister = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand { get; set; }
        public ICommand AddEntryCommand { get; set; }
        public ICommand RemoveEntryCommand { get; set; }

        public AddWarehouseEntriesVM(int warehouseId, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _warehouseId = warehouseId;
            _updaterService = viewUpdaterService;
            _windowService = windowService;
            _dbFactory = dbFactory;
            var _notifyChangesCommand = new RelayCommand(_ => ChangeCanRegister());
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(), _ => CanRegister);
            RemoveEntryCommand = new RelayCommand(entry => WarehouseEntries.Remove((WarehouseEntryVM)entry), _ => HasItems);
            AddEntryCommand = new RelayCommand(_ => WarehouseEntries.Add(new WarehouseEntryVM(_notifyChangesCommand, RemoveEntryCommand)));
            _ = Init();
        }
        async Task Init()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            WarehouseName = (await context.Warehouses.Where(w => w.Id == _warehouseId).FirstAsync()).Name ?? "";
            WarehouseEntries.CollectionChanged += (_, _) => { OnPropertyChanged(nameof(HasItems)); ChangeCanRegister(); };
        }

        async Task RegisterAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            IEnumerable<Resource> resources = WarehouseEntries
                .Where(e => e.SelectedType is MaterialType.Ammunition or MaterialType.Fuel)
                .Select(e =>
                {
                    return (Resource)(e.SelectedType switch
                    {
                        MaterialType.Ammunition => new Ammunition(e.Name, e.SelectedCaliber, (int)e.GetCount()) { WarehouseId = _warehouseId, MeasureUnit = MeasureUnit.Item },
                        MaterialType.Fuel => new Fuel(e.Name, e.SelectedFuelType, e.GetCount()) { WarehouseId = _warehouseId, MeasureUnit = MeasureUnit.Liter },
                        _ => throw new NullReferenceException(),
                    });
                });

            IEnumerable<Equipment> equipment = WarehouseEntries
                .Where(e => e.SelectedType is MaterialType.Vehicle or MaterialType.Weapon)
                .Select(e =>
                {
                    return (Equipment)(e.SelectedType switch
                    {
                        MaterialType.Vehicle => new Vehicle(e.Name, e.SelectedVehicleType, e.SelectedFuelType, (int)e.GetCount()) { WarehouseId = _warehouseId, },
                        MaterialType.Weapon => new Weapon(e.Name, e.SelectedCaliber, (int)e.GetCount()) { WarehouseId = _warehouseId },
                        _ => throw new NullReferenceException(),
                    });
                });

            await context.Resources.AddRangeAsync(resources);
            await context.Equipment.AddRangeAsync(equipment);

            await context.SaveChangesAsync();

            _updaterService.Update<CurrentWarehouseVM>();
            _windowService.OpenMessageWindow("Регистрация данных", "Данные о МТО были успешно зарегистрированы.");
        }

        void ChangeCanRegister()
            => CanRegister = WarehouseEntries.All(e =>
                !string.IsNullOrWhiteSpace(e.Name) &&
                e.GetCount() > 0);
    }
}
