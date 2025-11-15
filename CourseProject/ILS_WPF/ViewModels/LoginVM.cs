using ILS_WPF.Services.Interfaces;

namespace ILS_WPF.ViewModels
{
    internal class LoginVM : BaseVM
    {
        public event EventHandler<bool>? LoginSuccessed;

        private readonly IUserService? _userService;
        private readonly IAccountService? _accountService;

        public LoginVM(IUserService userService, IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;
        }

    }
}
