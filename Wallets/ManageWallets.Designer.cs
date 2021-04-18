namespace cryptoFinance
{
    partial class ManageWallets
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
            this.backButton = new System.Windows.Forms.Button();
            this.walletDataGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transferButton = new System.Windows.Forms.Button();
            this.manageWalletsWorker = new System.ComponentModel.BackgroundWorker();
            this.loadingBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.walletDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(12, 262);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(60, 23);
            this.backButton.TabIndex = 2;
            this.backButton.Text = "Atgal";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // walletDataGrid
            // 
            this.walletDataGrid.AllowUserToAddRows = false;
            this.walletDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.walletDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column2});
            this.walletDataGrid.Location = new System.Drawing.Point(12, 12);
            this.walletDataGrid.Name = "walletDataGrid";
            this.walletDataGrid.ReadOnly = true;
            this.walletDataGrid.RowHeadersVisible = false;
            this.walletDataGrid.Size = new System.Drawing.Size(367, 244);
            this.walletDataGrid.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Kriptovaliuta";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 130;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Piniginė";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 130;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Kiekis";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // transferButton
            // 
            this.transferButton.Location = new System.Drawing.Point(143, 262);
            this.transferButton.Name = "transferButton";
            this.transferButton.Size = new System.Drawing.Size(116, 23);
            this.transferButton.TabIndex = 4;
            this.transferButton.Text = "Perkelti valiutas";
            this.transferButton.UseVisualStyleBackColor = true;
            this.transferButton.Click += new System.EventHandler(this.TransferButton_Click);
            // 
            // manageWalletsWorker
            // 
            this.manageWalletsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ManageWalletsWorker_DoWork);
            this.manageWalletsWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ManageWalletsWorker_RunWorkerCompleted);
            // 
            // loadingBox
            // 
            this.loadingBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadingBox.Image = global::cryptoFinance.Properties.Resources.loading_screen;
            this.loadingBox.Location = new System.Drawing.Point(165, 124);
            this.loadingBox.Name = "loadingBox";
            this.loadingBox.Size = new System.Drawing.Size(63, 49);
            this.loadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingBox.TabIndex = 28;
            this.loadingBox.TabStop = false;
            this.loadingBox.Visible = false;
            // 
            // ManageWallets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 297);
            this.Controls.Add(this.loadingBox);
            this.Controls.Add(this.transferButton);
            this.Controls.Add(this.walletDataGrid);
            this.Controls.Add(this.backButton);
            this.Name = "ManageWallets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ManageWallets";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageWallets_FormClosing);
            this.Load += new System.EventHandler(this.ManageWallets_Load);
            ((System.ComponentModel.ISupportInitialize)(this.walletDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.DataGridView walletDataGrid;
        private System.Windows.Forms.Button transferButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.ComponentModel.BackgroundWorker manageWalletsWorker;
        private System.Windows.Forms.PictureBox loadingBox;
    }
}