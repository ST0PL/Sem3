using ILS_WPF.Models.Core.Requests;

namespace ILS_WPF
{
    public class SupplyResponseWrap
    {
        public SupplyResponse SupplyResponse { get; }
        public string Title { get; }

        public SupplyResponseWrap(SupplyResponse supplyResponse)
        {
            SupplyResponse = supplyResponse;
            Title = $"Заявка №{supplyResponse.RequestId} от {supplyResponse.Request?.RequestUnit?.Name}"; 
        }
    }
}
