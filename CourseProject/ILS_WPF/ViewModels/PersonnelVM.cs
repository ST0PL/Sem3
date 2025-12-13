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
        public bool HasItems => ActualPersonnel?.Count > 0;

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

        public bool IsAdmin { get; set; }

        public Rank[] Ranks { get; set; }
        public Speciality[] Specialities { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand OpenEditWindowCommand { get; set; }

        public PersonnelVM(IViewModelUpdaterService viewUpdaterService, IUserService userService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _userService = userService;
            _dbFactory = dbFactory;
            IsAdmin = userService.GetUser()!.Role == Role.Administrator;
            Ranks = [.. Enum.GetValues<Rank>().Order()];
            Specialities = [.. Enum.GetValues<Speciality>().Order()];
            _currentRank = Ranks[0];
            _currentSpeciality = Specialities[0];
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            viewUpdaterService.SetUpdateCommand<PersonnelVM>(RefreshCommand);
            OpenRegisterWindowCommand = new RelayCommand(_=>windowService.OpenPersonnelRegisterWindow(), _ => IsAdmin);
            OpenEditWindowCommand = new RelayCommand(arg =>
            {
                if (IsAdmin)
                    windowService.OpenPersonnelEditWindow((arg as Staff)!);
            });
            _  = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var currentUser = _userService.GetUser();
            var commandedUnit = await context.Units.Where(u => u.CommanderId == currentUser.StaffId).FirstAsync();
            var personnel = IsAdmin ? context.Personnel.Include(p => p.Unit).ToList() : await GetPersonnelFromTree(context, commandedUnit);
            ActualPersonnel = personnel.Where(
                    p =>
                    (CurrentRank == Rank.AnyRank || p.Rank == CurrentRank) &&
                    (CurrentSpeciality == Speciality.AnySpeciality || p.Speciality == CurrentSpeciality) &&
                    (string.IsNullOrWhiteSpace(Query) || (p.Unit != null && p.Unit.Name.Contains(Query, StringComparison.OrdinalIgnoreCase)) || p!.FullName!.Contains(Query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            OnPropertyChanged(nameof(HasItems));
        }


        async Task<List<Staff>> GetPersonnelFromTree(ILSContext context, Unit? unit)
        {
            if (unit == null)
                return [];

            if (unit.Type == UnitType.Battalion)
            {
                await context.Entry(unit).Collection(u => u.Personnel).LoadAsync();
                return [.. unit.Personnel];
            }

            int currentId;

            Queue<int> unitQueue = new();
            IQueryable<Unit> children;
            List<Staff> personnel = new();
            unitQueue.Enqueue(unit.Id);

            while (unitQueue.Count > 0)
            {
                currentId = unitQueue.Dequeue();
                children = context.Units
                    .Where(u => u.ParentId == currentId)
                    .Include(u => u.Personnel);

                foreach (var child in children)
                {
                    if (child.Type == UnitType.Battalion)
                    {
                        personnel.AddRange(child.Personnel);
                        continue;
                    }
                    unitQueue.Enqueue(child.Id);
                }
            }
            return [.. personnel];
        }
    }
}
