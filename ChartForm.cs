using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cryptoFinance
{
    public class ChartForm
    {
        private bool emptyAssetList { get; set; }

        public ChartForm()
        {
            
        }

        public async void Open(CurrentAssets form, DataGridForm dgf)
        {
            if (form.dataGridCurrentAssets.Rows.Count == 0)
            {
                emptyAssetList = true;
            }
            else
            {
                emptyAssetList = false;
            }

            if (emptyAssetList)
            {
                form.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
            }
            else
            {
                if (form.pieChart.Visible == false)
                { 
                    form.ShowLoading();
                    ConstructPieChart(form, dgf);
                    await Task.Delay(1000);
                    form.HideLoading();
                }
                else
                {
                    form.pieChart.Visible = false;
                }
            }
        }

        public async Task ConstructPieChartAsync(CurrentAssets form, DataGridForm dgf)
        {
            form.ShowLoading();
            ConstructPieChart(form, dgf);
            await Task.Delay(1000);
            form.HideLoading();
        }

        private void ConstructPieChart(CurrentAssets form, DataGridForm dgf)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            form.pieChart.Series.Clear();
            var coinList = dgf.ReturnCoinList();
            var coins = coinList.Where(x => x.quantity > 0).OrderByDescending(x => x.totalSum).ToList();
            var total = coins.Select(x => x.totalSum).ToList().Sum();
            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0:C2}", chartPoint.Y);

            var tooltip = new DefaultTooltip
            {
                SelectionMode = TooltipSelectionMode.OnlySender,
                Foreground = new SolidColorBrush(Colours.tooltipLabels)
            };

            form.pieChart.Series = CreateSeries(coins, total, labelPoint);
            form.pieChart.LegendLocation = LegendLocation.Right;
            form.pieChart.DefaultLegend.Foreground = new SolidColorBrush(Colours.chartLabels);
            form.pieChart.Font = Design.font6;
            form.pieChart.DataTooltip = tooltip;
            form.pieChart.Visible = true;
        }

        private SeriesCollection CreateSeries(List<ConstructingLists> coins, decimal total, Func<ChartPoint, string> labelPoint)
        {
            SeriesCollection series = new SeriesCollection();

            for (int i = 0; i < coins.Count; i++)
            {
                bool showLabel = true;

                if (double.Parse((coins[i].totalSum / total).ToString()) <= double.Parse("0.15"))
                {
                    showLabel = false;
                }

                series.Add(new PieSeries()
                {
                    Title = coins[i].name,
                    Values = new ChartValues<double> { double.Parse(coins[i].totalSum.ToString()) },
                    LabelPoint = labelPoint,
                    DataLabels = showLabel,
                    Stroke = new SolidColorBrush(Colours.chartGrid),
                    Foreground = new SolidColorBrush(Colours.chartLabels),
                    StrokeThickness = 1,
                    FontFamily = Design.mediaFont,
                    FontSize = 9
                });
            }

            return series;
        }

    }
}
