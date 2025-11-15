using ILS_WPF.Models.Database;
using ILS_WPF.Services;
using ILS_WPF.Services.Interfaces;
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
                    s.AddDbContext<ILSContext>();
                    s.AddTransient<IAccountService, AccountService>();
                    s.AddSingleton<IUserService, UserService>();
                    s.AddTransient<LoginWindow>();
                    s.AddSingleton<MainWindow>();
                })
                .Build();
            return _host;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitHost();
            var loginWindow = _host?.Services?.GetService<LoginWindow>();
            if (loginWindow?.ShowDialog() ?? false)
            {
                MainWindow = _host?.Services?.GetService<MainWindow>();
                MainWindow?.Show();
            }
        }
    }

}
