namespace cryptoFinance
{
    partial class AddOperation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.enterCryptoQuantityLabel = new System.Windows.Forms.Label();
            this.quantityBox = new System.Windows.Forms.TextBox();
            this.confirmButton = new System.Windows.Forms.Button();
            this.walletTextBox = new System.Windows.Forms.TextBox();
            this.enterWalletNameLabel = new System.Windows.Forms.Label();
            this.alertLabel = new System.Windows.Forms.Label();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.walletListView = new System.Windows.Forms.ListView();
            this.sumBox = new System.Windows.Forms.TextBox();
            this.sumLabel = new System.Windows.Forms.Label();
            this.priceLabel = new System.Windows.Forms.Label();
            this.priceBox = new System.Windows.Forms.TextBox();
            this.priceWorker = new System.ComponentModel.BackgroundWorker();
            this.refreshPrice = new System.Windows.Forms.RadioButton();
            this.dateLabel = new System.Windows.Forms.Label();
            this.feeBox = new System.Windows.Forms.TextBox();
            this.feeLabel = new System.Windows.Forms.Label();
            this.loadingBox = new System.Windows.Forms.PictureBox();
            this.dateBox = new System.Windows.Forms.DateTimePicker();
            this.buyButton = new System.Windows.Forms.Button();
            this.sellButton = new System.Windows.Forms.Button();
            this.cryptoBox = new System.Windows.Forms.TextBox();
            this.searchPicture = new System.Windows.Forms.PictureBox();
            this.suggestionsListView = new System.Windows.Forms.ListView();
            this.chooseCryptoLabel = new System.Windows.Forms.Label();
            this.top100ListView = new System.Windows.Forms.ListView();
            this.maxQuantityLabel = new System.Windows.Forms.Label();
            this.cryptoComboBox = new cryptoFinance.AdvancedComboBox();
            this.walletComboBox = new cryptoFinance.AdvancedComboBox();
            this.alertPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // enterCryptoQuantityLabel
            // 
            this.enterCryptoQuantityLabel.AutoSize = true;
            this.enterCryptoQuantityLabel.Location = new System.Drawing.Point(162, 198);
            this.enterCryptoQuantityLabel.Name = "enterCryptoQuantityLabel";
            this.enterCryptoQuantityLabel.Size = new System.Drawing.Size(35, 13);
            this.enterCryptoQuantityLabel.TabIndex = 16;
            this.enterCryptoQuantityLabel.Text = "Kiekis";
            // 
            // quantityBox
            // 
            this.quantityBox.Location = new System.Drawing.Point(107, 213);
            this.quantityBox.MaxLength = 30;
            this.quantityBox.Name = "quantityBox";
            this.quantityBox.Size = new System.Drawing.Size(147, 20);
            this.quantityBox.TabIndex = 8;
            this.quantityBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.quantityBox.TextChanged += new System.EventHandler(this.QuantityBox_TextChanged);
            this.quantityBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuantityBox_KeyPress);
            this.quantityBox.Leave += new System.EventHandler(this.QuantityBox_Leave);
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(132, 374);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(100, 33);
            this.confirmButton.TabIndex = 13;
            this.confirmButton.Text = "Tvirtinti operaciją";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // walletTextBox
            // 
            this.walletTextBox.Location = new System.Drawing.Point(107, 174);
            this.walletTextBox.MaxLength = 30;
            this.walletTextBox.Name = "walletTextBox";
            this.walletTextBox.Size = new System.Drawing.Size(146, 20);
            this.walletTextBox.TabIndex = 6;
            this.walletTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.walletTextBox.TextChanged += new System.EventHandler(this.WalletTextBox_TextChanged);
            this.walletTextBox.Enter += new System.EventHandler(this.WalletTextBox_Enter);
            this.walletTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletTextBox_KeyDown);
            // 
            // enterWalletNameLabel
            // 
            this.enterWalletNameLabel.AutoSize = true;
            this.enterWalletNameLabel.Location = new System.Drawing.Point(158, 157);
            this.enterWalletNameLabel.Name = "enterWalletNameLabel";
            this.enterWalletNameLabel.Size = new System.Drawing.Size(44, 13);
            this.enterWalletNameLabel.TabIndex = 15;
            this.enterWalletNameLabel.Text = "Piniginė";
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Location = new System.Drawing.Point(4, 4);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(29, 13);
            this.alertLabel.TabIndex = 7;
            this.alertLabel.Text = "label";
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.LightCoral;
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Location = new System.Drawing.Point(185, 3);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Size = new System.Drawing.Size(198, 38);
            this.alertPanel.TabIndex = 15;
            this.alertPanel.Visible = false;
            this.alertPanel.VisibleChanged += new System.EventHandler(this.AlertPanel_VisibleChanged);
            // 
            // walletListView
            // 
            this.walletListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.walletListView.HideSelection = false;
            this.walletListView.Location = new System.Drawing.Point(107, 191);
            this.walletListView.MultiSelect = false;
            this.walletListView.Name = "walletListView";
            this.walletListView.Size = new System.Drawing.Size(147, 81);
            this.walletListView.TabIndex = 24;
            this.walletListView.UseCompatibleStateImageBehavior = false;
            this.walletListView.View = System.Windows.Forms.View.Details;
            this.walletListView.Visible = false;
            this.walletListView.SelectedIndexChanged += new System.EventHandler(this.WalletListView_SelectedIndexChanged);
            this.walletListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletListView_KeyDown);
            this.walletListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.WalletListView_MouseClick);
            this.walletListView.MouseEnter += new System.EventHandler(this.WalletListView_MouseEnter);
            this.walletListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WalletListView_MouseMove);
            // 
            // sumBox
            // 
            this.sumBox.Location = new System.Drawing.Point(107, 330);
            this.sumBox.MaxLength = 30;
            this.sumBox.Name = "sumBox";
            this.sumBox.Size = new System.Drawing.Size(147, 20);
            this.sumBox.TabIndex = 12;
            this.sumBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sumBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SumBox_KeyPress);
            this.sumBox.Leave += new System.EventHandler(this.SumBox_Leave);
            // 
            // sumLabel
            // 
            this.sumLabel.AutoSize = true;
            this.sumLabel.Location = new System.Drawing.Point(134, 314);
            this.sumLabel.Name = "sumLabel";
            this.sumLabel.Size = new System.Drawing.Size(98, 13);
            this.sumLabel.TabIndex = 20;
            this.sumLabel.Text = "Investuojama suma";
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Location = new System.Drawing.Point(162, 236);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(34, 13);
            this.priceLabel.TabIndex = 18;
            this.priceLabel.Text = "Kaina";
            // 
            // priceBox
            // 
            this.priceBox.Location = new System.Drawing.Point(107, 252);
            this.priceBox.MaxLength = 30;
            this.priceBox.Name = "priceBox";
            this.priceBox.Size = new System.Drawing.Size(147, 20);
            this.priceBox.TabIndex = 9;
            this.priceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.priceBox.TextChanged += new System.EventHandler(this.PriceBox_TextChanged);
            this.priceBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PriceBox_KeyPress);
            this.priceBox.Leave += new System.EventHandler(this.PriceBox_Leave);
            // 
            // priceWorker
            // 
            this.priceWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PriceWorker_DoWork);
            this.priceWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PriceWorker_RunWorkerCompleted);
            // 
            // refreshPrice
            // 
            this.refreshPrice.AutoSize = true;
            this.refreshPrice.Location = new System.Drawing.Point(260, 255);
            this.refreshPrice.Name = "refreshPrice";
            this.refreshPrice.Size = new System.Drawing.Size(14, 13);
            this.refreshPrice.TabIndex = 10;
            this.refreshPrice.TabStop = true;
            this.refreshPrice.UseVisualStyleBackColor = true;
            this.refreshPrice.Click += new System.EventHandler(this.RefreshPrice_Click);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(162, 118);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 15;
            this.dateLabel.Text = "Data";
            // 
            // feeBox
            // 
            this.feeBox.Location = new System.Drawing.Point(107, 291);
            this.feeBox.MaxLength = 30;
            this.feeBox.Name = "feeBox";
            this.feeBox.Size = new System.Drawing.Size(147, 20);
            this.feeBox.TabIndex = 11;
            this.feeBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.feeBox.TextChanged += new System.EventHandler(this.FeeBox_TextChanged);
            this.feeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FeeBox_KeyPress);
            this.feeBox.Leave += new System.EventHandler(this.FeeBox_Leave);
            // 
            // feeLabel
            // 
            this.feeLabel.AutoSize = true;
            this.feeLabel.Location = new System.Drawing.Point(153, 275);
            this.feeLabel.Name = "feeLabel";
            this.feeLabel.Size = new System.Drawing.Size(55, 13);
            this.feeLabel.TabIndex = 19;
            this.feeLabel.Text = "Mokesčiai";
            // 
            // loadingBox
            // 
            this.loadingBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadingBox.Image = global::cryptoFinance.Properties.Resources.loading_screen;
            this.loadingBox.Location = new System.Drawing.Point(156, 162);
            this.loadingBox.Name = "loadingBox";
            this.loadingBox.Size = new System.Drawing.Size(63, 49);
            this.loadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingBox.TabIndex = 28;
            this.loadingBox.TabStop = false;
            this.loadingBox.Visible = false;
            // 
            // dateBox
            // 
            this.dateBox.CustomFormat = "yyyy-MM-dd";
            this.dateBox.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateBox.Location = new System.Drawing.Point(107, 134);
            this.dateBox.Name = "dateBox";
            this.dateBox.Size = new System.Drawing.Size(147, 20);
            this.dateBox.TabIndex = 5;
            // 
            // buyButton
            // 
            this.buyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buyButton.Location = new System.Drawing.Point(105, 12);
            this.buyButton.Name = "buyButton";
            this.buyButton.Size = new System.Drawing.Size(69, 39);
            this.buyButton.TabIndex = 1;
            this.buyButton.Text = "Pirkti";
            this.buyButton.UseVisualStyleBackColor = true;
            this.buyButton.Click += new System.EventHandler(this.BuyButton_Click);
            // 
            // sellButton
            // 
            this.sellButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sellButton.Location = new System.Drawing.Point(185, 12);
            this.sellButton.Name = "sellButton";
            this.sellButton.Size = new System.Drawing.Size(69, 39);
            this.sellButton.TabIndex = 2;
            this.sellButton.Text = "Parduoti";
            this.sellButton.UseVisualStyleBackColor = true;
            this.sellButton.Click += new System.EventHandler(this.SellButton_Click);
            // 
            // cryptoBox
            // 
            this.cryptoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.cryptoBox.ForeColor = System.Drawing.Color.Silver;
            this.cryptoBox.Location = new System.Drawing.Point(86, 83);
            this.cryptoBox.MaxLength = 30;
            this.cryptoBox.Name = "cryptoBox";
            this.cryptoBox.Size = new System.Drawing.Size(187, 20);
            this.cryptoBox.TabIndex = 3;
            this.cryptoBox.Text = "paieška";
            this.cryptoBox.Click += new System.EventHandler(this.CryptoBox_Click);
            this.cryptoBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CryptoBox_MouseClick);
            this.cryptoBox.TextChanged += new System.EventHandler(this.CryptoBox_TextChanged);
            this.cryptoBox.Enter += new System.EventHandler(this.CryptoBox_Enter);
            this.cryptoBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CryptoBox_KeyDown);
            this.cryptoBox.Leave += new System.EventHandler(this.CryptoBox_Leave);
            // 
            // searchPicture
            // 
            this.searchPicture.BackColor = System.Drawing.SystemColors.Window;
            this.searchPicture.BackgroundImage = global::cryptoFinance.Properties.Resources.search_icon;
            this.searchPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.searchPicture.Location = new System.Drawing.Point(273, 84);
            this.searchPicture.Name = "searchPicture";
            this.searchPicture.Size = new System.Drawing.Size(21, 17);
            this.searchPicture.TabIndex = 33;
            this.searchPicture.TabStop = false;
            // 
            // suggestionsListView
            // 
            this.suggestionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.suggestionsListView.HideSelection = false;
            this.suggestionsListView.Location = new System.Drawing.Point(86, 99);
            this.suggestionsListView.MultiSelect = false;
            this.suggestionsListView.Name = "suggestionsListView";
            this.suggestionsListView.Size = new System.Drawing.Size(208, 108);
            this.suggestionsListView.TabIndex = 22;
            this.suggestionsListView.UseCompatibleStateImageBehavior = false;
            this.suggestionsListView.View = System.Windows.Forms.View.Details;
            this.suggestionsListView.Visible = false;
            this.suggestionsListView.SelectedIndexChanged += new System.EventHandler(this.SuggestionsListView_SelectedIndexChanged);
            this.suggestionsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SuggestionsListView_KeyDown);
            this.suggestionsListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SuggestionsListView_MouseClick);
            this.suggestionsListView.MouseEnter += new System.EventHandler(this.SuggestionsListView_MouseEnter);
            this.suggestionsListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SuggestionsListView_MouseMove);
            // 
            // chooseCryptoLabel
            // 
            this.chooseCryptoLabel.AutoSize = true;
            this.chooseCryptoLabel.Location = new System.Drawing.Point(121, 67);
            this.chooseCryptoLabel.Name = "chooseCryptoLabel";
            this.chooseCryptoLabel.Size = new System.Drawing.Size(115, 13);
            this.chooseCryptoLabel.TabIndex = 14;
            this.chooseCryptoLabel.Text = "Pasirinkite kriptovaliutą";
            // 
            // top100ListView
            // 
            this.top100ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.top100ListView.HideSelection = false;
            this.top100ListView.Location = new System.Drawing.Point(86, 99);
            this.top100ListView.MultiSelect = false;
            this.top100ListView.Name = "top100ListView";
            this.top100ListView.Size = new System.Drawing.Size(208, 108);
            this.top100ListView.TabIndex = 23;
            this.top100ListView.UseCompatibleStateImageBehavior = false;
            this.top100ListView.View = System.Windows.Forms.View.Details;
            this.top100ListView.Visible = false;
            this.top100ListView.SelectedIndexChanged += new System.EventHandler(this.Top100ListView_SelectedIndexChanged);
            this.top100ListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Top100ListView_KeyDown);
            this.top100ListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Top100ListView_MouseClick);
            this.top100ListView.MouseEnter += new System.EventHandler(this.Top100ListView_MouseEnter);
            this.top100ListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Top100ListView_MouseMove);
            // 
            // maxQuantityLabel
            // 
            this.maxQuantityLabel.AutoSize = true;
            this.maxQuantityLabel.Location = new System.Drawing.Point(257, 216);
            this.maxQuantityLabel.Name = "maxQuantityLabel";
            this.maxQuantityLabel.Size = new System.Drawing.Size(56, 13);
            this.maxQuantityLabel.TabIndex = 17;
            this.maxQuantityLabel.Text = "max kiekis";
            this.maxQuantityLabel.Visible = false;
            this.maxQuantityLabel.Click += new System.EventHandler(this.MaxQuantityLabel_Click);
            // 
            // cryptoComboBox
            // 
            this.cryptoComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cryptoComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cryptoComboBox.FormattingEnabled = true;
            this.cryptoComboBox.HighlightColor = System.Drawing.Color.Aquamarine;
            this.cryptoComboBox.Location = new System.Drawing.Point(87, 83);
            this.cryptoComboBox.MaxDropDownItems = 10;
            this.cryptoComboBox.Name = "cryptoComboBox";
            this.cryptoComboBox.Size = new System.Drawing.Size(207, 21);
            this.cryptoComboBox.TabIndex = 4;
            this.cryptoComboBox.Visible = false;
            this.cryptoComboBox.DropDownClosed += new System.EventHandler(this.CryptoComboBox_DropDownClosed);
            this.cryptoComboBox.Leave += new System.EventHandler(this.CryptoComboBox_Leave);
            // 
            // walletComboBox
            // 
            this.walletComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.walletComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.walletComboBox.FormattingEnabled = true;
            this.walletComboBox.HighlightColor = System.Drawing.Color.Aquamarine;
            this.walletComboBox.Location = new System.Drawing.Point(107, 174);
            this.walletComboBox.Name = "walletComboBox";
            this.walletComboBox.Size = new System.Drawing.Size(146, 21);
            this.walletComboBox.TabIndex = 7;
            this.walletComboBox.Visible = false;
            this.walletComboBox.DropDownClosed += new System.EventHandler(this.WalletComboBox_DropDownClosed);
            this.walletComboBox.TextChanged += new System.EventHandler(this.WalletComboBox_TextChanged);
            // 
            // AddOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 419);
            this.Controls.Add(this.loadingBox);
            this.Controls.Add(this.top100ListView);
            this.Controls.Add(this.suggestionsListView);
            this.Controls.Add(this.walletListView);
            this.Controls.Add(this.chooseCryptoLabel);
            this.Controls.Add(this.searchPicture);
            this.Controls.Add(this.cryptoBox);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.sellButton);
            this.Controls.Add(this.buyButton);
            this.Controls.Add(this.dateBox);
            this.Controls.Add(this.feeLabel);
            this.Controls.Add(this.feeBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.refreshPrice);
            this.Controls.Add(this.priceBox);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.sumLabel);
            this.Controls.Add(this.sumBox);
            this.Controls.Add(this.enterWalletNameLabel);
            this.Controls.Add(this.walletTextBox);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.enterCryptoQuantityLabel);
            this.Controls.Add(this.cryptoComboBox);
            this.Controls.Add(this.quantityBox);
            this.Controls.Add(this.walletComboBox);
            this.Controls.Add(this.maxQuantityLabel);
            this.Name = "AddOperation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddOperation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddQuantityForm_FormClosing);
            this.Load += new System.EventHandler(this.AddQuantityForm_Load);
            this.Click += new System.EventHandler(this.AddQuantityForm_Click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AddQuantityForm_MouseMove);
            this.alertPanel.ResumeLayout(false);
            this.alertPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label enterCryptoQuantityLabel;
        private System.Windows.Forms.TextBox quantityBox;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.TextBox walletTextBox;
        private System.Windows.Forms.Label enterWalletNameLabel;
        private System.Windows.Forms.Label alertLabel;
        private System.Windows.Forms.Panel alertPanel;
        private System.Windows.Forms.ListView walletListView;
        private System.Windows.Forms.TextBox sumBox;
        private System.Windows.Forms.Label sumLabel;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.TextBox priceBox;
        private System.ComponentModel.BackgroundWorker priceWorker;
        private System.Windows.Forms.RadioButton refreshPrice;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.TextBox feeBox;
        private System.Windows.Forms.Label feeLabel;
        private System.Windows.Forms.PictureBox loadingBox;
        private System.Windows.Forms.DateTimePicker dateBox;
        private System.Windows.Forms.Button buyButton;
        private System.Windows.Forms.Button sellButton;
        private System.Windows.Forms.TextBox cryptoBox;
        private System.Windows.Forms.PictureBox searchPicture;
        private System.Windows.Forms.ListView suggestionsListView;
        private AdvancedComboBox cryptoComboBox;
        private System.Windows.Forms.Label chooseCryptoLabel;
        private System.Windows.Forms.ListView top100ListView;
        private AdvancedComboBox walletComboBox;
        private System.Windows.Forms.Label maxQuantityLabel;
    }
}