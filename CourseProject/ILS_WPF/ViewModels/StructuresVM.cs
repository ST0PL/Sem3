using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class StructuresVM : BaseVM
    {
        private IUserService _userService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private string _query;
        private UnitType _currentType;

        private List<Unit> _actualUnits;

        public List<Unit> ActualUnits { get => _actualUnits; set { _actualUnits = value; OnPropertyChanged(); } }
        public bool HasItems => ActualUnits?.Count > 0;

        public UnitType CurrentType
        {
            get => _currentType;
            set
            {
                _currentType = value;
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

        public UnitType[] Types { get; set; }


        public ICommand RefreshCommand { get; set; }
        public ICommand OpenRegisterWindowCommand { get; set; }
        public ICommand OpenEditWindowCommand { get; set; }

        public StructuresVM(IViewModelUpdaterService viewUpdaterService, IUserService userService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _userService = userService;
            _dbFactory = dbFactory;
            Types = [.. Enum.GetValues<UnitType>().Order()];
            CurrentType = Types[0];
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            viewUpdaterService.SetUpdateCommand<StructuresVM>(RefreshCommand);
            IsAdmin = _userService.GetUser()!.Role == Role.Administrator;
            OpenRegisterWindowCommand = new RelayCommand(_=>windowService.OpenUnitRegisterWindow(), _=> IsAdmin);
            OpenEditWindowCommand = new RelayCommand(arg => windowService.OpenUnitEditWindow((arg as Unit)!, IsAdmin));
            _  = LoadData();
        }

            async Task LoadData()
            {
                using var context = await _dbFactory.CreateDbContextAsync();
                var currentUser = _userService.GetUser();
                var commanderId = currentUser?.Staff?.Id;

                var filteredUnits = await context.Units
                         .Include(u => u.Commander)
                         .Include(u => u.AssignedWarehouse)
                         .Include(u => u.Personnel)
                         .Include(u => u.Children)
                         .Where(u => string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(u.Name, $"%{Query}%") || (u.CommanderId != null && EF.Functions.Like(u.Commander.FullName, $"%{Query}%")))
                         .ToArrayAsync();

                await LoadAllParents(context.Units, filteredUnits);

                ActualUnits = filteredUnits
                    .Where(u =>
                        (IsAdmin || (commanderId != null && HasCommanderInTree(u, commanderId.Value))) &&
                        (CurrentType == UnitType.AnyType || u.Type == CurrentType))
                    .ToList();
                OnPropertyChanged(nameof(HasItems));
            }

            async Task LoadAllParents(DbSet<Unit> dbSet, IEnumerable<Unit> units)
            {
                foreach(var unit in units)
                    unit.Parent = await dbSet.Where(u => u.Id == unit.ParentId).FirstOrDefaultAsync();
            }

        bool HasCommanderInTree(Unit unit, int commanderId)
        {
            Unit currentUnit = unit;
            while(currentUnit != null)
            {
                if (currentUnit.CommanderId == commanderId)
                    return true;
                currentUnit = currentUnit.Parent!;
            }
            return false;
        }
    }
}
