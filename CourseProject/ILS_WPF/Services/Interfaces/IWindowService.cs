using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ILS_WPF.Services.Interfaces
{
    public interface IWindowService
    {
        void OpenMainWindow();
        void CloseApplicationWindow();
        void OpenLoginWindow();
        void OpenPersonnelRegisterWindow(IDbContextFactory<ILSContext> dbFactory);
        void OpenPersonnelEditWindow(IDbContextFactory<ILSContext> dbFactory, Staff soldier);
    }
}
