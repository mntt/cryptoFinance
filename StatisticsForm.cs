using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class StatisticsForm
    {
        private CurrentAssets ca { get; set; }
        private int errorExporting = 0;
        private bool openTimeBox = false;
        private bool clicked = false;
        private string listname = "";
        private List<string> list = new List<string>();
        private List<string> backupCoinList = new List<string>();

        public StatisticsForm(CurrentAssets _ca)
        {
            ca = _ca;
            ca.coinListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.coinListView_ItemSelectionChanged);
            ca.exportData.Click += new System.EventHandler(this.exportData_Click);
            ca.exportList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.exportList_ItemSelectionChanged);
            ca.backToStats.Click += new System.EventHandler(this.backToStats_Click);
            ca.datePickerStart.CloseUp += new System.EventHandler(this.datePickerStart_CloseUp_1);
            ca.datePickerFinish.CloseUp += new System.EventHandler(this.datePickerFinish_CloseUp_1);
            ca.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            ca.exportBorder.Paint += new System.Windows.Forms.PaintEventHandler(this.exportBorder_Paint);
            ca.searchListBox.TextChanged += new System.EventHandler(SearchListBox_TextChanged);

            ca.dropdownInvestments.MouseEnter += new System.EventHandler(DropdownInvestments_MouseEnter);
            ca.dropdownInvestments.MouseLeave += new System.EventHandler(DropdownInvestments_MouseLeave);
            ca.dropdownInvestments.Click += new System.EventHandler(DropdownInvestments_Click);
            ca.variablesPanel.VisibleChanged += new System.EventHandler(VariablesPanel_VisibleChanged);

            ca.investmentsListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.investmentsListView_ItemSelectionChanged);





            ListViewSettings.Format(ca.investmentsListView);
            ListViewSettings.Format(ca.coinListView);
            ListViewSettings.Format(ca.exportList);
            ListViewSettings.SetListViewSizes(ca.investmentsListView);
            ListViewSettings.SetListViewSizes(ca.coinListView);
            ListViewSettings.SetListViewSizes(ca.exportList);
            SetStartDate(ca);

            //worker 
            RefreshCoinQuantitiesList(ca.coinListView);
            ca.investmentsListView.Items.Add("Investicijos");
            ca.investmentsListView.Items.Add("Dabartinė vertė");
            ca.investmentsListView.Items.Add("Grynasis pelnas");
            ca.investmentsListView.Items[0].Checked = true;
            ca.investmentsListView.Items[1].Checked = true;
            ca.investmentsListView.Items[2].Checked = true;
        }

        private void VariablesPanel_VisibleChanged(object sender, EventArgs e)
        {
            /*if (ca.variablesPanel.Visible)
            {
                ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbutton;
            }*/
        }

        private void SearchListBox_TextChanged(object sender, EventArgs e)
        {
            ca.datePickerStart.ValueChanged -= new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged -= new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);

            if (ca.searchListBox.Text.Length > 0)
            {
                if (listname == "investments")
                {
                    list.Clear();
                    List<string> tempList = new List<string>();
                    tempList.Add("Investicijos");
                    tempList.Add("Dabartinė vertė");
                    tempList.Add("Grynasis pelnas");

                    list = tempList.Where(x => x.ToLower().Contains(ca.searchListBox.Text.ToLower())).ToList();
                    ca.investmentsListView.Items.Clear();

                    foreach (var item in list)
                    {
                        ca.investmentsListView.Items.Add(item);
                    }
                }

                if (listname == "coins")
                {
                    list.Clear();

                    list = backupCoinList.Where(x => x.ToLower().Contains(ca.searchListBox.Text.ToLower())).ToList();
                    ca.coinListView.Items.Clear();

                    foreach (var item in list)
                    {
                        ca.coinListView.Items.Add(item);
                    }
                }
            }
            else
            {
                if (listname == "investments")
                {
                    ca.investmentsListView.Items.Clear();
                    ca.investmentsListView.Items.Add("Investicijos");
                    ca.investmentsListView.Items.Add("Dabartinė vertė");
                    ca.investmentsListView.Items.Add("Grynasis pelnas");
                }

                if (listname == "coins")
                {
                    ca.coinListView.Items.Clear();

                    foreach (var item in backupCoinList)
                    {
                        ca.coinListView.Items.Add(item);
                    }
                }
            }

            ca.datePickerStart.ValueChanged += new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged += new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);
        }

        private void DropdownInvestments_Click(object sender, EventArgs e)
        {
            if (!clicked)
            {
                clicked = true;
            }
            else
            {
                clicked = false;
            }

            ca.datePickerStart.ValueChanged -= new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged -= new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);

            ca.searchListBox.Text = "";

            if (ca.variablesPanel.Visible)
            {
                ca.variablesPanel.Visible = false;
                //ca.investmentsListView.Visible = false;
                //ca.coinListView.Visible = false;
                //ca.searchListBox.Visible = false;
            }
            else
            {
                if (listname == "investments" && !ca.variablesPanel.Visible)
                {
                    ca.variablesPanel.Visible = true;
                    ca.coinListView.Visible = false;
                    ca.investmentsListView.Visible = true;
                    //ca.searchListBox.Visible = true;
                }

                if (listname == "coins" && !ca.variablesPanel.Visible)
                {
                    ca.variablesPanel.Visible = true;
                    ca.investmentsListView.Visible = false;
                    ca.coinListView.Visible = true;
                }
            }

            ca.datePickerStart.ValueChanged += new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged += new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);
        }

        private void DropdownInvestments_MouseEnter(object sender, EventArgs e)
        {
            ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbuttonSelected;
        }

        private void DropdownInvestments_MouseLeave(object sender, EventArgs e)
        {
            if (clicked)
            {
                ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbuttonSelected;
            }
            else
            {
                ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbutton;
            }
        }

        private void coinListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void exportData_Click(object sender, EventArgs e)
        {
            OpenExport(ca);
        }

        private void exportList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ExportListItemSelectionChange(e);
        }

        private void backToStats_Click(object sender, EventArgs e)
        {
            BackToStats(ca);
        }

        private void datePickerStart_CloseUp_1(object sender, EventArgs e)
        {
            DatePickerStartCloseUp(ca);
        }

        private void datePickerStart_ValueChanged_1(object sender, EventArgs e)
        {
            DatePickerStartValueChange(ca, ca.chart);
        }

        private void datePickerFinish_CloseUp_1(object sender, EventArgs e)
        {
            DatePickerFinishCloseUp(ca);
        }

        private void datePickerFinish_ValueChanged_1(object sender, EventArgs e)
        {
            DatePickerFinishValueChange(ca, ca.chart);
        }

        private async void exportButton_Click(object sender, EventArgs e)
        {
            ca.ShowLoading();

            foreach (Control item in ca.Controls)
            {
                if (item.Name != "loadingBox")
                {
                    item.Enabled = false;
                }
            }

            ca.sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" };

            if (ca.exportList.CheckedItems.Count > 0)
            {

                ca.sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" };

                if (ca.sfd.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() => ExportProgress(ca));
                }
            }
            else
            {
                errorExporting = 2;
            }

            foreach (Control item in ca.Controls)
            {
                item.Enabled = true;
            }
            ExecuteAlertPanel();
            ca.HideLoading();
        }

        private void ExecuteAlertPanel()
        {
            if (errorExporting == 0)
            {
                AlertPanelControlInstance(25);
                ca.backToStats.PerformClick();
            }
            else if (errorExporting == 1)
            {
                AlertPanelControlInstance(14);
                ca.backToStats.PerformClick();
            }
            else if (errorExporting == 2)
            {
                MessageBox.Show("Nepasirinkote kokią ataskaitą norite iškelti.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void coinListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            CoinListItemCheck(ca);
        }

        private void exportBorder_Paint(object sender, PaintEventArgs e)
        {
            ca.exportBorder.BackColor = Colours.grid;
        }

        private void investmentsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            InvestmentListViewItemSelectionChange(e);
        }

        private void investmentsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            InvestmentListItemCheck(ca);
        }

        public void Open(CurrentAssets form)
        {
            if (!form.statisticsPanel.Visible)
            {
                form.datePickerStart.Value = form.startDate;
                form.datePickerFinish.Value = DateTime.Now;
                form.statisticsPanel.Location = new Point(117, 102);

                form.chartView.Visible = false;
                //form.exportList.Visible = false;
                //form.coinListView.Visible = false;
                form.backToStats.Visible = false;
                form.dropdownInvestments.Visible = false;
                //form.exportButton.Visible = false;
                form.exportdataPanel.Visible = false;
                //form.variablesPanel.Visible = false;
                form.variablesPanel.Visible = false;
                //form.investmentsListView.Visible = false;
                //form.searchListBox.Visible = false;
                //form.exportBorder.Visible = false;
                form.dashLabel.Visible = false;
                form.datePickerStart.Visible = false;
                form.datePickerFinish.Visible = false;
                form.investmentsListView.Visible = false;
                form.dashLabel.Select();
                SwitchButtonVisibility(form, true);

                form.statisticsPanel.Visible = true;
            }
            else
            {
                form.statisticsPanel.Visible = false;
            }
        }

        private void SwitchButtonVisibility(CurrentAssets form, bool switchVisibility)
        {
            form.statsmenuPanel.Visible = switchVisibility;
            //form.investmentsChartButton.Visible = switchVisibility;
            //form.cryptoQuantities.Visible = switchVisibility;
            //form.cryptoNetValues.Visible = switchVisibility;
            //form.exportData.Visible = switchVisibility;
        }

        public async void OpenInvestments(CurrentAssets form, string chart, Action showLoading)
        {
            ca.datePickerStart.ValueChanged -= new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged -= new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);

            clicked = false;
            ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbutton;
            listname = "investments";

            SwitchButtonVisibility(form, false);
            form.dashLabel.Visible = true;
            form.datePickerStart.Visible = true;
            form.datePickerFinish.Visible = true;
            form.backToStats.Visible = true;
            form.dropdownInvestments.Visible = true;
            form.variablesPanel.Visible = false;


            if (form.investmentsListView.CheckedItems.Count > 0)
            {
                await LoadStatsChart(form, chart);
            }
            else
            {
                form.nodataLabel.Visible = true;
                form.nodataLabel.Location = new Point(250, 150);
            }

            ca.datePickerStart.ValueChanged += new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged += new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);
            /*if (form.chartView.Visible)
            {
                form.chartView.Visible = false;
            }

            if (!form.chartView.Visible)
            {
                form.chartView.Select();
                
                
                
                
            }*/
        }

        private async void LoadCryptoQuantities(CurrentAssets form, string chart)
        {
            ca.datePickerStart.ValueChanged -= new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged -= new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);

            form.chartView.Select();
            SwitchButtonVisibility(form, false);

            for (int i = 0; i < form.coinListView.Items.Count; i++)
            {
                form.coinListView.Items[i].BackColor = Colours.formBackground;
            }

            //form.variablesPanel.Visible = false;
            form.backToStats.Visible = true;
            form.dropdownInvestments.Visible = true;
            form.dashLabel.Visible = true;
            form.datePickerStart.Visible = true;
            form.datePickerFinish.Visible = true;


            if (form.coinListView.CheckedItems.Count > 0)
            {
                await LoadStatsChart(form, chart);
            }
            else
            {
                form.nodataLabel.Visible = true;
                form.nodataLabel.Location = new Point(250, 150);
            }
            ca.datePickerStart.ValueChanged += new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged += new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);

        }

        public void OpenCryptoQuantities(CurrentAssets form, string chart, Action showLoading)
        {
            clicked = false;
            ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbutton;
            listname = "coins";
            LoadCryptoQuantities(form, chart);
        }

        private async void LoadNetValues(CurrentAssets form, string chart)
        {
            ca.datePickerStart.ValueChanged -= new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged -= new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);


            form.chartView.Select();
            SwitchButtonVisibility(form, false);
            //form.variablesPanel.Visible = false;
            form.backToStats.Visible = true;
            form.dropdownInvestments.Visible = true;
            form.dashLabel.Visible = true;
            form.datePickerStart.Visible = true;
            form.datePickerFinish.Visible = true;


            if (form.coinListView.CheckedItems.Count > 0)
            {
                await LoadStatsChart(form, chart);
            }
            else
            {
                form.nodataLabel.Visible = true;
                form.nodataLabel.Location = new Point(250, 150);
            }

            ca.datePickerStart.ValueChanged += new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged += new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);

        }

        public void OpenNetValues(CurrentAssets form, string chart, Action showLoading)
        {
            clicked = false;
            ca.dropdownInvestments.BackgroundImage = cryptoFinance.Properties.Resources.dropdownbutton;
            listname = "coins";
            LoadNetValues(form, chart);
        }

        private void OpenExport(CurrentAssets form)
        {


            foreach (ListViewItem item in form.exportList.CheckedItems)
            {
                item.Checked = false;
            }

            form.exportdataPanel.Visible = true;
            //form.exportList.BringToFront();
            //form.exportList.Select();
            SwitchButtonVisibility(form, false);
            //form.exportList.Visible = true;
            //form.exportBorder.Visible = true;
            //form.exportButton.Visible = true;
            form.exportData.Visible = true;
            form.backToStats.Visible = true;
            form.dropdownInvestments.Visible = false;
        }

        private void SetStartDate(CurrentAssets form)
        {
            var today = DateTime.Today;
            var startDate2 = today.AddDays(-60);
            var uniqueDates = Connection.db.GetTable<CryptoTable>().Take(1).Select(x => x.Date).ToList();

            if (uniqueDates.Count > 0)
            {
                if (uniqueDates[0] > startDate2)
                {
                    form.startDate = uniqueDates[0];
                }
                else
                {
                    form.startDate = startDate2;
                }
            }
            else
            {
                form.startDate = today;
            }
        }

        private List<string> GetSelectedCoins(ListView coinListView)
        {
            List<string> selectedCoins = new List<string>();
            foreach (var item in coinListView.CheckedItems)
            {
                var split = item.ToString().Split('{');
                selectedCoins.Add(split[1].Trim('}'));
            }

            return selectedCoins;
        }

        private async Task LoadStatsChart(CurrentAssets form, string chart)
        {
            form.ShowLoading();
            try
            {
                form.chartView.Visible = false;
                form.nodataLabel.Visible = false;

                if (chart == "investments")
                {
                    List<string> selectedItems = new List<string>();
                    foreach (var item in form.investmentsListView.CheckedItems)
                    {
                        var split = item.ToString().Split('{');
                        selectedItems.Add(split[1].Trim('}'));
                    }

                    ConstructChart.Build(chart, form.chartView, form.datePickerStart, form.datePickerFinish, selectedItems);
                }
                else if (chart == "crypto_quantities" || chart == "crypto_currentvalues")
                {
                    ConstructChart.Build(chart, form.chartView, form.datePickerStart, form.datePickerFinish, GetSelectedCoins(form.coinListView));
                }

                await Task.Delay(100);
                form.chartView.Visible = true;
            }
            catch
            {
                MessageBox.Show("Įvyko nenumatyta klaida. Nepavyko užkrauti grafiko.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            await Task.Delay(100);
            form.HideLoading();
        }

        private void RefreshCoinQuantitiesList(ListView coinListView)
        {
            var coins = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();
            var dates = Connection.db.GetTable<CryptoTable>().Where(x => x.Operation == "BUY" || x.Operation == "SELL")
                .Select(x => x.Date).ToList();
            //sita vieta suziureti, ar gerai surenkami duomenys
            var quantities = ConstructChart.ReturnChartData(dates, coins, true);

            coinListView.Items.Clear();
            backupCoinList.Clear();

            foreach (var coin in coins)
            {
                var sumQ = quantities.Where(x => x.name == coin).Select(x => x.quantity).ToList().Sum();

                if (sumQ > 0)
                {
                    coinListView.Items.Add(coin);
                    backupCoinList.Add(coin);
                }
            }
        }

        private List<string> GetAllCoins(ListView coinListView)
        {
            List<string> allCoins = new List<string>();

            foreach (var coin in coinListView.Items)
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

            foreach (var item in columnNameList)
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

        private DataTable ReturnQuantitiesTable(ListView coinListView)
        {
            var dates = Connection.db.GetTable<CryptoTable>().Select(x => x.Date).Distinct().ToList();
            var quantities = ConstructChart.ReturnChartData(dates, GetAllCoins(coinListView), true);
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

        private DataTable ReturnCurrentValuesTable(ListView coinListView)
        {
            GetCultureInfo info = new GetCultureInfo(".");
            var dates = Connection.db.GetTable<CryptoTable>().Select(x => x.Date).Distinct().ToList();
            var currentValues = ConstructChart.ReturnChartData(dates, GetAllCoins(coinListView), false);
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

        private List<DataTable> ReturnTables(ListView exportList, ListView coinListView)
        {
            List<DataTable> listOfTables = new List<DataTable>();

            if (exportList.Items[0].Checked)
            {
                listOfTables.Add(ReturnInvestmentsTable());
            }

            if (exportList.Items[1].Checked)
            {
                listOfTables.Add(ReturnQuantitiesTable(coinListView));
            }

            if (exportList.Items[2].Checked)
            {
                listOfTables.Add(ReturnCurrentValuesTable(coinListView));
            }

            return listOfTables;
        }

        private void ExportProgress(CurrentAssets form)
        {
            try
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    foreach (var table in ReturnTables(form.exportList, form.coinListView))
                    {
                        workbook.Worksheets.Add(table, table.TableName);
                    }
                    workbook.SaveAs(form.sfd.FileName);
                }

                errorExporting = 0;             
            }
            catch
            {
                errorExporting = 1;               
            }
        }

        private void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(ca.alertPanel, ca.alertLabel, 645, -38, 276, 38, 29, 13);
            apc.StartPanelAnimation(chooseLabelText);
        }

        private void DatePickerStartCloseUp(CurrentAssets form)
        {
            form.dashLabel.Select();
            if (form.datePickerStart.Value > form.datePickerFinish.Value)
            {
                MessageBox.Show("Negalima rinktis vėlesnės datos nei šiandien.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.datePickerStart.Value = form.startDate;
            }
        }

        private async void DatePickerStartValueChange(CurrentAssets form, string chart)
        {
            if (form.datePickerStart.Value <= form.datePickerFinish.Value && chart != "" && form.chartView.Visible)
            {
                await LoadStatsChart(form, chart);
            }
        }

        private void DatePickerFinishCloseUp(CurrentAssets form)
        {
            if (form.datePickerFinish.Value > DateTime.Now)
            {
                MessageBox.Show("Negalima rinktis vėlesnės datos nei šiandien.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.datePickerFinish.Value = DateTime.Now;
            }

            if (form.datePickerFinish.Value < form.datePickerStart.Value)
            {
                MessageBox.Show("Negalima rinktis ankstesnės datos nei pradinė data.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form.datePickerFinish.Value = DateTime.Now;
            }
        }

        private async void DatePickerFinishValueChange(CurrentAssets form, string chart)
        {
            if (form.datePickerFinish.Value >= form.datePickerStart.Value && chart != "" && form.chartView.Visible)
            {
                await LoadStatsChart(form, chart);
            }
        }

        private async void CoinListItemCheck(CurrentAssets form)
        {
            if (form.coinListView.CheckedItems.Count > 0)
            {
                await LoadStatsChart(form, form.chart);
            }
            else
            {
                form.chartView.Visible = false;
                form.nodataLabel.Visible = true;
                form.nodataLabel.Location = new Point(250, 150);
            }
        }

        private async void InvestmentListItemCheck(CurrentAssets form)
        {
            if (form.investmentsListView.CheckedItems.Count > 0)
            {
                await LoadStatsChart(form, form.chart);
            }
            else
            {
                form.chartView.Visible = false;
                form.nodataLabel.Visible = true;
                form.nodataLabel.Location = new Point(250, 150);
            }
        }

        private void BackToStats(CurrentAssets form)
        {
            form.nodataLabel.Visible = false;
            form.chartView.Visible = false;
            //form.exportList.Visible = false;
            //form.coinListView.Visible = false;
            form.backToStats.Visible = false;
            form.dropdownInvestments.Visible = false;
            //form.exportButton.Visible = false;
            form.exportdataPanel.Visible = false;
            form.variablesPanel.Visible = false;
            //form.searchListBox.Visible = false;
            //form.exportBorder.Visible = false;
            form.dashLabel.Visible = false;
            form.datePickerStart.Visible = false;
            form.datePickerFinish.Visible = false;
            //form.investmentsListView.Visible = false;
            form.dashLabel.Select();

            SwitchButtonVisibility(form, true);

            ca.datePickerStart.ValueChanged -= new System.EventHandler(this.datePickerStart_ValueChanged_1);
            ca.datePickerFinish.ValueChanged -= new System.EventHandler(this.datePickerFinish_ValueChanged_1);
            ca.coinListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.coinListView_ItemChecked);
            ca.investmentsListView.ItemChecked -= new System.Windows.Forms.ItemCheckedEventHandler(this.investmentsListView_ItemChecked);
        }

        private void InvestmentListViewItemSelectionChange(ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void ExportListItemSelectionChange(ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }
    }
}
