using ILS_WPF.Services.Interfaces;
using System.Windows.Input;

namespace ILS_WPF.Services
{
    internal class ViewModelUpdaterService : IViewModelUpdaterService
    {
        private Dictionary<Type, ICommand> _updateComands = new();

        public void SetUpdateCommand<T>(ICommand updateCommand)
            => _updateComands[typeof(T)] = updateCommand;

        public void Update<T>()
            => _updateComands[typeof(T)].Execute(null);
    }
}
