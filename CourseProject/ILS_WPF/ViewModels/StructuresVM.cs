using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class StructuresVM : BaseVM
    {
        private IViewModelUpdaterService _viewModelUpdaterService;
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
            OpenRegisterWindowCommand = new RelayCommand(_=>windowService.OpenUnitRegisterWindow(RefreshCommand));
            OpenEditWindowCommand = new RelayCommand(arg => windowService.OpenUnitEditWindow((arg as Unit)!, RefreshCommand));
            _  = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var currentUser = _userService.GetUser();
            var isAdmin = currentUser?.Role == Role.Administator;
            var commanderId = currentUser?.Staff?.Id;

            try
            {
               ActualUnits =
                    (await context.Units
                    .Include(u => u.Commander)
                    .Include(u => u.AssignedWarehouse)
                    .Include(u=>u.Personnel)
                    .Include(u=>u.Children)
                    .Where(u => string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(u.Name, $"%{Query}%") || (u.CommanderId != null && EF.Functions.Like(u.Commander.FullName, $"%{Query}%")))
                    .ToArrayAsync())
                    .Where(u =>
                        (isAdmin || (commanderId != null && HasCommanderInTree(u, commanderId.Value))) &&
                        (CurrentType == UnitType.AnyType || u.Type == CurrentType))
                    .ToList();
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
            OnPropertyChanged(nameof(HasItems));
        }

        bool HasCommanderInTree(Unit unit, int commanderId)
        {
            Unit currentUnit = unit;
            while(currentUnit != null)
            {
                if (currentUnit.CommanderId == commanderId)
                    return true;
                currentUnit = currentUnit.Parent;
            }
            return false;
        }
    }
}
