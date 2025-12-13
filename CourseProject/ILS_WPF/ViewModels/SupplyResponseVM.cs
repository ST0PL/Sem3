using ILS_WPF.Models.Core.Enums;
using ILS_WPF.Models.Core.Requests;
using ILS_WPF.Models.Database;
using ILS_WPF.MVMM;
using ILS_WPF.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace ILS_WPF.ViewModels
{
    public class SupplyResponseVM : BaseVM
    {
        private int _supplyResponseId;
        private ISupplyService _supplyService;
        private IViewModelUpdaterService _updaterService;
        private IWindowService _windowService;
        private IDbContextFactory<ILSContext> _dbFactory;
        private SupplyResponseWrap _supplyResponseWrap;
        private bool _canRepeat = false;

        public SupplyResponseWrap SupplyResponseWrap
        {
            get => _supplyResponseWrap;
            set
            {
                _supplyResponseWrap = value;
                OnPropertyChanged();
            }
        }

        public bool CanRepeat
        {
            get => _canRepeat;
            set
            {
                _canRepeat = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin { get; set; }
        public bool HasDetails => SupplyResponseWrap.SupplyResponse?.Request?.Details?.Count > 0;
        public bool HasUnprocessedDetails => SupplyResponseWrap.SupplyResponse.UnprocessedDetails.Count > 0;
        public bool IsCommentEnabled => !string.IsNullOrWhiteSpace(SupplyResponseWrap?.SupplyResponse?.Comment ?? "");

        public ICommand RepeatRequestCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public SupplyResponseVM(int supplyResponseId, ISupplyService supplyService, IViewModelUpdaterService updaterService, IWindowService windowService, IUserService userService, IDbContextFactory<ILSContext> dbFactory)
        {
            _supplyResponseId = supplyResponseId;
            _supplyService = supplyService;
            _updaterService = updaterService;
            _windowService = windowService;
            _dbFactory = dbFactory;
            IsAdmin = userService.GetUser()!.Role == Role.Administrator;
            RepeatRequestCommand = new RelayCommand(_ => RepeatRequest());
            RemoveCommand = new RelayCommand(async _ => await RemoveAsync());
            _ = LoadData(supplyResponseId);
        }

        async Task LoadData(int supplyResponseId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var response = await context.SupplyResponses
                .Where(r => r.Id == supplyResponseId)
                .Include(r=>r.UnprocessedDetails)
                .Include(r=>r.Request)
                    .ThenInclude(req=>req.RequestUnit)
                .Include(r => r.Request)
                    .ThenInclude(req=>req.Details)
                .FirstOrDefaultAsync();
            
            if (response == null)
                return;

            SupplyResponseWrap = new SupplyResponseWrap(response);
            CanRepeat = response.Status != SupplyResponseStatus.Success;
            OnPropertiesChanged(nameof(HasDetails), nameof(HasUnprocessedDetails), nameof(IsCommentEnabled));
        }

        void RepeatRequest()
        {
            var request = new SupplyRequest()
            {
                RequestUnitId = SupplyResponseWrap.SupplyResponse.Request!.RequestUnitId,
                Details = [..SupplyResponseWrap.SupplyResponse.UnprocessedDetails.Select(d=>(SupplyRequestDetail)d.Clone())],
                CreationTime = DateTime.Now
            };

            _supplyService.MakeSupplyRequestAsync(_dbFactory, request);
            _windowService.OpenMessageWindow("Заявки", "Повторная заявка успешно зарегистрирована.");
            _updaterService.Update<SupplyResponsesVM>();
            _updaterService.Update<MainCommanderVM>();
            _updaterService.Update<MainVM>();
        }

        async Task RemoveAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            var response = await context.SupplyResponses
                .Where(r => r.Id == _supplyResponseId)
                .Include(r => r.UnprocessedDetails)
                .Include(r => r.Request)
                    .ThenInclude(req => req.Details)
                .FirstOrDefaultAsync();

            if (response == null)
                return;

            context.SupplyRequestDetails.RemoveRange(response.UnprocessedDetails);

            if (response.Request != null)
            {
                context.SupplyRequestDetails.RemoveRange(response.Request.Details);
                context.SupplyRequests.Remove(response.Request);
            }

            context.SupplyResponses.Remove(response);
            await context.SaveChangesAsync();

            _windowService.OpenMessageWindow("Заявки", "Данные о заявке успешно удалены.");
            _updaterService.Update<SupplyResponsesVM>();
            _updaterService.Update<MainCommanderVM>();
        }

        void OnPropertiesChanged(params string[] properties)
        {
            foreach (var property in properties)
                OnPropertyChanged(property);
        }
    }
}
