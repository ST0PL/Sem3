using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ILS_WPF.Services.Interfaces
{
    public interface ISupplyService
    {
        Task MakeSupplyRequestAsync(IDbContextFactory<ILSContext> dbFactory, SupplyRequest request);
    }
}
