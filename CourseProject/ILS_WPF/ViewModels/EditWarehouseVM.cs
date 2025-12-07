using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class EditWarehouseVM : BaseVM
    {
        private int _warehouseId;
        private IViewModelUpdaterService _viewModelUpdaterService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private IWindowService _windowService;
        private string? _name;
        private WarehouseType _currentType;
        private ICommand _navigateBackCommand;

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public WarehouseType[] WarehouseTypes { get; set; }
        public WarehouseType CurrentType
        {
            get => _currentType;
            set
            {
                _currentType = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public EditWarehouseVM(int warehouseId, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory, ICommand navigateBackCommand)
        {
            _warehouseId = warehouseId;
            _viewModelUpdaterService = viewUpdaterService;
            _dbFactory = dbFactory;
            _windowService = windowService;
            _navigateBackCommand = navigateBackCommand;
            WarehouseTypes = Enum.GetValues<WarehouseType>().SkipLast(1).Order().ToArray();
            SaveCommand = new RelayCommand(async _=> await SaveAsync(), _=>!string.IsNullOrWhiteSpace(Name));
            RemoveCommand = new RelayCommand(async _ => await RemoveAsync());
            _ = InitFormFields();
        }

        async Task InitFormFields()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var warehouse = await context.Warehouses.Where(w=>w.Id == _warehouseId).FirstOrDefaultAsync();
            Name = warehouse!.Name;
            CurrentType = warehouse.Type;
        }


        async Task SaveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var warehouse = new Warehouse() { Id = _warehouseId, Name = _name, Type = CurrentType };
            context.Update(warehouse);
            await context.SaveChangesAsync();
            _viewModelUpdaterService.Update<WarehouseListVM>();
            _viewModelUpdaterService.Update<CurrentWarehouseVM>();
            _viewModelUpdaterService.Update<PersonnelVM>();
            _windowService.OpenMessageWindow("Изменение данных", "Данные о складе были успешно изменены.");
        }
        async Task RemoveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            await context.Warehouses.Where(w => w.Id == _warehouseId).ExecuteDeleteAsync();
            _viewModelUpdaterService.Update<WarehouseListVM>();
            _viewModelUpdaterService.Update<PersonnelVM>();
            await context.SaveChangesAsync();
            _navigateBackCommand.Execute(null);
            _windowService.OpenMessageWindow("Удаление данных", "Данные о складе были успешно изменены.");
        }
    }
}
