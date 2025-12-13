using ILS_WPF.Models.Core.Enums;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class WarehouseEntryVM : BaseVM
    {
        private ICommand _notifyChangesCommand;
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

        public ICommand RemoveCommand { get; set; }

        public WarehouseEntryVM(ICommand notifyChangesCommand, ICommand removeCommand)
        {
            _notifyChangesCommand = notifyChangesCommand;
            RemoveCommand = removeCommand;
            Types = [.. Enum.GetValues<MaterialType>().SkipLast(1)];
            Calibers = Enum.GetValues<Caliber>();
            VehicleTypes = Enum.GetValues<VehicleType>();
            FuelTypes = Enum.GetValues<FuelType>();
            _selectedType = Types[0];
        }

        new void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            _notifyChangesCommand.Execute(null);
        }

        public float GetCount()
            => _count;

        void OnPropertiesChanged(params List<string> propertyNames)
            => propertyNames.ForEach(OnPropertyChanged);

    }
}
