using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Equipments;
using ILS_WPF.Models.Core.Resources;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class EditWarehouseEntryVM : BaseVM
    {
        private IMaterial _entry;
        private int _warehouseId;
        private IViewModelUpdaterService _updaterService;
        private IWindowService _windowService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private MaterialType _selectedType;
        private Caliber _selectedCaliber;
        private VehicleType _selectedVehicleType;
        private FuelType _selectedFuelType;
        private float _count;
        private string _textCount;
        private string _name;

        public MaterialType[] Types { get; set; }
        public Caliber[] Calibers { get; set; }
        public VehicleType[] VehicleTypes { get; set; }
        public FuelType[] FuelTypes { get; set; }

        public MaterialType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertiesChanged(
                    nameof(IsCaliberTypeEnabled),
                    nameof(IsVehicleTypeEnabled),
                    nameof(IsFuelTypeEnabled));
            }
        }

        public Caliber SelectedCaliber
        {
            get => _selectedCaliber;
            set
            {
                _selectedCaliber = value;
                OnPropertyChanged();
            }
        }


        public VehicleType SelectedVehicleType
        {
            get => _selectedVehicleType;
            set
            {
                _selectedVehicleType = value;
                OnPropertyChanged();
            }
        }

        public FuelType SelectedFuelType
        {
            get => _selectedFuelType;
            set
            {
                _selectedFuelType = value;
                OnPropertyChanged();
            }
        }

        public string Count
        {
            get => _textCount;
            set
            {
                if (Regex.IsMatch(value, "^[0-9]+(\\.[0-9]*)?$"))
                {
                    _textCount = value;
                    _count = float.Parse(value, CultureInfo.InvariantCulture);
                }

                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public bool IsCaliberTypeEnabled => SelectedType == MaterialType.Weapon || SelectedType == MaterialType.Ammunition;
        public bool IsVehicleTypeEnabled => SelectedType == MaterialType.Vehicle;
        public bool IsFuelTypeEnabled => SelectedType == MaterialType.Fuel || SelectedType == MaterialType.Vehicle;

        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public EditWarehouseEntryVM(IMaterial entry, int warehouseId, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _entry = entry;
            _warehouseId = warehouseId;
            _updaterService = viewUpdaterService;
            _windowService = windowService;
            _dbFactory = dbFactory;
            Types = [.. Enum.GetValues<MaterialType>().SkipLast(1)];
            Calibers = Enum.GetValues<Caliber>();
            VehicleTypes = Enum.GetValues<VehicleType>();
            FuelTypes = Enum.GetValues<FuelType>();
            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => !string.IsNullOrWhiteSpace(Name) && _count > 0);
            RemoveCommand = new RelayCommand(async _ => await RemoveAsync());
            InitFormFields();
        }

        void InitFormFields()
        {
            _selectedType = _entry.MaterialType;
            _name = _entry.Name;

            switch (_entry)
            {
                case Ammunition ammo:
                    _selectedCaliber = ammo.Caliber;
                    Count = ammo.Quantity.ToString(CultureInfo.InvariantCulture);
                    break;
                case Fuel fuel:
                    _selectedFuelType = fuel.Type;
                    Count = fuel.Quantity.ToString(CultureInfo.InvariantCulture);
                    break;
                case Vehicle vehicle:
                    _selectedVehicleType = vehicle.Type;
                    _selectedFuelType = vehicle.FuelType;
                    Count = vehicle.Count.ToString(CultureInfo.InvariantCulture);
                    break;
                case Weapon weapon:
                    _selectedCaliber = weapon.Caliber;
                    Count = weapon.Count.ToString(CultureInfo.InvariantCulture);
                    break;
            }
        }

        async Task SaveAsync()
        {
            int? id = (_entry as IDbEntry)?.Id;
            if (id == null)
                return;

            using var context = await _dbFactory.CreateDbContextAsync();

            // Если тип не изменился, меняем существующую запись
            if(_entry.MaterialType == _selectedType)
            {
                if(_entry.MaterialType is MaterialType.Ammunition or MaterialType.Fuel)
                {
                    var resource = await context.Resources.Where(r => r.Id == id).FirstAsync();

                    // Установка общих свойств
                    resource.Name = _name;
                    resource.Quantity = _count;
                    
                    // Установка конкретных свойств
                    switch (resource)
                    {
                        case Ammunition ammo:
                            ammo.Caliber = _selectedCaliber;
                            break;
                        case Fuel fuel:
                            fuel.Type = _selectedFuelType;
                            break;
                    }
                }

                else if(_entry.MaterialType is MaterialType.Vehicle or MaterialType.Weapon)
                {
                    var equipment = await context.Equipment.Where(r => r.Id == id).FirstAsync();

                    // Установка общих свойств
                    equipment.Name = _name;
                    equipment.Count = (int)_count;

                    // Установка конкретных свойств
                    switch (equipment)
                    {
                        case Vehicle vehicle:
                            vehicle.Type = _selectedVehicleType;
                            vehicle.FuelType = _selectedFuelType;
                            break;
                        case Weapon weapon:
                            weapon.Caliber = _selectedCaliber;
                            break;
                    }
                }

            }

            // Иначе перемещаем между таблицами
            else
            {
                // В зависимости от первоначального типа, удаление старой записи происходит из разных таблиц
                switch (_entry.MaterialType)
                {
                    case MaterialType.Ammunition or MaterialType.Fuel:
                        await context.Resources.Where(r => r.Id == id).ExecuteDeleteAsync();
                        break;
                    case MaterialType.Vehicle or MaterialType.Weapon:
                        await context.Equipment.Where(r => r.Id == id).ExecuteDeleteAsync();
                        break;
                }

                if (_selectedType is MaterialType.Ammunition or MaterialType.Fuel)
                {
                    Resource resourceToAdd = _selectedType switch
                    {
                        MaterialType.Ammunition => new Ammunition(_name, _selectedCaliber, (int)_count),
                        MaterialType.Fuel => new Fuel(_name, _selectedFuelType, _count)
                    };
                    resourceToAdd.WarehouseId = _warehouseId;
                    await context.Resources.AddAsync(resourceToAdd);
                }
                else if (_selectedType is MaterialType.Vehicle or MaterialType.Weapon)
                {
                    Equipment equipmentToAdd = _selectedType switch
                    {
                        MaterialType.Vehicle => new Vehicle(_name, _selectedVehicleType, _selectedFuelType, (int)_count),
                        MaterialType.Weapon => new Weapon(_name, _selectedCaliber, (int)_count)
                    };
                    equipmentToAdd.WarehouseId = _warehouseId;
                    await context.Equipment.AddAsync(equipmentToAdd);
                }
            }

            
            await context.SaveChangesAsync();
            _updaterService.Update<CurrentWarehouseVM>();
            _windowService.OpenMessageWindow("Изменение данных", "Данные о МТО были успешно изменены.");
        }

        async Task RemoveAsync()
        {
            int? id = (_entry as IDbEntry)?.Id;
            if (id == null)
                return;

            using var context = await _dbFactory.CreateDbContextAsync();

            switch (_entry.MaterialType)
            {
                case MaterialType.Ammunition:
                case MaterialType.Fuel:
                    await context.Resources.Where(r => r.Id == id).ExecuteDeleteAsync();
                    break;
                case MaterialType.Vehicle:
                case MaterialType.Weapon:
                    await context.Equipment.Where(r => r.Id == id).ExecuteDeleteAsync();
                    break;
            }
            await context.SaveChangesAsync();
            _updaterService.Update<CurrentWarehouseVM>();
            _windowService.OpenMessageWindow("Удаление данных", "Данные о МТО были успешно удалены.");
        }

        void OnPropertiesChanged(params List<string> propertyNames)
            => propertyNames.ForEach(OnPropertyChanged);
    }
}
