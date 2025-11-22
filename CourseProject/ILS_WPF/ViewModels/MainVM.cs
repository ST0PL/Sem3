using ILS_WPF.Models;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using ILS_WPF.Views.Main;
using ILS_WPF.Views.Personnel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class MainVM : BaseVM
    {
        private readonly UserControl[] _views;
        private UserControl? _currentView;

        public UserControl? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
        public ICommand SetViewCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public MainVM(
            IWindowService windowService,
            IUserService userService,
            IAccountService accountService,
            IConfigurationService<Configuration> configurationService,
            IDbContextFactory<ILSContext> dbFactory)
        {
            _views =
                [
                    userService?.GetUser()?.Role < Role.Administator ?
                        new MainViewCommander() : new MainView(new StatVM(dbFactory)),
                    null,
                    null,
                    null,
                    new PersonnelView(new PersonnelVM(windowService, dbFactory))
                ];
            _currentView = _views[0];
            SetViewCommand = new RelayCommand(arg =>
            {
                if(int.TryParse(arg as string, out var index))
                    CurrentView = _views[index];
            });
            LogoutCommand = new RelayCommand(async _=>
            {
                configurationService.Reset();
                await configurationService.SaveAsync();
                windowService.CloseApplicationWindow();
                windowService.OpenLoginWindow();
            });
        }
    }
}
