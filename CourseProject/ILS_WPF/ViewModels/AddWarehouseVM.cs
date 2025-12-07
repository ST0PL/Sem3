using ILS_WPF.Models.Core;
using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class AddWarehouseVM : BaseVM
    {
        private IViewModelUpdaterService _viewModelUpdaterService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private IWindowService _windowService;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public WarehouseType[] WarehouseTypes { get; set; }
        public WarehouseType CurrentType { get; set; }


        public ICommand RegisterCommand { get; set; }

        public AddWarehouseVM(IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _viewModelUpdaterService = viewUpdaterService;
            _dbFactory = dbFactory;
            _windowService = windowService;
            WarehouseTypes = Enum.GetValues<WarehouseType>().SkipLast(1).Order().ToArray();
            CurrentType = WarehouseTypes[0];
            RegisterCommand = new RelayCommand(async _=> await RegisterAsync(), _=>!string.IsNullOrWhiteSpace(Name));
        }


        async Task RegisterAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            await context.Warehouses.AddAsync(new Warehouse(Name, CurrentType));
            await context.SaveChangesAsync();
            _viewModelUpdaterService.Update<WarehouseListVM>();
            _windowService.OpenMessageWindow("Регистрация данных", "Данные о складе были успешно зарегистрированы.");
        }
    }
}
