using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;

namespace ILS_WPF.Models.Core.Requests
{
    public class SupplyResponse : IDbEntry
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public virtual SupplyRequest? Request { get; set; }
        public SupplyResponseStatus Status { get; set; }
        public string? Comment { get; set; }
    }
}
