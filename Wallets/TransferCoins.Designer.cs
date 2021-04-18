namespace cryptoFinance
{
    partial class TransferCoins
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
            this.cryptoLabel = new System.Windows.Forms.Label();
            this.walletLabel = new System.Windows.Forms.Label();
            this.maxLabel = new System.Windows.Forms.Label();
            this.quantityBox = new System.Windows.Forms.TextBox();
            this.quantityLabel = new System.Windows.Forms.Label();
            this.transferToLabel = new System.Windows.Forms.Label();
            this.transferButton = new System.Windows.Forms.Button();
            this.maxAlert = new System.Windows.Forms.Label();
            this.feeBox = new System.Windows.Forms.TextBox();
            this.feesLabel = new System.Windows.Forms.Label();
            this.walletInBox = new System.Windows.Forms.TextBox();
            this.walletInAlert = new System.Windows.Forms.Label();
            this.alertLabel = new System.Windows.Forms.Label();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.walletInListView = new System.Windows.Forms.ListView();
            this.walletOutComboBox = new cryptoFinance.AdvancedComboBox();
            this.nameComboBox = new cryptoFinance.AdvancedComboBox();
            this.loadingBox = new System.Windows.Forms.PictureBox();
            this.alertPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cryptoLabel
            // 
            this.cryptoLabel.AutoSize = true;
            this.cryptoLabel.Location = new System.Drawing.Point(86, 55);
            this.cryptoLabel.Name = "cryptoLabel";
            this.cryptoLabel.Size = new System.Drawing.Size(65, 13);
            this.cryptoLabel.TabIndex = 7;
            this.cryptoLabel.Text = "Kriptovaliuta";
            // 
            // walletLabel
            // 
            this.walletLabel.AutoSize = true;
            this.walletLabel.Location = new System.Drawing.Point(86, 94);
            this.walletLabel.Name = "walletLabel";
            this.walletLabel.Size = new System.Drawing.Size(44, 13);
            this.walletLabel.TabIndex = 8;
            this.walletLabel.Text = "Piniginė";
            // 
            // maxLabel
            // 
            this.maxLabel.AutoSize = true;
            this.maxLabel.Location = new System.Drawing.Point(178, 153);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(29, 13);
            this.maxLabel.TabIndex = 10;
            this.maxLabel.Text = "max.";
            this.maxLabel.Visible = false;
            this.maxLabel.Click += new System.EventHandler(this.MaxLabel_Click);
            // 
            // quantityBox
            // 
            this.quantityBox.Location = new System.Drawing.Point(89, 150);
            this.quantityBox.Name = "quantityBox";
            this.quantityBox.Size = new System.Drawing.Size(83, 20);
            this.quantityBox.TabIndex = 3;
            this.quantityBox.TextChanged += new System.EventHandler(this.QuantityBox_TextChanged);
            this.quantityBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuantityBox_KeyPress);
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(86, 134);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(35, 13);
            this.quantityLabel.TabIndex = 9;
            this.quantityLabel.Text = "Kiekis";
            // 
            // transferToLabel
            // 
            this.transferToLabel.AutoSize = true;
            this.transferToLabel.Location = new System.Drawing.Point(86, 235);
            this.transferToLabel.Name = "transferToLabel";
            this.transferToLabel.Size = new System.Drawing.Size(47, 13);
            this.transferToLabel.TabIndex = 12;
            this.transferToLabel.Text = "Perkelti į";
            // 
            // transferButton
            // 
            this.transferButton.Location = new System.Drawing.Point(115, 365);
            this.transferButton.Name = "transferButton";
            this.transferButton.Size = new System.Drawing.Size(103, 28);
            this.transferButton.TabIndex = 6;
            this.transferButton.Text = "Perkelti valiutą";
            this.transferButton.UseVisualStyleBackColor = true;
            this.transferButton.Click += new System.EventHandler(this.TransferButton_Click);
            // 
            // maxAlert
            // 
            this.maxAlert.AutoSize = true;
            this.maxAlert.Location = new System.Drawing.Point(86, 173);
            this.maxAlert.Name = "maxAlert";
            this.maxAlert.Size = new System.Drawing.Size(85, 13);
            this.maxAlert.TabIndex = 13;
            this.maxAlert.Text = "Per didelis kiekis";
            this.maxAlert.Visible = false;
            // 
            // feeBox
            // 
            this.feeBox.Location = new System.Drawing.Point(89, 211);
            this.feeBox.Name = "feeBox";
            this.feeBox.Size = new System.Drawing.Size(83, 20);
            this.feeBox.TabIndex = 4;
            this.feeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FeeBox_KeyPress);
            // 
            // feesLabel
            // 
            this.feesLabel.AutoSize = true;
            this.feesLabel.Location = new System.Drawing.Point(86, 195);
            this.feesLabel.Name = "feesLabel";
            this.feesLabel.Size = new System.Drawing.Size(55, 13);
            this.feesLabel.TabIndex = 11;
            this.feesLabel.Text = "Mokesčiai";
            // 
            // walletInBox
            // 
            this.walletInBox.Location = new System.Drawing.Point(89, 251);
            this.walletInBox.Name = "walletInBox";
            this.walletInBox.Size = new System.Drawing.Size(166, 20);
            this.walletInBox.TabIndex = 5;
            this.walletInBox.TextChanged += new System.EventHandler(this.WalletInBox_TextChanged);
            this.walletInBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletInBox_KeyDown);
            this.walletInBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WalletInBox_KeyPress);
            // 
            // walletInAlert
            // 
            this.walletInAlert.AutoSize = true;
            this.walletInAlert.Location = new System.Drawing.Point(112, 274);
            this.walletInAlert.Name = "walletInAlert";
            this.walletInAlert.Size = new System.Drawing.Size(114, 13);
            this.walletInAlert.TabIndex = 17;
            this.walletInAlert.Text = "Pasirinkite kitą piniginę";
            this.walletInAlert.Visible = false;
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Location = new System.Drawing.Point(4, 4);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(29, 13);
            this.alertLabel.TabIndex = 18;
            this.alertLabel.Text = "label";
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.LightCoral;
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Location = new System.Drawing.Point(115, 3);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Size = new System.Drawing.Size(217, 38);
            this.alertPanel.TabIndex = 19;
            this.alertPanel.Visible = false;
            this.alertPanel.VisibleChanged += new System.EventHandler(this.AlertPanel_VisibleChanged);
            // 
            // walletInListView
            // 
            this.walletInListView.FullRowSelect = true;
            this.walletInListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.walletInListView.HideSelection = false;
            this.walletInListView.Location = new System.Drawing.Point(93, 267);
            this.walletInListView.MultiSelect = false;
            this.walletInListView.Name = "walletInListView";
            this.walletInListView.Size = new System.Drawing.Size(166, 95);
            this.walletInListView.TabIndex = 16;
            this.walletInListView.UseCompatibleStateImageBehavior = false;
            this.walletInListView.View = System.Windows.Forms.View.Details;
            this.walletInListView.Visible = false;
            this.walletInListView.SelectedIndexChanged += new System.EventHandler(this.WalletInListView_SelectedIndexChanged);
            this.walletInListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletInListView_KeyDown);
            this.walletInListView.MouseEnter += new System.EventHandler(this.WalletInListView_MouseEnter);
            this.walletInListView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WalletInListView_MouseMove);
            // 
            // walletOutComboBox
            // 
            this.walletOutComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.walletOutComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.walletOutComboBox.FormattingEnabled = true;
            this.walletOutComboBox.HighlightColor = System.Drawing.Color.Aquamarine;
            this.walletOutComboBox.ItemHeight = 15;
            this.walletOutComboBox.Location = new System.Drawing.Point(89, 110);
            this.walletOutComboBox.MaxDropDownItems = 5;
            this.walletOutComboBox.Name = "walletOutComboBox";
            this.walletOutComboBox.Size = new System.Drawing.Size(166, 21);
            this.walletOutComboBox.TabIndex = 2;
            this.walletOutComboBox.SelectedIndexChanged += new System.EventHandler(this.WalletOutComboBox_SelectedIndexChanged);
            this.walletOutComboBox.TextChanged += new System.EventHandler(this.WalletOutComboBox_TextChanged);
            this.walletOutComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WalletOutComboBox_KeyDown);
            this.walletOutComboBox.Leave += new System.EventHandler(this.WalletOutComboBox_Leave);
            // 
            // nameComboBox
            // 
            this.nameComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.nameComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.nameComboBox.DropDownWidth = 150;
            this.nameComboBox.FormattingEnabled = true;
            this.nameComboBox.HighlightColor = System.Drawing.Color.Aquamarine;
            this.nameComboBox.ItemHeight = 15;
            this.nameComboBox.Location = new System.Drawing.Point(89, 71);
            this.nameComboBox.MaxDropDownItems = 5;
            this.nameComboBox.Name = "nameComboBox";
            this.nameComboBox.Size = new System.Drawing.Size(166, 21);
            this.nameComboBox.TabIndex = 1;
            this.nameComboBox.SelectedIndexChanged += new System.EventHandler(this.NameComboBox_SelectedIndexChanged);
            this.nameComboBox.TextChanged += new System.EventHandler(this.NameComboBox_TextChanged);
            this.nameComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NameComboBox_KeyDown);
            this.nameComboBox.Leave += new System.EventHandler(this.NameComboBox_Leave);
            // 
            // loadingBox
            // 
            this.loadingBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadingBox.Image = global::cryptoFinance.Properties.Resources.loading_screen;
            this.loadingBox.Location = new System.Drawing.Point(136, 178);
            this.loadingBox.Name = "loadingBox";
            this.loadingBox.Size = new System.Drawing.Size(63, 49);
            this.loadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingBox.TabIndex = 20;
            this.loadingBox.TabStop = false;
            this.loadingBox.Visible = false;
            // 
            // TransferCoins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 405);
            this.Controls.Add(this.loadingBox);
            this.Controls.Add(this.walletInListView);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.walletInAlert);
            this.Controls.Add(this.walletInBox);
            this.Controls.Add(this.feesLabel);
            this.Controls.Add(this.feeBox);
            this.Controls.Add(this.maxAlert);
            this.Controls.Add(this.transferButton);
            this.Controls.Add(this.transferToLabel);
            this.Controls.Add(this.quantityLabel);
            this.Controls.Add(this.quantityBox);
            this.Controls.Add(this.maxLabel);
            this.Controls.Add(this.walletOutComboBox);
            this.Controls.Add(this.walletLabel);
            this.Controls.Add(this.nameComboBox);
            this.Controls.Add(this.cryptoLabel);
            this.Name = "TransferCoins";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransferCoins";
            this.Load += new System.EventHandler(this.TransferCoins_Load);
            this.Click += new System.EventHandler(this.TransferCoins_Click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TransferCoins_MouseMove);
            this.alertPanel.ResumeLayout(false);
            this.alertPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label cryptoLabel;
        private AdvancedComboBox nameComboBox; //System.Windows.Forms.ComboBox nameComboBox;
        private System.Windows.Forms.Label walletLabel;
        private AdvancedComboBox walletOutComboBox; //System.Windows.Forms.ComboBox walletOutComboBox;
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.TextBox quantityBox;
        private System.Windows.Forms.Label quantityLabel;
        private System.Windows.Forms.Label transferToLabel;
        private System.Windows.Forms.Button transferButton;
        private System.Windows.Forms.Label maxAlert;
        private System.Windows.Forms.TextBox feeBox;
        private System.Windows.Forms.Label feesLabel;
        private System.Windows.Forms.TextBox walletInBox;
        private System.Windows.Forms.Label walletInAlert;
        private System.Windows.Forms.Label alertLabel;
        private System.Windows.Forms.Panel alertPanel;
        private System.Windows.Forms.ListView walletInListView;
        private System.Windows.Forms.PictureBox loadingBox;
    }
}