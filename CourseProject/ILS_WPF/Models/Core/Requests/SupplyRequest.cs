namespace ILS_WPF.Models.Core.Requests
{
    public class SupplyRequest
    {
        public int Id { get; set; }
        public int RequestUnitId { get; set; }
        public Unit? RequestUnit { get; set; }
        public List<SupplyRequestDetail> Details { get; set; } = [];
        public DateTime CreationTime { get; set; }
    }
}
