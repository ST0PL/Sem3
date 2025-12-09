using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;

namespace ILS_WPF.Models.Core.Equipments
{
    public class Weapon : Equipment
    {
        public Caliber Caliber { get; set; }

        public Weapon() { } // Конструктор для EF

        public Weapon(string name, Caliber caliber, int count)
        {
            Name = name;
            MaterialType = MaterialType.Weapon;
            Caliber = caliber;
            Count = count;
        }

        public override bool IsMatches(SupplyRequestDetail detail)
            => detail.MaterialType == MaterialType.Weapon &&
                   Caliber == detail.Caliber;
    }
}
 