using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf_TermPaper_FinancialAccounting.Charts
{
    /// <summary>
    /// Interaction logic for TotalBarChart.xaml
    /// </summary>
    public partial class TotalBarChart : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public TotalBarChart(double inc_val, double outc_val)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Надходження",
                    Values = new ChartValues<double> { inc_val },
                    Fill = Brushes.SeaGreen,
                    MaxColumnWidth = 70
                },

                new ColumnSeries
                {
                    Title = "Витрати",
                    Values = new ChartValues<double> { outc_val },
                    Fill = Brushes.Orange,
                    MaxColumnWidth = 70
                }

            };

            Labels = new[] { "" };

            DataContext = this;
        }
    }
}
