using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;

namespace ILS_WPF.Models.Core.Resources
{
    public class Fuel : Resource
    {
        public FuelType Type { get; set; }

        protected Fuel() { }

        public Fuel(int id, string name, FuelType type, float quantity)
        {
            Id = id;
            Name = name;
            MaterialType = MaterialType.Fuel;
            Type = type;
            Quantity = quantity;
        }

        public override bool IsMatches(SupplyRequestDetail detail)
            => detail.MaterialType == MaterialType.Fuel &&
                   Type == detail.FuelType;
    }
}
