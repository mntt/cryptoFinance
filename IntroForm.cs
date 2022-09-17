using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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

        public async Task CheckLogos()
        {
            var uniqueCoins = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();

            foreach (var item in uniqueCoins)
            {
                ChangeProgressLabel("Tvarkomi logotipai...");
                var namesplit = item.Split('(');
                Image logo = Connection.iwdb.GetLogo(namesplit[0].Trim(' '), namesplit[1].Trim(')'));

                if (logo == null)
                {
                    bool customCoin = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == item).Select(x => x.CustomCoin).ToList().Last();

                    if (customCoin)
                    {
                        var searchlist = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => x.CryptoName == namesplit[0].Trim(' ') && x.CryptoSymbol == namesplit[1].Trim(')')).ToList();
                                                
                        if (searchlist.Count == 0)
                        {
                            Connection.iwdb.InsertCoinGeckoCryptoList(logo, namesplit[0].Trim(' '), namesplit[1].Trim(')'), namesplit[0].Trim(' '), 0);
                        }

                        Connection.iwdb.InsertCryptoLogo(namesplit[0].Trim(' '), cryptoFinance.Properties.Resources.defaultLogo);
                    }
                    else
                    {
                        MessageBox.Show(namesplit[0] + " (" + namesplit[1]);
                        string id = Connection.db.GetTable<CoingeckoCryptoList>()
                            .Where(x => x.CryptoName == namesplit[0] && x.CryptoSymbol == namesplit[1].Trim(')')).Select(x => x.CryptoId).First();
                        await DownloadLogo(id);
                    }
                }
                else
                {
                    //logo yra, ir bus priskirtas kraunant lenteles
                }
            }

            IncrementPB(17);
        }

        private bool IsInternetConnected()
        {
            string host = "coingecko.com";
            byte[] buffer = new byte[32];
            PingOptions pingOptions = new PingOptions();
            Ping p = new Ping();

            try
            {
                PingReply reply = p.Send(host, 2000, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch
            {
                return false;
            }
        }

        private async Task DownloadLogo(string id)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                string message = "";

                try
                {
                    string url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=eur&ids=" + id + "&order=market_cap_desc&per_page=100&page=1&sparkline=false";
                    var response = await client.GetAsync(url);
                    message = await response.Content.ReadAsStringAsync();
                    var split = message.Split('"');
                    var logoUrl = split[15];
                    Image image = DownloadImageFromUrl(logoUrl);
                    Connection.iwdb.InsertCryptoLogo(id, image);
                }
                catch
                {
                    Connection.iwdb.InsertCryptoLogo(id, null);
                }

                client.Dispose();
            }
            else
            {
                MessageBox.Show("Nėra interneto ryšio. Logotipų atsiųsti nepavyko.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 2000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        private void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var task2 = Task.Run(() => ca.LoadForms(this));
            task2.Wait();
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;

            this.Hide();
            ca.Show();
            ca.Focus();
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
                await CheckLogos();
                
                await ca.LoadForms(this);

                Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
                this.Hide();
                ca.Show();
                ca.Focus();
            }
        }
    }
}
