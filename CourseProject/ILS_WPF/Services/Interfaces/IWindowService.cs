using ILS_WPF.Models.Core;
using System.Windows.Input;

namespace ILS_WPF.Services.Interfaces
{
    public interface IWindowService
    {
        void OpenMainWindow();
        void CloseApplicationWindow();
        void OpenLoginWindow();
        void OpenMessageWindow(string title, string text);
        void OpenPersonnelRegisterWindow();
        void OpenPersonnelEditWindow(Staff soldier);
        void OpenUnitRegisterWindow();
        void OpenUnitEditWindow(Unit unit, bool IsAdmin);
        void OpenWarehouseRegisterWindow();
        void OpenWarehouseEditWindow(int warehouseId, ICommand navigateBackCommand);
        void OpenWarehouseEntryRegisterWindow(int warehouseId);
        void OpenWarehouseEntryEditWindow(IMaterial entry, int warehouseId);
        void OpenSupplyRequestWindow(int unitId);
        void OpenSupplyResponseWindow(int supplyResponseId);
    }
}
