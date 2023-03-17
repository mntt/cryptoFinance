using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class OperationsForm
    {
        private CurrentAssets ca { get; set; }
        private DataGridForm dgf { get; set; }
        private CurrentAssets forOptionsPanel { get; set; }
        private List<CryptoTable> operations { get; set; }
        private List<CryptoTable> list { get; set; }
        private bool reload { get; set; }
        private System.Timers.Timer optionsPanelTimer = new System.Timers.Timer();
        private WalletsForm wf { get; set; }
        private AddOperationForm aof { get; set; }
        private CryptoTable updatedItem { get; set; }

        private bool filter { get; set; }

        public OperationsForm(CurrentAssets _ca, DataGridForm _dgf)
        {
            ca = _ca;
            dgf = _dgf;
            optionsPanelTimer.Elapsed += new ElapsedEventHandler(OptionPanelTimer_Tick);
            optionsPanelTimer.Interval = 175;
            ca.operationDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OperationDataGrid_CellContentClick);
            ca.operationDataGrid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OperationDataGrid_Scroll);
            ca.loadMoreLabel.Click += new System.EventHandler(this.LoadMoreLabel_Click);
            ca.editButton.Click += new System.EventHandler(this.EditButton_Click);
            ca.Click += new System.EventHandler(this.CurrentAssets_Click);
            ca.operationDataGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OperationDataGrid_CellMouseClick);
            ca.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            ca.loadMoreLabel.MouseEnter += new System.EventHandler(this.LoadMoreLabel_MouseEnter);
            ca.loadMoreLabel.MouseLeave += new System.EventHandler(this.LoadMoreLabel_MouseLeave);
            ca.operationDataGrid.VisibleChanged += new System.EventHandler(this.OperationDataGrid_VisibleChanged);
            ca.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            ca.addOperation.Click += new System.EventHandler(this.AddOperation_Click);
            ca.operationDataGrid.SelectionChanged += new System.EventHandler(OperationDataGrid_SelectionChanged);
            ca.operationDataGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(OperationDataGrid_CellFormatting);
            ca.operationDataGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(OperationDataGrid_CellPainting);
            ca.filterOperationsBox.TextChanged += new System.EventHandler(FilterOperationsBox_TextChanged);

            LoadOperationsForm();
        }

        private void FilterOperationsBox_TextChanged(object sender, EventArgs e) 
        {
            if (ca.filterOperationsBox.Text.Length > 0)
            {
                filter = true;
                list = operations.Where(x => x.CryptoName.ToLower().Contains(ca.filterOperationsBox.Text.ToLower())).ToList();
                reload = false;
                LoadOperationsDataGrid(ca, 10);
            }
            else
            {
                filter = false;
                LoadOperationsForm();
            }
        }

        private void OperationDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //kodas sutvarko spalvu persiliejima
            using (Brush gridBrush = new SolidBrush(ca.operationDataGrid.GridColor))
            {
                using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        e.PaintContent(e.ClipBounds);
                        e.Handled = true;
                    }
                }
            } 
        }

        private void OperationDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int counter = 3;

            for (int i = 0; i < ca.operationDataGrid.Rows.Count; i++)
            {
                if(i == counter)
                {
                    ca.operationDataGrid.Rows[i].DefaultCellStyle.BackColor = Colours.formBackground;
                    counter += 4;
                }
            }
        }

        private void OperationDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ca.operationDataGrid.ClearSelection();
        }

        public List<CryptoTable> ReturnOperations() 
        {
            return operations;
        }

        public void SwapOperations(List<CryptoTable> _operations)
        {
            operations = _operations;
        }

        private void AddOperation_Click(object sender, EventArgs e)
        {
            ca.assetAlertLabel.Visible = false;
            ca.investmentsPanel.Visible = false;
            ca.buyButton.BackColor = Colours.transparent;
            ca.buyButton.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;
            ca.sellButton.BackColor = Colours.transparent;
            ca.sellButton.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;

            foreach (Control item in ca.contentPanel.Controls)
            {
                item.Visible = true;
                item.Enabled = true;

                if (item.Tag == "buy" || item.Tag == "sell" || item.Name == "priceBox" || item.Name == "quantityBox" || item.Name == "sumBox")
                {
                    item.Text = "";
                }
            }

            foreach (Control item in ca.addOperationPanel.Controls)
            {
                item.Visible = true;
                item.Enabled = true;
            }

            ca.top100listview.Clear();
            ca.suggestionsListView.Clear();
            ca.cryptoComboBox.Items.Clear();
            ca.walletComboBox.Items.Clear();
            ca.contentPanel.Visible = false;

            bool showAssetAlert = operations.Count == 0 ? true : false;
            aof = new AddOperationForm(ca, this, wf, dgf, showAssetAlert);
            ca.addOperationPanel.Location = new Point(117, 102);
            ca.addOperationPanel.Visible = true;
        }

        private void OptionPanelTimer_Tick(object source, EventArgs e)
        {
            forOptionsPanel.optionsPanel.Visible = true;
            
            optionsPanelTimer.Stop();
        }

        private void OperationDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OnCellContentClick(ca, e);
        }

        private void OperationDataGrid_Scroll(object sender, ScrollEventArgs e)
        {
            OnScroll(ca);
        }

        private void LoadMoreLabel_Click(object sender, EventArgs e)
        {
            LoadMoreLabel(ca);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!filter)
            {
                Edit(ca, ca.rowIndex, operations);
            }
            else
            {
                Edit(ca, ca.rowIndex, list);
            }
        }

        private void Edit(CurrentAssets form, int rowIndex, List<CryptoTable> operations)
        {
            int operationIndex = rowIndex / 4;
            var id = operations[operationIndex].Id;
            var operationID = operations[operationIndex].OperationID;
            var date = operations[operationIndex].Date;
            var operation = operations[operationIndex].Operation;
            var cryptoId = operations[operationIndex].CryptoId;
            var name = operations[operationIndex].CryptoName;
            var wallet = operations[operationIndex].Wallet;
            var quantity = operations[operationIndex].CryptoQuantity;
            var price = decimal.Parse(operations[operationIndex].LastPrice.ToString());
            var fee = decimal.Parse(operations[operationIndex].Fee.ToString());
            var sum = decimal.Parse(operations[operationIndex].Sum.ToString());

            var operationObject = CreateCryptoTableObject(id, operationID, date, operation, cryptoId, name, wallet, quantity, price, fee, sum);
            OpenAddOperationEditMode(form, operationObject);
            form.optionsPanel.Visible = false;
        }

        private void CurrentAssets_Click(object sender, EventArgs e)
        {
            HideOptionsPanel(ca);
        }

        private void OperationDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            HideOptionsPanel(ca);
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            ca.ShowLoading();
            foreach (Control item in ca.Controls)
            {
                item.Enabled = false;
            }
            await Task.Run(() => ExecuteDelete(ca, ca.rowIndex));

            if (operations.Count == 0)
            {
                ca.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
                ca.addOperation.Location = new Point(290, 140);
                ca.dataGridCurrentAssets.Visible = false;
                ca.pieChart.Visible = false;
            }

            ca.HideLoading();
        }

        private void LoadMoreLabel_MouseEnter(object sender, EventArgs e)
        {
            ca.loadMoreLabel.ForeColor = Colours.selectedItem;
        }

        private void LoadMoreLabel_MouseLeave(object sender, EventArgs e)
        {
            ca.loadMoreLabel.ForeColor = Colours.labelColor;
        }

        private void OperationDataGrid_VisibleChanged(object sender, EventArgs e)
        {
            HideGridSelection(ca);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ca.optionsPanel.Visible = false;
        }

        public void Open(CurrentAssets form, WalletsForm _wf)
        {
            wf = _wf;

            if (!form.investmentsPanel.Visible)
            {
                form.operationDataGrid.ClearSelection();
                form.investmentsPanel.Visible = true;
                form.filterOperationsBox.Text = "";
                form.filterOperationsBox.Visible = true;
                form.operationSearch.Visible = true;

                if(operations.Count == 0)
                {
                    ca.filterOperationsBox.Visible = false;
                    ca.operationSearch.Visible = false;
                    form.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
                    ca.addOperation.Location = new Point(290, 140);
                }
            }
            else
            {
                form.investmentsPanel.Visible = false;
            }
        }
 
        public void LoadOperationsForm()
        {            
            reload = true;
            operations = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "BUY" || x.Operation == "SELL").OrderByDescending(x => x.Date).ToList();
            LoadOperationsDataGrid(ca, 10);            
        }

        private CryptoTableData CreateCryptoTableObject(int id, int operationID, DateTime date, string operation, string cryptoId, string name, string wallet, decimal quantity, decimal price, decimal fee, decimal sum)
        {
            CryptoTableData ctd = new CryptoTableData
                    (
                        id,
                        operationID,
                        date,
                        operation,
                        cryptoId,
                        name,
                        quantity,
                        wallet,
                        price,
                        fee,
                        sum
                    );

            return ctd;
        }

        public void LoadMoreLabel(CurrentAssets form)
        {
            var howMany = (form.operationDataGrid.Rows.Count / 4);

            if (!filter)
            {
                if (operations.Count > howMany)
                {
                    form.operationDataGrid.Rows.Clear();
                    LoadOperationsDataGrid(form, howMany + 10);
                    form.operationDataGrid.FirstDisplayedScrollingRowIndex = form.operationDataGrid.RowCount - 1;
                }
            }
            else
            {
                if (list.Count > howMany)
                {
                    form.operationDataGrid.Rows.Clear();
                    LoadOperationsDataGrid(form, howMany + 10);
                    form.operationDataGrid.FirstDisplayedScrollingRowIndex = form.operationDataGrid.RowCount - 1;
                }
            }
        }

        public void OnScroll(CurrentAssets form)
        {
            if (form.operationDataGrid.DisplayedRowCount(false) +
                form.operationDataGrid.FirstDisplayedScrollingRowIndex >= form.operationDataGrid.RowCount)
            {
                if (!filter)
                {
                    var operations = Connection.db.GetTable<CryptoTable>().Where(x => x.Operation == "BUY" || x.Operation == "SELL").ToList();
                    form.loadMoreLabel.Visible = operations.Count <= (form.operationDataGrid.Rows.Count / 4) ? false : true;
                }
                else
                {
                    form.loadMoreLabel.Visible = list.Count <= (form.operationDataGrid.Rows.Count / 4) ? false : true;
                }
            }
            else
            {
                form.loadMoreLabel.Visible = false;
            }
        }

        public void OnCellContentClick(CurrentAssets form, DataGridViewCellEventArgs e)
        {
            forOptionsPanel = form;
            bool firstLineTopRightCorner = (e.RowIndex == 0 || e.RowIndex % 4 == 0);

            if (e.ColumnIndex == 6 && firstLineTopRightCorner)
            {
                form.optionsPanel.Visible = false;
                form.rowIndex = e.RowIndex;

                int pointx = 572;
                int pointy = Cursor.Position.Y - form.Location.Y - 130;

                if (pointy >= 215)
                {
                    pointy = 214;
                }

                form.optionsPanel.Location = new System.Drawing.Point(pointx, pointy);

                form.optionsPanel.Visible = true;

                optionsPanelTimer.Start();
            }
        }

        private void AssignNormalValues(CurrentAssets form, int counter, int firstLine, int secondLine, int thirdLine, int fourthLine)
        {
            GetCultureInfo gci = new GetCultureInfo(",");

            if (operations[counter].Operation == "SELL")
            {
                var buylogo = cryptoFinance.Properties.Resources.sellLogo;
                form.operationDataGrid.Rows[firstLine].Cells[0].Value = ResizeImage(buylogo, 22, 22);
            }
            else if (operations[counter].Operation == "BUY")
            {
                var selllogo = cryptoFinance.Properties.Resources.buyLogo;
                form.operationDataGrid.Rows[firstLine].Cells[0].Value = ResizeImage(selllogo, 22, 22);
            }

            form.operationDataGrid.Rows[firstLine].Cells[1].Value = operations[counter].Date.ToString("yyyy-MM-dd HH:mm");

            var moreoptions = cryptoFinance.Properties.Resources.more_options;
            form.operationDataGrid.Rows[firstLine].Cells[6].Value = ResizeImage(moreoptions, 22, 19);
            form.operationDataGrid.Rows.Add();
            form.operationDataGrid.Rows[secondLine].Cells[1].Value = "Kriptovaliuta";
            form.operationDataGrid.Rows[secondLine].Cells[2].Value = "Piniginė";
            form.operationDataGrid.Rows[secondLine].Cells[3].Value = "Kiekis";
            form.operationDataGrid.Rows[secondLine].Cells[4].Value = "Kaina";
            form.operationDataGrid.Rows[secondLine].Cells[5].Value = "Suma ir mokesčiai";
            form.operationDataGrid.Rows[secondLine].DefaultCellStyle.ForeColor = Colours.alternateCellBack;
            form.operationDataGrid.Rows.Add();

            var logo = Connection.iwdb.GetLogo(new ConstructingLists(operations[counter].CryptoName, operations[counter].CustomCoin));
            form.operationDataGrid.Rows[thirdLine].Cells[0].Value = ResizeImage(logo, 18, 18);

            form.operationDataGrid.Rows[thirdLine].Cells[1].Value = operations[counter].CryptoName;
            form.operationDataGrid.Rows[thirdLine].Cells[2].Value = operations[counter].Wallet;

            string quantityValue = operations[counter].CryptoQuantity.ToString("N8").TrimEnd('0');
            char[] qchars = quantityValue.ToCharArray();

            if (qchars.Last() == ',')
            {
                quantityValue = quantityValue.Trim(',');
            }
            form.operationDataGrid.Rows[thirdLine].Cells[3].Value = quantityValue;

            string priceValue = operations[counter].LastPrice.ToString("N8").TrimEnd('0');
            char[] pchars = priceValue.ToCharArray();
            if (pchars.Last() == ',')
            {
                priceValue = priceValue.Trim(',');
            }
            form.operationDataGrid.Rows[thirdLine].Cells[4].Value = priceValue + " €";
            
            form.operationDataGrid.Rows[thirdLine].Cells[5].Value = operations[counter].Sum.ToString("C2") + " + " + operations[counter].Fee.ToString("C2");
            form.operationDataGrid.Rows.Add();
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            if (image != null)
            {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            else
            {
                return null;
            }
        }

        private void DataGridDesign(CurrentAssets form)
        {
            form.operationDataGrid.Columns[0].DefaultCellStyle.NullValue = null;
            form.operationDataGrid.Columns[0].DefaultCellStyle.SelectionBackColor = Colours.formBackground;
            form.operationDataGrid.Columns[6].DefaultCellStyle.NullValue = null;
            form.operationDataGrid.Columns[6].DefaultCellStyle.SelectionBackColor = Colours.formBackground;
        }

        private void AssignFilteredValues(CurrentAssets form, List<CryptoTable> list, int counter, int firstLine, int secondLine, int thirdLine, int fourthLine)
        {
            if (list[counter].Operation == "SELL")
            {
                var buylogo = cryptoFinance.Properties.Resources.sellLogo;
                form.operationDataGrid.Rows[firstLine].Cells[0].Value = ResizeImage(buylogo, 22, 22);
            }
            else if (list[counter].Operation == "BUY")
            {
                var selllogo = cryptoFinance.Properties.Resources.buyLogo;
                form.operationDataGrid.Rows[firstLine].Cells[0].Value = ResizeImage(selllogo, 22, 22);
            }

            form.operationDataGrid.Rows[firstLine].Cells[1].Value = list[counter].Date.ToString("yyyy-MM-dd HH:mm");

            var moreoptions = cryptoFinance.Properties.Resources.more_options;
            form.operationDataGrid.Rows[firstLine].Cells[6].Value = ResizeImage(moreoptions, 22, 19);
            form.operationDataGrid.Rows.Add();
            form.operationDataGrid.Rows[secondLine].Cells[1].Value = "Kriptovaliuta";
            form.operationDataGrid.Rows[secondLine].Cells[2].Value = "Piniginė";
            form.operationDataGrid.Rows[secondLine].Cells[3].Value = "Kiekis";
            form.operationDataGrid.Rows[secondLine].Cells[4].Value = "Kaina";
            form.operationDataGrid.Rows[secondLine].Cells[5].Value = "Suma ir mokesčiai";
            form.operationDataGrid.Rows[secondLine].DefaultCellStyle.ForeColor = Colours.alternateCellBack;
            form.operationDataGrid.Rows.Add();

            var logo = Connection.iwdb.GetLogo(new ConstructingLists(list[counter].CryptoName, list[counter].CustomCoin));
            form.operationDataGrid.Rows[thirdLine].Cells[0].Value = ResizeImage(logo, 18, 18);

            form.operationDataGrid.Rows[thirdLine].Cells[1].Value = list[counter].CryptoName;
            form.operationDataGrid.Rows[thirdLine].Cells[2].Value = list[counter].Wallet;

            string quantityValue = list[counter].CryptoQuantity.ToString("N8").TrimEnd('0');
            char[] qchars = quantityValue.ToCharArray();
            if (qchars.Last() == ',')
            {
                quantityValue = quantityValue.Trim(',');
            }
            form.operationDataGrid.Rows[thirdLine].Cells[3].Value = quantityValue;

            string priceValue = list[counter].LastPrice.ToString("N8").TrimEnd('0');
            char[] pchars = priceValue.ToCharArray();
            if (pchars.Last() == ',')
            {
                priceValue = priceValue.Trim(',');
            }
            form.operationDataGrid.Rows[thirdLine].Cells[4].Value = priceValue + " €";


            form.operationDataGrid.Rows[thirdLine].Cells[5].Value = list[counter].Sum.ToString("C2") + " + " + list[counter].Fee.ToString("C2");
            form.operationDataGrid.Rows.Add();
        }

        private void FillDataGrid(CurrentAssets form, List<CryptoTable> list, int howMany, int multiplier)
        {
            form.operationDataGrid.Visible = false;
            form.operationDataGrid.Rows.Clear();

            for (int i = 0; i < howMany; i++)
            {
                try
                {
                    int firstLine = 0 + multiplier;
                    int secondLine = 1 + multiplier;
                    int thirdLine = 2 + multiplier;
                    int fourthLine = 3 + multiplier;
                    form.operationDataGrid.Rows.Add();

                    if (!filter)
                    {
                        AssignNormalValues(form, i, firstLine, secondLine, thirdLine, fourthLine);
                    }
                    else
                    {
                        AssignFilteredValues(form, list, i, firstLine, secondLine, thirdLine, fourthLine);
                    }

                    multiplier += 4;
                }
                catch
                {
                    form.operationDataGrid.Rows.RemoveAt(form.operationDataGrid.Rows.Count - 1);
                    break;
                } 
            }

            DataGridDesign(form);
            
            form.operationDataGrid.Visible = true;
        }

        private void LoadOperationsDataGrid(CurrentAssets form, int howMany)
        {
            if (reload)
            {
                if(updatedItem != null)
                {
                    operations.Where(x => x.Id == updatedItem.Id).ToList().ForEach(x => x.CryptoQuantity = updatedItem.CryptoQuantity);
                    operations.Where(x => x.Id == updatedItem.Id).ToList().ForEach(x => x.Sum = updatedItem.Sum);
                    operations.Where(x => x.Id == updatedItem.Id).ToList().ForEach(x => x.Fee = updatedItem.Fee);
                }
            }

            int multiplier = 0;
            FillDataGrid(form, list, howMany, multiplier);
            
            form.operationDataGrid.ClearSelection();
        }

        private decimal ReturnNewQuantity(string name, decimal quantity, string operation)
        {
            decimal newQuantity = Connection.db.GetTable<CurrentAssetsDB>()
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

        private ConstructingLists ReturnObject(string name, bool customCoin, string cryptoId, decimal quantity, DateTime date, decimal price, decimal currentValue)
        {
            ConstructingLists info = new ConstructingLists
                    (
                    date,
                    name,
                    customCoin,
                    cryptoId,
                    quantity,
                    price,
                    currentValue
                    );

            return info;
        }

        private void InteractWithCurrentAssetsDB(string cryptoId, string name, bool customCoin, DateTime date, decimal newQuantity, decimal price, decimal cv)
        {
            var coin = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.Cryptocurrency == name).ToList();

            if (coin.Count == 0)
            {
                Connection.iwdb.InsertCurrentAssets(cryptoId, name, customCoin, newQuantity, date, price, cv);
            }
            else
            {
                Connection.iwdb.UpdateCurrentAssets(true, cryptoId, newQuantity, date, price, cv);
            }
        }

        public void RefreshCurrentCoins(ConstructingLists coinObject)
        {
            List<ConstructingLists> coinList = dgf.ReturnCoinList();
            List<ConstructingLists> search = coinList.Where(x => x.cryptoId == coinObject.cryptoId).ToList();

            if (search.Count > 0)
            {
                dgf.UpdateDataInCoinList(coinObject);
            }
            else
            {
                dgf.AddObjectToCoinList(coinObject);
            }
        }

        public async void InsertCurrentAssets(DateTime date, string cryptoId, string name, bool customCoin, decimal quantity, decimal price, string operation)
        {
            var newQuantity = ReturnNewQuantity(name, quantity, operation);
            decimal cv = price * newQuantity;
            var coin = ReturnObject(name, customCoin, cryptoId, newQuantity, date, price, cv);
            InteractWithCurrentAssetsDB(cryptoId, name, customCoin, date, newQuantity, price, cv);
            await Task.Run(() => RefreshCurrentCoins(coin));
        }

        public void InsertCryptoTable(int operationID, DateTime date, string cryptoId, string name, bool customCoin, decimal quantity, string operation, string wallet, decimal sum, decimal price, decimal fee, decimal currentValue)
        {
            Connection.iwdb.InsertCryptoTable(operationID, date, cryptoId, name, customCoin, quantity, operation, wallet, sum, price, fee, currentValue);
        }

        private void OpenAddOperationEditMode(CurrentAssets form, CryptoTableData ctd)
        { 
            ca.investmentsPanel.Visible = false;
            
            ca.buyButton.BackColor = Colours.transparent;
            ca.buyButton.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;
            ca.sellButton.BackColor = Colours.transparent;
            ca.sellButton.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;

            foreach (Control item in ca.contentPanel.Controls)
            {
                item.Visible = true;
                item.Enabled = true;

                if (item.Tag == "buy" || item.Tag == "sell" || item.Name == "priceBox" || item.Name == "quantityBox" || item.Name == "sumBox")
                {
                    item.Text = "";
                }
            }

            foreach(Control item in ca.addOperationPanel.Controls)
            {
                item.Visible = true;
                item.Enabled = true;
            }

            ca.top100listview.Clear();
            ca.suggestionsListView.Clear();
            ca.cryptoComboBox.Items.Clear();
            ca.walletComboBox.Items.Clear();

            ca.contentPanel.Visible = true;

            aof = new AddOperationForm(ctd, form, this, wf, dgf);

            ca.addOperationPanel.Location = new Point(117, 102);
            ca.addOperationPanel.Visible = true;
        }

        private async Task DeletionFilter(CurrentAssets form, int rowIndex)
        {
            int operationIndex = rowIndex / 4;
            var operationid = list[operationIndex].OperationID;
            var id = list[operationIndex].Id;
            var cryptoId = list[operationIndex].CryptoId;
            var name = list[operationIndex].CryptoName;
            var date = list[operationIndex].Date;
            var operation = list[operationIndex].Operation;
            var quantity = list[operationIndex].CryptoQuantity;
            var price = (decimal)Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoId == cryptoId).OrderByDescending(x => x.Id).First().LastPrice;
            var wallet = list[operationIndex].Wallet;
            var fee = (decimal)list[operationIndex].Fee;
            var sum = (decimal)list[operationIndex].Sum;
            var customCoin = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.CryptoId == cryptoId).Select(x => x.CustomCoin).ToList().Last();
            decimal oldPrice = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.CryptoId == cryptoId).Select(x => x.Price).First();
            var testPrice = GetPrices.ById(cryptoId);
            decimal realPrice = testPrice > 0 ? testPrice : oldPrice;
            var cv = quantity * realPrice;

            List<CryptoTable> ids = new List<CryptoTable>();
            ids.Add(list[operationIndex]);

            if (operation == "BUY")
            {
                decimal counter = 0;

                var z = Connection.db.GetTable<CryptoTable>()
                        .Where(x => x.Operation == "SELL" && x.Id > id && x.CryptoId == cryptoId && x.Wallet == wallet).ToList();

                if (z.Count > 0)
                {
                    for (int j = 0; j < z.Count; j++)
                    {
                        counter += z[j].CryptoQuantity;
                        ids.Add(z[j]);

                        if (quantity < counter)
                        {
                            decimal updatedvalue = counter - quantity;

                            Connection.iwdb.UpdateCryptoTable(z[j].Id, operationid, "SELL", cryptoId, name, date, updatedvalue, wallet, price, fee, sum);
                            CryptoTable obj = new CryptoTable();
                            obj.Id = z[j].Id;
                            obj.CryptoQuantity = updatedvalue;
                            obj.Sum = z[j].LastPrice * (decimal)updatedvalue;
                            obj.Fee = z[j].Fee;
                            updatedItem = obj;
                            ids.Remove(ids.Last());

                            break;
                        }

                        if (quantity == counter || (quantity > counter && j == z.Count - 1))
                        {
                            break;
                        }
                    }
                }
            }

            foreach (var item in ids)
            {
                Connection.iwdb.DeleteByID(item.Id);
            }

            if (operation == "BUY")
            {
                Connection.iwdb.DeleteByOperationID(operationid);
            }

            updatedItem = null;
            CoinQuantity cq = new CoinQuantity();
            decimal q = cq.GetCoinQuantityByCryptoId(cryptoId);
            InteractWithCurrentAssetsDB(cryptoId, name, customCoin, date, q, realPrice, cv);
            var coin = ReturnObject(name, customCoin, cryptoId, q, date, realPrice, cv);
            RefreshCurrentCoins(coin);
            list.RemoveAt(operationIndex);
            LoadOperationsDataGrid(ca, 10);

            form.CountCurrentValue();
            await Task.Delay(100);
        }

        private async Task DeletionNormal(CurrentAssets form, int rowIndex)
        {
            int operationIndex = rowIndex / 4;
            var operationid = operations[operationIndex].OperationID;
            var id = operations[operationIndex].Id;
            var cryptoId = operations[operationIndex].CryptoId;
            var name = operations[operationIndex].CryptoName;
            var date = operations[operationIndex].Date;
            var operation = operations[operationIndex].Operation;
            var quantity = operations[operationIndex].CryptoQuantity;
            var price = (decimal)Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoId == cryptoId).OrderByDescending(x => x.Id).First().LastPrice;
            var wallet = operations[operationIndex].Wallet;
            var fee = (decimal)operations[operationIndex].Fee;
            var sum = (decimal)operations[operationIndex].Sum;
            var customCoin = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.CryptoId == cryptoId).Select(x => x.CustomCoin).ToList().Last();
            decimal oldPrice = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.CryptoId == cryptoId).Select(x => x.Price).First(); 
            var testPrice = GetPrices.ById(cryptoId);
            decimal realPrice = testPrice > 0 ? testPrice : oldPrice;
            var cv = quantity * realPrice;

            List<CryptoTable> ids = new List<CryptoTable>();
            ids.Add(operations[operationIndex]);

            if (operation == "BUY")
            {
                decimal counter = 0;

                var z = Connection.db.GetTable<CryptoTable>()
                        .Where(x => x.Operation == "SELL" && x.Id > id && x.CryptoId == cryptoId && x.Wallet == wallet).ToList();

                if (z.Count > 0)
                {
                    for (int j = 0; j < z.Count; j++)
                    {
                        counter += z[j].CryptoQuantity;
                        ids.Add(z[j]);

                        if (quantity < counter)
                        {
                            decimal updatedvalue = counter - quantity;

                            Connection.iwdb.UpdateCryptoTable(z[j].Id, operationid, "SELL", cryptoId, name, date, updatedvalue, wallet, realPrice, fee, sum);
                            CryptoTable obj = new CryptoTable();
                            obj.Id = z[j].Id;
                            obj.CryptoQuantity = updatedvalue;
                            obj.Sum = realPrice * updatedvalue;
                            cv = realPrice * updatedvalue;
                            obj.Fee = z[j].Fee;
                            updatedItem = obj;
                            ids.Remove(ids.Last());

                            break;
                        }

                        if (quantity == counter || (quantity > counter && j == z.Count - 1))
                        {
                            break;
                        }
                    }
                }
            }

            foreach (var item in ids)
            {
                Connection.iwdb.DeleteByID(item.Id);
            }

            if (operation == "BUY")
            {
                Connection.iwdb.DeleteByOperationID(operationid); //istrinamos visos wallet related operacijos
            }

            var totalCv = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.CryptoId == cryptoId).Select(x => x.CurrentValue).First();
            var newTotalCv = totalCv - cv;

            updatedItem = null;
            CoinQuantity cq = new CoinQuantity();
            decimal q = cq.GetCoinQuantityByCryptoId(cryptoId);
            InteractWithCurrentAssetsDB(cryptoId, name, customCoin, date, q, realPrice, newTotalCv);
            var coin = ReturnObject(name, customCoin, cryptoId, q, date, realPrice, newTotalCv);
            RefreshCurrentCoins(coin);
            LoadOperationsForm();

            form.CountCurrentValue();
            await Task.Delay(100);
        }

        private void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(ca.alertPanel, ca.alertLabel, 645, -38, 276, 38);
            apc.StartPanelAnimation(chooseLabelText);
        }

        public void ExecuteDelete(CurrentAssets form, int rowIndex)
        {
            form.optionsPanel.Visible = false;

            string message = "";
            int operationIndex = rowIndex / 4;

            List<CryptoTable> z = new List<CryptoTable>();
            string oper = "";
            if (!filter)
            {
                z = Connection.db.GetTable<CryptoTable>()
                       .Where(x => x.Operation == "SELL" && x.Id > operations[operationIndex].Id && x.Wallet == operations[operationIndex].Wallet && x.CryptoName == operations[operationIndex].CryptoName).ToList();
                oper = operations[operationIndex].Operation;
            }
            else
            {
                z = Connection.db.GetTable<CryptoTable>()
                       .Where(x => x.Operation == "SELL" && x.Id > list[operationIndex].Id && x.Wallet == list[operationIndex].Wallet && x.CryptoName == list[operationIndex].CryptoName).ToList();
                oper = list[operationIndex].Operation;
            }

            if (z.Count > 0 &&  oper == "BUY")
            {
                message = "Ar tikrai norite ištrinti šią operaciją?\nIštrynus šią operaciją, bus ištintos arba pakoreguotos aukščiau esančios,\nsusijusios pardavimo operacijos.";
            }
            else
            {
                message = "Ar tikrai norite ištrinti šią operaciją?";
            }
            
            string title = "Operacijos ištrynimas";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    ca.operationDataGrid.Visible = false;
                    Task task = filter ? Task.Run(() => DeletionFilter(form, rowIndex)) : Task.Run(() => DeletionNormal(form, rowIndex));
                    task.Wait();
                    ca.operationDataGrid.Visible = true;
                    wf.RefreshDataGrid();
                    dgf.UpdateDataGrid(form);
                }
                catch
                {
                    AlertPanelControlInstance(12);
                }
                finally
                {
                    foreach (Control item in ca.Controls)
                    {
                        item.Enabled = true;
                    }

                    ca.CountCurrentValue();
                }
            }
        }

        public void HideOptionsPanel(CurrentAssets form)
        {
            if (form.optionsPanel.Visible)
            {
                form.optionsPanel.Visible = false;
            }
        }

        public void HideGridSelection(CurrentAssets form)
        {
            if (form.operationDataGrid.Visible)
            {
                form.operationDataGrid.ClearSelection();
            }
        }

    }
}
