using System;
using System.Collections.Generic;
using LiveCharts;
using System.Windows.Forms;
using System.Linq;
using LiveCharts.Wpf;

namespace cryptoFinance
{
    public static class ConstructChart
    {
        private static List<DateTime> uniqueDates = new List<DateTime>();
        private static List<double> investments = new List<double>();
        private static List<double> currentValues = new List<double>();
        private static List<double> networth = new List<double>();
        private static List<ConstructingLists> chartData = new List<ConstructingLists>();

        public static void Build(string chart, LiveCharts.WinForms.CartesianChart chartView, DateTimePicker start, DateTimePicker finish, List<string> selectedCoins)
        {
            ClearChart(chartView);
            uniqueDates = ReturnUniqueDates(start, finish);

            if(chart == "investments")
            {
                investments = ReturnInvestmentsList();
                currentValues = ReturnCurrentValuesList();
                networth = ReturnNetWorthData();
                AddChartDataPoints(chartView, chart);
            }

            if(chart == "crypto_quantities")
            {
                chartData = ReturnChartData(uniqueDates, selectedCoins, true);
                AddChartDataPoints(chartView, chart);
            }

            if(chart == "crypto_currentvalues")
            {
                chartData = ReturnChartData(uniqueDates, selectedCoins, false);
                AddChartDataPoints(chartView, chart);
            }

            SetChartLabelsAndFormat(chartView, chart, selectedCoins);
        }

        private static ConstructingLists ReturnObject(DateTime date, string coin, double decimalValue)
        {
            ConstructingLists dataObject = new ConstructingLists
                    (
                        date,
                        coin,
                        decimalValue
                    );

            return dataObject;
        }

        private static double ReturnCV(DateTime date, string coin, double qTotal)
        {
            GetCultureInfo info = new GetCultureInfo(".");
            var price = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Date == date && x.CryptoName == coin && (x.Operation == "BUY" || x.Operation == "SELL")).Select(x => double.Parse(x.LastPrice.ToString())).ToList();
            double cv = 0;

            if (price.Count > 0)
            {
                cv = qTotal * price[price.Count - 1];
            }
            else
            {
                var latestPrice = Connection.db.GetTable<CryptoTable>().Where(x => x.Date == date && x.CryptoName == coin && x.Operation == "UpdatePrice").Select(x => double.Parse(x.LastPrice.ToString())).ToList();

                if (latestPrice.Count > 0)
                {
                    cv = qTotal * latestPrice[0];
                }
            }

            return cv;
        }

        public static List<ConstructingLists> ReturnChartData(List<DateTime> uniqueDates, List<string> selectedCoins, bool quantityOrCurrentValue)
        {
            List<ConstructingLists> dataList = new List<ConstructingLists>();

            foreach (var date in uniqueDates)
            {
                foreach (var coin in selectedCoins)
                {
                    var qBought = Connection.db.GetTable<CryptoTable>()
                        .Where(x => x.Date <= date && x.CryptoName == coin && x.Operation == "BUY").Select(x => x.CryptoQuantity).ToList().Sum();
                    var qSold = Connection.db.GetTable<CryptoTable>()
                        .Where(x => x.Date <= date && x.CryptoName == coin && x.Operation == "SELL").Select(x => x.CryptoQuantity).ToList().Sum();
                    var qTotal = qBought - qSold;

                    if (quantityOrCurrentValue)
                    {
                        dataList.Add(ReturnObject(date, coin, double.Parse(qTotal.ToString("0.00000000"))));
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

            if (chart == "investments")
            {
                series.Add(new LineSeries()
                {
                    Title = "Investicijos",
                    Values = new ChartValues<double>(investments)
                });

                series.Add(new LineSeries()
                {
                    Title = "Dabartinė vertė",
                    Values = new ChartValues<double>(currentValues)
                });

                series.Add(new LineSeries()
                {
                    Title = "Grynasis pelnas",
                    Values = new ChartValues<double>(networth)
                });
            }

            if (chart == "crypto_quantities" || chart == "crypto_currentvalues")
            {
                var names = chartData.Select(x => x.name).Distinct().ToList();

                foreach (var coin in names)
                {
                    var data = chartData.Where(x => x.name == coin).Select(x => double.Parse(x.quantity.ToString("0.0000"))).ToList();

                    series.Add(new LineSeries()
                    {
                        Title = coin,
                        Values = new ChartValues<double>(data)
                    });
                }
            }

            chartView.Series = series;
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

        private static List<double> ReturnCurrentValuesList()
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            var cryptoTable = Connection.db.GetTable<CryptoTable>().ToList();
            List<double> currentValues = new List<double>();

            foreach (var date in uniqueDates)
            {
                var value = cryptoTable.Where(x => x.Date == date && (x.Operation == "BUY" || x.Operation == "SELL")).Select(x => double.Parse(x.LastCurrentValue.ToString())).ToList();
                currentValues.Add(value[value.Count - 1]);
            }

            return currentValues;
        }

        public static List<double> ReturnCurrentValuesList(List<DateTime> dates)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            var cryptoTable = Connection.db.GetTable<CryptoTable>().ToList();
            List<double> currentValues = new List<double>();

            foreach (var date in dates)
            {
                var value = cryptoTable.Where(x => x.Date == date && (x.Operation == "BUY" || x.Operation == "SELL")).Select(x => double.Parse(x.LastCurrentValue.ToString())).ToList();
                currentValues.Add(value[value.Count - 1]);
            }

            return currentValues;
        }

        private static List<double> ReturnSumList(DateTime date, string operation)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            List<double> sumList = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Date <= date && x.Operation == operation).Select(x => double.Parse(x.Sum.ToString())).ToList();
            return sumList;
        }

        private static List<double> ReturnInvestmentsList()
        {
            List<double> investments = new List<double>();

            foreach (var date in uniqueDates)
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
                .Where(x => x.Date >= start.Value && x.Date <= finish.Value && (x.Operation == "BUY" || x.Operation == "SELL"))
                .Select(x => x.Date).Distinct().ToList();

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

        private static void SetChartLabelsAndFormat(LiveCharts.WinForms.CartesianChart chartView, string chart, List<string> selectedCoins)
        {
            chartView.LegendLocation = LegendLocation.Top;

            chartView.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = ReturnLabels(uniqueDates)
            });

            chartView.AxisY.Add(ReturnYAxis(chart, selectedCoins));
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

                LabelFormatter = ReturnFormat(chart, selectedCoins)
            };

            if(chart == "crypto_quantities" || chart == "crypto_currentvalues")
            {
                yAxis.MinValue = 0;
            }

            return yAxis;
        }

        private static Func<double, string> ReturnFormat(string chart, List<string> selectedCoins)
        {
            Func<double, string> formatFunc = null;

            if (chart == "investments" || chart == "crypto_currentvalues")
            {
                formatFunc = (x) => x.ToString("C2");
            }

            if (chart == "crypto_quantities")
            {
                var list = ReturnChartData(uniqueDates, selectedCoins, true);
                
                if(list.Count > 0)
                {
                    formatFunc = (x) => x.ToString("0.0000");
                }
            }

            return formatFunc;
        }

    }
}
