using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ILS_WPF.ViewModels
{
    class StatVM : BaseVM
    {
        private int _totalPersonnel;
        private int _unresolvedRequests;
        private int _resolvedPercent;

        public int TotalPersonnel { get => _totalPersonnel; set { _totalPersonnel = value;OnPropertyChanged(); } }
        public int UnresolvedRequests { get => _unresolvedRequests; set { _unresolvedRequests = value; OnPropertyChanged(); } }
        public int ResolvedPercent { get => _resolvedPercent; set { _resolvedPercent = value; OnPropertyChanged(); } }

        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public SolidColorPaint ChartFramePaint { get; set; }

        public StatVM()
        {
            SetupChart();
        }
        void SetupChart()
        {
           Series = new ISeries[]
           {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 5, 4, 6, 8, 3, 7 },
                    Name = "Количество заявок",
                    Stroke = new SolidColorPaint() { Color = SKColor.Parse("#00D492"), StrokeThickness = 1 },
                    Fill = null,
                    GeometrySize = 0
                }
           };
            var gridLineColor = new SolidColorPaint(SKColor.Parse("#272F31"));

            XAxes = new[]
                {
                new Axis
                {
                    ShowSeparatorLines = false,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#BFFFFFFF"))
                }
            };
            YAxes = new[]
            {
                new Axis
                {
                    ShowSeparatorLines = true,
                    SeparatorsPaint = gridLineColor,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#BFFFFFFF")),
                }
            };
            ChartFramePaint = new SolidColorPaint(SKColors.Black) { StrokeThickness = 2 };
        }

    }
}
    