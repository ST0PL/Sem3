using ILS_WPF.Services.Interfaces;
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
    }
}
