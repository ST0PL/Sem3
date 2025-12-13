using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class AccountsVM : BaseVM
    {
        private IDbContextFactory<ILSContext> _dbFactory;
        private string _query;
        private Role _currentRole;

        private List<User> _accounts;

        public List<User> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }
        public bool HasItems => Accounts?.Count > 0;

        public Role CurrentRole
        {
            get => _currentRole;
            set
            {
                _currentRole = value;
                _ = LoadData();
            }
        }

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                _ = LoadData();
            }
        }

        public Role[] Roles { get; set; }


        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand OpenEditWindowCommand { get; set; }

        public AccountsVM(IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _dbFactory = dbFactory;
            Roles = [.. Enum.GetValues<Role>().Order()];
            CurrentRole = Roles[0];
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            viewUpdaterService.SetUpdateCommand<AccountsVM>(RefreshCommand);
            OpenRegisterWindowCommand = new RelayCommand(_ => windowService.OpenAccountRegisterWindow());
            OpenEditWindowCommand = new RelayCommand(arg => windowService.OpenAccountEditWindow((arg as User)!));
            _ = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            Accounts = await context.Users
                .Where(u=>
                    (string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(u.Username, $"%{Query}%")) &&
                    (CurrentRole == Role.AnyRole || u.Role == CurrentRole))
                .Include(u=>u.Staff).ToListAsync();

            OnPropertyChanged(nameof(HasItems));
        }
    }
}
