using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class AddOperation : Form
    {
        private bool closePending = false;
        private Investments invForm { get; set; }
        private int lastViewItemIndex { get; set; }
        private int lviIndex { get; set; }        
        private double showAlert { get; set; }
        private bool editMode { get; set; }
        private CryptoTableData ctd { get; set; }
        private string operation { get; set; }
        private string operationForm { get; set; }
        private Action refreshOperationDataGrid { get; set; } 

        public AddOperation(Investments _invForm, Action _refreshOperationDataGrid)
        {
            InitializeComponent();
            editMode = false;
            invForm = _invForm;
            refreshOperationDataGrid = _refreshOperationDataGrid;
        }

        public AddOperation(CryptoTableData _ctd, Investments _invForm, Action _refreshOperationDataGrid)
        {
            InitializeComponent();
            editMode = true;
            ctd = _ctd;
            operation = ctd.operation;
            operationForm = ctd.operation;
            invForm = _invForm;
            refreshOperationDataGrid = _refreshOperationDataGrid;
        }

        private bool Validation()
        {
            bool validation = false;

            if (operation == "SELL" && maxQuantityLabel.Visible)
            {
                var split = maxQuantityLabel.Text.Split('.');
                var maxQuantity = split[1];
                var quantity = quantityBox.Text;
                validation = FormValidation.ReturnValidation(Controls, maxQuantity, quantity);
            }
            else if (operation == "BUY")
            {
                validation = FormValidation.ReturnValidation(Controls);
            }

            return validation;
        }

        private bool IsCustomCoin()
        {
            string name = "";
            bool customCoin = false;
            var nonCustomCoins = Connection.db.GetTable<CoingeckoCryptoList>().ToList();

            if (!editMode && operation == "BUY")
            {
                name = cryptoBox.Text;
            }
            else if (!editMode && operation == "SELL")
            {
                name = cryptoComboBox.Text;
            }
            else if (editMode)
            {
                name = ctd.name;
            }
           
            var search = nonCustomCoins.Where(x => (x.CryptoName + " (" + x.CryptoSymbol + ")") == name).ToList();
            if (search.Count == 0)
            {
                customCoin = true;
            }

            return customCoin;
        }

        private async Task ExecuteConfirmNormalMode()
        {
            var name = "";
            bool customCoin = IsCustomCoin();
            var wallet = "";

            if (operation == "BUY")
            {
                name = cryptoBox.Text;
                wallet = walletTextBox.Text;
            }
            else if (operation == "SELL")
            {
                name = cryptoComboBox.Text;
                wallet = walletComboBox.Text;
            }

            var date = DateTime.Parse(dateBox.Text);
            var quantity = ReformatText.ReturnNumeric(quantityBox.Text);
            var price = ReformatText.ReturnNumeric(priceBox.Text);
            var fee = ReformatText.ReturnNumeric(feeBox.Text);
            var sum = ReformatText.ReturnNumeric(sumBox.Text);

            await Task.Run(() => invForm.ExecuteConfirm(date, name, customCoin, quantity, operation, wallet, sum, price, fee));            
        }

        private async Task ExecuteConfirmEditMode()
        {
            operation = operationForm;
            var id = ctd.id;
            string name = ctd.name;
            bool customCoin = IsCustomCoin();
            string wallet = "";

            if (operation == "BUY")
            {
                wallet = walletTextBox.Text;
            }
            else if (operation == "SELL")
            {
                wallet = walletComboBox.Text;
            }

            var date = DateTime.Parse(dateBox.Text);
            var quantity = ReformatText.ReturnNumeric(quantityBox.Text);
            var price = ReformatText.ReturnNumeric(priceBox.Text);
            var fee = ReformatText.ReturnNumeric(feeBox.Text);
            var sum = ReformatText.ReturnNumeric(sumBox.Text);

            await Task.Run(() => invForm.UpdateOperation(id, date, operation, name, customCoin, wallet, quantity, price, fee, sum));
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

        private async void ExecuteConfirmation()
        {            
            try
            {
                ShowLoading();
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
                MessageBox.Show("Įvyko nenumatyta klaida. Bandykite dar kartą.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            finally
            {
                refreshOperationDataGrid();
                HideLoading();
                ExecuteAlertPanel();
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (!closePending)
            {
                if (!Validation() && !alertPanel.Visible)
                {
                    AlertPanelControlInstance(3);
                }
                else
                {
                    this.cryptoComboBox.Leave -= new System.EventHandler(this.CryptoComboBox_Leave);
                    closePending = true;
                    DisableControls();
                    ExecuteConfirmation();
                }
            }
        }

        private void ExecuteAlertPanel()
        {
            if (!invForm.noConnection)
            {
                AlertPanelControlInstance(22);
            }
            else
            {
                AlertPanelControlInstance(24);
            }
        }

        private void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(alertPanel, alertLabel, 180, -38, 198, 38, 29, 13);
            apc.StartPanelAnimation(chooseLabelText); 
        }

        private void QuantityBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.QuantityBox(quantityBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                priceBox.Select();
            }
        }

        private void WalletTextBox_TextChanged(object sender, EventArgs e)
        {
            if (walletTextBox.Text != "")
            {
                walletListView.Items.Clear();
                var list = Connection.iwdb.FetchWallets(walletTextBox.Text)
                    .OrderBy(abc => abc).Distinct().ToList();

                foreach (var item in list)
                {
                    walletListView.Items.Add(item);
                }

                if (walletListView.Items.Count > 0)
                {
                    ListViewSettings.SetColumnWidth(walletListView, 4, walletListView.Height, walletListView.Items.Count);
                    walletListView.Visible = true;
                }
            }

            if(walletTextBox.Text == "" || walletListView.Items.Count == 0)
            {
                walletListView.Visible = false;
            }
        }

        private void QuantityBox_Leave(object sender, EventArgs e)
        {
            GetCultureInfo gci = new GetCultureInfo(",");
            DecimalBoxRules.FormatQuantityBox(quantityBox);
            RecalculateTotalSum(); 
        }

        private void AddQuantityForm_Click(object sender, EventArgs e)
        {
            ClearSelections();

            if (walletListView.Visible == true)
            {
                walletListView.Visible = false;
            }

            enterCryptoQuantityLabel.Select();
        }

        private void AddQuantityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closePending = true;
            invForm.Enabled = true;
        }

        private void Result(double price)
        {
            switch (price)
            {
                case -1:
                    showAlert = -1;
                    break;
                case -2:
                    showAlert = -2;
                    break;
                default:
                    priceBox.Text = string.Format("{0:C4}", price);
                    break;
            }
        }

        private void GetPrice()
        {
            string[] nameSplit = new string[2];

            if(operationForm == "BUY")
            {
                nameSplit = cryptoBox.Text.Split('(');
            }
            else if(operationForm == "SELL")
            {
                nameSplit = cryptoComboBox.Text.Split('(');
            }

            var id = Connection.db.GetTable<CoingeckoCryptoList>()
                .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                .Select(x => x.CryptoId).ToList();

            priceBox.Text = "";
            showAlert = 0;

            double price = GetPrices.ById(id[0]);
            Result(price);
        }

        private void RefreshTop100ListView()
        {
            top100ListView.Items.Clear();
            var list = Connection.db.GetTable<CoingeckoCryptoList>().Take(100)
                .Select(x => x.Id + ". " + x.CryptoName + " (" + x.CryptoSymbol + ")").ToList();
            foreach (var item in list)
            {
                top100ListView.Items.Add(item);
            }
        }

        private void LoadNormalModeForm()
        {
            DisableControls();
            buyButton.Enabled = true;
            sellButton.Enabled = true;
            dateBox.Text = DateTime.Today.ToString();
            feeBox.Text = 0.ToString("C2");
        }

        private void LoadBuyForm()
        {
            cryptoComboBox.Visible = false;
            walletComboBox.Visible = false;
            maxQuantityLabel.Visible = false;
            searchPicture.Visible = true;
            cryptoBox.Visible = true;
            walletTextBox.Visible = true;

            if (!editMode)
            {
                ClearFields();
                SearchBoxSettings.OnLeavingFocus(cryptoBox);
            }
            else
            {
                buyButton.BackColor = Color.Green;
            }
        }

        private void LoadSellForm()
        {
            cryptoBox.Visible = false;
            walletTextBox.Visible = false;
            searchPicture.Visible = false;
            cryptoComboBox.Visible = true;
            walletComboBox.Visible = true;
            ToggleWalletRelatedBoxes();

            if (!editMode)
            {
                ClearFields();
            }
            else
            {
                sellButton.BackColor = Color.Red;
                CountMaxQuantity();
            }
        }

        private void LoadEditModeForm()
        {
            cryptoBox.Text = ctd.name;
            SearchBoxSettings.ShowRegular(cryptoBox);
            cryptoBox.Enabled = false;
            cryptoComboBox.Text = ctd.name; 
            cryptoComboBox.Enabled = false;
            dateBox.Text = ctd.date.ToString();
            walletTextBox.Text = ctd.wallet;
            walletComboBox.Text = ctd.wallet;
            AddWallets();
            quantityBox.Text = ctd.quantity.ToString("0.00000000");
            priceBox.Text = ctd.price.ToString("C2");
            feeBox.Text = ctd.fee.ToString("C2");
            sumBox.Text = ctd.sum.ToString("C2");
            walletListView.Visible = false;
            EnableControls();

            if (operationForm == "BUY")
            {
                LoadBuyForm();
            }
            else if (operationForm == "SELL")
            {
                LoadSellForm();
            }     
        }

        private void AddQuantityForm_Load(object sender, EventArgs e)
        { 
            chooseCryptoLabel.Select();
            RefreshTop100ListView();
            AddCoinsToComboBox();
            ListViewSettings.Format(walletListView);
            ListViewSettings.Format(top100ListView);
            ListViewSettings.Format(suggestionsListView);

            foreach (Control ctrl in this.Controls) 
            { 
                if (ctrl is TextBox) 
                { 
                    ctrl.MouseClick += HideListViews; 
                } 
            }
        
            if (!editMode)
            {
                LoadNormalModeForm();
            }
            else if(editMode)
            {
                LoadEditModeForm();
            }                
        }

        private void RecalculateTotalSum()
        {
            if (quantityBox.Text != "" && priceBox.Text != "" && feeBox.Text != "")
            {
                double q = ReformatText.ReturnNumeric(quantityBox.Text);
                double price = ReformatText.ReturnNumeric(priceBox.Text);
                double fee = ReformatText.ReturnNumeric(feeBox.Text);

                sumBox.Text = ((q * price) + fee).ToString("C2");
            }
            else
            {
                sumBox.Text = "";
            }
        }

        private void WalletListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (walletListView.SelectedItems.Count >= 0)
            {
                walletTextBox.Text = walletListView.SelectedItems[0].Text;
                walletListView.Visible = false;
            }
        }

        private void WalletListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(walletListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();           
        }
       
        private void AddQuantityForm_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseIsOutOfBounds(walletListView);
            ListViewSettings.WhenMouseIsOutOfBounds(top100ListView);
            ListViewSettings.WhenMouseIsOutOfBounds(suggestionsListView);
        }

        private void WalletTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenDownButtonPressed(walletListView, e);
            ListViewSettings.WhenUpButtonPressed(walletListView, e);
            ListViewSettings.WhenEnterButtonPressed(walletListView, e, walletTextBox, priceBox);      
            
            if(e.KeyCode == Keys.Enter)
            {
                quantityBox.Select();
            }
        }

        private void WalletListView_MouseEnter(object sender, EventArgs e)
        {
            walletListView.Focus();
        }

        private void WalletListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (walletListView.Focused)
            {
                e.Handled = true;
            }
        }

        private void SumBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(sumBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                confirmButton.Select();
            }
        }

        private void SumBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(sumBox);
        }

        private void PriceBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(priceBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                feeBox.Select();
            }
        }

        private void PriceBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(priceBox);
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
                switch(showAlert)
                {
                    case -1:
                        AlertPanelControlInstance(6);
                        break;
                    case -2:
                        MessageBox.Show("Kainos siuntimas užtruko.\nNepavyko užkrauti kainos", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }     
        }

        private void RefreshPrice_Click(object sender, EventArgs e)
        {
            if (!priceWorker.IsBusy)
            {
                showAlert = 0;
                priceWorker.RunWorkerAsync();
            }    
        }

        private void AlertPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (alertPanel.Visible == false && closePending)
            {
                invForm.Enabled = true;
                invForm.Focus();
                Close();
                Dispose();
            }
        }

        private void FeeBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(feeBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                sumBox.Select();
            }
        }

        private void FeeBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(feeBox);
            RecalculateTotalSum();
        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            operationForm = "BUY";

            if (!editMode)
            {
                operation = "BUY";
            }
 
            sellButton.BackColor = System.Drawing.SystemColors.Control;
            buyButton.BackColor = Color.Green;
            EnableForm();
            chooseCryptoLabel.Select();
        }

        private void CoinNotFoundEditModeBUY()
        {
            MessageBox.Show("Pardavimo operacija valiutai " + ctd.name + " negalima,\nnes jos kiekis pirkimo momentu buvo lygus 0.");
            buyButton.PerformClick();
        }

        private void OtherSellButtonOption()
        {
            if (editMode)
            {
                int search = walletComboBox.FindStringExact(ctd.wallet);
                if (search < 0 && operation == "BUY")
                {
                    walletComboBox.Text = "";
                    MessageBox.Show("Piniginės " + ctd.wallet + " kiekis pirkimo momentu buvo lygus 0.\nPasirinkite kitą piniginę.");
                }
            }

            buyButton.BackColor = System.Drawing.SystemColors.Control;
            sellButton.BackColor = Color.Red;
            EnableForm();
            chooseCryptoLabel.Select();
        }

        private void SellButton_Click(object sender, EventArgs e)
        {
            operationForm = "SELL";

            if (!editMode)
            {
                operation = "SELL";
            }

            int searchCoin = cryptoComboBox.FindStringExact(cryptoComboBox.Text);
            if (searchCoin < 0 && editMode && operation == "BUY")
            {
                CoinNotFoundEditModeBUY();
            }
            else
            {
                OtherSellButtonOption();
            } 
        }

        private void DisableControls()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = false;
            }
 
            confirmButton.Enabled = true;
        }

        private void EnableControls()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = true;
            }
        }
 
        private void ClearFields()
        {
            foreach (TextBox box in this.Controls.OfType<TextBox>())
            {
                if (box.Name != feeBox.Name)
                {
                    box.Text = "";
                }
            }

            foreach (ComboBox box in this.Controls.OfType<ComboBox>())
            {
                box.Text = "";
            }
        }

        private void EnableBuyControls()
        {
            cryptoComboBox.Visible = false;
            walletComboBox.Visible = false;
            maxQuantityLabel.Visible = false;
            searchPicture.Visible = true;
            cryptoBox.Visible = true;
            walletTextBox.Visible = true;

            if (!editMode)
            {
                ClearFields();
                SearchBoxSettings.OnLeavingFocus(cryptoBox);
            }
            else
            {
                cryptoBox.Enabled = false;
                buyButton.BackColor = Color.Green;
            }
        }

        private void ToggleWalletRelatedBoxes()
        {
            if (walletComboBox.Text == "")
            {
                quantityBox.Enabled = false;
                quantityBox.Text = "";
                maxQuantityLabel.Visible = false;
            }

            if (walletComboBox.Text != "" && walletComboBox.Items.Count > 0)
            {
                maxQuantityLabel.Visible = true;
            }
        }

        private void EnableSellControls()
        {
            cryptoBox.Visible = false;
            walletTextBox.Visible = false;
            searchPicture.Visible = false;
            cryptoComboBox.Visible = true;
            walletComboBox.Visible = true;
            ToggleWalletRelatedBoxes();
            
            if (!editMode)
            {
                ClearFields();
            }
            else
            {
                cryptoComboBox.Enabled = false;
                sellButton.BackColor = Color.Red;
                CountMaxQuantity();
            }
        }

        private void EnableForm()
        {
            EnableControls();

            if (operationForm == "BUY")
            {
                EnableBuyControls(); 
            }
            else if(operationForm == "SELL")
            {
                EnableSellControls();
            } 
        }

        private void CryptoBox_TextChanged(object sender, EventArgs e)
        {
            top100ListView.Visible = false;

            if (cryptoBox.Text != "")
            {
                suggestionsListView.Items.Clear();
                var list = Connection.iwdb.FetchCoinNames(cryptoBox.Text)
                    .OrderByDescending(x => x.quantity).Select(x => x.name).ToList();
                foreach (var item in list)
                {
                    suggestionsListView.Items.Add(item);
                }

                if (suggestionsListView.Items.Count > 0)
                {
                    ListViewSettings.SetColumnWidth(suggestionsListView, 4, suggestionsListView.Height, suggestionsListView.Items.Count);
                    suggestionsListView.Visible = true;
                }
            }

            if (cryptoBox.Text == "" || suggestionsListView.Items.Count == 0)
            {
                suggestionsListView.Visible = false;
            }
        }

        private void CryptoBox_Click(object sender, EventArgs e)
        {
            SearchBoxSettings.OnFocus(cryptoBox);
        }

        private void CryptoBox_Enter(object sender, EventArgs e)
        {
            if (cryptoBox.Visible == true)
            {
                cryptoBox.Select();
                SearchBoxSettings.OnFocus(cryptoBox);
            }
            else if (cryptoComboBox.Visible == true)
            {
                cryptoComboBox.Select();
            }
        }

        private void CryptoBox_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenDownButtonPressed(top100ListView, e);
            ListViewSettings.WhenUpButtonPressed(top100ListView, e);
            ListViewSettings.OnEnterButtonPressedWhenInvesting(this, top100ListView, e);
            ListViewSettings.WhenDownButtonPressed(suggestionsListView, e);
            ListViewSettings.WhenUpButtonPressed(suggestionsListView, e);
            ListViewSettings.OnEnterButtonPressedWhenInvesting(this, suggestionsListView, e);
            
            if(e.KeyCode == Keys.Enter)
            {
                walletTextBox.Select();
            } 
        }

        public void AssignNameToTextBox(string name)
        {
            cryptoBox.Text = name;
        }

        private void ClearSelections()
        {
            if (top100ListView.Visible || suggestionsListView.Visible)
            {
                top100ListView.Visible = false;
                suggestionsListView.Visible = false;
            }
            
            if (cryptoBox.Text == "")
            {
                SearchBoxSettings.OnLeavingFocus(cryptoBox);
            }

            this.ActiveControl = chooseCryptoLabel;
        }

        private void CryptoBox_Leave(object sender, EventArgs e)
        {
            if (top100ListView.Focused != true && top100ListView.Visible)
            {
                ClearSelections();
            }

            if (suggestionsListView.Focused != true && suggestionsListView.Visible)
            {
                ClearSelections();
            }

            if (cryptoBox.Text != "" && !editMode)
            {
                if (!priceWorker.IsBusy)
                {
                    showAlert = 0;
                    priceWorker.RunWorkerAsync();
                }
            }
        }

        private void CryptoBox_MouseClick(object sender, MouseEventArgs e)
        {
            top100ListView.Items[lviIndex].Selected = false; 
            ListViewSettings.SetColumnWidth(top100ListView, 4, top100ListView.Height, top100ListView.Items.Count);
            top100ListView.Visible = true;
        }

        private void SuggestionsListView_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenItIsNotFocused(suggestionsListView, e);
        }

        private void SuggestionsListView_MouseEnter(object sender, EventArgs e)
        {
            suggestionsListView.Focus();
        }

        private void SuggestionsListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(suggestionsListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }

        private void SuggestionsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suggestionsListView.SelectedItems.Count > 0)
            {
                cryptoBox.Text = suggestionsListView.SelectedItems[0].Text;               
                chooseCryptoLabel.Select();
            } 
        }

        private void Top100ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                foreach (ListViewItem item in top100ListView.Items)
                {
                    if(item.BackColor == Colours.SelectedItemColor())
                    {
                        lviIndex = top100ListView.Items.IndexOf(item);
                    }
                }
            }

            ListViewSettings.WhenItIsNotFocused(top100ListView, e); 
        }

        private void Top100ListView_MouseEnter(object sender, EventArgs e)
        {
            top100ListView.Focus();
        }

        private void Top100ListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(top100ListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }

        private void Top100ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (top100ListView.SelectedItems.Count > 0)
            {
                lviIndex = top100ListView.Items.IndexOf(top100ListView.SelectedItems[0]);
                var fixedName = FixLabelName.ReturnName(top100ListView.SelectedItems[0].Text);
                cryptoBox.Text = fixedName;
                chooseCryptoLabel.Select();
            }
        }

        private void EditModeAdd(CoinQuantity cq)
        {
            if (operation == "BUY")
            {
                double q = cq.GetCoinQuantityByNameAndDate(ctd.name, ctd.date);
                q -= ctd.quantity;

                if (q < 0.00000001)
                {
                    cryptoComboBox.Items.Remove(ctd.name);
                }
            }
            else if (operation == "SELL")
            {
                int search = cryptoComboBox.FindStringExact(ctd.name);
                if (search < 0)
                {
                    cryptoComboBox.Items.Add(ctd.name);
                }
            }
        }

        private void AddCoinsToComboBox()
        {
            cryptoComboBox.Items.Clear();
            CoinQuantity cq = new CoinQuantity();
            var coinNames = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            for (int i = 0; i < coinNames.Count; i++)
            {                
                double q = cq.GetCoinQuantityByName(coinNames[i]);

                if (q >= 0.00000001)
                {
                    cryptoComboBox.Items.Add(coinNames[i]);
                }
            }

            if (editMode)
            {
                EditModeAdd(cq);
            } 
        }

        private void Top100ListView_MouseClick(object sender, MouseEventArgs e)
        {
            walletTextBox.Select();
            if (!priceWorker.IsBusy)
            {
                showAlert = 0;
                priceWorker.RunWorkerAsync();
            }
        }

        private void SuggestionsListView_MouseClick(object sender, MouseEventArgs e)
        {            
            walletTextBox.Select();
            if (!priceWorker.IsBusy)
            {
                showAlert = 0;
                priceWorker.RunWorkerAsync();
            }
        }

        private void EditModeWallets(CoinQuantity cq)
        {
            if (operation == "BUY")
            {
                double q = cq.GetCoinQuantityByWalletAndDate(ctd.name, ctd.wallet, ctd.date);
                q -= ctd.quantity;

                if (q < 0.00000001)
                {
                    walletComboBox.Items.Remove(ctd.wallet);
                }          
            }
            else if (operation == "SELL")
            {
                int search = walletComboBox.FindStringExact(ctd.wallet);
                if (search < 0)
                {
                    walletComboBox.Items.Add(ctd.wallet);
                }            
            }
        }

        private void AddWallets()
        {
            walletComboBox.Items.Clear();
            CoinQuantity cq = new CoinQuantity();

            var wallets = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.CryptoName == cryptoComboBox.Text).Select(x => x.Wallet).Distinct().ToList();

            foreach (var wallet in wallets)
            {
                double q = cq.GetCoinQuantityByWallet(cryptoComboBox.Text, wallet);

                if (q >= 0.00000001)
                {
                    walletComboBox.Items.Add(wallet);
                }
            }

            if(editMode)
            {
                EditModeWallets(cq);
            }            
        }

        private void CryptoComboBox_Leave(object sender, EventArgs e)
        {
            AddWallets();
            walletComboBox.Text = "";
            quantityBox.Text = "";
            quantityBox.Enabled = false;
            maxQuantityLabel.Visible = false;
            
            if(cryptoComboBox.Text != "" && !editMode)
            {
                if (!priceWorker.IsBusy)
                {
                    showAlert = 0;
                    priceWorker.RunWorkerAsync();
                }
            }
        }

        private void CryptoComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (cryptoComboBox.SelectedIndex >= 0)
            {
                cryptoComboBox.Text = cryptoComboBox.Items[cryptoComboBox.SelectedIndex].ToString();
                walletComboBox.Select();
                AddWallets();
            } 
        }

        private void CheckForWalletAndExecute(bool walletFound)
        {
            if (!walletFound)
            {
                quantityBox.Enabled = false;
                maxQuantityLabel.Visible = false;
            }
 
            if (walletFound)
            {
                CoinQuantity cq = new CoinQuantity();
                double q = cq.GetCoinQuantityByWallet(cryptoComboBox.Text, walletComboBox.Text);

                if (editMode && operation == "BUY")
                {
                    q = cq.GetCoinQuantityByWalletAndDate(cryptoComboBox.Text, walletComboBox.Text, ctd.date);

                    if(walletComboBox.Text == ctd.wallet)
                    {
                        q -= ctd.quantity;
                    } 
                }
                else if (editMode && operation == "SELL")
                {
                    q = cq.GetCoinQuantityByWalletAndDate(cryptoComboBox.Text, walletComboBox.Text, ctd.date);
                    q += ctd.quantity;
                }

                maxQuantityLabel.Text = "max. " + string.Format("{0:F8}", q);
                quantityBox.Enabled = true;
                maxQuantityLabel.Visible = true;
            }
        }

        private void CountMaxQuantity()
        {
            if (editMode)
            {
                bool walletFound = false;
                if (walletComboBox.Text != "")
                {
                    walletFound = true;
                }
                CheckForWalletAndExecute(walletFound);
            }
            else
            {
                int searchCoin = cryptoComboBox.FindStringExact(cryptoComboBox.Text);
                if (searchCoin >= 0)
                {
                    bool walletFound = false;

                    int searchWallet = walletComboBox.FindStringExact(walletComboBox.Text);
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
            var quantity = maxQuantityLabel.Text.Split(' ');
            quantityBox.Text = string.Format("{0:F8}", quantity[1]);
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
            quantityBox.Select();
        }

        private void WalletListView_MouseClick(object sender, MouseEventArgs e)
        {
            quantityBox.Select();
        }

        private void WalletTextBox_Enter(object sender, EventArgs e)
        {
            lastViewItemIndex = -1;
        }

        private void HideListViews(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (top100ListView.Visible && !cryptoBox.Focused)
                {
                    top100ListView.Visible = false;
                }
                else if (suggestionsListView.Visible && !cryptoBox.Focused)
                {
                    suggestionsListView.Visible = false;
                }
                else if (walletListView.Visible && !walletTextBox.Focused)
                {
                    walletListView.Visible = false;
                } 
            }
        }

    }
}
                
                
