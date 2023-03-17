using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace cryptoFinance
{
    public class DataGridForm
    {
        private List<ConstructingLists> temporaryList = new List<ConstructingLists>();
        private int errorUpdatingPrices { get; set; }
        private List<ConstructingLists> coinList = new List<ConstructingLists>();
        private List<string> cannotUpdatePriceList = new List<string>();
        private System.Timers.Timer datagridTimer { get; set; }
        private bool ascendingSorting = true;

        private CurrentAssets ca { get; set; }
        public static Point sorting1 = new Point(350, 107);
        public static Point sorting2 = new Point(820, 107);
        private int repaintCells { get; set; }
        private bool finishRepainting { get; set; }

        public DataGridForm(CurrentAssets _ca)
        {
            ca = _ca;
            ca.dataGridCurrentAssets.VisibleChanged += new System.EventHandler(DataGridCurrentAssets_VisibleChanged);
            ca.dataGridCurrentAssets.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(DataGridCurrentAssets_ColumnHeaderMouseClick);
            ca.dataGridCurrentAssets.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(DataGridCurrentAssets_CellMouseEnter);
            ca.dataGridCurrentAssets.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(DataGridCurrentAssets_CellPainting);
            ca.dataGridCurrentAssets.MouseLeave += new System.EventHandler(DataGridCurrentAssets_MouseLeave);

            datagridTimer = new System.Timers.Timer();
            datagridTimer.Elapsed += new ElapsedEventHandler(DatagridTimer_Tick);
            datagridTimer.Interval = 100;
            coinList = CreateCoinList();

            UpdatePricesAndCurrentValue(ca);
            UpdateDataGrid(ca);
            
            if (coinList.Count == 0)
            {
                ca.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
            }
            else
            {
                ca.dataGridCurrentAssets.Visible = true;
            }
        }

        private void DataGridCurrentAssets_MouseLeave(object sender, EventArgs e)
        {
            finishRepainting = true;
            ca.dataGridCurrentAssets.Refresh();
        }

        private void ClearSelection()
        {
            foreach (DataGridViewRow row in ca.dataGridCurrentAssets.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Colours.formBackground;
                }
            }
        }

        private void DataGridCurrentAssets_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex == -1)
            {
                if (repaintCells != 1)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Kriptovaliutos", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
                else if (repaintCells == 1)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.selectedItem);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Kriptovaliutos", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }

                if (finishRepainting)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Kriptovaliutos", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
            }

            if (e.ColumnIndex == 4 && e.RowIndex == -1)
            {
                if (repaintCells != 4)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Grynoji vertė", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 117, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
                else if (repaintCells == 4)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.selectedItem);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Grynoji vertė", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 117, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }

                if (finishRepainting)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Grynoji vertė", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 117, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
            }
        }

        private void DataGridCurrentAssets_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 && (e.ColumnIndex == 1 || e.ColumnIndex == 4))
            {
                ca.dataGridCurrentAssets.Columns[e.ColumnIndex].HeaderCell.ToolTipText = ascendingSorting ? "Rūšiuoti A-Z" : "Rūšiuoti Z-A";
                finishRepainting = false;
                repaintCells = e.ColumnIndex;
                ca.dataGridCurrentAssets.Refresh();
            }
            else
            {
                repaintCells = 0;

                if (!finishRepainting)
                {
                    ca.dataGridCurrentAssets.Refresh();
                }

                finishRepainting = true;
            }           
        }

        private void DatagridTimer_Tick(object source, EventArgs e)
        {
            foreach(DataGridViewColumn col in ca.dataGridCurrentAssets.Columns)
            {
                col.HeaderCell.Style.BackColor = Colours.labelColor;
            }
            
            datagridTimer.Stop();
        }

        private void DataGridCurrentAssets_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == 1 || e.ColumnIndex == 4))
            {
                if (ascendingSorting)
                {
                    ca.dataGridCurrentAssets.Sort(ca.dataGridCurrentAssets.Columns[e.ColumnIndex], ListSortDirection.Ascending);
                    ascendingSorting = false;
                }
                else
                {
                    ca.dataGridCurrentAssets.Sort(ca.dataGridCurrentAssets.Columns[e.ColumnIndex], ListSortDirection.Descending);
                    ascendingSorting = true;
                }
            }

            ca.dataGridCurrentAssets.ClearSelection();
            ClearSelection();
        }

        public List<ConstructingLists> ReturnCoinList()
        {
            return coinList;
        }

        public void AddObjectToCoinList(ConstructingLists coinObject)
        {
            coinList.Add(coinObject);
        }

        public void UpdateDataInCoinList(ConstructingLists coinObject)
        {
            if(coinObject.quantity == 0)
            {
                coinList.RemoveAll(x => x.cryptoId == coinObject.cryptoId);
            }
            else
            {
                coinList.Where(x => x.cryptoId == coinObject.cryptoId).ToList().ForEach(x => x.quantity = coinObject.quantity);
                coinList.Where(x => x.cryptoId == coinObject.cryptoId).ToList().ForEach(x => x.price = coinObject.price);
                coinList.Where(x => x.cryptoId == coinObject.cryptoId).ToList().ForEach(x => x.totalSum = coinObject.totalSum);
            }
        }

        private void DataGridCurrentAssets_VisibleChanged(object sender, EventArgs e)
        {
            DataGridVisibleChange(ca);
        }

        public void Open(CurrentAssets form)
        {
            if (!form.dataGridCurrentAssets.Visible)
            {
                if(ca.dataGridCurrentAssets.Rows.Count == 0)
                {
                    form.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");                   
                }
                else
                {
                    form.dataGridCurrentAssets.Visible = true;
                }
            }
            else
            {
                form.dataGridCurrentAssets.Visible = false;
            }

            form.dataGridCurrentAssets.ClearSelection();
            ClearSelection();
        }

        private List<ConstructingLists> CreateCoinList()
        {
            List<ConstructingLists> coinList = new List<ConstructingLists>();
            decimal minimum = 0.00000001M;
            var coins = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.Quantity > minimum).ToList();

            foreach (var item in coins)
            {
                var obj = ReturnObject(DateTime.Now, item.Cryptocurrency, item.CustomCoin, item.CryptoId, item.Quantity, 
                    item.Price, item.CurrentValue);
                coinList.Add(obj);
            }

            return coinList;
        }

        private ConstructingLists ReturnObject(DateTime date, string name, bool customCoin, string cryptoId, 
            decimal quantity, decimal price, decimal currentValue)
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

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            if(image != null)
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

        public void UpdateDataGrid(CurrentAssets form)
        {

            bool tableVisible = form.dataGridCurrentAssets.Visible ? true : false;
            form.dataGridCurrentAssets.Visible = false;
            GetCultureInfo gci = new GetCultureInfo(",");
            form.dataGridCurrentAssets.Rows.Clear();

            coinList = coinList.OrderByDescending(x => x.totalSum).ToList();

            for (int i = 0; i < coinList.Count; i++)
            {
                form.dataGridCurrentAssets.Rows.Add();

                var image = Connection.iwdb.GetLogo(new ConstructingLists(coinList[i].name, coinList[i].customCoin));
                form.dataGridCurrentAssets.Rows[i].Cells[0].Value = ResizeImage(image, 20, 20);
                form.dataGridCurrentAssets.Rows[i].Cells[1].Value = coinList[i].name;

                string quantityValue = coinList[i].quantity.ToString("N8").TrimEnd('0');
                char[] qchars = quantityValue.ToCharArray();
                if (qchars.Last() == ',')
                {
                    quantityValue = quantityValue.Trim(',');
                }
                form.dataGridCurrentAssets.Rows[i].Cells[2].Value = quantityValue;
                form.dataGridCurrentAssets.Rows[i].Cells[3].Value = coinList[i].price;
                form.dataGridCurrentAssets.Rows[i].Cells[4].Value = coinList[i].totalSum;
            }

            Design.CurrentAssetsDataGrid(ca.dataGridCurrentAssets, true);
            
            if (tableVisible)
            {
                form.dataGridCurrentAssets.Visible = true;
            }
        }

        public void UpdatePricesAndCurrentValue(CurrentAssets form)
        {
            var task = Task.Run(() => InsertPricesToDatabase(form));
            task.Wait();
        }

        private async Task FetchPrices()
        {
            cannotUpdatePriceList.Clear();
            decimal currentValue = 0;
            decimal price = 0;
            DateTime today = DateTime.Now;

            errorUpdatingPrices = 0;
            var updatedList = GetPrices.ById(coinList);

            for (int i = 0; i < updatedList.Count; i++)
            {
                switch (updatedList[i].price)
                {
                    case -1: //no connection
                        errorUpdatingPrices = -1;
                        break;
                    case -2: //error decoding data or cannot reach API
                        errorUpdatingPrices = -2;
                        break;
                    case -3: //takes too long to get price
                        errorUpdatingPrices = -3;
                        break;
                    default:
                        price = updatedList[i].price;
                        break;
                }


                if (errorUpdatingPrices == 0)
                {
                    currentValue = price * updatedList[i].quantity;
                    temporaryList.Add(ReturnObject(today, updatedList[i].name, updatedList[i].customCoin, updatedList[i].cryptoId, updatedList[i].quantity, price, currentValue));
                }
                else
                {
                    cannotUpdatePriceList.Add(updatedList[i].name);
                    price = Connection.db.GetTable<CurrentAssetsDB>()
                        .Where(x => x.Cryptocurrency == updatedList[i].name).Select(x => x.Price).ToList().First();
                    currentValue = price * updatedList[i].quantity;
                    temporaryList.Add(ReturnObject(today, updatedList[i].name, updatedList[i].customCoin, updatedList[i].cryptoId, updatedList[i].quantity, price, currentValue));
                }
            }

            await Task.Delay(100);
        }

        private async Task InsertPricesToDatabase(CurrentAssets form)
        {
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

                foreach (var item in temporaryList)
                {
                    bool customCoin = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoId == item.cryptoId).Select(x => x.CustomCoin).ToList().Last();
                    Connection.iwdb.UpdateCurrentAssets(false, item.cryptoId, item.quantity, DateTime.Now, item.price, item.totalSum);
                    Connection.iwdb.UpdatePrice(DateTime.Today, item.cryptoId, item.name, customCoin, 0, "UpdatePrice", 0, item.price, 0);
                }
            }
            else
            {
                MessageBox.Show("Nepavyko pasiekti coingecko API per 1 minutę.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridVisibleChange(CurrentAssets form)
        {
            if (form.dataGridCurrentAssets.Visible)
            {
                form.dataGridCurrentAssets.ClearSelection();
                ClearSelection();
            }
        }

        public int ReturnError()
        {
            return errorUpdatingPrices;
        }

    }
}
