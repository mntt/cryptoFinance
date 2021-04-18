using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class TransferCoins : Form
    {
        private List<ConstructingLists> dataGridList { get; set; }
        private Action refreshDataGrid { get; set; }
        private int lastViewItemIndex { get; set; }

        public TransferCoins(Action _refreshDataGrid, List<ConstructingLists> _dataGridList)
        {
            InitializeComponent();
            dataGridList = _dataGridList;
            refreshDataGrid = _refreshDataGrid;
        }

        private bool Validation()
        {
            bool validation = false;

            if (maxLabel.Visible)
            {
                var split = maxLabel.Text.Split('.');
                validation = FormValidation.ReturnValidation(Controls, split[1], quantityBox.Text);

                if(walletOutComboBox.Text == walletInBox.Text)
                { 
                    validation = false;
                }
                MessageBox.Show("last: "+validation+"");
            }

            return validation;
        }

        private void TransferCoins_Load(object sender, EventArgs e)
        {
            ListViewSettings.Format(walletInListView);
            walletOutComboBox.Items.Clear();
            nameComboBox.Items.Clear();
            FillNameComboBox();
            nameComboBox.Select();
        }

        private void FillNameComboBox()
        {
            CoinQuantity cq = new CoinQuantity();
            var coinNames = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            foreach (var name in coinNames)
            {
                var q = cq.GetCoinQuantityByName(name);
                if (q >= 0.00000001)
                {
                    nameComboBox.Items.Add(name);
                }     
            }
        }

        private void ExecuteIfWalletIsFound()
        {
            CoinQuantity cq = new CoinQuantity();
            var wallets = Connection.db.GetTable<CryptoTable>()
               .Where(x => x.CryptoName == nameComboBox.Text).Select(x => x.Wallet).Distinct().ToList();
            walletOutComboBox.Items.Clear();

            foreach (var wallet in wallets)
            {
                var q = cq.GetCoinQuantityByWallet(nameComboBox.Text, wallet);

                if (q > 0.00000001)
                {
                    walletOutComboBox.Items.Add(wallet);
                }
            }
        }

        private void ExecuteIfWalletIsNotFound()
        {
            walletOutComboBox.Items.Clear();
            quantityBox.Text = "";
            walletOutComboBox.Text = "";
            maxLabel.Visible = false;
        }

        private void NameComboBox_TextChanged(object sender, EventArgs e)
        {
            bool nameIsFound = false;

            var search = nameComboBox.FindStringExact(nameComboBox.Text);
            if(search > 0)
            {
                nameIsFound = true;
            }

            if (nameIsFound)
            {
                ExecuteIfWalletIsFound();
            }
            else
            {
                ExecuteIfWalletIsNotFound();
            } 
        }

        private void WalletOutComboBox_TextChanged(object sender, EventArgs e)
        {
            var quantity = dataGridList
                .Where(x => x.name == nameComboBox.Text && x.wallet == walletOutComboBox.Text)
                .Select(x => x.quantity).ToList();
               
            if(quantity.Count > 0)
            {
                double maxQ = quantity[0];
                maxLabel.Visible = true;
                maxLabel.Text = "max. " + string.Format("{0:F8}", maxQ);
            }

            if(walletOutComboBox.Text == "")
            {
                maxLabel.Visible = false;
                quantityBox.Text = "";
            }
        }

        private void MaxLabel_Click(object sender, EventArgs e)
        {
            var q = maxLabel.Text.Split(' ');

            quantityBox.Text = q[1].ToString();
        }

        private void QuantityBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.QuantityBox(quantityBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                feeBox.Select();
            }
        }

        private void InsertWalletOutInfo(string coinName, double quantity, string operation, string walletOut, double fee)
        {
            Connection.iwdb.InsertCryptoTable(DateTime.Today, coinName, quantity, operation, walletOut, 0, 0, fee, 0);
        }

        private void InsertWalletInInfo(string coinName, double quantity, string operation, string walletIn)
        {
            Connection.iwdb.InsertCryptoTable(DateTime.Today, coinName, quantity, operation, walletIn, 0, 0, 0, 0);
        }

        private void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(alertPanel, alertLabel, 115, -38, 217, 38, 29, 13);
            apc.StartPanelAnimation(chooseLabelText);
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

        private void TransferButton_Click(object sender, EventArgs e)
        {
            if (!Validation() && !alertPanel.Visible)
            {
                AlertPanelControlInstance(3);
            }
            else
            {
                transferButton.Enabled = false;
                DisableControls();
                ExecuteTransfer();
            }
        }

        private void DisableControls()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = false;
            }
        }

        private async Task UpdateDatabase()
        {
            InsertWalletOutInfo(nameComboBox.Text, double.Parse(quantityBox.Text), "TRANSFER_OUT", walletOutComboBox.Text, ReformatText.ReturnNumeric(feeBox.Text));
            InsertWalletInInfo(nameComboBox.Text, double.Parse(quantityBox.Text), "TRANSFER_IN", walletInBox.Text);
            await Task.Delay(100);
        }

        private async void ExecuteTransfer()
        {
            try
            {
                ShowLoading();
                await Task.Run(() => UpdateDatabase());
            }
            catch
            {
                MessageBox.Show("Įvyko klaida. Perkėlimas nepavyko.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                refreshDataGrid();
                HideLoading();
                AlertPanelControlInstance(23);
            }
        }

        private void FeeBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(feeBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                walletInBox.Select();
            }
        }

        private void ToggleWalletAlert()
        {
            if (walletInBox.Text == walletOutComboBox.Text)
            {
                walletInBox.ForeColor = Color.Red;
                walletInAlert.Visible = true;
                walletInListView.Visible = false;
            }
            else
            {
                walletInBox.ForeColor = Color.Black;
                walletInAlert.Visible = false;

                if (walletInListView.Items.Count > 0)
                {
                    ListViewSettings.SetColumnWidth(walletInListView, 4, walletInListView.Height, walletInListView.Items.Count);
                    walletInListView.Visible = true;
                }
                else
                {
                    walletInListView.Visible = false;
                }
            }
        }

        private void WalletInBox_TextChanged(object sender, EventArgs e)
        {
            if (walletInBox.Text != "")
            {
                walletInAlert.Visible = false;
                walletInBox.ForeColor = Color.Black;

                walletInListView.Items.Clear();
                var list = Connection.iwdb.FetchWallets(walletInBox.Text)
                    .OrderBy(x => x).Distinct().ToList();
                foreach(var item in list)
                {
                    walletInListView.Items.Add(item);
                }

                ToggleWalletAlert();
            }
            else
            {
                walletInListView.Visible = false;
                walletInAlert.Visible = false;
            }
        }

        private void AlertPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(alertPanel.Visible == false && transferButton.Enabled == false)
            {
                this.Close();
            }
        }

        private void QuantityBox_TextChanged(object sender, EventArgs e)
        {
            if(quantityBox.Text != "")
            {
                var temp = maxLabel.Text.Split(' ');

                if (double.Parse(quantityBox.Text) > double.Parse(temp[1]))
                {
                    quantityBox.ForeColor = Color.Red;
                    maxAlert.Visible = true;
                }
                else
                {
                    quantityBox.ForeColor = Color.Black;
                    maxAlert.Visible = false;
                }
            }
            else
            {
                quantityBox.ForeColor = Color.Black;
                maxAlert.Visible = false;
            }
        }

        private void TransferCoins_Click(object sender, EventArgs e)
        {
            if(walletInListView.Visible == true)
            {
                walletInListView.Visible = false;
            }

            cryptoLabel.Select();
        }

        private void NameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            walletOutComboBox.Text = "";
            quantityBox.Text = "";
            maxLabel.Visible = false;
            walletOutComboBox.Select();   
        }

        private void WalletOutComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            quantityBox.Select();
        }

        private void WalletInListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (walletInListView.SelectedItems.Count >= 0)
            {
                walletInBox.Text = walletInListView.SelectedItems[0].Text;
                walletInListView.Visible = false;
            }
        }

        private void WalletInBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if(walletInListView.Visible == true)
                {
                    walletInListView.Visible = false;
                }
            }
        }

        private void WalletInListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(walletInListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }
 
        private void TransferCoins_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseIsOutOfBounds(walletInListView);
        }

        private void NameComboBox_Leave(object sender, EventArgs e)
        {
            new System.Threading.Timer((s) =>
            {
                nameComboBox.Invoke(new Action(() =>
                {
                    nameComboBox.Select(0, 0);
                }));
            }, null, 10, System.Threading.Timeout.Infinite);
        }

        private void WalletOutComboBox_Leave(object sender, EventArgs e)
        {
            new System.Threading.Timer((s) =>
            {
                nameComboBox.Invoke(new Action(() =>
                {
                    walletOutComboBox.Select(0, 0);
                }));
            }, null, 10, System.Threading.Timeout.Infinite);
        }

        private void WalletInBox_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenDownButtonPressed(walletInListView, e);
            ListViewSettings.WhenUpButtonPressed(walletInListView, e); 
            ListViewSettings.WhenEnterButtonPressed(walletInListView, e, walletInBox, transferToLabel);           
        }

        private void WalletInListView_MouseEnter(object sender, EventArgs e)
        {
            walletInListView.Focus();
        }

        private void WalletInListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (walletInListView.Focused)
            {
                e.Handled = true;
            }
        }

        private void NameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void WalletOutComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
