using ILS_WPF.Models.Core.Enums;

namespace ILS_WPF.Models.Core.Requests
{
    public class SupplyResponse
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public virtual SupplyRequest? Request { get; set; }
        public SupplyResponseStatus Status { get; set; }
        public string? Comment { get; set; }
    }
}
