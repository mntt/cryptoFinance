using System;
using System.Collections.Generic;
using LiveCharts;
using System.Windows.Forms;
using System.Linq;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace cryptoFinance
{
    public static class ConstructChart
    {
        private static List<DateTime> uniqueDates = new List<DateTime>();
        private static List<double> investments = new List<double>();
        private static List<double> currentValues = new List<double>();
        private static List<double> networth = new List<double>();
        private static List<ConstructingLists> chartData = new List<ConstructingLists>();
        private static List<string> selectedItems = new List<string>();

        public static void Build(string chart, LiveCharts.WinForms.CartesianChart chartView, DateTimePicker start, DateTimePicker finish, List<string> _selectedItems)
        {
            ClearChart(chartView);
            uniqueDates = ReturnUniqueDates(start, finish);
            selectedItems = _selectedItems;

            if (chart == "investments")
            {
                investments = ReturnInvestmentsList(uniqueDates);
                currentValues = ReturnCurrentValuesList(uniqueDates);
                networth = ReturnNetWorthData();
                AddChartDataPoints(chartView, chart);
            }

            if (chart == "crypto_quantities")
            {
                chartData = ReturnChartData(uniqueDates, selectedItems, true);
                AddChartDataPoints(chartView, chart);
            }

            if (chart == "crypto_currentvalues")
            {
                chartData = ReturnChartData(uniqueDates, selectedItems, false);
                AddChartDataPoints(chartView, chart);
            }

            SetChartLabelsAndFormat(chartView, chart);
        }

        private static ConstructingLists ReturnObject(DateTime date, string coin, decimal decimalValue)
        {
            ConstructingLists dataObject = new ConstructingLists
                    (
                        date,
                        coin,
                        decimalValue
                    );

            return dataObject;
        }

        private static decimal ReturnCV(DateTime date, string coin, decimal qTotal)
        {
            GetCultureInfo info = new GetCultureInfo(".");
            var price = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Date >= date && x.CryptoName == coin && (x.Operation == "BUY" || x.Operation == "SELL")).Select(x => decimal.Parse(x.LastPrice.ToString())).ToList();
            decimal cv = 0;

            if (price.Count > 0)
            {
                cv = qTotal * price[price.Count - 1];
            }
            else
            {
                var latestPrice = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Date == date && x.CryptoName == coin && x.Operation == "UpdatePrice")
                    .Select(x => decimal.Parse(x.LastPrice.ToString())).ToList();

                if (latestPrice.Count > 0)
                {
                    cv = qTotal * latestPrice[0];
                }
            }

            return cv;
        }

        public static List<ConstructingLists> ReturnChartData(List<DateTime> uniqueDates, List<string> _selectedItems, bool quantityOrCurrentValue)
        {
            List<ConstructingLists> dataList = new List<ConstructingLists>();

            foreach (var date in uniqueDates)
            {
                foreach (var coin in _selectedItems)
                {
                    var qBought = Connection.db.GetTable<CryptoTable>()
                        .Where(x => x.Date.Date <= date && x.CryptoName == coin && x.Operation == "BUY").Select(x => x.CryptoQuantity).ToList().Sum();
                    var qSold = Connection.db.GetTable<CryptoTable>()
                        .Where(x => x.Date.Date <= date && x.CryptoName == coin && x.Operation == "SELL").Select(x => x.CryptoQuantity).ToList().Sum();
                    var qTotal = qBought - qSold;

                    if (quantityOrCurrentValue)
                    {
                        string quantityValue = qTotal.ToString("N8").TrimEnd('0');
                        char[] qchars = quantityValue.ToCharArray();
                        if (qchars.Last() == ',')
                        {
                            quantityValue = quantityValue.Trim(',');
                        }

                        dataList.Add(ReturnObject(date, coin, decimal.Parse(quantityValue)));
                    }
                    else
                    {
                        dataList.Add(ReturnObject(date, coin, ReturnCV(date, coin, qTotal)));
                    }
                }
            }

            return dataList;
        }

        private static void AddChartDataPoints(LiveCharts.WinForms.CartesianChart chartView, string chart)
        {
            SeriesCollection series = new SeriesCollection();

            if (chart == "investments" && selectedItems.Count > 0)
            {
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    if (selectedItems[i] == "Investicijos")
                    {
                        series.Add(new LineSeries()
                        {
                            Title = "Investicijos",
                            Values = new ChartValues<double>(investments),
                        });
                    }

                    if (selectedItems[i] == "Dabartinė vertė")
                    {
                        series.Add(new LineSeries()
                        {
                            Title = "Dabartinė vertė",
                            Values = new ChartValues<double>(currentValues),
                        });
                    }

                    if (selectedItems[i] == "Grynasis pelnas")
                    {
                        series.Add(new LineSeries()
                        {
                            Title = "Grynasis pelnas",
                            Values = new ChartValues<double>(networth),
                        });
                    }
                }
            }

            if (chart == "crypto_quantities" || chart == "crypto_currentvalues")
            {
                var names = chartData.Select(x => x.name).Distinct().ToList();

                foreach (var coin in names)
                {
                    var data = chartData.Where(x => x.name == coin).Select(x => (double)x.quantity).ToList();

                    series.Add(new LineSeries()
                    {
                        Title = coin,
                        Values = new ChartValues<double>(data),
                    });
                }
            }

            if (series.Count > 0)
            {
                chartView.Series = series;
            }
        }

        private static List<double> ReturnNetWorthData()
        {
            List<double> networth = new List<double>();

            for (int i = 0; i < investments.Count; i++)
            {
                double value1 = investments[i];
                double value2 = currentValues[i];
                double value = value1 + value2;
                networth.Add(value);
            }

            return networth;
        }

        public static List<double> ReturnNetWorthData(List<double> investments, List<double> currentValues)
        {
            List<double> networth = new List<double>();

            for (int i = 0; i < investments.Count; i++)
            {
                double value1 = investments[i];
                double value2 = currentValues[i];
                double value = value1 + value2;
                networth.Add(value);
            }

            return networth;
        }

        public static List<double> ReturnCurrentValuesList(List<DateTime> dates)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            var cryptoTable = Connection.db.GetTable<CryptoTable>().ToList();
            List<double> currentValues = new List<double>();

            foreach (var date in dates)
            {
                var value = cryptoTable.Where(x => x.Date.Date == date && (x.Operation == "BUY" || x.Operation == "SELL")).Select(x => double.Parse(x.LastCurrentValue.ToString())).ToList();
                currentValues.Add(value[value.Count - 1]);
            }

            return currentValues;
        }

        private static List<double> ReturnSumList(DateTime date, string operation)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            var fixedDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            List<double> sumList = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Date <= fixedDate && x.Operation == operation).Select(x => double.Parse(x.Sum.ToString())).ToList();

            return sumList;
        }

        public static List<double> ReturnInvestmentsList(List<DateTime> dates)
        {
            List<double> investments = new List<double>();

            foreach (var date in dates)
            {
                double sum = 0;

                if (ReturnSumList(date, "BUY").Count > 0)
                {
                    sum -= ReturnSumList(date, "BUY").Sum();
                }

                if (ReturnSumList(date, "SELL").Count > 0)
                {
                    sum += ReturnSumList(date, "SELL").Sum();
                }

                investments.Add(sum);
            }

            return investments;
        }

        private static List<string> ReturnLabels(List<DateTime> uniqueDates)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < uniqueDates.Count; i++)
            {
                list.Add(uniqueDates[i].ToString("yyyy-MM-dd"));
            }

            return list;
        }

        private static List<DateTime> ReturnUniqueDates(DateTimePicker start, DateTimePicker finish)
        {
            var uniqueDates = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Date.Date >= DateTime.Parse(start.Value.ToString("yyyy-MM-dd")) && x.Date.Date <= DateTime.Parse(finish.Value.ToString("yyyy-MM-dd")) && (x.Operation == "BUY" || x.Operation == "SELL"))
                .Select(x => x.Date.Date).Distinct().ToList();

            return uniqueDates;
        }

        private static void ClearChart(LiveCharts.WinForms.CartesianChart chartView)
        {
            uniqueDates.Clear();
            investments.Clear();
            currentValues.Clear();
            networth.Clear();
            chartData.Clear();
            chartView.Series.Clear();
            chartView.AxisX.Clear();
            chartView.AxisY.Clear();
        }

        private static void SetChartLabelsAndFormat(LiveCharts.WinForms.CartesianChart chartView, string chart)
        {
            chartView.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = ReturnLabels(uniqueDates),
                Foreground = new SolidColorBrush(Colours.chartLabels),
                FontFamily = Design.mediaFont,
                FontSize = 9
            });

            chartView.AxisY.Add(ReturnYAxis(chart, selectedItems));

            chartView.LegendLocation = LegendLocation.Top;
            chartView.DefaultLegend.Foreground = new SolidColorBrush(Colours.chartLabels);
            chartView.DefaultLegend.FontFamily = Design.mediaFont;
            chartView.DefaultLegend.FontSize = 9;
            chartView.DataTooltip.Foreground = new SolidColorBrush(Colours.tooltipLabels);
            chartView.DataTooltip.FontFamily = Design.mediaFont;
            chartView.DataTooltip.FontSize = 9;
        }

        private static LiveCharts.Wpf.Axis ReturnYAxis(string chart, List<string> selectedCoins)
        {
            LiveCharts.Wpf.Axis yAxis = new LiveCharts.Wpf.Axis
            {
                Separator = new LiveCharts.Wpf.Separator
                {
                    StrokeThickness = 0.5,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 4 }),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                },

                LabelFormatter = ReturnFormat(chart, selectedCoins),
                Foreground = new SolidColorBrush(Colours.chartLabels),
                FontFamily = Design.mediaFont,
                FontSize = 9
            };

            if (chart == "crypto_quantities" || chart == "crypto_currentvalues")
            {
                yAxis.MinValue = 0;
            }

            return yAxis;
        }

        private static Func<double, string> ReturnFormat(string chart, List<string> _selectedItems)
        {
            Func<double, string> formatFunc = null;

            if (chart == "investments" || chart == "crypto_currentvalues")
            {
                formatFunc = (x) => x.ToString("C2");
            }

            if (chart == "crypto_quantities")
            {
                var list = ReturnChartData(uniqueDates, _selectedItems, true);

                if (list.Count > 0)
                {
                    formatFunc = (x) => x.ToString();
                }
            }

            return formatFunc;
        }

    }
}
