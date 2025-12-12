using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class SupplyRequestVM : BaseVM
    {
        private int _unitId;
        private ISupplyService _supplyService;
        private IViewModelUpdaterService _updaterService;
        private IWindowService _windowService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private bool _canMakeSupplyRequest;

        public ObservableCollection<WarehouseEntryVM> SupplyRequestDetails { get; set; } = [];

        public bool HasItems => SupplyRequestDetails.Count > 0;

        public bool CanMakeSupplyRequest
        {
            get => _canMakeSupplyRequest;
            set
            {
                _canMakeSupplyRequest = value;
                OnPropertyChanged();
            }
        }

        public ICommand MakeRequestCommand { get; set; }
        public ICommand AddDetailCommand { get; set; }
        public ICommand RemoveDetailCommand { get; set; }

        public SupplyRequestVM(int unitId, ISupplyService supplyService, IViewModelUpdaterService viewUpdaterService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _unitId = unitId;
            _supplyService = supplyService;
            _updaterService = viewUpdaterService;
            _windowService = windowService;
            _dbFactory = dbFactory;
            var _notifyChangesCommand = new RelayCommand(_ => ChangeCanMakeRequest());
            MakeRequestCommand = new RelayCommand(_ => MakeSupplyRequest(), _ => CanMakeSupplyRequest);
            RemoveDetailCommand = new RelayCommand(entry => SupplyRequestDetails.Remove((WarehouseEntryVM)entry), _ => HasItems);
            AddDetailCommand = new RelayCommand(_ => SupplyRequestDetails.Add(new WarehouseEntryVM(_notifyChangesCommand, RemoveDetailCommand)));
            SupplyRequestDetails.CollectionChanged += (_, _) => OnPropertyChanged(nameof(HasItems));
        }

        void MakeSupplyRequest()
        {
            _supplyService.MakeSupplyRequestAsync(
                _dbFactory,
                new SupplyRequest()
                {
                    RequestUnitId = _unitId,
                    Details = [.. SupplyRequestDetails.Select(d=>(SupplyRequestDetail)d)],
                    CreationTime = DateTime.Now
                });
            _updaterService.Update<MainCommanderVM>();
            _windowService.OpenMessageWindow("Заявки", "Заявка на снабжение была успешно отправлена.");
        }

        void ChangeCanMakeRequest()
            => CanMakeSupplyRequest = SupplyRequestDetails.All(d => d.GetCount() > 0);
    }
}
