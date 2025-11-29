using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Database;

namespace ILS_WPF.Models.Core
{
    public abstract class WarehouseEntry<T> : IDbEntry
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public MaterialType MaterialType { get; set; }
        public int? WarehouseId { get; set; }
        public virtual Warehouse? Warehouse { get; set; }

        public abstract bool IsEmpty();
        public abstract void Increase(T amount);
        public abstract T Decrease(T amount);
        public abstract bool IsMatches(SupplyRequestDetail detail);
    }
}
