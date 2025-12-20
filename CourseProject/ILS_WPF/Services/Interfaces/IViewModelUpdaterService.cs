using System.Windows.Input;

namespace ILS_WPF.Services.Interfaces
{
    public interface IViewModelUpdaterService
    {
        public void Update<T>();
        public void SetUpdateCommand<T>(ICommand updateCommand);
    }
}
