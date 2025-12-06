using ILS_WPF.Models.Database;
using ILS_WPF.Models.Core.Enums;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using ILS_WPF.MVMM;
using System.Windows.Input;
using ILS_WPF.Services.Interfaces;

namespace ILS_WPF.ViewModels
{
    public class StatVM : BaseVM
    {
        private IDbContextFactory<ILSContext> _dbFactory;

        private int _totalPersonnel;
        private int _unresolvedRequests;
        private int _resolvedPercent;
        private ISeries[] _series;

        public int TotalPersonnel { get => _totalPersonnel; set { _totalPersonnel = value; OnPropertyChanged(); } }
        public int UnresolvedRequests { get => _unresolvedRequests; set { _unresolvedRequests = value; OnPropertyChanged(); } }
        public int ResolvedPercent { get => _resolvedPercent; set { _resolvedPercent = value; OnPropertyChanged(); } }

        public ISeries[] Series { get => _series; set { _series = value; OnPropertyChanged(); } }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public SolidColorPaint ChartFramePaint { get; set; }

        public ICommand RefreshCommand { get; set; }

        public StatVM(IViewModelUpdaterService viewUpdaterService, IDbContextFactory<ILSContext> dbFactory)
        {
            _dbFactory = dbFactory;
            RefreshCommand = new RelayCommand(async _=> await LoadData());
            viewUpdaterService.SetUpdateCommand<StatVM>(RefreshCommand);
            SetupChart();
            _ = LoadData();
        }
        void SetupChart()
        {
            XAxes = new[]
                {
                new Axis
                {
                    Labels = Enumerable.Range(0, 24)
                        .Select(h => $"{h:00}:00")
                        .ToArray(),
                    ShowSeparatorLines = false,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#BFFFFFFF"))
                }
            };
            YAxes = new[]
            {
                new Axis
                {
                    ShowSeparatorLines = true,
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#272F31")),
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#BFFFFFFF")),
                    MinStep = 1
                }
            };
            ChartFramePaint = new SolidColorPaint(SKColors.Black) { StrokeThickness = 2 };
        }
        async Task LoadData()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            TotalPersonnel = await context.Personnel.CountAsync();
            UnresolvedRequests = await context.SupplyResponses
                .Where(r=>r.Status == SupplyResponseStatus.Denied)
                .CountAsync();

            var totalResponses = await context.SupplyResponses.CountAsync();

            ResolvedPercent = totalResponses == 0 ? 0 :
                (await context.SupplyResponses.Where(r => r.Status == SupplyResponseStatus.Success).CountAsync())
                / totalResponses * 100;


            var autoResolvedCount = new int[DateTime.Now.Hour+1];

            var times = await context.SupplyResponses
                .Where(
                    r => r.Status == SupplyResponseStatus.Success &&
                    r.Request != null &&
                    r.Request.CreationTime.Date == DateTime.Today)
                .Select(r => r.Request.CreationTime)
                .ToListAsync();

            foreach (var time in times)
                autoResolvedCount[time.Hour]++;

            Series = new ISeries[]
            {
                new LineSeries<int>
                {
                    Values = autoResolvedCount,
                    Name = "Количество заявок",
                    Stroke = new SolidColorPaint() { Color = SKColor.Parse("#00D492"), StrokeThickness = 1 },
                    Fill = null,
                    GeometrySize = 0
                }
            };

        }
    }
}
    