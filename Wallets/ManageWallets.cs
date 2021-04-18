using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class ManageWallets : Form
    {
        private List<ConstructingLists> dataGridList = new List<ConstructingLists>();
        private Form form { get; set; }

        public ManageWallets(Form _form)
        {
            InitializeComponent();
            form = _form;
        }

        private void ManageWallets_Load(object sender, EventArgs e)
        {
            transferButton.TabStop = false;
            loadingBox.Visible = true;
            manageWalletsWorker.RunWorkerAsync();           
        }

        private void FetchAllData()
        {
            CoinQuantity cq = new CoinQuantity();
            var coinNames = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            foreach (var name in coinNames)
            {
                var distinctWallets = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.CryptoName == name).Select(x => x.Wallet).Distinct().ToList();

                foreach (var wallet in distinctWallets)
                {
                    var q = cq.GetCoinQuantityByWallet(name, wallet);

                    if (q >= 0.00000001)
                    {
                        AddDataToDataGridList(q, name, wallet);
                    }
                }
            }
        }

        private void AddDataToDataGridList(double q, string name, string wallet)
        {
            ConstructingLists info = new ConstructingLists(name, q, wallet);
            dataGridList.Add(info);
        }

        private void AddDataToDataGrid()
        {
            GetCultureInfo gci = new GetCultureInfo(",");

            for (int i = 0; i < dataGridList.Count; i++)
            {
                walletDataGrid.Rows.Add();
                walletDataGrid.Rows[i].Cells[0].Value = dataGridList[i].name;
                walletDataGrid.Rows[i].Cells[1].Value = dataGridList[i].wallet;
                walletDataGrid.Rows[i].Cells[2].Value = dataGridList[i].quantity.ToString("0.00000000");
            }
        }

        private void ManageWallets_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.FormClosing -= ManageWallets_FormClosing;
            this.Close();
            form.Show();
        }

        public void RefreshDataGrid()
        {
            walletDataGrid.Rows.Clear();
            dataGridList.Clear();
            FetchAllData();
            AddDataToDataGrid();
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < walletDataGrid.Rows.Count; i++)
            {
                ConstructingLists info = new ConstructingLists
                    (
                    walletDataGrid.Rows[i].Cells[0].Value.ToString(),
                    double.Parse(walletDataGrid.Rows[i].Cells[2].Value.ToString()),
                    walletDataGrid.Rows[i].Cells[1].Value.ToString()                    
                    );

                dataGridList.Add(info);
            }

            Action action = RefreshDataGrid;
            TransferCoins tc = new TransferCoins(action, dataGridList);
            tc.Show();
        }

        private void ManageWalletsWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            FetchAllData(); 
        }

        private void ManageWalletsWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            AddDataToDataGrid();
            loadingBox.Visible = false;
        }

    }
}
