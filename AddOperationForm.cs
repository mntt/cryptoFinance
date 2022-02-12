using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class AddOperationForm
    {
        private CurrentAssets ca { get; set; }

        private bool closePending = false;
        private int lastViewItemIndex { get; set; }
        private int lviIndex { get; set; }
        private double showAlert { get; set; }

        private bool errorConfirming = false;

        private bool errorValidating = false;
        private bool editMode { get; set; }
        private CryptoTableData ctd { get; set; }
        private string operation { get; set; }
        private string operationForm { get; set; }
        private OperationsForm of { get; set; }
        private WalletsForm wf { get; set; }
        private DataGridForm dgf { get; set; }
        private CoingeckoListDownloader cd { get; set; }
        private bool showAssetAlert { get; set; }
        private System.Timers.Timer refreshTimer { get; set; }

        private Image[] refreshImages = new Image[4]
        { Properties.Resources.refresh, Properties.Resources.refresh2, Properties.Resources.refresh3, Properties.Resources.refresh4 };

        private decimal apiPrice { get; set; }

        public AddOperationForm(CurrentAssets _ca, OperationsForm _of, WalletsForm _wf, DataGridForm _dgf, bool _showAssetAlert)
        {
            editMode = false;
            of = _of;
            wf = _wf;
            dgf = _dgf;
            ca = _ca;
            showAssetAlert = _showAssetAlert;
            AddEvents();
            LoadLastTimeUpdatedLabel();
            RefreshTop100ListView();
            AddCoinsToComboBox();
            ListViewSettings.Format(ca.walletListView);
            ListViewSettings.Format(ca.top100listview);
            ListViewSettings.Format(ca.suggestionsListView);

            foreach (Control ctrl in ca.contentPanel.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.MouseClick += HideListViews;
                }
            }

            LoadNormalModeForm();

            refreshTimer = new System.Timers.Timer();
            refreshTimer.Elapsed += new ElapsedEventHandler(RefreshTimer_Tick);
            refreshTimer.Interval = 10;
        }

        public AddOperationForm(CryptoTableData _ctd, CurrentAssets _ca, OperationsForm _of, WalletsForm _wf, DataGridForm _dgf)
        {
            editMode = true;
            ctd = _ctd;
            of = _of;
            wf = _wf;
            dgf = _dgf;
            operation = ctd.operation;
            operationForm = ctd.operation;
            ca = _ca;
            AddEvents();
            LoadLastTimeUpdatedLabel();
            AddCoinsToComboBox();
            ListViewSettings.Format(ca.walletListView);

            foreach (Control ctrl in ca.contentPanel.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.MouseClick += HideListViews;
                }
            }

            LoadEditModeForm();

            refreshTimer = new System.Timers.Timer();
            refreshTimer.Elapsed += new ElapsedEventHandler(RefreshTimer_Tick);
            refreshTimer.Interval = 50;
        }

        private void RefreshTimer_Tick(object source, EventArgs e)
        {
            if (ca.refreshPrice.BackgroundImage == refreshImages[0])
            {
                ca.refreshPrice.BackgroundImage = refreshImages[1];
            }
            else if (ca.refreshPrice.BackgroundImage == refreshImages[1])
            {
                ca.refreshPrice.BackgroundImage = refreshImages[2];
            }
            else if (ca.refreshPrice.BackgroundImage == refreshImages[2])
            {
                ca.refreshPrice.BackgroundImage = refreshImages[3];
            }
            else if (ca.refreshPrice.BackgroundImage == refreshImages[3])
            {
                ca.refreshPrice.BackgroundImage = refreshImages[0];
            }
        }

        private void LoadLastTimeUpdatedLabel()
        {
            var date = Connection.db.GetTable<LastTimeUpdatedList>().ToList();

            if (date.Count == 0)
            {
                ca.lastTimeUpdated.Text += "Paskutinis atnaujinimas: niekada.";
            }
            else
            {
                ca.lastTimeUpdated.Text = "Paskutinis atnaujinimas: " + date[date.Count - 1].Date;
            }
        }

        private void AddEvents()
        {
            ca.backToOperations.Click += new System.EventHandler(this.BackToOperations_Click);
            ca.updateCryptoList.Click += new System.EventHandler(this.UpdateCryptoList_Click);
            ca.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            ca.Click += new System.EventHandler(this.CloseAndHideLists);
            ca.contentPanel.Click += new System.EventHandler(this.CloseAndHideLists);
            ca.addOperationPanel.Click += new System.EventHandler(this.CloseAndHideLists);
            ca.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurrentAssets_FormClosing);
            ca.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CurrentAssets_MouseMove);
            ca.addOperationPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AddOperationPanel_MouseMove);
            ca.walletTextBox.TextChanged += new System.EventHandler(this.WalletTextBox_TextChanged);
            ca.walletTextBox.Enter += new System.EventHandler(this.WalletTextBox_Enter);
            ca.walletTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletTextBox_KeyDown);
            ca.walletListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.WalletListView_ItemSelectionChanged);
            ca.walletListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletListView_KeyDown);
            ca.walletListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WalletListView_MouseClick);
            ca.walletListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WalletListView_MouseMove);
            ca.quantityBox.TextChanged += new System.EventHandler(this.QuantityBox_TextChanged);
            ca.quantityBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuantityBox_KeyPress);
            ca.quantityBox.Leave += new System.EventHandler(this.QuantityBox_Leave);
            ca.alertPanel.VisibleChanged += new System.EventHandler(this.AlertPanel_VisibleChanged);
            ca.sumBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SumBox_KeyPress);
            ca.sumBox.Leave += new System.EventHandler(this.SumBox_Leave);
            ca.priceBox.TextChanged += new System.EventHandler(this.PriceBox_TextChanged);
            ca.priceBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PriceBox_KeyPress);
            ca.priceBox.Leave += new System.EventHandler(this.PriceBox_Leave);
            ca.priceWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PriceWorker_DoWork);
            ca.priceWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PriceWorker_RunWorkerCompleted);
            ca.refreshPrice.Click += new System.EventHandler(this.RefreshPrice_Click);
            ca.feeBox.TextChanged += new System.EventHandler(this.FeeBox_TextChanged);
            ca.feeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FeeBox_KeyPress);
            ca.feeBox.Leave += new System.EventHandler(this.FeeBox_Leave);
            ca.buyButton.Click += new System.EventHandler(this.BuyButton_Click);
            ca.sellButton.Click += new System.EventHandler(this.SellButton_Click);
            ca.cryptoBox.Click += new System.EventHandler(this.CryptoBox_Click);
            ca.cryptoBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CryptoBox_MouseClick);
            ca.cryptoBox.TextChanged += new System.EventHandler(this.CryptoBox_TextChanged);
            ca.cryptoBox.Enter += new System.EventHandler(this.CryptoBox_Enter);
            ca.cryptoBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CryptoBox_KeyDown);
            ca.cryptoBox.Leave += new System.EventHandler(this.CryptoBox_Leave);
            ca.suggestionsListView.SelectedIndexChanged += new System.EventHandler(this.SuggestionsListView_SelectedIndexChanged);
            ca.suggestionsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SuggestionsListView_KeyDown);
            ca.suggestionsListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SuggestionsListView_MouseClick);
            ca.suggestionsListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SuggestionsListView_MouseMove);
            ca.top100listview.SelectedIndexChanged += new System.EventHandler(this.Top100ListView_SelectedIndexChanged);
            ca.top100listview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Top100ListView_KeyDown);
            ca.top100listview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Top100ListView_MouseClick);
            ca.top100listview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Top100ListView_MouseMove);
            ca.maxqLabel.Click += new System.EventHandler(this.MaxQuantityLabel_Click);
            ca.cryptoComboBox.DropDownClosed += new System.EventHandler(this.CryptoComboBox_DropDownClosed);
            ca.cryptoComboBox.Leave += new System.EventHandler(this.CryptoComboBox_Leave);
            ca.walletComboBox.DropDownClosed += new System.EventHandler(this.WalletComboBox_DropDownClosed);
            ca.walletComboBox.TextChanged += new System.EventHandler(this.WalletComboBox_TextChanged);
            ca.addOperationPanel.VisibleChanged += new System.EventHandler(this.AddOperationPanel_VisibleChanged);
            ca.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            ca.investmentsPanel.Click += new System.EventHandler(this.InvestmentsPanel_Click);
            ca.confirmTime.Click += new System.EventHandler(this.ConfirmTime_Click);
            ca.dateBox.CloseUp += new System.EventHandler(this.DateBox_CloseUp);
            ca.dateBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DateBox_MouseDown);
            ca.maxqLabel.MouseEnter += new System.EventHandler(MaxLabel_MouseEnter);
            ca.maxqLabel.MouseLeave += new System.EventHandler(MaxLabel_MouseLeave);

            foreach (Control ctrl in ca.addOperationPanel.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.MouseClick += HideListViews;
                }
            }
        }

        private void MaxLabel_MouseEnter(object sender, EventArgs e)
        {
            ca.maxqLabel.ForeColor = Colours.selectedItem;
        }

        private void MaxLabel_MouseLeave(object sender, EventArgs e)
        {
            ca.maxqLabel.ForeColor = Colours.labelColor;
        }

        private void DateBox_MouseDown(object sender, MouseEventArgs e)
        {
            ca.timePanel.Visible = false;
            ca.dateBox.Select();
        }

        private void DateBox_CloseUp(object sender, EventArgs e)
        {
            ca.timeBox.Select();
            var time = DateTime.Now;
            string time2 = time.ToString("HH:mm");
            ca.timeBox.Text = time2;
            ca.timePanel.Location = new Point(509, 192);
            ca.timePanel.BringToFront();
            ca.timePanel.Visible = true;
        }

        private void ConfirmTime_Click(object sender, EventArgs e)
        {
            string time = ca.timeBox.Text;
            string[] date = ca.dateBox.Text.Split(' ');
            ca.timePanel.Visible = false;
            ca.dateBox.Text = date[0] + " " + time; 
        }

        private void InvestmentsPanel_Click(object sender, EventArgs e)
        {
            ca.operationDataGrid.ClearSelection();
        }

        private void AddOperationPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!ca.addOperationPanel.Visible)
            {
                RemoveEvents();
                
                ctd = default(CryptoTableData);
                editMode = default(bool);
                operation = default(string);
                operationForm = default(string);
            }
        }

        private void RemoveEvents()
        {
            ca.updateCryptoList.Click -= new System.EventHandler(this.UpdateCryptoList_Click);
            ca.confirmButton.Click -= new System.EventHandler(this.ConfirmButton_Click);
            ca.Click -= new System.EventHandler(this.CloseAndHideLists);
            ca.contentPanel.Click -= new System.EventHandler(this.CloseAndHideLists);
            ca.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.CurrentAssets_FormClosing);
            ca.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.CurrentAssets_MouseMove);
            ca.addOperationPanel.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.AddOperationPanel_MouseMove);
            ca.walletTextBox.TextChanged -= new System.EventHandler(this.WalletTextBox_TextChanged);
            ca.walletTextBox.Enter -= new System.EventHandler(this.WalletTextBox_Enter);
            ca.walletTextBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.WalletTextBox_KeyDown);
            ca.walletListView.ItemSelectionChanged -= new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.WalletListView_ItemSelectionChanged);
            ca.walletListView.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.WalletListView_KeyDown);
            ca.walletListView.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.WalletListView_MouseClick);
            ca.walletListView.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.WalletListView_MouseMove);
            ca.quantityBox.TextChanged -= new System.EventHandler(this.QuantityBox_TextChanged);
            ca.quantityBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.QuantityBox_KeyPress);
            ca.quantityBox.Leave -= new System.EventHandler(this.QuantityBox_Leave);
            ca.alertPanel.VisibleChanged -= new System.EventHandler(this.AlertPanel_VisibleChanged);
            ca.sumBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.SumBox_KeyPress);
            ca.sumBox.Leave -= new System.EventHandler(this.SumBox_Leave);
            ca.priceBox.TextChanged -= new System.EventHandler(this.PriceBox_TextChanged);
            ca.priceBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.PriceBox_KeyPress);
            ca.priceBox.Leave -= new System.EventHandler(this.PriceBox_Leave);
            ca.priceWorker.DoWork -= new System.ComponentModel.DoWorkEventHandler(this.PriceWorker_DoWork);
            ca.priceWorker.RunWorkerCompleted -= new System.ComponentModel.RunWorkerCompletedEventHandler(this.PriceWorker_RunWorkerCompleted);
            ca.refreshPrice.Click -= new System.EventHandler(this.RefreshPrice_Click);
            ca.feeBox.TextChanged -= new System.EventHandler(this.FeeBox_TextChanged);
            ca.feeBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.FeeBox_KeyPress);
            ca.feeBox.Leave -= new System.EventHandler(this.FeeBox_Leave);
            ca.buyButton.Click -= new System.EventHandler(this.BuyButton_Click);
            ca.sellButton.Click -= new System.EventHandler(this.SellButton_Click);
            ca.cryptoBox.Click -= new System.EventHandler(this.CryptoBox_Click);
            ca.cryptoBox.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.CryptoBox_MouseClick);
            ca.cryptoBox.TextChanged -= new System.EventHandler(this.CryptoBox_TextChanged);
            ca.cryptoBox.Enter -= new System.EventHandler(this.CryptoBox_Enter);
            ca.cryptoBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.CryptoBox_KeyDown);
            ca.cryptoBox.Leave -= new System.EventHandler(this.CryptoBox_Leave);
            ca.suggestionsListView.SelectedIndexChanged -= new System.EventHandler(this.SuggestionsListView_SelectedIndexChanged);
            ca.suggestionsListView.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.SuggestionsListView_KeyDown);
            ca.suggestionsListView.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.SuggestionsListView_MouseClick);
            ca.suggestionsListView.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.SuggestionsListView_MouseMove);
            ca.top100listview.SelectedIndexChanged -= new System.EventHandler(this.Top100ListView_SelectedIndexChanged);
            ca.top100listview.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.Top100ListView_KeyDown);
            ca.top100listview.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.Top100ListView_MouseClick);
            ca.top100listview.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.Top100ListView_MouseMove);
            ca.maxqLabel.Click -= new System.EventHandler(this.MaxQuantityLabel_Click);
            ca.cryptoComboBox.DropDownClosed -= new System.EventHandler(this.CryptoComboBox_DropDownClosed);
            ca.cryptoComboBox.Leave -= new System.EventHandler(this.CryptoComboBox_Leave);
            ca.walletComboBox.DropDownClosed -= new System.EventHandler(this.WalletComboBox_DropDownClosed);
            ca.walletComboBox.TextChanged -= new System.EventHandler(this.WalletComboBox_TextChanged);
            ca.cancelButton.Click -= new System.EventHandler(this.CancelButton_Click);
            ca.investmentsPanel.Click -= new System.EventHandler(this.InvestmentsPanel_Click);
            ca.confirmTime.Click -= new System.EventHandler(this.ConfirmTime_Click);
            ca.dateBox.CloseUp -= new System.EventHandler(this.DateBox_CloseUp);
            ca.dateBox.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DateBox_MouseDown);
        }

        private void BackToOperations_Click(object sender, EventArgs e)
        {
            RemoveEvents();
            ca.addOperationPanel.Visible = false;
            ca.contentPanel.Visible = false;
            ca.investmentsPanel.Visible = true;

            if (showAssetAlert)
            {
                ca.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
            }
            else
            {
                ca.assetAlertLabel.Visible = false;
            }
        }

        private void UpdateCryptoList_Click(object sender, EventArgs e)
        {
            UpdateCryptoList(ca);
        }

        public void UpdateCryptoList(CurrentAssets form)
        {
            ca.coinsPanel.Visible = false;
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NonClientAreaEnabled;
            form.Invalidate(true);

            Design.ProgressBarStyling(form.progressBar);
            form.progressBarPanel.Visible = true;
            cd = new CoingeckoListDownloader();
            cd.StartCA(form, this, form.progressBar, form.progressLabel);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        { 
            if (cd.worker.IsBusy)
            {
                ca.progressLabel.Text = "Atšaukiama...";
                ca.progressBar.ForeColor = Colours.canceledProgress;
                cd.worker.CancelAsync();
                cd.workerCancelButtonPressed = true;
            }
        }

        public void UpdateDate()
        {
            var date = Connection.db.GetTable<LastTimeUpdatedList>().ToList().Last();
            ca.lastTimeUpdated.Text = "Paskutinis atnaujinimas: " + date.Date.ToString();
            
        }

        private bool Validation()
        {
            bool validation = false;
            List<Control> controlList = new List<Control>();
            var quantity = ca.quantityBox.Text;

            if (operation == "SELL")
            {    
                foreach(Control item in ca.contentPanel.Controls)
                {
                    if(item.Tag != "sell" || item.Tag != "box")
                    {
                        controlList.Add(item);
                    }
                }

                validation = FormValidation.ReturnValidation(controlList, ca.maxqLabel.Text, quantity);
            }
            else if (operation == "BUY")
            {
                foreach (Control item in ca.contentPanel.Controls)
                {
                    if (item.Tag != "buy" || item.Tag != "box")
                    {
                        controlList.Add(item);
                    }
                }

                validation = FormValidation.ReturnValidation(controlList);
            }

            
            controlList.Clear();           
            return validation;
        }

        private bool IsCustomCoin()
        {
            string name = "";
            bool customCoin = false;
           
            if (!editMode && operation == "BUY")
            {
                name = ca.cryptoBox.Text;
            }
            else if (!editMode && operation == "SELL")
            {
                name = ca.cryptoComboBox.Text;
            }
            else if (editMode)
            {
                name = ctd.name;
            }

            var checkCryptoTable = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == name).Select(x => x.CustomCoin).ToList();

            if(checkCryptoTable.Count > 0)
            {
                customCoin = checkCryptoTable[0];
            }
            else
            {
                var checkCoingeckoList = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => (x.CryptoName + " (" + x.CryptoSymbol + ")") == name).ToList();

                if (checkCoingeckoList.Count > 0)
                {
                    customCoin = false;
                }
                else
                {
                    customCoin = true;
                }
            }

            return customCoin;
        }

        private async Task ExecuteConfirmNormalMode()
        {
            var name = "";
            bool customCoin = IsCustomCoin();
            var wallet = "";
            int operationID = 0;

            if (operation == "BUY")
            {
                name = ca.cryptoBox.Text;
                wallet = ca.walletTextBox.Text;

                var searchList = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "BUY" && x.CryptoName == name && x.Wallet == wallet).Distinct().ToList();

                if (searchList.Count == 0)
                {    
                    var maxOperationNr = Connection.db.GetTable<CryptoTable>().Where(x => x.OperationID > 0).Select(x => x.OperationID).ToList();

                    if(maxOperationNr.Count > 0)
                    {
                        operationID = maxOperationNr.Max() + 1;
                    }
                    else
                    {
                        operationID = 1;

                    }
                }
                else
                {
                    operationID = searchList[0].OperationID;
                }
            }
            else if (operation == "SELL")
            {
                name = ca.cryptoComboBox.Text;
                wallet = ca.walletComboBox.Text;
            }

            var date = DateTime.Parse(ca.dateBox.Text);
            var quantity = ReformatText.ReturnNumeric(ca.quantityBox.Text);
            var price = apiPrice; //ReformatText.ReturnNumeric(ca.priceBox.Text);
            var fee = ReformatText.ReturnNumeric(ca.feeBox.Text);
            var sum = ReformatText.ReturnNumeric(ca.sumBox.Text);

            await Task.Run(() => ExecuteConfirm(ca, operationID, date, name, customCoin, quantity, operation, wallet, sum, price, fee));
        }
        
        private async Task ExecuteConfirm(CurrentAssets form, int operationID, DateTime date, string _name, bool customCoin, decimal quantity, string operation, string wallet, decimal sum, decimal price, decimal fee)
        {
            string name = _name;

            if (!customCoin)
            {
                var namesplit = name.Split('(');
                Image logo = Connection.iwdb.GetLogo(namesplit[0], namesplit[1].Trim(')'));

                if(logo == null)
                {
                    string id = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => x.CryptoName == namesplit[0] && x.CryptoSymbol == namesplit[1].Trim(')')).Select(x => x.CryptoId).First();
                    await DownloadLogo(id);
                }            
            }
            else
            {
                int nameLenght = name.Length;

                if (nameLenght > 3)
                {
                    name = name + " (" + name.Substring(0, 3).ToUpper() + ")";
                }

                var namesplit = name.Split('(');
                Image logo = Connection.iwdb.GetLogo(namesplit[0], namesplit[1].Trim(')'));

                if(logo == null)
                {
                    Connection.iwdb.InsertCoinGeckoCryptoList(cryptoFinance.Properties.Resources.defaultLogo, namesplit[0], namesplit[1].Trim(')'), namesplit[0], 0);
                }
            }

            of.InsertCurrentAssets(date, name, customCoin, quantity, price, operation);
            of.InsertCryptoTable(operationID, date, name, customCoin, quantity, operation, wallet, sum, price, fee, form.currentValue);
            of.LoadOperationsForm();
            wf.RefreshDataGrid();
            dgf.UpdateDataGrid(form);
            form.CountCurrentValue();

            await Task.Delay(100);
        }

        private async Task DownloadLogo(string id)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            string message = "";

            try
            {
                string url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=eur&ids=" + id + "&order=market_cap_desc&per_page=100&page=1&sparkline=false";
                var response = await client.GetAsync(url);
                message = await response.Content.ReadAsStringAsync();
                var split = message.Split('"');
                var logoUrl = split[15];
                Image image = DownloadImageFromUrl(logoUrl);
                Connection.iwdb.InsertCryptoLogo(id, image);
            }
            catch
            {
                Connection.iwdb.InsertCryptoLogo(id, null);
            }
            
            client.Dispose();
        }

        private System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 3000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        private async Task ExecuteConfirmEditMode()
        {
            operation = operationForm;
            var id = ctd.id;
            string name = ctd.name;
            bool customCoin = IsCustomCoin();
            string wallet = "";
            int operationID = ctd.operationID;

            if (operation == "BUY")
            {
                wallet = ca.walletTextBox.Text;
            }
            else if (operation == "SELL")
            {
                wallet = ca.walletComboBox.Text;
            }

            var date = DateTime.Parse(ca.dateBox.Text);
            var quantity = ReformatText.ReturnNumeric(ca.quantityBox.Text);
            var price = apiPrice;
            var fee = ReformatText.ReturnNumeric(ca.feeBox.Text);
            var sum = ReformatText.ReturnNumeric(ca.sumBox.Text);

            await Task.Run(() => ExecuteUpdate(ca, id, operationID, date, operation, name, customCoin, wallet, quantity, price, fee, sum));
        }

        private async Task ExecuteUpdate(CurrentAssets form, int id, int operationID, DateTime date, string operation, string name, bool customCoin, string wallet, decimal quantity, decimal price, decimal fee, decimal sum)
        {
            var operations = of.ReturnOperations();
            Connection.iwdb.UpdateCryptoTable(id, operationID, operation, name, date, quantity, wallet, price, fee, sum);
            RefreshCurrentAssets(name, customCoin, date, price);
            var search = operations.Where(x => x.Id == id).ToList();

            if (search.Count > 0)
            {
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.Date = date);
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.Operation = operation);
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.CryptoName = name);
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.Wallet = wallet);
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.CryptoQuantity = quantity);
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.LastPrice = decimal.Parse(price.ToString()));
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.Sum = decimal.Parse(sum.ToString()));
                operations.Where(x => x.Id == id).ToList().ForEach(x => x.Fee = decimal.Parse(fee.ToString()));
            }

            of.SwapOperations(operations);
            of.LoadOperationsForm();
            wf.RefreshDataGrid();
            dgf.UpdateDataGrid(form);

            await Task.Delay(100);
        }

        private async void RefreshCurrentAssets(string name, bool customCoin, DateTime date, decimal price)
        {
            CoinQuantity cq = new CoinQuantity();
            decimal q = cq.GetCoinQuantityByName(name);
            decimal cv = price * q;
            string id = ReturnId(customCoin, name);
            var coin = ReturnObject(name, customCoin, id, q, date, price, cv);
            InteractWithCurrentAssetsDB(name, customCoin, date, q, price, cv);
            await Task.Run(() => of.RefreshCurrentCoins(coin));
        }

        private void InteractWithCurrentAssetsDB(string name, bool customCoin, DateTime date, decimal newQuantity, decimal price, decimal cv)
        {
            var coin = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.Cryptocurrency == name).ToList();

            if (coin.Count == 0)
            {
                Connection.iwdb.InsertCurrentAssets(name, customCoin, newQuantity, date, price, cv);
            }
            else
            {
                Connection.iwdb.UpdateCurrentAssets(true, name, newQuantity, date, price, cv);
            }
        }

        private string ReturnId(bool customCoin, string name)
        {
            string id = "custom";

            if (!customCoin)
            {
                var nameSplit = name.Split('(');
                id = Connection.db.GetTable<CoingeckoCryptoList>()
                    .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                    .Select(x => x.CryptoId).ToList().First();
            }

            return id;
        }

        private ConstructingLists ReturnObject(string name, bool customCoin, string id, decimal quantity, DateTime date, decimal price, decimal currentValue)
        {
            ConstructingLists info = new ConstructingLists
                    (
                    date,
                    name,
                    customCoin,
                    id,
                    quantity,
                    price,
                    currentValue
                    );

            return info;
        }

        private async void ExecuteConfirmation()
        {
            ca.operationDataGrid.Visible = false;
            try
            {
                if (!editMode)
                {
                    await Task.Run(() => ExecuteConfirmNormalMode());
                }
                else if (editMode)
                {
                    await Task.Run(() => ExecuteConfirmEditMode());
                }
            }
            catch
            {
                errorConfirming = true;
                ca.addOperationPanel.Visible = false;
                ca.contentPanel.Visible = false;
                ca.investmentsPanel.Visible = true;

                if (showAssetAlert)
                {
                    ca.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
                    ca.addOperation.Location = new Point(290, 140);
                }
            }
        
            ca.addOperationPanel.Visible = false;
            ca.contentPanel.Visible = false;
            ca.investmentsPanel.Visible = true;
            ca.operationDataGrid.Visible = true;

            if (ca.operationDataGrid.Rows.Count > 0)
            {
                ca.addOperation.Location = new Point(3, 3);
            }
        }

        private void ConfirmationTask()
        {
            if (!closePending && !ca.alertPanel.Visible)
            {
                if (!Validation() && !ca.alertPanel.Visible)
                {
                    errorValidating = true;
                    
                }
                else
                {
                    ca.confirmButton.Enabled = false;
                    closePending = true;
                    ca.buyButton.Enabled = false;
                    ca.sellButton.Enabled = false;
                    ca.backToOperations.Enabled = false;
                    ExecuteConfirmation();
                    
                }
            }
        }

        private async void ConfirmButton_Click(object sender, EventArgs e)
        {
            ca.ShowLoading();
            ca.confirmButton.Enabled = false;

            foreach(Control item in ca.Controls)
            {
                if(item.Name != "loadingBox")
                {
                    item.Enabled = false;
                }
            }

            await Task.Run(() => ConfirmationTask());

            foreach (Control item in ca.Controls)
            {
                item.Enabled = true;
            }

            of.LoadOperationsForm();
            ExecuteAlertPanel();
            ca.HideLoading();
            ca.confirmButton.Enabled = true;
        }

        private void ExecuteAlertPanel()
        {          
            if(ca.noConnection)
            {
                AlertPanelControlInstance(24);
            }
            else if (errorConfirming)
            {
                AlertPanelControlInstance(7);
            }
            else if (errorValidating)
            {
                AlertPanelControlInstance(3);
            }
            else if (!ca.noConnection && !errorConfirming && !errorValidating)
            {
                AlertPanelControlInstance(22);
            }
        }

        private void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(ca.alertPanel, ca.alertLabel, 645, -38, 276, 38);
            apc.StartPanelAnimation(chooseLabelText);
        }

        private void QuantityBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.QuantityBox(ca.quantityBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                ca.priceBox.Select();
            }
        }

        private void WalletTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ca.walletTextBox.Text != "")
            {
                ca.walletListView.Items.Clear();
                var list = Connection.iwdb.FetchWallets(ca.walletTextBox.Text)
                    .OrderBy(abc => abc).Distinct().ToList();

                foreach (var item in list)
                {
                    ca.walletListView.Items.Add(item);
                }

                if (ca.walletListView.Items.Count > 0)
                {
                    ListViewSettings.SetListViewSizes(ca.walletListView); 
                    ca.walletListView.Visible = true;
                }
            }

            if (ca.walletTextBox.Text == "" || ca.walletListView.Items.Count == 0)
            {
                ca.walletListView.Visible = false;
            }
        }

        private void QuantityBox_Leave(object sender, EventArgs e)
        {
            GetCultureInfo gci = new GetCultureInfo(",");
            DecimalBoxRules.FormatQuantityBox(ca.quantityBox);
            RecalculateTotalSum();
        }

        private void CloseAndHideLists(object sender, EventArgs e)
        {
            ClearSelections();

            if(!ca.suggestionsListView.Visible && !ca.top100listview.Visible)
            {
                ca.coinsPanel.Visible = false;
            }

            if (ca.walletListView.Visible == true)
            {
                ca.walletListView.Visible = false;
            }

            ca.enterCryptoQuantityLabel.Select();
        }

        private void CurrentAssets_FormClosing(object sender, FormClosingEventArgs e)
        {
            closePending = true;
        }

        private void Result(decimal price)
        {
            switch (price)
            {
                case -1:
                    showAlert = -1;
                    break;
                case -2:
                    showAlert = -2;
                    break;
                case -3:
                    showAlert = -3;
                    break;
                default:
                    apiPrice = price;
                    ca.priceBox.Text = apiPrice.ToString();
                    DecimalBoxRules.FormatCurrencyBox(ca.priceBox);
                    break;
            }
        }

        private void GetPrice()
        {
            string[] nameSplit = new string[2];

            if (operationForm == "BUY")
            {
                nameSplit = ca.cryptoBox.Text.Split('(');
            }
            else if (operationForm == "SELL")
            {
                nameSplit = ca.cryptoComboBox.Text.Split('(');
            }

            var id = Connection.db.GetTable<CoingeckoCryptoList>()
                .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                .Select(x => x.CryptoId).ToList();

            ca.priceBox.Text = "";
            showAlert = 0;

            decimal price = GetPrices.ById(id[0]);
            Result(price);
        }

        private void RefreshTop100ListView()
        {
            ca.top100listview.Items.Clear();
            var list = Connection.db.GetTable<CoingeckoCryptoList>().Take(100)
                .Select(x => x.Id + ". " + x.CryptoName + " (" + x.CryptoSymbol + ")").ToList();
            foreach (var item in list)
            {
                ca.top100listview.Items.Add(item);
            }
        }

        private void LoadNormalModeForm()
        {
            ca.timePanel.Visible = false;
            ca.dateBox.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            ca.feeBox.Text = 0.ToString("C2");
            ca.walletListView.Visible = false;
            ca.coinsPanel.Visible = false;
            ca.suggestionsListView.Visible = false;
            ca.top100listview.Visible = false;
            ca.maxqPanel2.Visible = false;
        }

        private void AssignEditModeVariables()
        {
            ca.cryptoBox.Text = ctd.name;
            SearchBoxSettings.ShowRegular(ca.cryptoBox);
            ca.cryptoComboBox.Text = ctd.name;
            ca.dateBox.Text = ctd.date.ToString();
            ca.walletTextBox.Text = ctd.wallet;
            ca.walletComboBox.Text = ctd.wallet;
            AddWallets();

            string quantityValue = ctd.quantity.ToString("N8").TrimEnd('0');
            char[] qchars = quantityValue.ToCharArray();
            if (qchars.Last() == ',')
            {
                quantityValue = quantityValue.Trim(',');
            }
            ca.quantityBox.Text = quantityValue;

            string priceValue = ctd.price.ToString("N8").TrimEnd('0');
            char[] pchars = priceValue.ToCharArray();
            if (pchars.Last() == ',')
            {
                priceValue = priceValue.Trim(',');
            }
            ca.priceBox.Text = priceValue + " €";

            ca.feeBox.Text = ctd.fee.ToString("C2");

            string sumValue = ctd.sum.ToString("N8").TrimEnd('0');
            char[] sumchars = sumValue.ToCharArray();
            if (sumchars.Last() == ',')
            {
                sumValue = sumValue.Trim(',');
            }
            ca.sumBox.Text = sumValue + " €";     
        }

        private void LoadEditModeForm()
        {
            ca.timePanel.Visible = false;
            AssignEditModeVariables();
            
            if (operationForm == "BUY")
            {
                ca.cryptoBox.Enabled = false;
                ca.cryptoComboBox.Visible = false;
                ca.walletComboBox.Visible = false;
                ca.maxqPanel2.Visible = false;
                ca.buyButton.BackColor = Colours.green;
                ca.buyButton.FlatAppearance.MouseOverBackColor = Colours.green;
            }
            else if (operationForm == "SELL")
            {
                ca.cryptoComboBox.Enabled = false;
                ca.cryptoBox.Visible = false;
                ca.walletTextBox.Visible = false;
                ca.searchPicture.Visible = false;
                ToggleWalletRelatedBoxes();
                CountMaxQuantity();
                ca.sellButton.BackColor = Colours.red;
                ca.sellButton.FlatAppearance.MouseOverBackColor = Colours.red;
            }

            ca.coinsPanel.Visible = false;
            ca.walletListView.Visible = false;
            ca.suggestionsListView.Visible = false;
            ca.top100listview.Visible = false;
        }

        private void RecalculateTotalSum()
        {
            if (ca.quantityBox.Text != "" && ca.priceBox.Text != "" && ca.feeBox.Text != "")
            {
                decimal q = ReformatText.ReturnNumeric(ca.quantityBox.Text);
                decimal fee = ReformatText.ReturnNumeric(ca.feeBox.Text);

                ca.sumBox.Text = ((q * apiPrice) + fee).ToString("C2");
            }
            else
            {
                ca.sumBox.Text = "";
            }
        }

        private void WalletListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ca.walletTextBox.Text = e.Item.Text;
            ca.walletListView.Visible = false;
        }

        private void WalletListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(ca.walletListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }

        private void AddOperationPanel_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseIsOutOfBounds(ca.walletListView);
            ListViewSettings.WhenMouseIsOutOfBounds(ca.top100listview);
            ListViewSettings.WhenMouseIsOutOfBounds(ca.suggestionsListView);
        }

        private void WalletTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenDownButtonPressed(ca.walletListView, e);
            ListViewSettings.WhenUpButtonPressed(ca.walletListView, e);
            ListViewSettings.WhenEnterButtonPressed(ca.walletListView, e, ca.walletTextBox, ca.priceBox);

            if (e.KeyCode == Keys.Enter)
            {
                ca.walletListView.Visible = false;
                ca.quantityBox.Select();
            }
        }

        private void WalletListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (ca.walletListView.Focused)
            {
                e.Handled = true;
            }
        }

        private void SumBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(ca.sumBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                ca.confirmButton.Select();
            }
        }

        private void SumBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(ca.sumBox);
        }

        private void PriceBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(ca.priceBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                ca.feeBox.Select();
            }
        }

        private void PriceBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(ca.priceBox);
            apiPrice = ReformatText.ReturnNumeric(ca.priceBox.Text);
            RecalculateTotalSum();
        }

        private void PriceWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            GetPrice();
        }

        private void PriceWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!closePending)
            {
                switch (showAlert)
                {
                    case -1:
                        AlertPanelControlInstance(6);
                        break;
                    case -2:
                        AlertPanelControlInstance(8);
                        break;
                    case -3:
                        AlertPanelControlInstance(9);
                        break;
                }
            }
            refreshTimer.Stop();
            ca.refreshPrice.BackgroundImage = refreshImages[0];
        }

        private async void RefreshPrice_Click(object sender, EventArgs e)
        {
            refreshTimer.Start();
            if (!ca.priceWorker.IsBusy)
            {
                showAlert = 0;
                ca.priceWorker.RunWorkerAsync();
            }

            await Task.Delay(100);
        }

        private void AlertPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (ca.alertPanel.Visible == false && closePending)
            {
                ca.Enabled = true;
                ca.BringToFront();
                ca.addOperationPanel.Visible = false;
                ca.contentPanel.Visible = true;
                ca.investmentsPanel.Visible = true;
            }
        }

        private void FeeBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(ca.feeBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                ca.sumBox.Select();
            }
        }

        private void FeeBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(ca.feeBox);
            RecalculateTotalSum();
        }
 
        private void BuyButton_Click(object sender, EventArgs e)
        {
            operationForm = "BUY";
            ca.sellButton.BackColor = Colours.transparent;
            ca.sellButton.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;
            ca.buyButton.BackColor = Colours.green;
            ca.buyButton.FlatAppearance.MouseOverBackColor = Colours.green;
            ca.cryptoComboBox.Visible = false;
            ca.walletComboBox.Visible = false;
            ca.coinsPanel.Visible = false;
            ca.top100listview.Visible = false;
            ca.suggestionsListView.Visible = false;
            ca.walletListView.Visible = false;
            ca.cryptoBox.Visible = true;
            ca.walletTextBox.Visible = true;
            ca.searchPicture.Visible = true;
            ca.quantityBox.Enabled = true;

            if (!editMode)
            {
                operation = "BUY";
                SearchBoxSettings.OnLeavingFocus(ca.cryptoBox);
            }
            else if(editMode)
            {
                ca.cryptoComboBox.Enabled = true;
                ca.cryptoBox.Enabled = false;
                AssignEditModeVariables();
            }

            ca.maxqPanel2.Visible = false;
            ca.contentPanel.Visible = true;
            ca.chooseCryptoLabel.Select();
        }

        private void SellButton_Click(object sender, EventArgs e)
        {
            operationForm = "SELL";
            ca.sellButton.BackColor = Colours.red;
            ca.sellButton.FlatAppearance.MouseOverBackColor = Colours.red;
            ca.buyButton.BackColor = Colours.transparent;
            ca.buyButton.FlatAppearance.MouseOverBackColor = Colours.transparent;
            ca.cryptoComboBox.Visible = true;
            ca.walletComboBox.Visible = true;
            ca.coinsPanel.Visible = false;
            ca.top100listview.Visible = false;
            ca.suggestionsListView.Visible = false;
            ca.walletListView.Visible = false;
            ca.cryptoBox.Visible = false;
            ca.walletTextBox.Visible = false;
            ca.searchPicture.Visible = false;
            
            if(ca.walletComboBox.Text == "")
            {
                ca.quantityBox.Enabled = false;
            }

            if (ca.walletComboBox.Text != "" && ca.cryptoComboBox.Text != "")
            {
                ca.maxqPanel2.Visible = true;
            }

            if (!editMode)
            {
                operation = "SELL";               
            }
            else if (editMode)
            {
                ca.cryptoComboBox.Enabled = false;
                ca.cryptoBox.Enabled = true;
                AssignEditModeVariables();
                ToggleWalletRelatedBoxes();
                CountMaxQuantity();

                if(operation == "BUY")
                {
                    int searchCoin = ca.cryptoComboBox.FindStringExact(ca.cryptoComboBox.Text);

                    if (searchCoin >= 0)
                    {
                        CoinQuantity cq = new CoinQuantity();
                        decimal value = cq.GetCoinQuantityByNameAndDate(ca.cryptoComboBox.Text, DateTime.Parse(ca.dateBox.Text)) - ctd.quantity;

                        if (value > 0)
                        {
                            int search = ca.walletComboBox.FindStringExact(ca.walletComboBox.Text);
  
                            if (search < 0)
                            {
                                decimal value2 = cq.GetCoinQuantityByWalletAndDate(ca.cryptoComboBox.Text, ca.walletComboBox.Text, DateTime.Parse(ca.dateBox.Text)) - ctd.quantity;

                                if (value2 == 0)
                                {
                                    MessageBox.Show("Piniginės " + ctd.wallet + " pasirinkti negalima,\nnes iki operacijos datos piniginės kiekis buvo lygus 0.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ca.walletComboBox.Text = "";
                                    ca.quantityBox.Text = "";
                                }
                            }
                        }                                      
                    }
                    else if (searchCoin < 0)
                    {
                        ca.walletComboBox.Text = "";
                        ca.quantityBox.Text = "";
                        MessageBox.Show("Valiutai " + ctd.name + " operacijos pakeisti negalima,\nnes iki šios datos, tai buvo vienintelė pirkimo operacija.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ca.buyButton.PerformClick();
                    }
                }
            }

            ca.contentPanel.Visible = true;
            ca.chooseCryptoLabel.Select();            
        }

        private void ToggleWalletRelatedBoxes()
        {
            if (ca.walletComboBox.Text == "")
            {
                ca.quantityBox.Enabled = false;
                ca.quantityBox.Text = "";
                ca.maxqPanel2.Visible = false;
            }

            if (ca.walletComboBox.Text != "" && ca.walletComboBox.Items.Count > 0)
            {
                ca.maxqPanel2.Visible = true;
            }
        }

        private void CryptoBox_TextChanged(object sender, EventArgs e)
        {
            ca.top100listview.Visible = false;

            if (ca.cryptoBox.Text != "")
            {
                ca.suggestionsListView.Items.Clear();
                var list = Connection.iwdb.FetchCoinNames(ca.cryptoBox.Text)
                    .OrderByDescending(x => x.quantity).Select(x => x.name).ToList();
                foreach (var item in list)
                {
                    ca.suggestionsListView.Items.Add(item);
                }

                if (ca.suggestionsListView.Items.Count > 0)
                {
                    ListViewSettings.SetListViewSizes(ca.suggestionsListView);
                    ca.coinsPanel.Visible = true;
                    ca.suggestionsListView.Visible = true;
                }
            }

            if (ca.cryptoBox.Text == "")
            {
                ca.coinsPanel.Visible = false;
                ca.suggestionsListView.Visible = false;
            }
        }

        private void CurrentAssets_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseIsOutOfBounds(ca.walletListView);
            ListViewSettings.WhenMouseIsOutOfBounds(ca.top100listview);
            ListViewSettings.WhenMouseIsOutOfBounds(ca.suggestionsListView);
        }

        private void CryptoBox_Click(object sender, EventArgs e)
        {
            SearchBoxSettings.OnFocus(ca.cryptoBox);
        }

        private void CryptoBox_Enter(object sender, EventArgs e)
        {
            if (ca.cryptoBox.Visible == true)
            {
                ca.cryptoBox.Select();
                SearchBoxSettings.OnFocus(ca.cryptoBox);
            }
        }

        private void CryptoBox_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenDownButtonPressed(ca.top100listview, e);
            ListViewSettings.WhenUpButtonPressed(ca.top100listview, e);
            ListViewSettings.OnEnterButtonPressedWhenInvesting(this, ca.top100listview, e);
            ListViewSettings.WhenDownButtonPressed(ca.suggestionsListView, e);
            ListViewSettings.WhenUpButtonPressed(ca.suggestionsListView, e);
            ListViewSettings.OnEnterButtonPressedWhenInvesting(this, ca.suggestionsListView, e);

            if (e.KeyCode == Keys.Enter)
            {
                ca.top100listview.Visible = false;
                ca.suggestionsListView.Visible = false;
                ca.walletTextBox.Select();
            }
        }

        public void AssignNameToTextBox(string name)
        {
            ca.cryptoBox.Text = name;
        }

        private void ClearSelections()
        {
            if (ca.top100listview.Visible || ca.suggestionsListView.Visible || ca.coinsPanel.Visible)
            {
                ca.top100listview.Visible = false;
                ca.suggestionsListView.Visible = false;
            }

            if (ca.cryptoBox.Text == "")
            {
                SearchBoxSettings.OnLeavingFocus(ca.cryptoBox);
            }

            ca.ActiveControl = ca.chooseCryptoLabel;
        }

        private void CryptoBox_Leave(object sender, EventArgs e)
        {
            if (ca.top100listview.Focused != true && ca.top100listview.Visible)
            {
                ClearSelections();
            }

            if (ca.suggestionsListView.Focused != true && ca.suggestionsListView.Visible)
            {
                ClearSelections();
            }

            if (ca.cryptoBox.Text != "" && !editMode)
            {
                if (!ca.priceWorker.IsBusy)
                {
                    showAlert = 0;
                    ca.priceWorker.RunWorkerAsync();
                }
            }
        }

        private void CryptoBox_MouseClick(object sender, MouseEventArgs e)
        {
            ca.top100listview.Items[lviIndex].Selected = false;
            ListViewSettings.SetListViewSizes(ca.top100listview);
            ca.coinsPanel.Visible = true;
            ca.top100listview.Visible = true;
        }

        private void SuggestionsListView_KeyDown(object sender, KeyEventArgs e)
        {

            ListViewSettings.WhenItIsNotFocused(ca.suggestionsListView, e);
        }

        private void SuggestionsListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(ca.suggestionsListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }

        private void SuggestionsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ca.suggestionsListView.SelectedItems.Count > 0)
            {
                ca.coinsPanel.Visible = false;
                ca.cryptoBox.Text = ca.suggestionsListView.SelectedItems[0].Text;
                ca.chooseCryptoLabel.Select();
            }
        }

        private void Top100ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                foreach (ListViewItem item in ca.top100listview.Items)
                {
                    if (item.BackColor == Colours.selectedItem)
                    {
                        lviIndex = ca.top100listview.Items.IndexOf(item);
                    }
                }
            }

            ListViewSettings.WhenItIsNotFocused(ca.top100listview, e);
        }

        private void Top100ListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(ca.top100listview, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }

        private void Top100ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ca.top100listview.SelectedItems.Count > 0)
            {
                ca.coinsPanel.Visible = false;
                lviIndex = ca.top100listview.Items.IndexOf(ca.top100listview.SelectedItems[0]);
                var fixedName = FixLabelName.ReturnName(ca.top100listview.SelectedItems[0].Text);
                ca.cryptoBox.Text = fixedName;
                ca.chooseCryptoLabel.Select();
            }
        }

        private void EditModeAdd(CoinQuantity cq)
        {
            if (operation == "BUY")
            {
                decimal q = cq.GetCoinQuantityByNameAndDate(ctd.name, ctd.date);
                decimal minimum = 0.00000001M;
                q -= ctd.quantity;

                if (q < minimum)
                {
                    ca.cryptoComboBox.Items.Remove(ctd.name);
                }
            }
            else if (operation == "SELL")
            {
                int search = ca.cryptoComboBox.FindStringExact(ctd.name);
                if (search < 0)
                {
                    ca.cryptoComboBox.Items.Add(ctd.name);
                }
            }
        }

        private void AddCoinsToComboBox()
        {
            ca.cryptoComboBox.Items.Clear();
            CoinQuantity cq = new CoinQuantity();
            decimal minimum = 0.00000001M;
            var coinNames = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            for (int i = 0; i < coinNames.Count; i++)
            {
                decimal q = cq.GetCoinQuantityByName(coinNames[i]);

                if (q >= minimum)
                {
                    ca.cryptoComboBox.Items.Add(coinNames[i]);
                }
            }

            if (editMode)
            {
                EditModeAdd(cq);
            }
        }

        private void Top100ListView_MouseClick(object sender, MouseEventArgs e)
        {
            ca.coinsPanel.Visible = false;
            ca.walletTextBox.Select();
            if (!ca.priceWorker.IsBusy)
            {
                showAlert = 0;
                ca.priceWorker.RunWorkerAsync();
            }
        }

        private void SuggestionsListView_MouseClick(object sender, MouseEventArgs e)
        {
            ca.coinsPanel.Visible = false;
            ca.walletTextBox.Select();
            if (!ca.priceWorker.IsBusy)
            {
                showAlert = 0;
                ca.priceWorker.RunWorkerAsync();
            }
        }

        private void EditModeWallets(CoinQuantity cq)
        {
            if (operation == "BUY")
            {
                decimal q = cq.GetCoinQuantityByWalletAndDate(ctd.name, ctd.wallet, ctd.date);
                decimal minimum = 0.00000001M;
                q -= ctd.quantity;

                if (q < minimum)
                {
                    ca.walletComboBox.Items.Remove(ctd.wallet);
                }
            }
            else if (operation == "SELL")
            {
                int search = ca.walletComboBox.FindStringExact(ctd.wallet);
                if (search < 0)
                {
                    ca.walletComboBox.Items.Add(ctd.wallet);
                }
            }
        }

        private void AddWallets()
        {
            ca.walletComboBox.Items.Clear();
            CoinQuantity cq = new CoinQuantity();
            decimal minimum = 0.00000001M;
            var wallets = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.CryptoName == ca.cryptoComboBox.Text).Select(x => x.Wallet).Distinct().ToList();

            foreach (var wallet in wallets)
            {
                decimal q = cq.GetCoinQuantityByWallet(ca.cryptoComboBox.Text, wallet);

                if (q >= minimum)
                {
                    ca.walletComboBox.Items.Add(wallet);
                }
            }

            if (editMode)
            {
                EditModeWallets(cq);
            }
        }

        private void CryptoComboBox_Leave(object sender, EventArgs e)
        {
            AddWallets();
            ca.walletComboBox.Text = "";
            ca.quantityBox.Text = "";
            ca.quantityBox.Enabled = false;
            ca.maxqPanel2.Visible = false;

            if (ca.cryptoComboBox.Text != "" && !editMode)
            {
                if (!ca.priceWorker.IsBusy)
                {
                    showAlert = 0;
                    ca.priceWorker.RunWorkerAsync();
                }
            }
        }

        private void CryptoComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (ca.cryptoComboBox.SelectedIndex >= 0)
            {
                ca.cryptoComboBox.Text = ca.cryptoComboBox.Items[ca.cryptoComboBox.SelectedIndex].ToString();
                ca.walletComboBox.Select();
                AddWallets();
            }
        }

        private void CheckForWalletAndExecute(bool walletFound)
        {
            if (!walletFound)
            {
                ca.quantityBox.Enabled = false;
                ca.maxqPanel2.Visible = false;
            }

            if (walletFound)
            {
                CoinQuantity cq = new CoinQuantity();
                decimal q = cq.GetCoinQuantityByWallet(ca.cryptoComboBox.Text, ca.walletComboBox.Text);

                if (editMode && operation == "BUY")
                {
                    q = cq.GetCoinQuantityByWalletAndDate(ca.cryptoComboBox.Text, ca.walletComboBox.Text, ctd.date);

                    if (ca.walletComboBox.Text == ctd.wallet)
                    {
                        q -= ctd.quantity;
                    }
                }
                else if (editMode && operation == "SELL")
                {
                    q = cq.GetCoinQuantityByWalletAndDate(ca.cryptoComboBox.Text, ca.walletComboBox.Text, ctd.date);
                    q += ctd.quantity;
                }

                string quantityValue = q.ToString("N8").TrimEnd('0');
                char[] qchars = quantityValue.ToCharArray();
                if (qchars.Last() == ',')
                {
                    quantityValue = quantityValue.Trim(',');
                }

                ca.maxqLabel.Text = quantityValue;
                ca.quantityBox.Enabled = true;
                ca.maxqPanel2.Visible = true;
            }
        }

        private void CountMaxQuantity()
        {
            if (editMode)
            {
                bool walletFound = false;
                if (ca.walletComboBox.Text != "")
                {
                    walletFound = true;
                }
                CheckForWalletAndExecute(walletFound);
            }
            else
            {
                int searchCoin = ca.cryptoComboBox.FindStringExact(ca.cryptoComboBox.Text);
                if (searchCoin >= 0)
                {
                    bool walletFound = false;

                    int searchWallet = ca.walletComboBox.FindStringExact(ca.walletComboBox.Text);
                    if (searchWallet >= 0)
                    {
                        walletFound = true;
                    }

                    CheckForWalletAndExecute(walletFound);
                }
            }
        }

        private void WalletComboBox_TextChanged(object sender, EventArgs e)
        {
            CountMaxQuantity();
        }

        private void MaxQuantityLabel_Click(object sender, EventArgs e)
        {
            ca.quantityBox.Text = ca.maxqLabel.Text;
        }

        private void QuantityBox_TextChanged(object sender, EventArgs e)
        {
            RecalculateTotalSum();
        }

        private void PriceBox_TextChanged(object sender, EventArgs e)
        {
            RecalculateTotalSum();
        }

        private void FeeBox_TextChanged(object sender, EventArgs e)
        {
            RecalculateTotalSum();
        }

        private void WalletComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(ca.walletComboBox.SelectedIndex >= 0)
            {
                ca.walletComboBox.Text = ca.walletComboBox.SelectedItem.ToString();
                ca.quantityBox.Select();
            } 
        }

        private void WalletListView_MouseClick(object sender, MouseEventArgs e)
        {
            ca.quantityBox.Select();
        }

        private void WalletTextBox_Enter(object sender, EventArgs e)
        {
            lastViewItemIndex = -1;
        }

        private void HideListViews(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ca.top100listview.Visible && !ca.cryptoBox.Focused)
                {
                    ca.coinsPanel.Visible = false;
                    ca.top100listview.Visible = false;
                }
                else if (ca.suggestionsListView.Visible && !ca.cryptoBox.Focused)
                {
                    ca.coinsPanel.Visible = false;
                    ca.suggestionsListView.Visible = false;
                }
                else if (ca.walletListView.Visible && !ca.walletTextBox.Focused)
                {
                    ca.walletListView.Visible = false;
                }
            }
        }

    }
}
