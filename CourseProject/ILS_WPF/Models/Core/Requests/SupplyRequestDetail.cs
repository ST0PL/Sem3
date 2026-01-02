using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.ViewModels;

namespace ILS_WPF.Models.Core.Requests
{
    public class SupplyRequestDetail : IDbEntry, ICloneable
    {
        public int Id { get; set; }
        public MaterialType MaterialType { get; set; }
        public float Count { get; set; }

        public Caliber? Caliber { get; set; }
        public FuelType? FuelType { get; set; }
        public VehicleType? VehicleType { get; set; }

        public int? SupplyRequestId { get; set; }
        public virtual SupplyRequest? SupplyRequest { get; set; }
        
        public int? SupplyResponseId { get; set; }
        public virtual SupplyResponse? SupplyResponse { get; set; }

        public SupplyRequestDetail(MaterialType materialType, float count)
        {
            MaterialType = materialType;
            Count = count;
        }

        public SupplyRequestDetail WithCaliber(Caliber caliber)
        {
            Caliber = caliber;
            return this;
        }

        public SupplyRequestDetail WithFuelType(FuelType fuelType)
        {
            FuelType = fuelType;
            return this;
        }

        public SupplyRequestDetail WithVehicleType(VehicleType vehicleType)
        {
            VehicleType = vehicleType;
            return this;
        }

        public object Clone()
        {
            return new SupplyRequestDetail(MaterialType, Count)
            {
                Caliber = Caliber,
                FuelType = FuelType,
                VehicleType = VehicleType
            };
        }

        public static explicit operator SupplyRequestDetail(WarehouseEntryVM entry)
        {
            SupplyRequestDetail result = new(entry.SelectedType, entry.GetCount());
            switch (result.MaterialType)
            {
                case MaterialType.Ammunition:
                case MaterialType.Weapon:
                    result.WithCaliber(entry.SelectedCaliber);
                    break;
                case MaterialType.Fuel:
                    result.WithFuelType(entry.SelectedFuelType);
                    break;
                case MaterialType.Vehicle:
                    result.WithFuelType(entry.SelectedFuelType)
                          .WithVehicleType(entry.SelectedVehicleType);
                    break;
            }
            return result;
        }
    }
}
