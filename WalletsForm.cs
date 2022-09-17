using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class WalletsForm
    {
        private CurrentAssets ca { get; set; }

        private List<ConstructingLists> dataGridList = new List<ConstructingLists>();

        private List<ConstructingLists> filterList = new List<ConstructingLists>();

        private List<ConstructingLists> newDataGridList = new List<ConstructingLists>();

        private int lastViewItemIndex { get; set; }

        private bool errorConfirming = false;

        private int repaintCells { get; set; }
        private bool finishRepainting { get; set; }

        private int[] filters = new int[2];

        public WalletsForm(CurrentAssets _ca)
        {
            ca = _ca;
            
            ca.transferButton.Click += new System.EventHandler(this.TransferButton_Click);
            ca.walletDataGrid.SelectionChanged += new System.EventHandler(this.WalletDataGrid_SelectionChanged);
            ca.backToWallets.Click += new System.EventHandler(this.BackToWallets_Click);
            ca.walletDataGrid.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.WalletDataGrid_CellMouseEnter);
            ca.walletDataGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.WalletDataGrid_ColumnHeaderMouseClick);
            ca.walletDataGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(WalletDataGrid_CellPainting);
            ca.walletDataGrid.MouseLeave += new System.EventHandler(WalletDataGrid_MouseLeave);
            ca.Click += new System.EventHandler(this.CurrentAssets_Click);
            ca.filterBox.TextChanged += new System.EventHandler(FilterBox_TextChanged);
            ca.filterListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(FilterListView_ItemSelectionChanged);
            ca.applyFilterButton.Click += new System.EventHandler(ApplyFilterButton_Click);
            ca.removeFilterButton.Click += new System.EventHandler(RemoveFilterButton_Click);
            ca.walletsPanel.VisibleChanged += new System.EventHandler(WalletsPanel_VisibleChanged);
            ca.qLabel.MouseEnter += new System.EventHandler(MaxLabel_MouseEnter);
            ca.qLabel.MouseLeave += new System.EventHandler(MaxLabel_MouseLeave);

            ListViewSettings.Format(ca.walletInListView);
            ListViewSettings.Format(ca.filterListView);
            ListViewSettings.SetListViewSizes(ca.filterListView);

            RefreshDataGrid();
        }

        private void MaxLabel_MouseEnter(object sender, EventArgs e)
        {
            ca.qLabel.ForeColor = Colours.selectedItem;
        }

        private void MaxLabel_MouseLeave(object sender, EventArgs e)
        {
            ca.qLabel.ForeColor = Colours.labelColor;
        }

        private void WalletsPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!ca.walletsPanel.Visible)
            {
                ca.filterPanel.Visible = false;
            }
        }

        private void RemoveFilterButton_Click(object sender, EventArgs e)
        {
            ca.filterPanel.Visible = false;
            Array.Clear(filters, 0, 2);
            newDataGridList.Clear();
            newDataGridList.AddRange(dataGridList);
            RefreshDataGrid();
        }

        private void FilterDataGrid()
        {
            newDataGridList.Clear();

            if (ca.filterPanel.Location.X == 253)
            {
                for (int i = 0; i < ca.filterListView.CheckedItems.Count; i++)
                {
                    newDataGridList.AddRange(dataGridList.Where(x => x.name == ca.filterListView.CheckedItems[i].Text).ToList());
                }

                filters[0] = 1;
            }

            if (ca.filterPanel.Location.X == 453)
            {
                for (int i = 0; i < ca.filterListView.CheckedItems.Count; i++)
                {
                    newDataGridList.AddRange(dataGridList.Where(x => x.wallet == ca.filterListView.CheckedItems[i].Text).ToList());
                }

                filters[1] = 2;
            }
           
            GetCultureInfo gci = new GetCultureInfo(",");

            ca.walletDataGrid.Rows.Clear();

            for (int i = 0; i < newDataGridList.Count; i++)
            {
                ca.walletDataGrid.Rows.Add();

                var namesplit = newDataGridList[i].name.Split('(');
                var image = Connection.iwdb.GetLogo(namesplit[0], namesplit[1].Trim(')'));

                ca.walletDataGrid.Rows[i].Cells[0].Value = ResizeImage(image, 20, 20);
                ca.walletDataGrid.Rows[i].Cells[1].Value = newDataGridList[i].name;
                ca.walletDataGrid.Rows[i].Cells[2].Value = newDataGridList[i].wallet;

                string quantityValue = newDataGridList[i].quantity.ToString("N8").TrimEnd('0');
                char[] qchars = quantityValue.ToCharArray();
                if (qchars.Last() == ',')
                {
                    quantityValue = quantityValue.Trim(',');
                }
                ca.walletDataGrid.Rows[i].Cells[3].Value = quantityValue;
            }
        }

        private void ApplyFilterButton_Click(object sender, EventArgs e)
        {
            ca.filterPanel.Visible = false;

            if(ca.filterListView.CheckedItems.Count > 0)
            {
                FilterDataGrid();
                repaintCells = 0;
                ca.walletDataGrid.Refresh();
                ca.walletDataGrid.ClearSelection();
            }
            else
            {
                RefreshDataGrid();
                Array.Clear(filters, 0, filters.Length);
            }
        }

        private void FilterListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void FilterBox_TextChanged(object sender, EventArgs e)
        {
            filterList.Clear();
            ca.filterListView.Items.Clear();

            if (ca.filterPanel.Location.X == 253)
            {
                filterList = newDataGridList.Where(x => x.name.ToLower().Contains(ca.filterBox.Text.ToLower())).ToList();
                var templist = filterList.Select(x => x.name).Distinct().OrderBy(x => x).ToList();

                for (int i = 0; i < templist.Count; i++)
                {
                    ca.filterListView.Items.Add(templist[i]);
                }
            }
            
            if(ca.filterPanel.Location.X == 453)
            {
                filterList = newDataGridList.Where(x => x.wallet.ToLower().Contains(ca.filterBox.Text.ToLower())).ToList();
                var templist = filterList.Select(x => x.wallet).Distinct().OrderBy(x => x).ToList();

                for (int i = 0; i < templist.Count; i++)
                {
                    ca.filterListView.Items.Add(templist[i]);
                }
            }
        }

        private void WalletDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ca.filterBox.Text = "";

            if (e.ColumnIndex == 1)
            {
                ca.filterListView.Items.Clear();
                var coins = newDataGridList.Select(x => x.name).Distinct().OrderBy(x => x).ToList();
                for (int i = 0; i < coins.Count; i++)
                {
                    ca.filterListView.Items.Add(coins[i]);
                }

                if (ca.filterPanel.Visible && ca.filterPanel.Location.X == 453)
                {
                    ca.filterPanel.Visible = false;
                    ca.filterPanel.Location = new Point(253, 165);
                    ca.filterPanel.BringToFront();
                    ca.filterPanel.Visible = true;
                }
                else if (!ca.filterPanel.Visible)
                {
                    ca.filterPanel.Location = new Point(253, 165);
                    ca.filterPanel.BringToFront();
                    ca.filterPanel.Visible = true;
                }
                else
                {
                    ca.filterPanel.Visible = false;
                }
            }
            
            if(e.ColumnIndex == 2)
            {
                ca.filterListView.Items.Clear();
                var wallets = newDataGridList.Select(x => x.wallet).Distinct().OrderBy(x => x).ToList();
                for (int i = 0; i < wallets.Count; i++)
                {
                    ca.filterListView.Items.Add(wallets[i]);
                }

                if (ca.filterPanel.Visible && ca.filterPanel.Location.X == 253)
                {
                    ca.filterPanel.Visible = false;
                    ca.filterPanel.Location = new Point(453, 165);
                    ca.filterPanel.BringToFront();
                    ca.filterPanel.Visible = true;
                }
                else if (!ca.filterPanel.Visible)
                {
                    ca.filterPanel.Location = new Point(453, 165);
                    ca.filterPanel.BringToFront();
                    ca.filterPanel.Visible = true;
                }
                else
                {
                    ca.filterPanel.Visible = false;
                }
            }
        }

        private void WalletDataGrid_MouseLeave(object sender, EventArgs e)
        {
            finishRepainting = true;
            ca.walletDataGrid.Refresh();
        }

        private void WalletDataGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 && (e.ColumnIndex == 1 || e.ColumnIndex == 2))
            {
                ca.walletDataGrid.Columns[e.ColumnIndex].HeaderCell.ToolTipText = "Filtruoti";
                finishRepainting = false;
                repaintCells = e.ColumnIndex;    
                
                ca.walletDataGrid.Refresh();
            }
            else
            {
                repaintCells = 0;

                if (!finishRepainting)
                {
                    ca.walletDataGrid.Refresh();
                }

                finishRepainting = true;
            }
        }
        
        private void BackToWallets_Click(object sender, EventArgs e)
        {
            ca.backToWallets.Visible = false;
            ca.transferButton.Visible = true;
            ca.transferPanel.Visible = false;
            ca.walletDataGrid.Visible = true;
            RemoveEvents();

            Array.Clear(filters, 0, 2);
            repaintCells = 0;
        }

        public void WalletDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ca.filterPanel.Visible = false;
        }

        public List<ConstructingLists> ReturnDataList()
        {
            return dataGridList;
        }

        public void Open(CurrentAssets form)
        {
            if (!form.walletsPanel.Visible)
            {
                if (dataGridList.Count == 0)
                {
                    form.ShowAssetAlertLabel("Jūs neturite įsigiję kriptovaliutų. Atlikite savo pirmą investiciją.");
                }
                else
                {
                    form.backToWallets.Visible = false;
                    form.transferButton.Visible = true;
                    form.walletsPanel.Visible = true;
                    AddDataToDataGrid(form);
                    
                    newDataGridList.Clear();
                    newDataGridList.AddRange(dataGridList);

                    Array.Clear(filters, 0, filters.Length);

                    form.walletDataGrid.Visible = true;
                    form.walletDataGrid.ClearSelection();
                }
            }
            else
            {
                form.walletsPanel.Visible = false;
            }
        }

        private void FetchAllData()
        {
            dataGridList.Clear();
            CoinQuantity cq = new CoinQuantity();
            decimal minimum = 0.00000001M;
            var coinNames = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            foreach (var name in coinNames)
            {
                var distinctWallets = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.CryptoName == name).Select(x => x.Wallet).Distinct().ToList();

                foreach (var wallet in distinctWallets)
                {
                    var q = cq.GetCoinQuantityByWallet(name, wallet);

                    if (q >= minimum)
                    {

                        var operationID = Connection.db.GetTable<CryptoTable>()
                            .Where(x => x.CryptoName == name && x.Wallet == wallet)
                            .Select(x => x.OperationID).Distinct().ToList();

                        var customCoin = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == name).Select(x => x.CustomCoin).ToList().Last();

                        AddDataToDataGridList(operationID[0], name, customCoin, q, wallet);
                    }
                }
            }
        }

        private void AddDataToDataGridList(int operationID, string name, bool customCoin, decimal q, string wallet)
        {
            ConstructingLists info = new ConstructingLists(operationID, name, customCoin, q, wallet);
            dataGridList.Add(info);
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

        private void AddDataToDataGrid(CurrentAssets form)
        {
            GetCultureInfo gci = new GetCultureInfo(",");

            form.walletDataGrid.Rows.Clear();

            for (int i = 0; i < dataGridList.Count; i++)
            {
                form.walletDataGrid.Rows.Add();

                var namesplit = dataGridList[i].name.Split('(');
                var image = Connection.iwdb.GetLogo(namesplit[0], namesplit[1].Trim(')'));

                form.walletDataGrid.Rows[i].Cells[0].Value = ResizeImage(image, 20, 20);
                form.walletDataGrid.Rows[i].Cells[1].Value = dataGridList[i].name;
                form.walletDataGrid.Rows[i].Cells[2].Value = dataGridList[i].wallet;

                string quantityValue = dataGridList[i].quantity.ToString("N8").TrimEnd('0');
                char[] qchars = quantityValue.ToCharArray();
                if (qchars.Last() == ',')
                {
                    quantityValue = quantityValue.Trim(',');
                }
                form.walletDataGrid.Rows[i].Cells[3].Value = quantityValue;
            }
        }

        public void RefreshDataGrid()
        {
            ca.walletDataGrid.Rows.Clear();
            dataGridList.Clear();
            FetchAllData();
            newDataGridList.Clear();
            newDataGridList.AddRange(dataGridList);
            AddDataToDataGrid(ca);
            ca.walletDataGrid.ClearSelection();
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            ca.walletOutComboBox.TextChanged += new System.EventHandler(this.WalletOutComboBox_TextChanged);
            ca.qLabel.Click += new System.EventHandler(Maxq_Click);
            ca.execTransferButton.Click += new System.EventHandler(this.ExecTransferButton_Click);
            ca.walletInBox.TextChanged += new System.EventHandler(this.WalletInBox_TextChanged);
            ca.transferPanel.Click += new System.EventHandler(this.TransferPanel_Click);
            ca.nameComboBox.SelectedIndexChanged += new System.EventHandler(this.NameComboBox_SelectedIndexChanged);
            ca.walletOutComboBox.SelectedIndexChanged += new System.EventHandler(this.WalletOutComboBox_SelectedIndexChanged);
            ca.walletInListView.SelectedIndexChanged += new System.EventHandler(this.WalletInListView_SelectedIndexChanged);
            ca.walletInBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WalletInBox_KeyPress);
            ca.walletInListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WalletInListView_MouseMove);
            ca.transferPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransferPanel_MouseMove);
            ca.nameComboBox.Leave += new System.EventHandler(this.NameComboBox_Leave);
            ca.walletOutComboBox.Leave += new System.EventHandler(this.WalletOutComboBox_Leave);
            ca.walletInBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletInBox_KeyDown);
            ca.walletInListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletInListView_KeyDown);
            ca.nameComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameComboBox_KeyDown);
            ca.walletOutComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletOutComboBox_KeyDown);
            ca.walletsPanel.Click += new System.EventHandler(this.WalletsPanel_Click);
            
            ca.quantityTransferBox.TextChanged += new System.EventHandler(this.QuantityTransferBox_TextChanged);
            ca.quantityTransferBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuantityTransferBox_KeyPress);
            ca.quantityTransferBox.Leave += new System.EventHandler(this.QuantityTransferBox_Leave);
            ca.feeTransferBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FeeTransferBox_KeyPress);
            ca.feeTransferBox.Leave += new System.EventHandler(this.FeeTransferBox_Leave);

            foreach (Control item in ca.transferPanel.Controls)
            {
                item.Enabled = true;
            }

            ca.nameComboBox.Text = "";
            ca.walletOutComboBox.Text = "";
            ca.walletInBox.Text = "";
            ca.quantityTransferBox.Text = "";
            ca.feeTransferBox.Text = 0.ToString("C2");

            ca.walletInListView.Items.Clear();
            ca.walletOutComboBox.Items.Clear();
            ca.nameComboBox.Items.Clear();
            FillNameComboBox();
            ca.walletDataGrid.Visible = false;
            ca.transferPanel.Visible = true;
            ca.backToWallets.Visible = true;
            ca.transferButton.Visible = false;
            ca.nameComboBox.Select();
        }

        private void WalletDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

                    if(filters[0] == 1)
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIconHighlighted, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    }

                    if(filters[0] == 0)
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    }
                   
                    e.Handled = true;
                }
                else if (repaintCells == 1)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.selectedItem);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Kriptovaliutos", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }

                if (finishRepainting)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Kriptovaliutos", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);

                    if (filters[0] == 1)
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIconHighlighted, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    }
                    else
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIcon, e.CellBounds.X + 275, e.CellBounds.Y + 2, 20, 20);
                    }

                    e.Handled = true;
                }
            }

            if (e.ColumnIndex == 2 && e.RowIndex == -1)
            {
                if (repaintCells != 2)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Piniginės", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);

                    if(filters[1] == 2)
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIconHighlighted, e.CellBounds.X + 165, e.CellBounds.Y + 2, 20, 20);
                    }
                    else
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIcon, e.CellBounds.X + 165, e.CellBounds.Y + 2, 20, 20);
                    }
                    
                    e.Handled = true;
                }
                else if (repaintCells == 2)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.selectedItem);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Piniginės", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);
                    e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIcon, e.CellBounds.X + 165, e.CellBounds.Y + 2, 20, 20);
                    e.Handled = true;
                }

                if (finishRepainting)
                {
                    System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(Colours.labelColor);
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                    System.Drawing.SolidBrush brush2 = new System.Drawing.SolidBrush(Colours.alternateCellBack);
                    Font font11 = new Font("Arial Black", 11);
                    e.Graphics.DrawString("Piniginės", font11, brush2, e.CellBounds.X + 6, e.CellBounds.Y + 4);

                    if (filters[1] == 2)
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIconHighlighted, e.CellBounds.X + 165, e.CellBounds.Y + 2, 20, 20);
                    }
                    else
                    {
                        e.Graphics.DrawImage(cryptoFinance.Properties.Resources.filterIcon, e.CellBounds.X + 165, e.CellBounds.Y + 2, 20, 20);
                    }

                    e.Handled = true;
                }
            }     
        }

        private void CurrentAssets_Click(object sender, EventArgs e)
        {
            ca.walletInListView.Visible = false;
            ca.filterPanel.Visible = false;
        }

        private void WalletsPanel_Click(object sender, EventArgs e)
        {
            ca.walletInListView.Visible = false;
        }

        private void RemoveEvents()
        {
            ca.walletDataGrid.SelectionChanged -= new System.EventHandler(this.WalletDataGrid_SelectionChanged);
            ca.walletOutComboBox.TextChanged -= new System.EventHandler(this.WalletOutComboBox_TextChanged);
            ca.qLabel.Click -= new System.EventHandler(Maxq_Click);
            ca.execTransferButton.Click -= new System.EventHandler(this.ExecTransferButton_Click);
            ca.walletInBox.TextChanged -= new System.EventHandler(this.WalletInBox_TextChanged);
            ca.transferPanel.Click -= new System.EventHandler(this.TransferPanel_Click);
            ca.nameComboBox.SelectedIndexChanged -= new System.EventHandler(this.NameComboBox_SelectedIndexChanged);
            ca.walletOutComboBox.SelectedIndexChanged -= new System.EventHandler(this.WalletOutComboBox_SelectedIndexChanged);
            ca.walletInListView.SelectedIndexChanged -= new System.EventHandler(this.WalletInListView_SelectedIndexChanged);
            ca.walletInBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.WalletInBox_KeyPress);
            ca.walletInListView.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.WalletInListView_MouseMove);
            ca.transferPanel.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.TransferPanel_MouseMove);
            ca.nameComboBox.Leave -= new System.EventHandler(this.NameComboBox_Leave);
            ca.walletOutComboBox.Leave -= new System.EventHandler(this.WalletOutComboBox_Leave);
            ca.walletInBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.WalletInBox_KeyDown);
            ca.walletInListView.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.WalletInListView_KeyDown);
            ca.nameComboBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.NameComboBox_KeyDown);
            ca.walletOutComboBox.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.WalletOutComboBox_KeyDown);

            ca.quantityTransferBox.TextChanged -= new System.EventHandler(this.QuantityTransferBox_TextChanged);
            ca.quantityTransferBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.QuantityTransferBox_KeyPress);
            ca.quantityTransferBox.Leave -= new System.EventHandler(this.QuantityTransferBox_Leave);
            ca.feeTransferBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.FeeTransferBox_KeyPress);
            ca.feeTransferBox.Leave -= new System.EventHandler(this.FeeTransferBox_Leave);
        }

        private bool Validation()
        {
            bool validation = false;

            if (ca.maxqPanel.Visible)
            {
                validation = FormValidation.ReturnValidation(ca.transferPanel.Controls, ca.qLabel.Text, ca.quantityTransferBox.Text);

                if (ca.walletOutComboBox.Text == ca.walletInBox.Text)
                {
                    validation = false;
                }
            }

            return validation;
        }

        private void FillNameComboBox()
        {
            CoinQuantity cq = new CoinQuantity();
            decimal minimum = 0.00000001M;
            var coinNames = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            foreach (var name in coinNames)
            {
                var q = cq.GetCoinQuantityByName(name);
                if (q >= minimum)
                {
                    ca.nameComboBox.Items.Add(name);
                }
            }
        }

        private void ExecuteIfWalletIsFound()
        {
            CoinQuantity cq = new CoinQuantity();
            var wallets = Connection.db.GetTable<CryptoTable>()
               .Where(x => x.CryptoName == ca.nameComboBox.Text).Select(x => x.Wallet).Distinct().ToList();
            ca.walletOutComboBox.Items.Clear();

            foreach (var wallet in wallets)
            {
                var q = cq.GetCoinQuantityByWallet(ca.nameComboBox.Text, wallet);
                decimal minimum = 0.00000001M;

                if (q > minimum)
                {
                    ca.walletOutComboBox.Items.Add(wallet);
                }
            }
        }

        private void ExecuteIfWalletIsNotFound()
        {
            ca.walletOutComboBox.Items.Clear();
            ca.quantityTransferBox.Text = "";
            ca.walletOutComboBox.Text = "";
            ca.maxqPanel.Visible = false;
        }

        private void WalletOutComboBox_TextChanged(object sender, EventArgs e)
        {
            var quantity = dataGridList
                .Where(x => x.name == ca.nameComboBox.Text && x.wallet == ca.walletOutComboBox.Text)
                .Select(x => x.quantity).ToList();

            if (quantity.Count > 0)
            {
                decimal maxQ = quantity[0];
                string quantityValue = maxQ.ToString("N8").TrimEnd('0');
                char[] qchars = quantityValue.ToCharArray();
                if (qchars.Last() == ',')
                {
                    quantityValue = quantityValue.Trim(',');
                }

                ca.qLabel.Text = quantityValue;
                ca.maxqPanel.Visible = true;
            }

            if (ca.walletOutComboBox.Text == "")
            {
                ca.maxqPanel.Visible = false;
                ca.quantityTransferBox.Text = "";
            }
        }

        private void Maxq_Click(object sender, EventArgs e)
        {
            ca.quantityTransferBox.Text = ca.qLabel.Text;
        }

        private void InsertWalletOutInfo(int operationID, string coinName, bool customCoin, decimal quantity, string operation, string walletOut, decimal fee)
        {
            Connection.iwdb.InsertCryptoTable(operationID, DateTime.Now, coinName, customCoin, quantity, operation, walletOut, 0, 0, fee, 0);
        }

        private void InsertWalletInInfo(int operationID, string coinName, bool customCoin, decimal quantity, string operation, string walletIn)
        {
            Connection.iwdb.InsertCryptoTable(operationID, DateTime.Now, coinName, customCoin, quantity, operation, walletIn, 0, 0, 0, 0);
        }

        private void AlertPanelControlInstance(int chooseLabelText)
        {
            AlertPanelControl apc = new AlertPanelControl(ca.alertPanel, ca.alertLabel, 645, -38, 276, 38);
            apc.StartPanelAnimation(chooseLabelText);
        }

        private async void TransferExecution()
        {
            ca.execTransferButton.Enabled = false;
            DisableControls();

            try
            {                
                await Task.Run(() => UpdateDatabase());
            }
            catch
            {
                errorConfirming = true;
                ca.transferPanel.Visible = false;
                ca.backToWallets.Visible = false;
                ca.walletDataGrid.Visible = true;
            }

            RemoveEvents();
            RefreshDataGrid();

            ca.transferPanel.Visible = false;
            ca.backToWallets.Visible = false;
            ca.transferButton.Visible = true;
            ca.walletDataGrid.Visible = true;
        }

        private void ExecuteAlertPanel()
        {
            if (!errorConfirming)
            {
                AlertPanelControlInstance(23);
            }
            else if (errorConfirming)
            {
                AlertPanelControlInstance(7);
            }
        }

        private async void ExecTransferButton_Click(object sender, EventArgs e)
        {
            if (!ca.alertPanel.Visible)
            {
                if (!Validation() && !ca.alertPanel.Visible)
                {
                    AlertPanelControlInstance(3);
                }
                else
                {
                    ca.ShowLoading();
                    foreach (Control item in ca.Controls)
                    {
                        if (item.Name != "loadingBox")
                        {
                            item.Enabled = false;
                        }
                    }
                    await Task.Run(() => TransferExecution());
                    foreach (Control item in ca.Controls)
                    {
                        item.Enabled = true;
                    }
                    ExecuteAlertPanel();
                    ca.HideLoading();
                }
            }  
        }

        private void DisableControls()
        {
            foreach (Control ctrl in ca.transferPanel.Controls)
            {
                ctrl.Enabled = false;
            }
        }

        private void UpdateDatabase()
        {
            int operationID = dataGridList.Where(x => x.name == ca.nameComboBox.Text && x.wallet == ca.walletOutComboBox.Text).Select(x => x.operationID).First();

            var customCoin = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == ca.nameComboBox.Text).Select(x => x.CustomCoin).ToList().Last();
            InsertWalletOutInfo(operationID, ca.nameComboBox.Text, customCoin, decimal.Parse(ca.quantityTransferBox.Text), "TRANSFER_OUT", ca.walletOutComboBox.Text, ReformatText.ReturnNumeric(ca.feeTransferBox.Text));
            InsertWalletInInfo(operationID, ca.nameComboBox.Text, customCoin, decimal.Parse(ca.quantityTransferBox.Text), "TRANSFER_IN", ca.walletInBox.Text);
        }

        private void ToggleWalletAlert()
        {
            if (ca.walletInBox.Text == ca.walletOutComboBox.Text)
            {
                ca.walletInBox.ForeColor = Color.Red;
                ca.walletInListView.Visible = false;
            }
            else
            {
                ca.walletInBox.ForeColor = Color.Black;

                if (ca.walletInListView.Items.Count > 0)
                {
                    ListViewSettings.SetListViewSizes(ca.walletInListView);
                    ca.walletInListView.Visible = true;
                    ca.walletInListView.BringToFront();
                }
                else
                {
                    ca.walletInListView.Visible = false;
                }
            }
        }

        private void WalletInBox_TextChanged(object sender, EventArgs e)
        {
            if (ca.walletInBox.Text != "")
            {
                ca.walletInBox.ForeColor = Color.Black;

                ca.walletInListView.Items.Clear();
                var list = Connection.iwdb.FetchWallets(ca.walletInBox.Text)
                    .OrderBy(x => x).Distinct().ToList();
                foreach (var item in list)
                {
                    ca.walletInListView.Items.Add(item);
                }

                ToggleWalletAlert();
            }
            else
            {
                ca.walletInListView.Visible = false;
            }
        }

        private void QuantityTransferBox_TextChanged(object sender, EventArgs e)
        {
            if (ca.quantityTransferBox.Text != "" && ca.maxqPanel.Visible)
            {
                if (decimal.Parse(ca.quantityTransferBox.Text) > decimal.Parse(ca.qLabel.Text))
                {
                    ca.quantityTransferBox.ForeColor = Color.Red;
                }
                else
                {
                    ca.quantityTransferBox.ForeColor = Color.Black;
                }
            }
            else
            {
                ca.quantityTransferBox.ForeColor = Color.Black;
            }
        }

        private void TransferPanel_Click(object sender, EventArgs e)
        {
            ca.walletInListView.Visible = false;
            ca.cryptoLabel.Select();
        }

        private void NameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ca.walletOutComboBox.Text = "";
            ca.quantityTransferBox.Text = "";
            ca.maxqPanel.Visible = false;
            ca.walletOutComboBox.Select();           
        }

        private void WalletOutComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ca.walletInBox.Select();
        }

        private void WalletInListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ca.walletInListView.SelectedItems.Count >= 0)
            {
                ca.walletInBox.Text = ca.walletInListView.SelectedItems[0].Text;
                ca.walletInListView.Visible = false;
                ca.quantityTransferBox.Select();
            }
        }

        private void WalletInBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (ca.walletInListView.Visible == true)
                {
                    ca.walletInListView.Visible = false;
                }

                ca.quantityTransferBox.Select();
            }
        }

        private void WalletInListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseMoves(ca.walletInListView, lastViewItemIndex, e);
            lastViewItemIndex = ListViewSettings.ReturnLastIndex();
        }

        private void TransferPanel_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewSettings.WhenMouseIsOutOfBounds(ca.walletInListView);
        }

        private void NameComboBox_Leave(object sender, EventArgs e)
        {
            new System.Threading.Timer((s) =>
            {
                ca.nameComboBox.Invoke(new Action(() =>
                {
                    ca.nameComboBox.Select(0, 0);
                }));
            }, null, 10, System.Threading.Timeout.Infinite);

            bool nameIsFound = false;
            var search = ca.nameComboBox.FindStringExact(ca.nameComboBox.Text);

            if (search >= 0)
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

        private void WalletOutComboBox_Leave(object sender, EventArgs e)
        {
            new System.Threading.Timer((s) =>
            {
                ca.nameComboBox.Invoke(new Action(() =>
                {
                    ca.walletOutComboBox.Select(0, 0);
                }));
            }, null, 10, System.Threading.Timeout.Infinite);
        }

        private void WalletInBox_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewSettings.WhenDownButtonPressed(ca.walletInListView, e);
            ListViewSettings.WhenUpButtonPressed(ca.walletInListView, e);
            ListViewSettings.WhenEnterButtonPressed(ca.walletInListView, e, ca.walletInBox, ca.transferToLabel);
        }

        private void WalletInListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (ca.walletInListView.Focused)
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

        private void QuantityTransferBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.QuantityBox(ca.quantityTransferBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                ca.feeTransferBox.Select();
            }
        }

        private void QuantityTransferBox_Leave(object sender, EventArgs e)
        {
            GetCultureInfo gci = new GetCultureInfo(",");
            DecimalBoxRules.FormatQuantityBox(ca.quantityTransferBox);
        }

        private void FeeTransferBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            DecimalBoxRules.CurrencyBox(ca.feeTransferBox, e);

            if (e.KeyChar == (char)Keys.Return)
            {
                ca.execTransferButton.Select();
            }
        }

        private void FeeTransferBox_Leave(object sender, EventArgs e)
        {
            DecimalBoxRules.FormatCurrencyBox(ca.feeTransferBox);
        }

    }
}
