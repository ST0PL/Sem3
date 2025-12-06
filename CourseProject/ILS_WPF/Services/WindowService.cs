using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;
using ILS_WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace ILS_WPF.Services
{
    class WindowService : IWindowService
    {
        private IServiceProvider _serviceProvider;

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
            => new Views.Personnel.AddWindow(new AddPersonnelVM(
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();

        public void OpenPersonnelEditWindow(Staff soldier, ICommand dataRefreshCommand)
            => new Views.Personnel.EditWindow(new EditPersonnelVM(soldier, _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();

        public void OpenMessageWindow(string title, string text)
            => new MessageWindow(title, text).Show();

        public void OpenUnitRegisterWindow(ICommand dataRefreshCommand)
            => new Views.Structures.AddWindow(new AddUnitVM(
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();

        public void OpenUnitEditWindow(Unit unit, ICommand dataRefreshCommand)
            => new Views.Structures.EditWindow(new EditUnitVM(unit,
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!, dataRefreshCommand)).ShowDialog();
    }
}
