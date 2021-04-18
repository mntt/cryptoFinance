namespace cryptoFinance
{
    partial class CurrentAssets
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
            this.dataGridCurrentAssets = new System.Windows.Forms.DataGridView();
            this.refreshPricesButton = new System.Windows.Forms.Button();
            this.alertLabel = new System.Windows.Forms.Label();
            this.currentValueLabel = new System.Windows.Forms.Label();
            this.chartButton = new System.Windows.Forms.Button();
            this.pieChart = new LiveCharts.WinForms.PieChart();
            this.investButton = new System.Windows.Forms.Button();
            this.walletsButton = new System.Windows.Forms.Button();
            this.statisticsButton = new System.Windows.Forms.Button();
            this.refreshDataGridTimer = new System.Windows.Forms.Timer(this.components);
            this.alertPanel = new System.Windows.Forms.Panel();
            this.loadingBox = new System.Windows.Forms.PictureBox();
            this.backgroundLoading = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCurrentAssets)).BeginInit();
            this.alertPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridCurrentAssets
            // 
            this.dataGridCurrentAssets.AllowUserToAddRows = false;
            this.dataGridCurrentAssets.AllowUserToResizeColumns = false;
            this.dataGridCurrentAssets.AllowUserToResizeRows = false;
            this.dataGridCurrentAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCurrentAssets.Location = new System.Drawing.Point(36, 118);
            this.dataGridCurrentAssets.Name = "dataGridCurrentAssets";
            this.dataGridCurrentAssets.ReadOnly = true;
            this.dataGridCurrentAssets.RowHeadersVisible = false;
            this.dataGridCurrentAssets.Size = new System.Drawing.Size(522, 169);
            this.dataGridCurrentAssets.TabIndex = 0;
            this.dataGridCurrentAssets.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridCurrentAssets_CellFormatting);
            // 
            // refreshPricesButton
            // 
            this.refreshPricesButton.Location = new System.Drawing.Point(12, 12);
            this.refreshPricesButton.Name = "refreshPricesButton";
            this.refreshPricesButton.Size = new System.Drawing.Size(97, 23);
            this.refreshPricesButton.TabIndex = 2;
            this.refreshPricesButton.Text = "Atnaujinti kainas";
            this.refreshPricesButton.UseVisualStyleBackColor = true;
            this.refreshPricesButton.Click += new System.EventHandler(this.RefreshPricesButton_Click);
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Location = new System.Drawing.Point(4, 4);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(29, 13);
            this.alertLabel.TabIndex = 4;
            this.alertLabel.Text = "label";
            // 
            // currentValueLabel
            // 
            this.currentValueLabel.AutoSize = true;
            this.currentValueLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.currentValueLabel.Location = new System.Drawing.Point(234, 77);
            this.currentValueLabel.Name = "currentValueLabel";
            this.currentValueLabel.Size = new System.Drawing.Size(30, 20);
            this.currentValueLabel.TabIndex = 5;
            this.currentValueLabel.Text = "...";
            // 
            // chartButton
            // 
            this.chartButton.Location = new System.Drawing.Point(12, 41);
            this.chartButton.Name = "chartButton";
            this.chartButton.Size = new System.Drawing.Size(97, 23);
            this.chartButton.TabIndex = 6;
            this.chartButton.Text = "Diagrama";
            this.chartButton.UseVisualStyleBackColor = true;
            this.chartButton.Click += new System.EventHandler(this.ChartButton_Click);
            // 
            // pieChart
            // 
            this.pieChart.Location = new System.Drawing.Point(36, 118);
            this.pieChart.Name = "pieChart";
            this.pieChart.Size = new System.Drawing.Size(522, 169);
            this.pieChart.TabIndex = 10;
            this.pieChart.Text = "pieChart1";
            this.pieChart.Visible = false;
            // 
            // investButton
            // 
            this.investButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.investButton.Location = new System.Drawing.Point(201, 374);
            this.investButton.Name = "investButton";
            this.investButton.Size = new System.Drawing.Size(60, 47);
            this.investButton.TabIndex = 11;
            this.investButton.Text = "Operacijos";
            this.investButton.UseVisualStyleBackColor = true;
            this.investButton.Click += new System.EventHandler(this.InvestButton_Click);
            // 
            // walletsButton
            // 
            this.walletsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.walletsButton.Location = new System.Drawing.Point(267, 374);
            this.walletsButton.Name = "walletsButton";
            this.walletsButton.Size = new System.Drawing.Size(60, 47);
            this.walletsButton.TabIndex = 14;
            this.walletsButton.Text = "Piniginės";
            this.walletsButton.UseVisualStyleBackColor = true;
            this.walletsButton.Click += new System.EventHandler(this.WalletsButton_Click);
            // 
            // statisticsButton
            // 
            this.statisticsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.statisticsButton.Location = new System.Drawing.Point(333, 374);
            this.statisticsButton.Name = "statisticsButton";
            this.statisticsButton.Size = new System.Drawing.Size(60, 47);
            this.statisticsButton.TabIndex = 15;
            this.statisticsButton.Text = "Statistika";
            this.statisticsButton.UseVisualStyleBackColor = true;
            this.statisticsButton.Click += new System.EventHandler(this.StatisticsButton_Click);
            // 
            // refreshDataGridTimer
            // 
            this.refreshDataGridTimer.Interval = 200;
            this.refreshDataGridTimer.Tick += new System.EventHandler(this.RefreshDataGridTimer_Tick);
            // 
            // alertPanel
            // 
            this.alertPanel.BackColor = System.Drawing.Color.LightCoral;
            this.alertPanel.Controls.Add(this.alertLabel);
            this.alertPanel.Location = new System.Drawing.Point(418, 3);
            this.alertPanel.Name = "alertPanel";
            this.alertPanel.Size = new System.Drawing.Size(170, 38);
            this.alertPanel.TabIndex = 16;
            this.alertPanel.Visible = false;
            this.alertPanel.VisibleChanged += new System.EventHandler(this.AlertPanel_VisibleChanged);
            // 
            // loadingBox
            // 
            this.loadingBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadingBox.Enabled = false;
            this.loadingBox.Image = global::cryptoFinance.Properties.Resources.loading_screen;
            this.loadingBox.Location = new System.Drawing.Point(264, 175);
            this.loadingBox.Name = "loadingBox";
            this.loadingBox.Size = new System.Drawing.Size(63, 49);
            this.loadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadingBox.TabIndex = 18;
            this.loadingBox.TabStop = false;
            this.loadingBox.Visible = false;
            // 
            // CurrentAssets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 433);
            this.Controls.Add(this.loadingBox);
            this.Controls.Add(this.alertPanel);
            this.Controls.Add(this.statisticsButton);
            this.Controls.Add(this.walletsButton);
            this.Controls.Add(this.investButton);
            this.Controls.Add(this.pieChart);
            this.Controls.Add(this.chartButton);
            this.Controls.Add(this.currentValueLabel);
            this.Controls.Add(this.refreshPricesButton);
            this.Controls.Add(this.dataGridCurrentAssets);
            this.Name = "CurrentAssets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CurrentAssets";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurrentAssets_FormClosing);
            this.Load += new System.EventHandler(this.CurrentAssets_Load);
            this.VisibleChanged += new System.EventHandler(this.CurrentAssets_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCurrentAssets)).EndInit();
            this.alertPanel.ResumeLayout(false);
            this.alertPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridCurrentAssets;
        private System.Windows.Forms.Button refreshPricesButton;
        private System.Windows.Forms.Label alertLabel;
        public System.Windows.Forms.Label currentValueLabel;
        private System.Windows.Forms.Button chartButton;
        private LiveCharts.WinForms.PieChart pieChart;
        private System.Windows.Forms.Button investButton;
        private System.Windows.Forms.Button walletsButton;
        private System.Windows.Forms.Button statisticsButton;
        private System.Windows.Forms.Timer refreshDataGridTimer;
        private System.Windows.Forms.Panel alertPanel;
        private System.Windows.Forms.PictureBox loadingBox;
        private System.ComponentModel.BackgroundWorker backgroundLoading;
    }
}