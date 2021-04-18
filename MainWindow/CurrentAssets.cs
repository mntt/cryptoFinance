using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class CurrentAssets : Form
    { 
        private bool useRefreshButton = false;
        private List<string> cannotUpdatePriceList = new List<string>();
        private List<ConstructingLists> coinList = new List<ConstructingLists>();
        private List<ConstructingLists> temporaryList = new List<ConstructingLists>();
        private DateTime startDate { get; set; }

        public CurrentAssets()
        {
            InitializeComponent(); 
        }
     
        public void CreateCoinList()
        {
            var coins = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.Quantity > 0).ToList();

            foreach (var item in coins)
            {
                string id = "custom";
                if (!item.CustomCoin)
                {
                    var nameSplit = item.Cryptocurrency.Split('(');
                    id = Connection.db.GetTable<CoingeckoCryptoList>()
                        .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                        .Select(x => x.CryptoId).ToList().First();
                }

                ConstructingLists info = new ConstructingLists
                    (
                        DateTime.Now,
                        item.Cryptocurrency,
                        item.CustomCoin,
                        id,
                        item.Quantity,
                        (double)item.Price,
                        (double)item.CurrentValue
                    );
                coinList.Add(info);
            }
        }

        private void CurrentAssets_Load(object sender, EventArgs e)
        {
            TogglePriceAlert();        
        }

        public void CountCurrentValue()
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            double currentValue = Connection.db.GetTable<CurrentAssetsDB>()
                .Select(x => double.Parse(x.CurrentValue.ToString())).ToList().Sum();

            currentValueLabel.Text = currentValue.ToString("C2");
        }

        public void UpdateDataGrid()
        {
            var data = coinList.Where(x => x.quantity > 0).OrderByDescending(x => x.totalSum).ToList();

            DataTable table = new DataTable();
            table.Columns.Add("Kriptovaliuta", typeof(string));
            table.Columns.Add("Kiekis", typeof(string));
            table.Columns.Add("Kaina", typeof(string));
            table.Columns.Add("Grynoji vertė", typeof(double));

            foreach (var item in data)
            {
                DataRow row = table.NewRow();
                row["Kriptovaliuta"] = item.name;
                row["Kiekis"] = item.quantity.ToString("0.00000000");
                row["Kaina"] = item.price.ToString("C4");
                row["Grynoji vertė"] = item.totalSum;
                table.Rows.Add(row);
            }

            dataGridCurrentAssets.Visible = false;
            dataGridCurrentAssets.DataSource = null;
            dataGridCurrentAssets.DataSource = table;
            DataGridViewSettings.FormatCurrentAssets(dataGridCurrentAssets); 
            dataGridCurrentAssets.Visible = true;
        }

        public void UpdatePricesAndCurrentValue()
        {
            var task = Task.Run(() => InsertPricesToDatabase());
            task.Wait();
        }

        private ConstructingLists CreateObject(ConstructingLists item, DateTime today, double price, double currentValue)
        {
            ConstructingLists obj = new ConstructingLists
                                    (
                                        today,
                                        item.name,
                                        item.customCoin,
                                        item.id,
                                        item.quantity,
                                        price,
                                        currentValue
                                    );

            return obj;
        }

        private async Task FetchPrices()
        {
            foreach (var item in coinList)
            {
                double currentValue = 0;
                double price = 0;
                DateTime today = DateTime.Now;

                if (!item.customCoin)
                {
                    var result = GetPrices.ById(item.id);

                    switch (result)
                    {
                        case -1:
                            cannotUpdatePriceList.Add(item.id);
                            break;
                        case -2:
                            price = Connection.db.GetTable<CurrentAssetsDB>()
                                .Where(x => x.Cryptocurrency == item.name).Select(x => (double)x.Price).ToList().First();
                            break;
                        default:
                            price = result;
                            break;
                    }
                }
                else
                {
                    price = Connection.db.GetTable<CurrentAssetsDB>()
                        .Where(x => x.Cryptocurrency == item.name).Select(x => (double)x.Price).ToList().First();
                }

                currentValue = price * item.quantity;
                temporaryList.Add(CreateObject(item, today, price, currentValue));
            }
                
            await Task.Delay(100);
        }

        private async Task InsertPricesToDatabase()
        {
            cannotUpdatePriceList.Clear();
            temporaryList.Clear();
            int timeout = 60000;
            var timeoutcancel = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, timeoutcancel.Token);
            var task = FetchPrices();

            if (await Task.WhenAny(task, delayTask) == task)
            {
                timeoutcancel.Cancel();
                coinList.Clear();
                coinList.AddRange(temporaryList);

                if (cannotUpdatePriceList.Count == 0)
                {
                    foreach (var item in coinList)
                    {
                        Connection.iwdb.UpdateCurrentAssets(false, item.name, item.quantity, DateTime.Now, item.price, item.totalSum);
                        Connection.iwdb.UpdatePrice(DateTime.Today, item.name, 0, "UpdatePrice", 0, item.price, 0);
                    }
                } 
            }
            else
            {
                MessageBox.Show("Nepavyko pasiekti coingecko API per 1 minutę.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        public void EnableAllButtons()
        {
            chartButton.Enabled = true;
            investButton.Enabled = true;
            walletsButton.Enabled = true;
            statisticsButton.Enabled = true;
            refreshPricesButton.Enabled = true;
        }

        private void AlertPanelControlInstance(int labelTextNumber)
        {
            AlertPanelControl apc = new AlertPanelControl(alertPanel, alertLabel, 418, -38, 170, 38, 29, 13);
            apc.StartPanelAnimation(labelTextNumber);
        }

        private void TogglePriceAlert()
        {
            if (!alertPanel.Visible && cannotUpdatePriceList.Count > 0)
            {
                AlertPanelControlInstance(0);
            }
        }

        private void CurrentAssets_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void DisableAllButtons()
        {
            refreshPricesButton.Enabled = false;
            chartButton.Enabled = false;
            investButton.Enabled = false;
            walletsButton.Enabled = false;
            statisticsButton.Enabled = false;
        }

        private async Task RefreshData()
        {
            cannotUpdatePriceList.Clear();
            UpdatePricesAndCurrentValue();
            UpdateDataGrid();
            CountCurrentValue();
            await Task.Delay(100);
        }

        public List<string> GetCannotUpdatePriceList()
        {
            return cannotUpdatePriceList;
        }

        public async Task GetCurrentValue()
        {
            await Task.Run(() => RefreshData());
        }

        private void ShowLoading()
        {
            loadingBox.Enabled = true;
            loadingBox.Visible = true;
        }

        private void HideLoading()
        {
            loadingBox.Enabled = false;
            loadingBox.Visible = false;
        }

        private async void RefreshPricesButton_Click(object sender, EventArgs e)
        {
            if (refreshPricesButton.Enabled)
            {
                DisableAllButtons();
                ShowLoading();
                await Task.Run(() => RefreshData());
                if (pieChart.Visible)
                {
                    await LoadChart();
                }

                TogglePriceAlert();
                if (!alertPanel.Visible)
                {
                    EnableAllButtons();
                }
                HideLoading();
            }            
        }

        private async Task LoadChart()
        {
            ConstructPieChart();
            await Task.Delay(100);
        }

        private async void ChartButton_Click(object sender, EventArgs e)
        {
            if (pieChart.Visible == false)
            {
                loadingBox.Visible = true;
                await LoadChart();
                pieChart.Visible = true;
                loadingBox.Visible = false;
            }
            else
            {
                pieChart.Visible = false;
            } 
        }

        private void DataGridCurrentAssets_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*if(e.ColumnIndex == dataGridCurrentAssets.Columns[4].Index && cannotUpdatePriceList.Count == 0)
            {
                var cell = dataGridCurrentAssets.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = "Kaina sėkmingai atnaujinta!";
            }
            else if(e.ColumnIndex == dataGridCurrentAssets.Columns[4].Index && cannotUpdatePriceList.Count > 0)
            {
                var cell = dataGridCurrentAssets.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = "Kainos atnaujinti nepavyko!";
            }*/
        }

        private void InvestButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Investments inv = new Investments(this);
            inv.Show();
        }

        private void WalletsButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageWallets manageWallets = new ManageWallets(this);
            manageWallets.Show();
        }

        public void SetStartDate()
        {
            var today = DateTime.Today;
            var startDate2 = today.AddDays(-60);
            var uniqueDates = Connection.db.GetTable<CryptoTable>().Take(1).Select(x => x.Date).ToList();

            if (uniqueDates.Count > 0)
            {
                startDate = uniqueDates[0];
            }
            else
            {
                startDate = startDate2;
            }
        }

        private void StatisticsButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Statistics stats = new Statistics(this, startDate);
            stats.Show();
        }

        private void RefreshDataGridTimer_Tick(object sender, EventArgs e)
        {            
            refreshPricesButton.PerformClick();
            refreshDataGridTimer.Stop();
        }

        private void CurrentAssets_VisibleChanged(object sender, EventArgs e)
        {
            if (useRefreshButton)
            {
                refreshDataGridTimer.Start();
            }
            else
            {
                useRefreshButton = true;
            }
        }

        private void AlertPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!alertPanel.Visible)
            {
                EnableAllButtons();
            }
            else
            {
                DisableAllButtons();
            }
        }

        private SeriesCollection CreateSeries(List<ConstructingLists> coins, double total, Func<ChartPoint, string> labelPoint)
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
                });
            }

            return series;
        }

        public void ConstructPieChart()
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            try
            {
                pieChart.Series.Clear();
                var coins = coinList.Where(x => x.quantity > 0).OrderByDescending(x => x.totalSum).ToList();
                Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0:C2}", chartPoint.Y);
                var total = coins.Select(x => x.totalSum).ToList().Sum();

                var tooltip = new DefaultTooltip
                {
                    SelectionMode = TooltipSelectionMode.OnlySender
                };

                pieChart.Series = CreateSeries(coins, total, labelPoint);
                pieChart.LegendLocation = LegendLocation.Right;
                pieChart.DataTooltip = tooltip;
            }
            catch
            {
                MessageBox.Show("Įvyko nenumatyta klaida. Bandykite dar kartą.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData(ConstructingLists coinObject)
        {
            var search = coinList.Where(x => x.name == coinObject.name).ToList();

            if (search.Count > 0)
            {
                coinList.Where(x => x.name == coinObject.name).ToList().ForEach(x => x.quantity = coinObject.quantity);
            }
            else
            {
                coinList.Add(coinObject);
            }
        }

    }
}
