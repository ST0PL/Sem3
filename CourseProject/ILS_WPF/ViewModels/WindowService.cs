using ILS_WPF.Models.Core;
using ILS_WPF.Models.Database;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

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

        public void OpenPersonnelRegisterWindow(IDbContextFactory<ILSContext> dbFactory)
            => new Views.Personnel.AddWindow(new PersonnelWindowVM(dbFactory));
        public void OpenPersonnelEditWindow(IDbContextFactory<ILSContext> dbFactory, Staff soldier)
            => new Views.Personnel.AddWindow(new PersonnelWindowVM(dbFactory));
    }
}
