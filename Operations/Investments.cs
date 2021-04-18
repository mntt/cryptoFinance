using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class Investments : Form
    {
        private List<CryptoTable> operations { get; set; }
        private CurrentAssets form { get; set; }
        public bool noConnection { get; set; } 
        private bool reload { get; set; }

        private double currentValue = 0;
        private int rowIndex { get; set; }

        public Investments(CurrentAssets _form)
        {
            InitializeComponent();
            form = _form;
            reload = true;
            investmentsTimer.Start();
        }

        private void UpdateCryptoList_Click(object sender, EventArgs e)
        {
            progressBar.ForeColor = Color.Cyan;
            progressBar.Value = 0;
            progressLabel.Text = "";
            progressBarPanel.Visible = true;
            updateCryptoList.Enabled = false;
            CoingeckoListDownloader.Start(this, progressBar, progressLabel);
        }

        private void Investments_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar.Visible)
            {
                DialogResult result = MessageBox.Show("Vykdomas kriptovaliutų sąrašo atnaujinimas. " +
                    "\nAr norite nutraukti siuntimą?", "Pranešimas", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var task = Task.Run(()=> cancelButton.PerformClick());
                    task.Wait();
                    this.FormClosing -= Investments_FormClosing;
                    Application.Exit();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.FormClosing -= Investments_FormClosing;
                Application.Exit();
            }
        }

        private void InvestmentsBackButton_Click(object sender, EventArgs e)
        {
            if (progressBar.Visible)
            {
                DialogResult result = MessageBox.Show("Vykdomas kriptovaliutų sąrašo atnaujinimas. " +
                    "\nAr norite nutraukti siuntimą?", "Pranešimas", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var task = Task.Run(() => cancelButton.PerformClick());
                    task.Wait();
                    this.FormClosing -= Investments_FormClosing;
                    this.Close();
                    this.Dispose();
                    form.Show();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.FormClosing -= Investments_FormClosing;
                this.Close();
                this.Dispose();
                form.Show();
            }
        }

        private void FillDataGrid(int howMany, int multiplier)
        {
            for (int i = 0; i < howMany; i++)
            {
                try
                {
                    int firstLine = 0 + multiplier;
                    int secondLine = 1 + multiplier;
                    int thirdLine = 2 + multiplier;

                    operationDataGrid.Rows.Add();
                    operationDataGrid.Rows[firstLine].Cells[0].Value = operations[i].Date.ToString("yyyy-MM-dd");
                    operationDataGrid.Rows[firstLine].Cells[4].Value = cryptoFinance.Properties.Resources.more_options;
                    operationDataGrid.Rows.Add();
                    operationDataGrid.Rows[secondLine].Cells[0].Value = operations[i].Operation;
                    operationDataGrid.Rows[secondLine].Cells[1].Value = operations[i].CryptoName;
                    operationDataGrid.Rows[secondLine].Cells[2].Value = operations[i].Wallet;
                    operationDataGrid.Rows[secondLine].Cells[3].Value = operations[i].CryptoQuantity.ToString("0.0000");
                    operationDataGrid.Rows.Add();
                    operationDataGrid.Rows[thirdLine].Cells[0].Value = operations[i].LastPrice.ToString("C2");
                    operationDataGrid.Rows[thirdLine].Cells[1].Value = operations[i].Fee.ToString("C2");
                    operationDataGrid.Rows[thirdLine].Cells[2].Value = operations[i].Sum.ToString("C2");
                    multiplier += 3;
                }
                catch
                {
                    operationDataGrid.Rows.RemoveAt(operationDataGrid.Rows.Count - 1);
                    break;
                }
            }
        }

        private void LoadOperations(int howMany)
        {
            if (reload)
            {
                operations = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "BUY" || x.Operation == "SELL").OrderByDescending(x => x.Id).ToList();
            }

            int multiplier = 0;
            operationDataGrid.Columns[4].DefaultCellStyle.NullValue = null;
            FillDataGrid(howMany, multiplier);
            operationDataGrid.ClearSelection();
        }

        private void Investments_Load(object sender, EventArgs e)
        {
            LoadOperations(10);
            operationDataGrid.Select();
            lastTimeUpdated.Visible = true;
            LoadLastTimeUpdatedLabel();
            updateCryptoList.Visible = true;
        }

        public void LoadLastTimeUpdatedLabel()
        {            
            var date = Connection.db.GetTable<LastTimeUpdatedList>().ToList();

            if(date.Count == 0)
            {
                lastTimeUpdated.Text += "Paskutinį kart atnaujintas: niekada.";
            }
            else
            {
                lastTimeUpdated.Text = "Paskutinį kart atnaujintas: " + date[date.Count - 1].Date;
            }
        }
       
        private void CountCurrentValue()
        {
            var task1 = Task.Run(() => form.GetCurrentValue());
            task1.Wait();

            if(form.GetCannotUpdatePriceList().Count > 0)
            {
                noConnection = true;
            }

            GetCultureInfo gci = new GetCultureInfo(",");
            currentValue = ReformatText.ReturnNumeric(form.currentValueLabel.Text.Trim('€'));
        }

        private void InsertCryptoTable(DateTime date, string name, double quantity, string operation, string wallet, double sum, double price, double fee)
        {
            Connection.iwdb.InsertCryptoTable(date, name, quantity, operation, wallet, sum, price, fee, currentValue);
        }

        public void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(alertPanel, alertLabel, 152, -38, 217, 38, 29, 13);
            apc.StartPanelAnimation(chooseLabelText);
        }

        private double ReturnNewQuantity(string name, double quantity, string operation)
        {
            double newQuantity = Connection.db.GetTable<CurrentAssetsDB>()
                .Where(x => x.Cryptocurrency == name).Select(x => x.Quantity).ToList().Sum();

            if (operation == "BUY")
            {
                newQuantity += quantity;
            }
            else if (operation == "SELL")
            {
                newQuantity -= quantity;
            }

            return newQuantity;
        }

        private void InteractWithCurrentAssetsDB(string name, bool customCoin, DateTime date, double newQuantity, double price, double cv)
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

        private void InsertCurrentAssets(DateTime date, string name, bool customCoin, double quantity, double price, string operation)
        {
            var newQuantity = ReturnNewQuantity(name, quantity, operation);
            double cv = price * newQuantity;
            string id = "custom";

            if (!customCoin)
            {
                var nameSplit = name.Split('(');
                id = Connection.db.GetTable<CoingeckoCryptoList>()
                    .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                    .Select(x => x.CryptoId).ToList().First();
            }

            try
            {
                var coin = ReturnObject(name, customCoin, id, newQuantity, date, price, cv);
                InteractWithCurrentAssetsDB(name, customCoin, date, newQuantity, price, cv);
                form.RefreshData(coin);
            }
            catch
            {
                MessageBox.Show("Įvyko klaida, nepavyko atnaujinti Current Assets\nduomenų bazės.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private ConstructingLists ReturnObject(string name, bool customCoin, string id, double quantity, DateTime date, double price, double currentValue)
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (CoingeckoListDownloader.worker.IsBusy)
            {
                progressLabel.Text = "Atšaukiama...";
                progressBar.ForeColor = DefaultBackColor;
                CoingeckoListDownloader.worker.CancelAsync();
                CoingeckoListDownloader.workerCancelButtonPressed = true;
            }
        }

        public void EnableButtons()
        {
            updateCryptoList.Enabled = true;
        }

        public void OpenAddQuantityForm()
        {
            reload = true;
            Action action = RefreshOperationsDataGrid;
            AddOperation addQuantity = new AddOperation(this, action);
            this.Enabled = false;
            addQuantity.Show();
            addQuantity.Select();
        }

        public async Task ExecuteConfirm(DateTime date, string name, bool customCoin, double quantity, string operation, string wallet, double sum, double price, double fee)
        {
            InsertCurrentAssets(date, name, customCoin, quantity, price, operation);
            CountCurrentValue();
            InsertCryptoTable(date,name,quantity, operation, wallet, sum, price, fee);
            await Task.Delay(100);
        }

        private void InvestmentsTimer_Tick(object sender, EventArgs e)
        {
            investmentsTimer.Stop();
        }

        private void OpenAddQuantityEditMode(CryptoTableData ctd)
        {
            reload = false;
            Action action = RefreshOperationsDataGrid;
            AddOperation addQuantity = new AddOperation(ctd, this, action);
            this.Enabled = false;
            addQuantity.Show();
            addQuantity.Select();
        }

        private void RefreshOperationsDataGrid()
        {
            operationDataGrid.Rows.Clear();
            LoadOperations(10);
        }

        private void RefreshCurrentAssets(string name, bool customCoin, DateTime date, double price)
        {
            CoinQuantity cq = new CoinQuantity();
            double q = cq.GetCoinQuantityByName(name);
            double cv = price * q;
            string id = "custom";

            if (!customCoin)
            {
                var nameSplit = name.Split('(');
                id = Connection.db.GetTable<CoingeckoCryptoList>()
                        .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                        .Select(x => x.CryptoId).ToList().First();
            }
   
            try
            {
                var coin = ReturnObject(name, customCoin, id, q, date, price, cv);
                InteractWithCurrentAssetsDB(name, customCoin, date, q, price, cv); 
                form.RefreshData(coin);
            }
            catch
            {
                MessageBox.Show("Įvyko klaida, nepavyko atnaujinti Current Assets\nduomenų bazės.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task UpdateOperation(int id, DateTime date, string operation, string name, bool customCoin, string wallet, double quantity, double price, double fee, double sum)
        {
            Connection.iwdb.UpdateCryptoTable(id, operation, name, date, quantity, wallet, price, fee, sum);
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

            await Task.Delay(100);
        }

        private void OperationDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool firstLineTopRightCorner = (e.RowIndex == 0 || e.RowIndex % 3 == 0);

            if (e.ColumnIndex == 4 && firstLineTopRightCorner)
            {
                optionsPanel.Visible = false;
                rowIndex = e.RowIndex;

                int pointx = (int)(Cursor.Position.X / 3.7);
                int pointy = (int)(Cursor.Position.Y / 1.6);

                optionsPanel.Location = new System.Drawing.Point(pointx, pointy);
                optionsPanel.Visible = true;
            }
        }

        private void OperationDataGrid_Leave(object sender, EventArgs e)
        {
            operationDataGrid.ClearSelection();
        }

        private void OperationDataGrid_Scroll(object sender, ScrollEventArgs e)
        {
            if (operationDataGrid.DisplayedRowCount(false) +
                operationDataGrid.FirstDisplayedScrollingRowIndex >= operationDataGrid.RowCount)
            {
                var operations = Connection.db.GetTable<CryptoTable>().Where(x => x.Operation == "BUY" || x.Operation == "SELL").ToList();

                if (operations.Count == (operationDataGrid.Rows.Count / 3))
                {
                    loadMoreLabel.Visible = false;
                }
                else
                {
                    loadMoreLabel.Visible = true;
                }
            }
            else
            {
                loadMoreLabel.Visible = false;
            }
        }

        private void LoadMoreLabel_Click(object sender, EventArgs e)
        {
            var howMany = (operationDataGrid.Rows.Count / 3);
            
            if(operations.Count > howMany)
            {
                operationDataGrid.Rows.Clear();
                LoadOperations(howMany + 10);
                operationDataGrid.FirstDisplayedScrollingRowIndex = operationDataGrid.RowCount - 1;               
            } 
        }

        private void LoadMoreLabel_MouseEnter(object sender, EventArgs e)
        {
            loadMoreLabel.ForeColor = Color.AliceBlue;
        }

        private void LoadMoreLabel_MouseLeave(object sender, EventArgs e)
        {
            loadMoreLabel.ForeColor = Color.Black;
        }

        private void AddOperationPicture_Click(object sender, EventArgs e)
        {
            OpenAddQuantityForm();
        }

        private CryptoTableData CreateCryptoTableObject(int id, DateTime date, string operation, string name, string wallet, double quantity, double price, double fee, double sum)
        {
            CryptoTableData ctd = new CryptoTableData
                    (
                        id,
                        date,
                        operation,
                        name,
                        quantity,
                        wallet,
                        price,
                        fee,
                        sum
                    );

            return ctd;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            int operationIndex = rowIndex / 3;
            var id = operations[operationIndex].Id;
            var date = operations[operationIndex].Date;
            var operation = operations[operationIndex].Operation;
            var name = operations[operationIndex].CryptoName;
            var wallet = operations[operationIndex].Wallet;
            var quantity = operations[operationIndex].CryptoQuantity;
            var price = double.Parse(operations[operationIndex].LastPrice.ToString()); 
            var fee = double.Parse(operations[operationIndex].Fee.ToString());
            var sum = double.Parse(operations[operationIndex].Sum.ToString());

            var operationObject = CreateCryptoTableObject(id, date, operation, name, wallet, quantity, price, fee, sum);
            OpenAddQuantityEditMode(operationObject);
            optionsPanel.Visible = false;
        }

        private void Investments_MouseClick(object sender, MouseEventArgs e)
        {
            if (optionsPanel.Visible)
            {
                optionsPanel.Visible = false;
            }
        }

        private void OperationDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (optionsPanel.Visible)
            {
                optionsPanel.Visible = false;
            }
        }

        private async Task ExecuteDelete()
        {
            int operationIndex = rowIndex / 3;
            var id = operations[operationIndex].Id;
            var name = operations[operationIndex].CryptoName;
            var date = operations[operationIndex].Date;
            var quantity = operations[operationIndex].CryptoQuantity;
            var price = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == name).OrderByDescending(x => x.Id).First().LastPrice;
            var cv = quantity * (double)price;
            var customCoin = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.Cryptocurrency == name).Select(x => x.CustomCoin).ToList().First();
            var cryptoid = "custom";

            if (!customCoin)
            {
                var nameSplit = name.Split('(');
                cryptoid = Connection.db.GetTable<CoingeckoCryptoList>()
                    .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                    .Select(x => x.CryptoId).ToList().First();
            }

            try
            {
                Connection.iwdb.DeleteOperation(id);
                CoinQuantity cq = new CoinQuantity();
                double q = cq.GetCoinQuantityByName(name);
                InteractWithCurrentAssetsDB(name, customCoin, date, q, (double)price, cv);
                var coin = ReturnObject(name, customCoin, cryptoid, q, date, (double)price, cv);
                form.RefreshData(coin);
                RefreshOperationsDataGrid();
            }
            catch
            {
                MessageBox.Show("Įvyko klaida, nepavyko ištrinti operacijos\nir atnaujinti Current Assets duomenų.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            await Task.Delay(100);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            optionsPanel.Visible = false;
            string message = "Ar tikrai norite ištrinti šią operaciją?";
            string title = "Operacijos ištrynimas";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            { 
                try
                {
                    ShowLoading();
                    var task = Task.Run(() => ExecuteDelete());
                    task.Wait();
                }
                catch
                {
                    MessageBox.Show("Klaida. Operacijos ištrinti nepavyko.", title);
                }
                finally
                {
                    HideLoading();
                } 
            }
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
    }
}
