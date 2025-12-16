using ILS_WPF.Models;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class LoginVM : BaseVM
    {
        private readonly IWindowService _windowService;
        private readonly IConfigurationService<Configuration?> _configurationService;
        private readonly IUserService? _userService;
        private readonly IAccountService? _accountService;

        private string _userName;
        private string _password;
        private bool _isPasswordCorrect = true;

        public string Username
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public bool IsPasswordCorrect
        {
            get => _isPasswordCorrect;
            set { _isPasswordCorrect = value; OnPropertyChanged(); }
        }

        public bool RememberMe { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginVM(
            IWindowService windowService,
            IConfigurationService<Configuration?> configurationService,
            IUserService userService,
            IAccountService accountService)
        {
            _windowService = windowService;
            _configurationService = configurationService;
            _userService = userService;
            _accountService = accountService;
            LoginCommand = new RelayCommand(
                async _ => await PerformLogin(),
                _ => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));
            _ = CheckRememberMe();
            _ = accountService.CreateDefaultUserIfNotExist();
        }

        private async Task CheckRememberMe()
        {
            if (_configurationService?.Configuration?.RememberMe ?? false)
            {
                if (string.IsNullOrWhiteSpace(_configurationService.Configuration.Username) ||
                    string.IsNullOrWhiteSpace(_configurationService.Configuration.Hash))
                    return;

                var user = await _accountService!.LoginWithHashAsync(
                    _configurationService.Configuration.Username ?? "",
                    _configurationService.Configuration.Hash ?? "");

                

                if (user != null)
                {
                    _userService!.SetUser(user);
                    await _windowService.OpenMainWindow(true);

                }
            }
        }
        private async Task PerformLogin()
        {
            var user = await _accountService!.LoginAsync(Username!, Password!);

            IsPasswordCorrect = user != null;

            if (user != null && _configurationService.Configuration != null)
            {
                if (RememberMe)
                {
                    _configurationService.Configuration.Username = user.Username;
                    _configurationService.Configuration.Hash = user.Hash;
                }
                _configurationService.Configuration.RememberMe = RememberMe;
                await _configurationService.SaveAsync();
                _userService!.SetUser(user);
                await _windowService!.OpenMainWindow(false);
            }
        }

    }
}
