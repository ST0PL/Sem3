using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Equipments;
using ILS_WPF.Models.Core.Resources;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class CurrentWarehouseVM : BaseVM
    {
        private IDbContextFactory<ILSContext> _dbFactory;
        private int _warehouseId;
        private string? _name;
        private object[] _items;
        private string _query;
        private MaterialType _selectedMaterialType;

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public object[] Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                _ = LoadData();
            }
        }
        public MaterialType[] MaterialTypes { get; set; }
        public MaterialType SelectedMaterialType
        {
            get => _selectedMaterialType;
            set
            {
                _selectedMaterialType = value;
                _ = LoadData();
            }
        }

        public bool HasItems => _items?.Length > 0;

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand OpenEditWindowCommand { get; set; }
        public ICommand OpenEditEntryWindowCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }

        public CurrentWarehouseVM(
            int warehouseId,
            IViewModelUpdaterService viewUpdaterService,
            IWindowService windowService,
            IDbContextFactory<ILSContext> dbFactory,
            ICommand navigateBackCommand,
            bool isAdmin)
        {
            _warehouseId = warehouseId;
            _dbFactory = dbFactory;
            MaterialTypes = [.. Enum.GetValues<MaterialType>().Order()];
            _selectedMaterialType = MaterialTypes[0];
            RefreshCommand = new RelayCommand(async _ => await LoadData()); //
            OpenRegisterWindowCommand = new RelayCommand(_ => windowService.OpenWarehouseEntryRegisterWindow(warehouseId), _ => isAdmin);
            OpenEditWindowCommand = new RelayCommand(warehouse => windowService.OpenWarehouseEditWindow(_warehouseId, navigateBackCommand), _=> isAdmin);
            OpenEditEntryWindowCommand = new RelayCommand(entry =>
            {
                if (isAdmin)
                    windowService.OpenWarehouseEntryEditWindow((entry as IMaterial)!, _warehouseId);
            });
            NavigateBackCommand = navigateBackCommand;
            viewUpdaterService.SetUpdateCommand<CurrentWarehouseVM>(RefreshCommand);
            _ = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var warehouse = await context.Warehouses
                .Include(w=>w.Resources)
                .Include(w=>w.Equipments)
                .Where(w=>w.Id == _warehouseId)
                .FirstOrDefaultAsync();

            if (warehouse == null)
            {
                NavigateBackCommand.Execute(null);
                return;
            }

            Name = warehouse.Name;
            Items = [.. warehouse.Resources.Where(IsResourceMatches), .. warehouse.Equipments.Where(IsEquipmentMatches)];
            OnPropertyChanged(nameof(HasItems));
        }

        bool IsResourceMatches(Resource resource)
        {
            if (SelectedMaterialType != MaterialType.AnyType && resource.MaterialType != SelectedMaterialType)
                return false;

            if (string.IsNullOrWhiteSpace(Query) || (resource.Name?.Contains(Query, StringComparison.OrdinalIgnoreCase) ?? false))
                return true;

            return resource switch
            {
                Ammunition ammunition => IsLocaleContainsText(ammunition.Caliber.ToString(), Query),
                Fuel fuel => IsLocaleContainsText(fuel.Type.ToString(), Query),
                _ => false
            };
        }

        bool IsEquipmentMatches(Equipment equipment)
        {
            if (SelectedMaterialType != MaterialType.AnyType && equipment.MaterialType != SelectedMaterialType)
                return false;

            if (string.IsNullOrWhiteSpace(Query) || (equipment.Name?.Contains(Query, StringComparison.OrdinalIgnoreCase) ?? false))
                return true;

            return equipment switch
            {
                Vehicle vehicle => IsLocaleContainsText(vehicle.FuelType.ToString(), Query) ||
                                   IsLocaleContainsText(vehicle.Type.ToString(), Query),
                Weapon weapon => IsLocaleContainsText(weapon.Caliber.ToString(), Query),
                _=> false
            };
        }

        bool IsLocaleContainsText(string? value, string text)
        {
            if (value is null)
                return false;
            return GetLocale(value).Contains(text, StringComparison.OrdinalIgnoreCase);
        }

        string GetLocale(string key)
            => (string)Application.Current.Resources[key];
    }
}
