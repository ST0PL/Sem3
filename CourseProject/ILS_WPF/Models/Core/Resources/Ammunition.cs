using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;

namespace ILS_WPF.Models.Core.Resources
{
    public class Ammunition : Resource
    {
        public Caliber Caliber { get; set; }

        protected Ammunition() { }

        public Ammunition(string name, Caliber caliber, int quantity)
        {
            Name = name;
            MaterialType = MaterialType.Ammunition;
            Caliber = caliber;
            Quantity = quantity;
        }

        public override bool IsMatches(SupplyRequestDetail detail)
            => detail.MaterialType == MaterialType.Ammunition &&
                   Caliber == detail.Caliber;
    }
}
