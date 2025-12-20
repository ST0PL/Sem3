using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class EditAccountVM : BaseVM
    {
        private User _account;
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

        public bool IsRoot { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand WrapCheckedCommand { get; set; }

        public EditAccountVM(User account, IUserService userService, IAccountService accountService, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _account = account;
            _accountService = accountService;
            _viewModelUpdaterService = viewUpdaterService;
            _dbFactory = dbFactory;
            _windowService = windowService;
            Roles = Enum.GetValues<Role>().SkipLast(1).Order().ToArray();
            Ranks = Enum.GetValues<Rank>().Order().ToArray();
            Specialities = Enum.GetValues<Speciality>().Order().ToArray();
            _currentSpeciality = Specialities[0];
            _currentRank = Ranks[0];
            IsRoot = account.Username == "admin";
            SaveCommand = new RelayCommand(async _ => await SaveAsync(),
                _ => !string.IsNullOrWhiteSpace(Username) &&
                     (CurrentRole == Role.Administrator || (CurrentRole == Role.Commander && SelectedSoldier != null)));
            RemoveCommand = new RelayCommand(async _ => await RemoveAsync(), _ => !IsRoot && userService.GetUser()!.Id != account.Id);
            WrapCheckedCommand = new RelayCommand(wrap => OnWrapCheckChanged((wrap as Wrap<Staff>)!));
            InitFormFields();
            _ = LoadPersonnel();
        }

        void InitFormFields()
        {
            _username = _account.Username;
            _currentRole = _account.Role;
            SelectedSoldier = _account.Staff;
        }

        async Task LoadPersonnel()
        {
            Personnel = [];

            using var context = await _dbFactory.CreateDbContextAsync();
            Personnel = await context.Personnel
                .Where(p =>
                    CurrentRole != Role.Administrator && // администратор не может быть прикреплен к записи военнослужащего
                    context.Units.Any(u=>u.CommanderId == p.Id) && // командир должен управлять подразделением
                    (_account.StaffId == p.Id || !context.Users.Any(u => u.StaffId == p.Id)) && // командир или прикреплен к редактируемому аккаунту или не прикреплен вовсе
                    (CurrentRank == Rank.AnyRank || p.Rank == CurrentRank) &&
                    (CurrentSpeciality == Speciality.AnySpeciality || p.Speciality == CurrentSpeciality) &&
                    (string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(p.FullName, $"%{Query}%")))
                    .Select(p => new Wrap<Staff>(p) { IsChecked = SelectedSoldier != null && SelectedSoldier.Id == p.Id })
                    .OrderByDescending(w=>w.IsChecked)
                    .ToArrayAsync();

            OnPropertyChanged(nameof(HasItems));
        }

        async Task SaveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            if (!string.IsNullOrWhiteSpace(Password))
                await _accountService.ChangePasswordAsync(_account.Username, Password);

            int? selectedSoldierId = SelectedSoldier?.Id;

            await context.Users.Where(u => u.Id == _account.Id)
                .ExecuteUpdateAsync(setter => setter
                        .SetProperty(u => u.Username, _username)
                        .SetProperty(u => u.Role, _currentRole)
                        .SetProperty(u => u.StaffId, selectedSoldierId));

            await context.SaveChangesAsync();
            _viewModelUpdaterService.Update<AccountsVM>();
            _windowService.OpenMessageWindow("Изменение данных", "Данные об аккаунте были успешно изменены");
        }

        async Task RemoveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            await _accountService.RemoveAsync(_account.Id);
             _viewModelUpdaterService.Update<AccountsVM>();
            _windowService.OpenMessageWindow("Удаление данных", "Данные об аккаунте были успешно удалены");
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
