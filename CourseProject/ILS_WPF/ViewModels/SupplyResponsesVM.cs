using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class SupplyResponsesVM : BaseVM
    {
        private IUserService _userService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private string _query;
        private SupplyResponseStatus _currentStatus;

        private List<SupplyResponseWrap> _supplyResponses;

        public List<SupplyResponseWrap> SupplyResponses
        {
            get => _supplyResponses;
            set
            {
                _supplyResponses = value;
                OnPropertyChanged();
            }
        }
        public bool HasItems => SupplyResponses?.Count > 0;

        public SupplyResponseStatus CurrentStatus
        {
            get => _currentStatus;
            set
            {
                _currentStatus = value;
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

        public SupplyResponseStatus[] Statuses { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand OpenSupplyResponseWindowCommand { get; set; }

        public SupplyResponsesVM(IViewModelUpdaterService viewUpdaterService, IUserService userService, IWindowService windowService, IDbContextFactory<ILSContext> dbFactory)
        {
            _userService = userService;
            _dbFactory = dbFactory;
            Statuses = [.. Enum.GetValues<SupplyResponseStatus>().Order()];
            CurrentStatus = Statuses[0];
            RefreshCommand = new RelayCommand(async _ => await LoadData());
            viewUpdaterService.SetUpdateCommand<SupplyResponsesVM>(RefreshCommand);
            OpenSupplyResponseWindowCommand = new RelayCommand(w => windowService.OpenSupplyResponseWindow(((SupplyResponseWrap)w).SupplyResponse.Id));
            _ = LoadData();
        }

        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var currentUser = _userService.GetUser();
            var isAdmin = currentUser?.Role == Role.Administator;
            var commanderId = currentUser?.Staff?.Id;

            SupplyResponses = [.. context.SupplyResponses
                .Include(r => r.Request)
                    .ThenInclude(req => req.RequestUnit)
                .ToList()
                .Where(r =>
                    (isAdmin || (r.Request?.RequestUnit?.CommanderId == commanderId)) &&
                    (CurrentStatus == SupplyResponseStatus.AnyStatus || r.Status == CurrentStatus) &&
                    (string.IsNullOrWhiteSpace(Query) || ((r.Request!.RequestUnit?.Name?.Contains(Query, StringComparison.OrdinalIgnoreCase) ?? false) || r.Request.CreationTime.ToString("yyyy-MM-dd HH:mm").Contains(Query, StringComparison.OrdinalIgnoreCase))))
                    .Select(r=>new SupplyResponseWrap(r))];
            OnPropertyChanged(nameof(HasItems));
        }
    }
}
