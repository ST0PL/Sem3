using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class AddAccountVM : BaseVM
    {
        private IAccountService _accountService;
        private IViewModelUpdaterService _viewModelUpdaterService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private IWindowService _windowService;
        private string _username;
        private string _password;
        private Role _currentRole;
        private Rank _currentRank;
        private Speciality _currentSpeciality;
        private string _query;
        private Wrap<Staff>[] _personnel;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public Role CurrentRole
        {
            get => _currentRole;
            set
            {
                _currentRole = value;
                SelectedSoldier = null;
                _ = LoadPersonnel();
            }
        }

        public Rank CurrentRank
        {
            get => _currentRank;
            set
            {
                _currentRank = value;
                _ = LoadPersonnel();
            }
        }

        public Speciality CurrentSpeciality
        {
            get => _currentSpeciality;
            set
            {
                _currentSpeciality = value;
                _ = LoadPersonnel();
            }
        }

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                _ = LoadPersonnel();
            }
        }
        public Wrap<Staff>[] Personnel
        {
            get => _personnel;
            set 
            {
                _personnel = value;
                OnPropertyChanged();
            }
        }
        public bool HasItems => Personnel?.Length > 0;

        public Staff? SelectedSoldier { get; set; }

        public Role[] Roles { get; set; }
        public Rank[] Ranks { get; set; }
        public Speciality[] Specialities { get; set; }

        public ICommand RegisterCommand { get; set; }
        public ICommand WrapCheckedCommand { get; set; }

        public AddAccountVM(IAccountService accountService, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _accountService = accountService;
            _viewModelUpdaterService = viewUpdaterService;
            _dbFactory = dbFactory;
            _windowService = windowService;
            Roles = Enum.GetValues<Role>().SkipLast(1).Order().ToArray();
            Ranks = Enum.GetValues<Rank>().Order().ToArray();
            Specialities = Enum.GetValues<Speciality>().Order().ToArray();
            _currentSpeciality = Specialities[0];
            _currentRank = Ranks[0];
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync(),
                _ => !string.IsNullOrWhiteSpace(Username) &&
                     !string.IsNullOrWhiteSpace(Password) &&
                     (CurrentRole == Role.Administrator || (CurrentRole == Role.Commander && SelectedSoldier != null)));
            WrapCheckedCommand = new RelayCommand(wrap => OnWrapCheckChanged((wrap as Wrap<Staff>)!));
            _ = LoadPersonnel();
        }

        async Task LoadPersonnel()
        {
            Personnel = [];

            using var context = await _dbFactory.CreateDbContextAsync();
            Personnel = await context.Personnel
                .Where(p =>
                    CurrentRole != Role.Administrator && // администратор не может быть прикреплен к записи военнослужащего
                    context.Units.Any(u => u.CommanderId == p.Id) && // командир должен управлять подразделением
                    !context.Users.Any(u => u.StaffId == p.Id) && // командир не должен быть уже прикреплен к учетной записи
                    (CurrentRank == Rank.AnyRank || p.Rank == CurrentRank) &&
                    (CurrentSpeciality == Speciality.AnySpeciality || p.Speciality == CurrentSpeciality) &&
                    (string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(p.FullName, $"%{Query}%")))
                    .Select(p => new Wrap<Staff>(p) { IsChecked = SelectedSoldier != null && SelectedSoldier.Id == p.Id })
                    .OrderByDescending(w => w.IsChecked)
                    .ToArrayAsync();

            OnPropertyChanged(nameof(HasItems));
        }

        async Task RegisterAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            bool isUsernameExist = await context.Users.AnyAsync(u => u.Username.ToLower() == Username.ToLower());
            
            if (isUsernameExist)
            {
                _windowService.OpenMessageWindow("Ошибка регистрации", "Данное имя пользователя уже зарегистрировано.");
                return;
            }
            await _accountService.RegisterAsync(Username, Password, CurrentRole, SelectedSoldier?.Id);
            _viewModelUpdaterService.Update<AccountsVM>();
            _windowService.OpenMessageWindow("Регистрация данных", "Аккаунт был успешно зарегистрирован.");
        }

        void OnWrapCheckChanged(Wrap<Staff> wrap)
        {
            SelectedSoldier = wrap.IsChecked ? wrap.Value : null;
            if (wrap.IsChecked)
            {
                foreach (var w in Personnel)
                {
                    if (w != wrap)
                        w.IsChecked = false;
                }
            }

        }
    }
}
