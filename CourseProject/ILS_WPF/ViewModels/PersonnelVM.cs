using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class PersonnelVM : BaseVM
    {
        private IUserService _userService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private string _query;
        private Rank _currentRank;
        private Speciality _currentSpeciality;

        private List<Staff> _actualPersonnel;

        public List<Staff> ActualPersonnel { get => _actualPersonnel; set { _actualPersonnel = value; OnPropertyChanged(); } }
        public bool HasItems => ActualPersonnel.Count > 0;

        public Rank CurrentRank
        {
            get => _currentRank;
            set
            {
                _currentRank = value;
                _ = LoadData();
            }
        }
        public Speciality CurrentSpeciality
        {
            get => _currentSpeciality;
            set
            { 
                _currentSpeciality = value;
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

        public Rank[] Ranks { get; set; }
        public Speciality[] Specialities { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand OpenEditWindowCommand { get; set; }

        public PersonnelVM(IUserService userService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _userService = userService;
            _dbFactory = dbFactory;
            Ranks = [.. Enum.GetValues<Rank>().Order()];
            Specialities = [.. Enum.GetValues<Speciality>().Order()];
            CurrentRank = Ranks[0];
            CurrentSpeciality = Specialities[0];
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            OpenRegisterWindowCommand = new RelayCommand(_=>windowService.OpenPersonnelRegisterWindow(RefreshCommand));
            OpenEditWindowCommand = new RelayCommand(arg => windowService.OpenPersonnelEditWindow((arg as Staff)!, RefreshCommand));
            _  = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var currentUser = _userService.GetUser();
            var isAdmin = currentUser?.Role == Role.Administator;
            var userUnitId = currentUser?.Staff?.UnitId;

            ActualPersonnel = context.Personnel.Include(p=>p.Unit).ToList().Where(
                p =>
                (isAdmin || (p.Unit != null && p.UnitId == userUnitId )) &&
                (CurrentRank == Rank.AnyRank || p.Rank == CurrentRank) &&
                (CurrentSpeciality == Speciality.AnySpeciality || p.Speciality == CurrentSpeciality) &&
                (string.IsNullOrWhiteSpace(Query) || ( (p.Unit != null && p.Unit.Name.Contains(Query, StringComparison.OrdinalIgnoreCase)) || p!.FullName!.Contains(Query, StringComparison.OrdinalIgnoreCase))))
                .ToList();
            OnPropertyChanged(nameof(HasItems));
        }
    }
}
