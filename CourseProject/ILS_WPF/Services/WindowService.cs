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
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenPersonnelEditWindow(Staff soldier)
            => new Views.Personnel.EditWindow(new EditPersonnelVM(soldier, _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenMessageWindow(string title, string text)
            => new MessageWindow(title, text).Show();

        public void OpenUnitRegisterWindow()
            => new Views.Structures.AddWindow(new AddUnitVM(
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenUnitEditWindow(Unit unit, bool IsAdmin)
            => new Views.Structures.EditWindow(new EditUnitVM(unit,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!,
                IsAdmin)).ShowDialog();

        public void OpenWarehouseRegisterWindow()
            => new Views.Warehouses.AddWindow(new AddWarehouseVM(
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                _serviceProvider.GetService<IUserService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenWarehouseEditWindow(int warehouseId, ICommand navigateBackCommand)
            => new Views.Warehouses.EditWindow(new EditWarehouseVM(warehouseId,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!,
                navigateBackCommand)).ShowDialog();

        public void OpenWarehouseEntryRegisterWindow(int warehouseId)
            => new Views.Warehouses.AddEntriesWindow(new AddWarehouseEntriesVM(
                warehouseId,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenWarehouseEntryEditWindow(IMaterial entry, int warehouseId)
            => new Views.Warehouses.EditEntryWindow(new EditWarehouseEntryVM(
                entry,
                warehouseId,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenSupplyRequestWindow(int unitId)
            => new Views.Main.SupplyRequestWindow(new SupplyRequestVM(
                unitId,
                _serviceProvider.GetService<ISupplyService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();
        public void OpenSupplyResponseWindow(int supplyResponseId)
            => new Views.SupplyResponses.SupplyResponseWindow(new SupplyResponseVM(
                supplyResponseId,
                _serviceProvider.GetService<ISupplyService>()!,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IUserService>()!,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenAccountRegisterWindow()
            => new Views.Accounts.AddWindow(new AddAccountVM(
                _serviceProvider.GetService<IAccountService>()!,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();

        public void OpenAccountEditWindow(User account)
            => new Views.Accounts.EditWindow(new EditAccountVM(account,
                _serviceProvider.GetService<IAccountService>()!,
                _serviceProvider.GetService<IViewModelUpdaterService>()!,
                this,
                _serviceProvider.GetService<IDbContextFactory<ILSContext>>()!)).ShowDialog();
    }
}
