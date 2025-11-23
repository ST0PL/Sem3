using ILS_WPF.Models.Core;
using System.Windows.Input;

namespace ILS_WPF.Services.Interfaces
{
    public interface IWindowService
    {
        void OpenMainWindow();
        void CloseApplicationWindow();
        void OpenLoginWindow();
        void OpenPersonnelRegisterWindow(ICommand dataRefreshCommand);
        void OpenPersonnelEditWindow(Staff soldier, ICommand dataRefreshCommand);
        void OpenMessageWindow(string title, string text);
    }
}
