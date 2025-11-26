using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    class WindowService : IWindowService
    {
        public IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public void OpenLoginWindow()
        {
            Application.Current.MainWindow = _serviceProvider.GetService<LoginWindow>();
            Application.Current.MainWindow?.Show();
        }

        public void OpenMainWindow()
        {
            Application.Current.MainWindow = _serviceProvider.GetService<MainWindow>();
            Application.Current.MainWindow?.Show();
        }
        public void CloseApplicationWindow()
            => Application.Current.MainWindow?.Close();

        public void OpenPersonnelRegisterWindow(ICommand dataRefreshCommand)
            => new Views.Personnel.AddWindow(new AddPersonnelVM(this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();

        public void OpenPersonnelEditWindow(Staff soldier, ICommand dataRefreshCommand)
            => new Views.Personnel.EditWindow(new EditPersonnelVM(soldier, this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();

        public void OpenMessageWindow(string title, string text)
            => new MessageWindow(title, text).Show();

        public void OpenUnitRegisterWindow(ICommand dataRefreshCommand)
            => new Views.Structures.AddWindow(new AddUnitVM(this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();

        public void OpenUnitEditWindow(Unit unit, ICommand dataRefreshCommand)
        {
            throw new NotImplementedException();
        }
    }
}
