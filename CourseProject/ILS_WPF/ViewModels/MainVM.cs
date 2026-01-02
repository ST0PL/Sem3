using ILS_WPF.Models;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using ILS_WPF.Views.Accounts;
using ILS_WPF.Views.Main;
using ILS_WPF.Views.Personnel;
using ILS_WPF.Views.Structures;
using ILS_WPF.Views.SupplyResponses;
using ILS_WPF.Views.Warehouses;
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

        public bool IsAdmin { get; set; }

        public ICommand SetViewCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public MainVM(
            IViewModelUpdaterService viewUpdaterService,
            IWindowService windowService,
            IUserService userService,
            IAccountService accountService,
            IConfigurationService<Configuration> configurationService,
            IDbContextFactory<ILSContext> dbFactory)
        {
            IsAdmin = userService.GetUser()!.Role == Role.Administrator;
            _views =
                [
                    userService?.GetUser()?.Role < Role.Administrator ?
                        new MainViewCommander(new MainCommanderVM(viewUpdaterService, userService, windowService, dbFactory)) : new MainView(new StatVM(viewUpdaterService,dbFactory)),
                    new SupplyResponsesView(new SupplyResponsesVM(viewUpdaterService, userService, windowService, dbFactory)),
                    new WarehousesView(new WarehousesVM(viewUpdaterService, userService, windowService, dbFactory)),
                    new StructuresView(new StructuresVM(viewUpdaterService, userService, windowService, dbFactory)),
                    new PersonnelView(new PersonnelVM(viewUpdaterService,userService, windowService, dbFactory)),
                    new AccountsView(new AccountsVM(viewUpdaterService, windowService, dbFactory))
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
                windowService.OpenLoginWindow();
            });
        }
    }
}
