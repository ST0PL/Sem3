using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Equipments;
using ILS_WPF.Models.Core.Resources;
using ILS_WPF.Models.Database;

namespace ILS_WPF.Models.Core
{
    public class Warehouse : IDbEntry
    {
        public int Id { get; set; }
        public WarehouseType Type { get; set; }
        public string? Name { get; set; }
        public virtual List<Resource> Resources { get; set; } = [];
        public virtual List<Equipment> Equipments { get; set; } = [];

        public Warehouse(int id, string name, WarehouseType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        protected Warehouse() { } // Для EF
    }
}
