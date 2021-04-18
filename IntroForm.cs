using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public partial class IntroForm : Form
    {
        private CurrentAssets ca;

        public IntroForm()
        {
            InitializeComponent();
            this.Text = " ";
            progressBar.ForeColor = Color.Cyan;
            ca = new CurrentAssets();
            worker.RunWorkerAsync(ca);
        }

        private void LoadCurrentAssetsData()
        {
            var task1 = Task.Run(() => ca.CreateCoinList());
            task1.Wait();
            progressBar.Value = 20;
            var task2 = Task.Run(() => ca.SetStartDate());
            task2.Wait();
            progressBar.Value = 40;
            var task3 = Task.Run(() => ca.UpdateDataGrid());
            task3.Wait();
            progressBar.Value = 60;
            var task4 = Task.Run(() => ca.CountCurrentValue());
            task4.Wait();
            progressBar.Value = 80;
            var task5 = Task.Run(() => ca.UpdatePricesAndCurrentValue());
            task5.Wait();
            progressBar.Value = 100;
            
            ca.EnableAllButtons();
        }

        private void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LoadCurrentAssetsData();
            e.Result = e.Argument;            
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            var ca = e.Result as CurrentAssets;
            if(ca != null)
            {
                this.Hide();
                ca.Show();
                ca.Focus();
            }
        }

    }
}
