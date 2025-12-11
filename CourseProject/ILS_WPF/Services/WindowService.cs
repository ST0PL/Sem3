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


        private void OpenServiceWindow<T>() where T : Window
        {
            Application.Current.MainWindow = _serviceProvider.GetService<T>();
            Application.Current.MainWindow?.Show();
        }

        public void OpenLoginWindow()
            => OpenServiceWindow<LoginWindow>();

        public void OpenMainWindow()
            => OpenServiceWindow<MainWindow>();

        public void CloseApplicationWindow()
            => Application.Current.MainWindow?.Close();
            
        public void OpenPersonnelRegisterWindow()
            => new Views.Personnel.AddWindow(new AddPersonnelVM(
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenPersonnelEditWindow(Staff soldier)
            => new Views.Personnel.EditWindow(new EditPersonnelVM(soldier, _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenMessageWindow(string title, string text)
            => new MessageWindow(title, text).Show();

        public void OpenUnitRegisterWindow()
            => new Views.Structures.AddWindow(new AddUnitVM(
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenUnitEditWindow(Unit unit)
            => new Views.Structures.EditWindow(new EditUnitVM(unit,
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenWarehouseRegisterWindow()
            => new Views.Warehouses.AddWindow(new AddWarehouseVM(
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenWarehouseEditWindow(int warehouseId, ICommand navigateBackCommand)
            => new Views.Warehouses.EditWindow(new EditWarehouseVM(warehouseId,
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!,
                navigateBackCommand)).ShowDialog();

        public void OpenWarehouseEntryRegisterWindow(int warehouseId)
            => new Views.Warehouses.AddEntriesWindow(new AddWarehouseEntriesVM(
                warehouseId,
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenWarehouseEntryEditWindow(IMaterial entry, int warehouseId)
            => new Views.Warehouses.EditEntryWindow(new EditWarehouseEntryVM(
                entry,
                warehouseId,
                _serviceProvider.GetService<IViewModelUpdaterService>(),
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenSupplyRequestWindow()
        {
            throw new NotImplementedException();
        }
    }
}
