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

            UpdateDataGrid(ca);
            UpdatePricesAndCurrentValue(ca);

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
            for (int i = 0; i < ca.dataGridCurrentAssets.Rows.Count; i++)
            {
                for (int j = 0; j < ca.dataGridCurrentAssets.Columns.Count; j++)
                {
                    ca.dataGridCurrentAssets.Rows[i].Cells[j].Style.BackColor = Colours.formBackground;
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
                    Font font9 = new Font("Arial Black", 9);
                    e.Graphics.DrawString("Kriptovaliutos", font9, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
                else if (repaintCells == 1)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.selectedItem);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font9 = new Font("Arial Black", 9);
                    e.Graphics.DrawString("Kriptovaliutos", font9, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }

                if (finishRepainting)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font9 = new Font("Arial Black", 9);
                    e.Graphics.DrawString("Kriptovaliutos", font9, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
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
                    Font font9 = new Font("Arial Black", 9);
                    e.Graphics.DrawString("Grynoji vertė", font9, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 105, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
                else if (repaintCells == 4)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.selectedItem);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font9 = new Font("Arial Black", 9);
                    e.Graphics.DrawString("Grynoji vertė", font9, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 105, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }

                if (finishRepainting)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font9 = new Font("Arial Black", 9);
                    e.Graphics.DrawString("Grynoji vertė", font9, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.sortingIcon, e.CellBounds.X + 105, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }
            }

            //kodas sutvarko spalvu persiliejima
            /*using (Brush gridBrush = new SolidBrush(ca.dataGridCurrentAssets.GridColor))
            {
                using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // Clear cell 
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        //Bottom line drawing
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);

                        // here you force paint of content
                        e.PaintContent(e.ClipBounds);
                        e.Handled = true;
                    }
                }
            }*/
        }

        private void DataGridCurrentAssets_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 && (e.ColumnIndex == 1 || e.ColumnIndex == 4))
            {
                if (ascendingSorting)
                {
                    ca.dataGridCurrentAssets.Columns[e.ColumnIndex].HeaderCell.ToolTipText = "Rūšiuoti A-Z";
                }
                else
                {
                    ca.dataGridCurrentAssets.Columns[e.ColumnIndex].HeaderCell.ToolTipText = "Rūšiuoti Z-A";
                }

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

        public bool cannotUpdatePrices()
        {
            if(cannotUpdatePriceList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                coinList.RemoveAll(x => x.name == coinObject.name);
            }
            else
            {
                coinList.Where(x => x.name == coinObject.name).ToList().ForEach(x => x.quantity = coinObject.quantity);
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

        private string ReturnCustomName(CurrentAssetsDB item)
        {
            var nameSplit = item.Cryptocurrency.Split('(');
            string id = Connection.db.GetTable<CoingeckoCryptoList>()
                                .Where(x => x.CryptoName == nameSplit[0] && x.CryptoSymbol == nameSplit[1].Trim(')'))
                                .Select(x => x.CryptoId).ToList().First();
            return id;
        }

        private List<ConstructingLists> CreateCoinList()
        {
            List<ConstructingLists> coinList = new List<ConstructingLists>();
            decimal minimum = 0.00000001M;
            var coins = Connection.db.GetTable<CurrentAssetsDB>().Where(x => x.Quantity > minimum).ToList();

            foreach (var item in coins)
            {
                string id = "custom";
                if (!item.CustomCoin)
                {
                    id = ReturnCustomName(item);
                }

                var obj = ReturnObject(DateTime.Now, item.Cryptocurrency, item.CustomCoin, id, item.Quantity, 
                    item.Price, item.CurrentValue);
                coinList.Add(obj);
            }

            return coinList;
        }

        private ConstructingLists ReturnObject(DateTime date, string name, bool customCoin, string id, 
            decimal quantity, decimal price, decimal currentValue)
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
 
        private DataTable ReturnDataTable(List<ConstructingLists> coinList)
        {
            var data = coinList.Where(x => x.quantity > 0).OrderByDescending(x => x.totalSum).ToList();

            DataTable table = new DataTable();
            table.Columns.Add(" ", typeof(Image));
            
            table.Columns.Add("Kriptovaliuta", typeof(string));
            table.Columns.Add("Kiekis", typeof(string));
            table.Columns.Add("Kaina", typeof(string));
            table.Columns.Add("Grynoji vertė", typeof(decimal));

            foreach (var item in data)
            {
                DataRow row = table.NewRow();

                var namesplit = item.name.Split('(');
                var image = Connection.iwdb.GetLogo(namesplit[0], namesplit[1].Trim(')'));
                row[" "] = ResizeImage(image, 20, 20);

                row["Kriptovaliuta"] = item.name;

                string quantityValue = item.quantity.ToString("N8").TrimEnd('0');
                char[] qchars = quantityValue.ToCharArray();
                if (qchars.Last() == ',')
                {
                    quantityValue = quantityValue.Trim(',');
                }
                row["Kiekis"] = quantityValue;

                string priceValue = item.price.ToString("N8").TrimEnd('0');
                char[] pchars = priceValue.ToCharArray();
                if (pchars.Last() == ',')
                {
                    priceValue = priceValue.Trim(',');
                }

                row["Kaina"] = priceValue + " €";
                row["Grynoji vertė"] = item.totalSum;
                table.Rows.Add(row);
            }

            return table;
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
            bool tableVisible = false;
            if (form.dataGridCurrentAssets.Visible)
            {
                tableVisible = true;
            }

            form.dataGridCurrentAssets.Visible = false;
            GetCultureInfo gci = new GetCultureInfo(",");

            form.dataGridCurrentAssets.Rows.Clear();

            coinList = coinList.OrderByDescending(x => x.totalSum).ToList();

            for (int i = 0; i < coinList.Count; i++)
            {
                form.dataGridCurrentAssets.Rows.Add();

                var namesplit = coinList[i].name.Split('(');
                var image = Connection.iwdb.GetLogo(namesplit[0].Trim(' '), namesplit[1].Trim(')'));

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
            foreach (var item in coinList)
            {                
                decimal currentValue = 0;
                decimal price = 0;
                DateTime today = DateTime.Now;

                if (!item.customCoin)
                {
                    errorUpdatingPrices = 0;
                    var result = GetPrices.ById(item.id);

                    switch (result)
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
                            price = result;
                            break;
                    }
                }
                else
                {
                    price = Connection.db.GetTable<CurrentAssetsDB>()
                        .Where(x => x.Cryptocurrency == item.name).Select(x => x.Price).ToList().First();
                }

                if (errorUpdatingPrices == 0)
                {
                    currentValue = price * item.quantity;
                    temporaryList.Add(ReturnObject(today, item.name, item.customCoin, item.id, item.quantity, price, currentValue));
                }
                else
                {
                    cannotUpdatePriceList.Add(item.name);
                    price = Connection.db.GetTable<CurrentAssetsDB>()
                        .Where(x => x.Cryptocurrency == item.name).Select(x => x.Price).ToList().First();
                    currentValue = price * item.quantity;
                    temporaryList.Add(ReturnObject(today, item.name, item.customCoin, item.id, item.quantity, price, currentValue));
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
                    bool customCoin = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == item.name).Select(x => x.CustomCoin).ToList().Last();
                    Connection.iwdb.UpdateCurrentAssets(false, item.name, item.quantity, DateTime.Now, item.price, item.totalSum);
                    Connection.iwdb.UpdatePrice(DateTime.Today, item.name, customCoin, 0, "UpdatePrice", 0, item.price, 0);
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
