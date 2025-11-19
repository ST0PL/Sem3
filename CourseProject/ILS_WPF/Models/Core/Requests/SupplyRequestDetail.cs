using ILS_WPF.Models.Core.Enums;

namespace ILS_WPF.Models.Core.Requests
{
    public class SupplyRequestDetail
    {
        public int Id { get; set; }
        public MaterialType MaterialType { get; set; }
        public float Count { get; set; }

        public Caliber? Caliber { get; set; }
        public FuelType? FuelType { get; set; }
        public VehicleType? VehicleType { get; set; }

        public int SupplyRequestId { get; set; }
        public virtual SupplyRequest? SupplyRequest { get; set; }

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
    }
}
