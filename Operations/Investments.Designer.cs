namespace cryptoFinance
{
    partial class Investments
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.updateCryptoList = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.lastTimeUpdated = new System.Windows.Forms.Label();
            this.alertLabel = new System.Windows.Forms.Label();
            this.alertPanel = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressBarPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.progressLabel = new System.Windows.Forms.Label();
            this.investmentsTimer = new System.Windows.Forms.Timer(this.components);
            this.operationDataGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewImageColumn();
            this.loadMoreLabel = new System.Windows.Forms.Label();
            this.addOperationPicture = new System.Windows.Forms.PictureBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.deleteButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.loadingBox = new System.Windows.Forms.PictureBox();
            this.alertPanel.SuspendLayout();
            this.progressBarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addOperationPicture)).BeginInit();
            this.optionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // updateCryptoList
            // 
            this.updateCryptoList.Location = new System.Drawing.Point(4, 62);
            this.updateCryptoList.Name = "updateCryptoList";
            this.updateCryptoList.Size = new System.Drawing.Size(103, 27);
            this.updateCryptoList.TabIndex = 4;
            this.updateCryptoList.Text = "Atnaujinti sąrašą";
            this.updateCryptoList.UseVisualStyleBackColor = true;
            this.updateCryptoList.Visible = false;
            this.updateCryptoList.Click += new System.EventHandler(this.UpdateCryptoList_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(12, 381);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(57, 27);
            this.backButton.TabIndex = 7;
            this.backButton.Text = "Atgal";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.InvestmentsBackButton_Click);
            // 
            // lastTimeUpdated
            // 
            this.lastTimeUpdated.AutoSize = true;
            this.lastTimeUpdated.Location = new System.Drawing.Point(2, 46);
            this.lastTimeUpdated.Name = "lastTimeUpdated";
            this.lastTimeUpdated.Size = new System.Drawing.Size(28, 13);
            this.lastTimeUpdated.TabIndex = 20;
            this.lastTimeUpdated.Text = "date";
            this.lastTimeUpdated.Visible = false;
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Location = new System.Drawing.Point(4, 4);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(29, 13);
            this.alertLabel.TabIndex = 21;
            this.alertLabel.Text = "label";
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.LightCoral;
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Location = new System.Drawing.Point(152, 3);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Size = new System.Drawing.Size(217, 38);
            this.alertPanel.TabIndex = 22;
            this.alertPanel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(3, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(458, 11);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 24;
            // 
            // progressBarPanel
            // 
            this.progressBarPanel.Controls.Add(this.cancelButton);
            this.progressBarPanel.Controls.Add(this.progressLabel);
            this.progressBarPanel.Controls.Add(this.progressBar);
            this.progressBarPanel.Location = new System.Drawing.Point(1, 3);
            this.progressBarPanel.Name = "progressBarPanel";
            this.progressBarPanel.Size = new System.Drawing.Size(464, 40);
            this.progressBarPanel.TabIndex = 25;
            this.progressBarPanel.Visible = false;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(386, 17);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 20);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "Atšaukti";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.BackColor = System.Drawing.SystemColors.Control;
            this.progressLabel.Location = new System.Drawing.Point(1, 17);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(29, 13);
            this.progressLabel.TabIndex = 25;
            this.progressLabel.Text = "label";
            // 
            // investmentsTimer
            // 
            this.investmentsTimer.Interval = 1000;
            this.investmentsTimer.Tick += new System.EventHandler(this.InvestmentsTimer_Tick);
            // 
            // operationDataGrid
            // 
            this.operationDataGrid.AllowUserToAddRows = false;
            this.operationDataGrid.AllowUserToResizeColumns = false;
            this.operationDataGrid.AllowUserToResizeRows = false;
            this.operationDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.operationDataGrid.ColumnHeadersVisible = false;
            this.operationDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.operationDataGrid.Location = new System.Drawing.Point(4, 232);
            this.operationDataGrid.Name = "operationDataGrid";
            this.operationDataGrid.RowHeadersVisible = false;
            this.operationDataGrid.Size = new System.Drawing.Size(461, 127);
            this.operationDataGrid.TabIndex = 27;
            this.operationDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OperationDataGrid_CellContentClick);
            this.operationDataGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OperationDataGrid_CellMouseClick);
            this.operationDataGrid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OperationDataGrid_Scroll);
            this.operationDataGrid.Leave += new System.EventHandler(this.OperationDataGrid_Leave);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 70;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 165;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 70;
            // 
            // Column5
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "System.Drawing.Bitmap";
            this.Column5.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column5.HeaderText = "EditColumn";
            this.Column5.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.Width = 23;
            // 
            // loadMoreLabel
            // 
            this.loadMoreLabel.AutoSize = true;
            this.loadMoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.loadMoreLabel.Location = new System.Drawing.Point(190, 362);
            this.loadMoreLabel.Name = "loadMoreLabel";
            this.loadMoreLabel.Size = new System.Drawing.Size(87, 13);
            this.loadMoreLabel.TabIndex = 28;
            this.loadMoreLabel.Text = "Užkrauti daugiau";
            this.loadMoreLabel.Visible = false;
            this.loadMoreLabel.Click += new System.EventHandler(this.LoadMoreLabel_Click);
            this.loadMoreLabel.MouseEnter += new System.EventHandler(this.LoadMoreLabel_MouseEnter);
            this.loadMoreLabel.MouseLeave += new System.EventHandler(this.LoadMoreLabel_MouseLeave);
            // 
            // addOperationPicture
            // 
            this.addOperationPicture.BackgroundImage = global::cryptoFinance.Properties.Resources.add_operation_button;
            this.addOperationPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.addOperationPicture.Location = new System.Drawing.Point(4, 186);
            this.addOperationPicture.Name = "addOperationPicture";
            this.addOperationPicture.Size = new System.Drawing.Size(44, 40);
            this.addOperationPicture.TabIndex = 29;
            this.addOperationPicture.TabStop = false;
            this.addOperationPicture.Click += new System.EventHandler(this.AddOperationPicture_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "EditCell";
            this.dataGridViewImageColumn1.Image = global::cryptoFinance.Properties.Resources.edit_logo;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.Width = 15;
            // 
            // optionsPanel
            // 
            this.optionsPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.optionsPanel.Controls.Add(this.deleteButton);
            this.optionsPanel.Controls.Add(this.editButton);
            this.optionsPanel.Location = new System.Drawing.Point(288, 147);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Size = new System.Drawing.Size(174, 79);
            this.optionsPanel.TabIndex = 30;
            this.optionsPanel.Visible = false;
            // 
            // deleteButton
            // 
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Location = new System.Drawing.Point(29, 43);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(117, 23);
            this.deleteButton.TabIndex = 1;
            this.deleteButton.Text = "Ištrinti operaciją";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // editButton
            // 
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editButton.Location = new System.Drawing.Point(29, 14);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(117, 23);
            this.editButton.TabIndex = 0;
            this.editButton.Text = "Koreguoti operaciją";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // loadingBox
            // 
            this.loadingBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadingBox.Image = global::cryptoFinance.Properties.Resources.loading_screen;
            this.loadingBox.Location = new System.Drawing.Point(202, 186);
            this.loadingBox.Name = "loadingBox";
            this.loadingBox.Size = new System.Drawing.Size(63, 49);
            this.loadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingBox.TabIndex = 31;
            this.loadingBox.TabStop = false;
            this.loadingBox.Visible = false;
            // 
            // Investments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 420);
            this.Controls.Add(this.loadingBox);
            this.Controls.Add(this.optionsPanel);
            this.Controls.Add(this.addOperationPicture);
            this.Controls.Add(this.loadMoreLabel);
            this.Controls.Add(this.operationDataGrid);
            this.Controls.Add(this.progressBarPanel);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.updateCryptoList);
            this.Controls.Add(this.lastTimeUpdated);
            this.Name = "Investments";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Investments";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Investments_FormClosing);
            this.Load += new System.EventHandler(this.Investments_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Investments_MouseClick);
            this.alertPanel.ResumeLayout(false);
            this.alertPanel.PerformLayout();
            this.progressBarPanel.ResumeLayout(false);
            this.progressBarPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operationDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addOperationPicture)).EndInit();
            this.optionsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button updateCryptoList;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label lastTimeUpdated;
        private System.Windows.Forms.Label alertLabel;
        private System.Windows.Forms.Panel alertPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Panel progressBarPanel;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Timer investmentsTimer;
        private System.Windows.Forms.DataGridView operationDataGrid;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Label loadMoreLabel;
        private System.Windows.Forms.PictureBox addOperationPicture;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn Column5;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.PictureBox loadingBox;
    }
}