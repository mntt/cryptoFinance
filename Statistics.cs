using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class Statistics : Form
    {
        private Form form { get; set; }
        private bool formOpened { get; set; }
        private string chart { get; set; }
        private SaveFileDialog sfd { get; set; }
        private DateTime startDate { get; set; }

        public Statistics(Form _form, DateTime _startDate)
        {
            InitializeComponent();
            form = _form;
            startDate = _startDate;
            datePickerStart.Value = startDate;
            formOpened = true;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.FormClosing -= Statistics_FormClosing;
            this.Close();
            form.Show();
        }

        private void Statistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private List<string> GetSelectedCoins()
        {
            List<string> selectedCoins = new List<string>();
            foreach (var item in coinList.CheckedItems)
            {
                var split = item.ToString().Split('{');
                selectedCoins.Add(split[1].Trim('}'));
            }

            return selectedCoins;
        }

        private async Task LoadChart()
        {
            try
            {
                nodataLabel.Visible = false;
                ConstructChart.Build(chart, chartView, datePickerStart, datePickerFinish, GetSelectedCoins());
                await Task.Delay(100);
            }
            catch
            {
                MessageBox.Show("Įvyko klaida. Nepavyko užkrauti grafiko.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowLoading()
        {
            CenterLoadingBox();
            loadingBox.Enabled = true;
            loadingBox.Visible = true;
        }

        private void HideLoading()
        {
            loadingBox.Enabled = false;
            loadingBox.Visible = false;
        }

        private void CenterLoadingBox()
        {
            loadingBox.Left = (this.ClientSize.Width - loadingBox.Width) / 2;
            loadingBox.Top = (this.ClientSize.Height - loadingBox.Height) / 2;
        }

        private async void InvestmentsChart_Click(object sender, EventArgs e)
        {
            if (chartView.Visible == false)
            {
                chart = "investments";
                ShowLoading();
                await LoadChart(); 
                chartView.Visible = true;
                HideLoading();
            }
            else
            {
                chartView.Visible = false;
            }
        }

        private async void DatePickerStart_ValueChanged(object sender, EventArgs e)
        {
            if (formOpened)
            {
                if (datePickerStart.Value <= datePickerFinish.Value)
                {
                    ShowLoading();
                    await LoadChart();
                    HideLoading();
                }
            } 
        }

        private async void DatePickerFinish_ValueChanged(object sender, EventArgs e)
        {
            if (formOpened)
            {
                if (datePickerFinish.Value >= datePickerStart.Value)
                {
                    ShowLoading();
                    await LoadChart();
                    HideLoading();
                } 
            }
        }

        private void CryptoQuantities_Click(object sender, EventArgs e)
        {
            chart = "crypto_quantities";
            TurnOnControls(false);
            coinList.Visible = true;
            apply.Visible = true;
        }

        private void RefreshCoinList()
        {
            var coins = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();
            var dates = Connection.db.GetTable<CryptoTable>().Select(x => x.Date).Distinct().ToList();
            var quantities = ConstructChart.ReturnChartData(dates, coins, true);

            coinList.Items.Clear();
            
            foreach(var coin in coins)
            {
                var sumQ = quantities.Where(x => x.name == coin).Select(x => x.quantity).ToList().Sum();

                if(sumQ > 0)
                {
                    coinList.Items.Add(coin);
                }
            }   
        }

        private void CoinList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            } 
        }

        private async void Apply_Click(object sender, EventArgs e)
        {
            coinList.Visible = false;
            apply.Visible = false;

            if (coinList.CheckedItems.Count > 0)
            {
                ShowLoading();
                await LoadChart();
                chartView.Visible = true;
                HideLoading();
            }
            else
            {
                chartView.Visible = false;
            }

            TurnOnControls(true);
        }

        private void TurnOnControls(bool switchEnable)
        {
            investmentsChartButton.Enabled = switchEnable;
            cryptoQuantities.Enabled = switchEnable;
            cryptoNetValues.Enabled = switchEnable;
            exportData.Enabled = switchEnable;
            datePickerStart.Enabled = switchEnable;
            datePickerFinish.Enabled = switchEnable;
            backButton.Enabled = switchEnable;
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            ListViewSettings.Format(coinList);
            ListViewSettings.Format(exportList);
            ListViewSettings.SetColumnWidth(coinList, 21, 217, coinList.Items.Count);
            ListViewSettings.SetColumnWidth(exportList, 21, 86, exportList.Items.Count);

            TurnOnControls(false);
            ShowLoading();
            loadWorker.RunWorkerAsync();
        }

        private void LoadWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            RefreshCoinList();
        }

        private void LoadWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            HideLoading();
            TurnOnControls(true);
        }

        private void CryptoNetValues_Click(object sender, EventArgs e)
        {
            chart = "crypto_currentvalues";
            TurnOnControls(false);
            coinList.Visible = true;
            apply.Visible = true;
        }

        private void ExportData_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in exportList.CheckedItems)
            {
                item.Checked = false;
            }

            exportPanel.Visible = true;
            closeButton.Visible = true; 
        }

        private void ExportListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            closeButton.Visible = false;
            exportPanel.Visible = false; 
        }

        private List<string> GetAllCoins()
        {
            List<string> allCoins = new List<string>();
            
            foreach (var coin in coinList.Items)
            {
                var split = coin.ToString().Split('{');
                var name = split[1].Trim('}');
                allCoins.Add(name);
            }

            return allCoins;
        }

        private DataTable CreateTable(string tableName, List<TableColumnList> columnNameList)
        {
            DataTable table = new DataTable();

            foreach(var item in columnNameList)
            {
                table.Columns.Add(item.name, item.type);
            }

            table.TableName = tableName;

            return table;
        }

        private DataTable ReturnInvestmentsTable()
        {
            List<TableColumnList> columnNameList = new List<TableColumnList>();

            TableColumnList column1 = new TableColumnList("Data", typeof(DateTime));
            TableColumnList column2 = new TableColumnList("Investicijos", typeof(string));
            TableColumnList column3 = new TableColumnList("Dabartinė vertė", typeof(string));
            TableColumnList column4 = new TableColumnList("Grynasis pelnas", typeof(string));
            columnNameList.Add(column1);
            columnNameList.Add(column2);
            columnNameList.Add(column3);
            columnNameList.Add(column4);

            var table = CreateTable("Investicijos", columnNameList);
            var dates = Connection.db.GetTable<CryptoTable>().Where(x => x.Operation == "BUY" || x.Operation == "SELL").Select(x => x.Date).Distinct().ToList();
            var investments = ConstructChart.ReturnInvestmentsList(dates);
            var currentValues = ConstructChart.ReturnCurrentValuesList(dates);
            var netWorth = ConstructChart.ReturnNetWorthData(investments, currentValues);

            for (int i = 0; i < dates.Count; i++)
            {
                DataRow row = table.NewRow();
                row[columnNameList[0].name] = dates[i];
                row[columnNameList[1].name] = investments[i].ToString("C2");
                row[columnNameList[2].name] = currentValues[i].ToString("C2");
                row[columnNameList[3].name] = netWorth[i].ToString("C2");
                table.Rows.Add(row);
            }

            return table;
        }

        private List<TableColumnList> ReturnColumnList(List<string> names)
        {
            List<TableColumnList> columnNameList = new List<TableColumnList>();
            TableColumnList column1 = new TableColumnList("Data", typeof(DateTime));
            columnNameList.Add(column1);
            for (int i = 0; i < names.Count; i++)
            {
                TableColumnList column = new TableColumnList(names[i], typeof(double));
                columnNameList.Add(column);
            }

            return columnNameList;
        }

        private DataTable ReturnQuantitiesTable()
        {
            var dates = Connection.db.GetTable<CryptoTable>().Select(x => x.Date).Distinct().ToList();
            var quantities = ConstructChart.ReturnChartData(dates, GetAllCoins(), true);
            var names = quantities.Select(x => x.name).Distinct().ToList();
            var columnNameList = ReturnColumnList(names);
            var table = CreateTable("Kriptovaliutų kiekiai", columnNameList);

            for (int i = 0; i < dates.Count; i++)
            {
                DataRow row = table.NewRow();
                row[columnNameList[0].name] = dates[i];

                for (int c = 1; c < table.Columns.Count; c++)
                {
                    var q = quantities.Where(x => x.date == dates[i] && x.name == table.Columns[c].ColumnName).Select(x => x.quantity).ToList();
                    row[table.Columns[c].ColumnName] = q[0];
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private DataTable ReturnCurrentValuesTable()
        {
            GetCultureInfo info = new GetCultureInfo(".");
            var dates = Connection.db.GetTable<CryptoTable>().Select(x => x.Date).Distinct().ToList();
            var currentValues = ConstructChart.ReturnChartData(dates, GetAllCoins(), false);
            var names = currentValues.Select(x => x.name).Distinct().ToList();
            var columnNameList = ReturnColumnList(names);
            var table = CreateTable("Kriptovaliutų grynosios vertės", columnNameList);

            for (int i = 0; i < dates.Count; i++)
            {
                DataRow row = table.NewRow();
                row[columnNameList[0].name] = dates[i];

                for (int c = 1; c < table.Columns.Count; c++)
                {
                    var cv = currentValues.Where(x => x.date == dates[i] && x.name == table.Columns[c].ColumnName).Select(x => x.quantity).ToList();
                    row[table.Columns[c].ColumnName] = cv[0];
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private List<DataTable> ReturnTables()
        {
            List<DataTable> listOfTables = new List<DataTable>();

            if (exportList.Items[0].Checked)
            {
                listOfTables.Add(ReturnInvestmentsTable());
            }

            if (exportList.Items[1].Checked)
            {          
                listOfTables.Add(ReturnQuantitiesTable());
            }

            if (exportList.Items[2].Checked)
            {
                listOfTables.Add(ReturnCurrentValuesTable());
            }

            return listOfTables;
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            TurnOnControls(false);
            confirmButton.Enabled = false;

            sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                closeButton.Enabled = false;
                loadingBox.Left = (this.ClientSize.Width - loadingBox.Width) / 2;
                loadingBox.Top = (this.ClientSize.Height - loadingBox.Height) / 2;
                loadingBox.Visible = true;
                exportWorker.RunWorkerAsync();
            }
        }

        private void ExportWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    foreach (var table in ReturnTables())
                    {
                        workbook.Worksheets.Add(table, table.TableName);
                    }
                    workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Failas sėkmingai iškeltas.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Įvyko klaida. Failo iškelti nepavyko", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            exportPanel.Visible = false;
            closeButton.Visible = false;
            confirmButton.Enabled = true;
            closeButton.Enabled = true;
            HideLoading();
            TurnOnControls(true);
        }

        private void DatePickerFinish_CloseUp(object sender, EventArgs e)
        {
            if (datePickerFinish.Value > DateTime.Today)
            {
                MessageBox.Show("Negalima rinktis vėlesnės datos nei šiandien.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                datePickerFinish.Value = DateTime.Today;
            }

            if (datePickerFinish.Value < datePickerStart.Value)
            {
                MessageBox.Show("Negalima rinktis ankstesnės datos nei pradinė data.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                datePickerFinish.Value = DateTime.Today;
            }
        }

        private void DatePickerStart_CloseUp(object sender, EventArgs e)
        {
            if (datePickerStart.Value > datePickerFinish.Value)
            {
                MessageBox.Show("Negalima rinktis vėlesnės datos nei šiandien.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                datePickerStart.Value = startDate;
            }
        }

    }
}
