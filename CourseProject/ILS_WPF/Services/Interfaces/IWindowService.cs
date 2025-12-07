using ILS_WPF.Models.Core;
using System.Windows.Input;

namespace ILS_WPF.Services.Interfaces
{
    public interface IWindowService
    {
        void OpenMainWindow();
        void CloseApplicationWindow();
        void OpenLoginWindow();
        void OpenPersonnelRegisterWindow();
        void OpenPersonnelEditWindow(Staff soldier);
        void OpenUnitRegisterWindow();
        void OpenUnitEditWindow(Unit unit);
        void OpenWarehouseRegisterWindow();
        void OpenWarehouseEditWindow(int warehouseId, ICommand navigateBackCommand);
        void OpenWarehouseEntryRegisterWindow();
        void OpenWarehouseEntryEditWindow(object entry);
        void OpenMessageWindow(string title, string text);
    }
}
