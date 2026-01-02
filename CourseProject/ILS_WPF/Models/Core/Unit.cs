using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Database;

namespace ILS_WPF.Models.Core
{
    public class Unit : IDbEntry
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public UnitType Type { get; set; }
        public int? CommanderId { get; set; }
        public virtual Staff? Commander { get; set; }
        public int? ParentId { get; set; }
        public virtual Unit? Parent { get; set; }
        public virtual List<Unit> Children { get; set; } = [];
        public virtual List<Staff> Personnel { get; set; } = [];
        public int? AssignedWarehouseId { get; set; }
        public virtual Warehouse? AssignedWarehouse { get; set; }
        public virtual List<SupplyRequest> SupplyRequests { get; set; } = [];
    }
}
