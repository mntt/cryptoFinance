using System;
using System.Linq;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class IntroForm : Form
    {
        private CurrentAssets ca;

        public IntroForm(CurrentAssets _ca)
        {
            InitializeComponent();
            this.Text = " ";
            ca = _ca;

            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NonClientAreaEnabled;
            Design.ProgressBarStyling(progressBar);     
        }

        public void ChangeProgressLabel(string text)
        {
            progress.Text = text;
        }

        public void IncrementPB(int progress)
        {
            progressBar.Increment(progress);
        }

        private async void IntroForm_Load(object sender, EventArgs e)
        {
            var list = Connection.db.GetTable<CoingeckoCryptoList>().ToList();
            if (list.Count == 0)
            {
                CoingeckoListDownloader cd = new CoingeckoListDownloader(this, ca, progressBar, true);
            }
            else
            {
                await ca.LoadForms(this);

                Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
                this.Hide();
                ca.Show();
                ca.Focus();
            }
        }
    }
}
