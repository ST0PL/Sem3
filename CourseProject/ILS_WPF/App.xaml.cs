using ILS_WPF.Models;
using ILS_WPF.Models.Database;
using ILS_WPF.Services;
using ILS_WPF.Services.Interfaces;
using ILS_WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace ILS_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;
        private IHost InitHost()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(s =>
                {
                    s.AddDbContextFactory<ILSContext>();
                    s.AddSingleton<IConfigurationService<Configuration?>, ConfigurationService>(_=>new ConfigurationService("config.json"));
                    s.AddSingleton<IUserService, UserService>();
                    s.AddSingleton<IWindowService, WindowService>();
                    s.AddSingleton<IViewModelUpdaterService, ViewModelUpdaterService>();
                    s.AddTransient<ISupplyService, SupplyService>();
                    s.AddTransient<IAccountService, AccountService>();
                    s.AddTransient<LoginVM>();
                    s.AddTransient<LoginWindow>();
                    s.AddTransient<MainVM>();
                    s.AddTransient<MainWindow>();
                })
                .Build();
            return _host;
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            InitHost();
            var configService = _host?.Services?.GetService<IConfigurationService<Configuration?>>();
            if (configService != null)
                await configService.LoadAsync();

            _host?.Services.GetService<IWindowService>()?.OpenLoginWindow();
        }
    }

}
