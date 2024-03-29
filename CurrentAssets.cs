﻿using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class CurrentAssets : Form
    {
        private Image[] refreshImages = new Image[4]
        { Properties.Resources.refresh, Properties.Resources.refresh2, Properties.Resources.refresh3, Properties.Resources.refresh4 };
        public DateTime startDate { get; set; }
        public int rowIndex { get; set; }
        public decimal currentValue = 0;
        public string chart = "";
        private Container container = new Container();
        public bool noConnection { get; set; } 
        public SaveFileDialog sfd { get; set; }
        public bool openCoinChart { get; set; }
        private DataGridForm dgf { get; set; }
        private ChartForm cf { get; set; }
        private OperationsForm op { get; set; }
        private StatisticsForm sf { get; set; }
        private WalletsForm wf { get; set; }
        private LoadingForm lf { get; set; }

        public CurrentAssets()
        {
            InitializeComponent();
        }

        private void AddFormsToContainer()
        {
            container.Add(dataGridCurrentAssets);
            container.Add(pieChart);
            container.Add(investmentsPanel);
            container.Add(addOperationPanel);
            container.Add(statisticsPanel);
            container.Add(walletsPanel);
            container.Add(transferPanel);
            container.Add(assetAlertLabel);
        }

        private void CurrentAssets_Load(object sender, EventArgs e)
        {
            lf = new LoadingForm();
            refreshPricesButton.BackgroundImage = refreshImages[0];
            CountCurrentValue();  
            AddFormsToContainer();
            Design.SetFormDesign(this);
            currentAssetsButton.BackColor = Colours.buttonMouseOverBack;
        }

        public void CountCurrentValue()
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            currentValue = Connection.db.GetTable<CurrentAssetsDB>()
                .Select(x => (decimal)x.CurrentValue).ToList().Sum();

            currentValueLabel.Text = currentValue.ToString("C2");
        }

        public void EnableFormButtons(bool enable)
        {
            chartButton.Enabled = enable;
            operationButton.Enabled = enable;
            walletsButton.Enabled = enable;
            statisticsButton.Enabled = enable;
            refreshPricesButton.Enabled = enable;
        }

        public void AlertPanelControlInstance(int labelTextNumber)
        {
            AlertPanelControl apc = new AlertPanelControl(alertPanel, alertLabel, 645, -38, 276, 38);
            apc.StartPanelAnimation(labelTextNumber);
        }

        private void TogglePriceAlert()
        {
            if (!alertPanel.Visible && dgf.ReturnError() == -1)
            {
                AlertPanelControlInstance(0);
            }
        }

        private void CurrentAssets_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar.Visible)
            {
                DialogResult result = MessageBox.Show("Vykdomas kriptovaliutų sąrašo atnaujinimas. " +
                    "\nAr norite nutraukti siuntimą?", "Pranešimas", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var task = Task.Run(() => cancelButton.PerformClick());
                    task.Wait();
                    this.FormClosing -= CurrentAssets_FormClosing;
                    Application.Exit();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.FormClosing -= CurrentAssets_FormClosing;
                Application.Exit();
            }
        }

        private async Task RefreshDataGridAndCurrentValue()
        {
            dgf.UpdatePricesAndCurrentValue(this);
            dgf.UpdateDataGrid(this);
            CountCurrentValue();
            await Task.Delay(100);
        }

        public async Task GetCurrentValue()
        {
            await Task.Run(() => RefreshDataGridAndCurrentValue());
        }

        public void ShowAssetAlertLabel(string text)
        {
            assetAlertLabel.Text = text;
            assetAlertLabel.Location = new Point(267, 206);
            assetAlertLabel.Visible = true;
            assetAlertLabel.BringToFront();
        }

        private void CenterLoadingBox()
        {
            lf.Location = new Point(this.Location.X+8, this.Location.Y);
            lf.Size = new Size(this.Width-17, this.Height-9);
            lf.loadingBox.Location = new Point((this.Width / 2) - (lf.loadingBox.Width / 2), (this.Height / 2) - (lf.loadingBox.Height / 2));
            this.Enabled = false;
        }

        public void ShowLoading()
        { 
            CenterLoadingBox();
            lf.Show();
        }

        public void HideLoading()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Enabled = true;
            lf.Hide();
        }

        private async void RefreshPricesButton_Click(object sender, EventArgs e)
        {
            if (refreshPricesButton.Enabled)
            {
                EnableFormButtons(false);
                refreshTimer.Start();

                await Task.Run(() => RefreshDataGridAndCurrentValue());

                if (pieChart.Visible)
                {
                    await cf.ConstructPieChartAsync(this, dgf);
                }

                TogglePriceAlert();
                if (!alertPanel.Visible)
                {
                    EnableFormButtons(true);
                }

                refreshTimer.Stop();
                refreshPricesButton.BackgroundImage = refreshImages[0];
            }
        }

        private void HideControls()
        {
            foreach (var item in container.Components)
            {
                Control control = (Control)item;
                control.Visible = false;
            }
        }

        private void UnselectAllButtons()
        {
            currentAssetsButton.BackColor = Colours.panelBackground;
            chartButton.BackColor = Colours.panelBackground;
            operationButton.BackColor = Colours.panelBackground;
            statisticsButton.BackColor = Colours.panelBackground;
            walletsButton.BackColor = Colours.panelBackground;
        }

        private void currentAssetsButton_Click(object sender, EventArgs e)
        {
            if (!dataGridCurrentAssets.Visible)
            {
                UnselectAllButtons();
                currentAssetsButton.BackColor = Colours.buttonMouseOverBack; 
                HideControls();
                dgf.Open(this);
            }

            currentValueLabel.Select();
        }

        private void ChartButton_Click(object sender, EventArgs e)
        {
            if (!chartView.Visible)
            {
                UnselectAllButtons();
                chartButton.BackColor = Colours.buttonMouseOverBack; 
                HideControls();
                cf.Open(this, dgf);
            }

            currentValueLabel.Select();
        }

        private void InvestButton_Click(object sender, EventArgs e)
        {
            if (!operationDataGrid.Visible)
            {
                UnselectAllButtons();
                operationButton.BackColor = Colours.buttonMouseOverBack;
                HideControls();

                op.Open(this, wf);
            }

            currentValueLabel.Select();
        }

        private void StatisticsButton_Click(object sender, EventArgs e)
        {
            if (!statisticsPanel.Visible)
            {
                UnselectAllButtons();
                statisticsButton.BackColor = Colours.buttonMouseOverBack;
                HideControls();
                sf.Open(this);
            }

            currentValueLabel.Select();
        }
            
        private void WalletsButton_Click(object sender, EventArgs e)
        {
            if (!walletsPanel.Visible)
            {
                UnselectAllButtons();
                walletsButton.BackColor = Colours.buttonMouseOverBack; 
                HideControls();
                wf.Open(this);
            }

            currentValueLabel.Select();
        }

        private void AlertPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!alertPanel.Visible)
            {
                EnableFormButtons(true);
            }
            else
            {
                EnableFormButtons(false);
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            if (refreshPricesButton.BackgroundImage == refreshImages[0])
            {
                refreshPricesButton.BackgroundImage = refreshImages[1];
            }
            else if (refreshPricesButton.BackgroundImage == refreshImages[1])
            {
                refreshPricesButton.BackgroundImage = refreshImages[2];
            }
            else if (refreshPricesButton.BackgroundImage == refreshImages[2])
            {
                refreshPricesButton.BackgroundImage = refreshImages[3];
            }
            else if (refreshPricesButton.BackgroundImage == refreshImages[3])
            {
                refreshPricesButton.BackgroundImage = refreshImages[0];
            }
        }

        private void investmentsChartButton_Click(object sender, EventArgs e)
        {
            chart = "investments";
            Action action = ShowLoading;
            sf.OpenInvestments(this, chart, action);
        }

        private void cryptoQuantities_Click(object sender, EventArgs e)
        {
            chart = "crypto_quantities";
            Action action = ShowLoading;
            sf.OpenCryptoQuantities(this, chart, action);
        }

        private void cryptoNetValues_Click(object sender, EventArgs e)
        {
            chart = "crypto_currentvalues";
            Action action = ShowLoading;
            sf.OpenNetValues(this, chart, action);
        }

        public async Task LoadForms(IntroForm form)
        {
            await Task.Delay(1);
            form.ChangeProgressLabel("Ruošiama duomenų lentelė...");
            dgf = new DataGridForm(this);
            await Task.Delay(1);
            form.IncrementPB(20);

            await Task.Delay(1);
            form.ChangeProgressLabel("Ruošiami duomenys skritulinei diagramai..."); 
            cf = new ChartForm();
            await Task.Delay(1);
            form.IncrementPB(20);

            await Task.Delay(1);
            form.ChangeProgressLabel("Ruošiami operacijų duomenys..."); 
            op = new OperationsForm(this, dgf);
            await Task.Delay(1);
            form.IncrementPB(20);

            await Task.Delay(1);
            form.ChangeProgressLabel("Ruošiami statistiniai duomenys...");            
            sf = new StatisticsForm(this);
            await Task.Delay(1);
            form.IncrementPB(20);

            await Task.Delay(1);
            form.ChangeProgressLabel("Ruošiami piniginių duomenys...");           
            wf = new WalletsForm(this);
            await Task.Delay(1);
            form.IncrementPB(20);
            await Task.Delay(100);
        }

        private void ClearSelection()
        {
            for (int i = 0; i < dataGridCurrentAssets.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridCurrentAssets.Columns.Count; j++)
                {
                    dataGridCurrentAssets.Rows[i].Cells[j].Style.BackColor = Colours.formBackground;
                }
            }
        }

        private void CurrentAssets_MouseClick(object sender, MouseEventArgs e)
        {
            dataGridCurrentAssets.ClearSelection();
            ClearSelection();
            operationDataGrid.ClearSelection();
            walletDataGrid.ClearSelection();
        }

        public void PressRefreshButton()
        {
            refreshPricesButton.PerformClick();
        }

        private void optionsPanel_Paint(object sender, PaintEventArgs e)
        {     
            ControlPaint.DrawBorder(e.Graphics, optionsPanel.DisplayRectangle, Colours.grid, ButtonBorderStyle.Solid);
        }

        protected override CreateParams CreateParams
        {
            //sutvarko visus flickering

            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

    }
}
