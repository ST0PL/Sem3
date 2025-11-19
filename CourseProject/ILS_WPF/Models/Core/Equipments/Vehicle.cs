using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;

namespace ILS_WPF.Models.Core.Equipments
{
    public class Vehicle : Equipment
    {
        public VehicleType Type { get; set; }
        public FuelType FuelType { get; set; }

        protected Vehicle() { }

        public Vehicle(int id, string name, VehicleType type, FuelType fuelType, int count)
        {
            Id = id;
            Name = name;
            MaterialType = MaterialType.Vehicle;
            Type = type;
            FuelType = fuelType;
            Count = count;
        }

        public override bool IsMatches(SupplyRequestDetail detail)
            => detail.MaterialType == MaterialType.Vehicle &&
                   Type == detail.VehicleType && FuelType == detail.FuelType;
    }
}
