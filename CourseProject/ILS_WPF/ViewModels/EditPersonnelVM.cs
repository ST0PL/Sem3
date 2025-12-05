using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class EditPersonnelVM : BaseVM
    {
        private Staff _soldier;
        private IDbContextFactory<ILSContext> _dbFactory;
        private IWindowService _windowService;
        private ICommand _dataRefreshCommand;
        private string _fullName;
        private Rank _currentRank;
        private string _query;
        private Wrap<Unit>[] _units;

        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); } }

        public Rank CurrentRank
        {
            get => _currentRank;
            set
            {
                _currentRank = value;
                SelectedUnit = null;
                _ = LoadUnits();
            }
        }
        public Speciality CurrentSpeciality { get; set; }

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                _ = LoadUnits();
            }
        }
        public Wrap<Unit>[] Units { get => _units; set { _units = value; OnPropertyChanged(); } }
        public bool HasItems => Units?.Length > 0;
        public Unit? SelectedUnit { get; set; }

        public Rank[] Ranks { get; set; }
        public Speciality[] Specialities { get; set; }
        
        public ICommand WrapCheckedCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public EditPersonnelVM(Staff soldier, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory, ICommand dataRefreshCommand)
        {
            _soldier = soldier;
            _dbFactory = dbFactory;
            _windowService = windowService;
            _dataRefreshCommand = dataRefreshCommand;
            Ranks = Enum.GetValues<Rank>().SkipLast(1).Order().ToArray();
            Specialities = Enum.GetValues<Speciality>().SkipLast(1).Order().ToArray();
            CurrentRank = Ranks[0];
            CurrentSpeciality = Specialities[0];
            WrapCheckedCommand = new RelayCommand(wrap => OnWrapCheckChanged((wrap as Wrap<Unit>)!));
            SaveCommand = new RelayCommand(async _=> await SaveAsync(), _=>!string.IsNullOrWhiteSpace(FullName));
            RemoveCommand = new RelayCommand(async _ => await RemoveAsync());
            InitFormProperties();
        }

        void InitFormProperties()
        {
            FullName = _soldier.FullName!;
            CurrentRank = _soldier.Rank;
            CurrentSpeciality = _soldier.Speciality;
            SelectedUnit = _soldier.Unit;
        }

        async Task LoadUnits()
        {
            Units = [];

            if (CurrentRank <= UnitRankMatcher.MaxBattalionRank)
            {
                using var context = await _dbFactory.CreateDbContextAsync();
                Units = await context.Units
                    .Include(u => u.Commander)
                    .Where(u => u.Type == UnitType.Battalion && (string.IsNullOrWhiteSpace(Query) || EF.Functions.Like(u.Name, $"%{Query}%")))
                    .Select(u => new Wrap<Unit>(u))
                    .ToArrayAsync();
                foreach (var w in Units)
                    w.IsChecked = w.Value.Id == SelectedUnit?.Id;
            }

            OnPropertyChanged(nameof(HasItems));
        }

        async Task SaveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            context.Attach(_soldier);

            if ((SelectedUnit?.Id != _soldier.Unit?.Id))
                // При смене подразделения открепляем солдата от управляемых им подразделений
                await context.Units.Where(u => u.CommanderId == _soldier.Id).ForEachAsync(u => u.CommanderId = null);

            _soldier.FullName = FullName;
            _soldier.Rank = CurrentRank;
            _soldier.Speciality = CurrentSpeciality;
            _soldier.UnitId = SelectedUnit?.Id;

            await context.SaveChangesAsync();
            _dataRefreshCommand.Execute(null);
            _windowService.OpenMessageWindow("Изменение данных", "Данные о военнослужащем были успешно изменены.");
        }
        async Task RemoveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var profiles = context.Users.Where(u => u.StaffId == _soldier.Id);
            context.Users.RemoveRange(profiles);
            context.Personnel.Remove(_soldier);
            await context.SaveChangesAsync();
            _dataRefreshCommand.Execute(null);
            _windowService.OpenMessageWindow("Удаление данных", "Данные о военнослужащем были успешно удалены.");
        }

        void OnWrapCheckChanged(Wrap<Unit> wrap)
        {
            SelectedUnit = wrap.IsChecked ? wrap.Value : null;
            if (wrap.IsChecked)
            {
                foreach (var w in Units)
                {
                    if (w != wrap)
                        w.IsChecked = false;
                }
            }

        }
    }
}
