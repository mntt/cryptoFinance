using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class CoingeckoListDownloader
    {
        private ProgressBar progressBar { get; set; }
        private CurrentAssets caform { get; set; }
        private IntroForm introform { get; set; }
        private List<CoingeckoTokenData> coingeckoList { get; set; }
        private List<CoingeckoListInfo> tokenListWithFullData { get; set; }
        private Label progressLabel { get; set; }
        public BackgroundWorker worker { get; set; }
        public bool workerCancelButtonPressed { get; set; }
        private List<CoingeckoListInfo> marketCaps { get; set; }
        private AddOperationForm aof { get; set; }
        private bool loadForms { get; set; }

        public CoingeckoListDownloader(CurrentAssets _form, AddOperationForm _aof, ProgressBar _progressBar, Label _progressLabel)
        {
            //add operation form download instance
            caform = _form;
            aof = _aof;
            progressBar = _progressBar;
            progressLabel = _progressLabel;
            workerCancelButtonPressed = false;
            tokenListWithFullData = new List<CoingeckoListInfo>();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        public CoingeckoListDownloader(IntroForm _form, CurrentAssets _caForm, ProgressBar _progressBar, bool _loadForms)
        {
            //intro form download instance
            introform = _form;
            caform = _caForm;
            progressBar = _progressBar;
            loadForms = _loadForms;
            tokenListWithFullData = new List<CoingeckoListInfo>();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            introform.ChangeProgressLabel("Siunčiamas coingecko sąrašas...");
            worker.RunWorkerAsync();
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

        private void NoInternetError()
        {
            if (introform != null)
            {
                MessageBox.Show("Nėra interneto ryšio. Bandykite vėliau.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            if (caform != null)
            {
                MessageBox.Show("Nėra interneto ryšio. Bandykite vėliau.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TerminateDownloading();
            }
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected)
            {
                //20%
                var task1 = Task.Run(() => UpdateListTask(e));
                task1.Wait();
                //70%
                var task2 = Task.Run(() => UpdateMarketCapsTask(e));
                task2.Wait();
                //10%
                var task3 = Task.Run(() => DownloadLogos(e));
                task3.Wait();
            }
            else
            {
                NoInternetError();   
            }
        }

        private async Task UpdateListTask(System.ComponentModel.DoWorkEventArgs e)
        {
            if (caform != null)
            {
                caform.progressLabel.Text = "Laukiama kol bus pasiekiamas coingecko API...";
            }

            await Task.Delay(120000); //to avoid "too many requests" error - using a free API plan
            DownloadListData();
            Progress(20);
            await Task.Delay(100);
        }

        private void DownloadListData()
        {
            try
            {
                string jsonURL = new WebClient().DownloadString("https://api.coingecko.com/api/v3/coins/list");
                coingeckoList = (List<CoingeckoTokenData>)JsonConvert.DeserializeObject(jsonURL, typeof(List<CoingeckoTokenData>));
            }
            catch
            {
                if (introform != null)
                {
                    MessageBox.Show("Klaida apdorojant duomenis.\nNepavyksta atsiųsti kriptovaliutų sąrašo.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }

                if (caform != null)
                {
                    MessageBox.Show("Klaida apdorojant duomenis.\nNepavyksta atsiųsti kriptovaliutų sąrašo.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TerminateDownloading();
                }
            }
        }

        private async Task UpdateMarketCapsTask(System.ComponentModel.DoWorkEventArgs e)
        {
            Task task = DownloadMarketCaps(e);
            await Task.WhenAll(task);
            InsertMcaps(e);
        }

        private async Task DownloadMarketCaps(System.ComponentModel.DoWorkEventArgs e)
        {
            var ids = ReturnFixedIdList(coingeckoList.Select(x => x.id).ToList(), e);
            marketCaps = new List<CoingeckoListInfo>();
            JObject data = null;
            string jsonURL = "";

            for (int i = 0; i < ids.Count; i++)
            {
                try
                {
                    if (ids.Count / 2 == i)
                    {
                        Progress(25);
                        await Task.Delay(60000);
                    }

                    jsonURL = new WebClient().DownloadString("https://api.coingecko.com/api/v3/simple/price?ids=" + ids[i] + "&vs_currencies=eur&include_market_cap=true&include_24hr_vol=false&include_24hr_change=false&include_last_updated_at=false");
                    data = (JObject)JsonConvert.DeserializeObject(jsonURL);

                    for (int j = 0; j < coingeckoList.Count; j++)
                    {
                        string id = coingeckoList[j].id;
                        string mcaplink = $"{id}.eur_market_cap";
                        double mcap = 0;

                        try
                        {
                            mcap = data.SelectToken(mcaplink).Value<double>();
                            CoingeckoListInfo info = new CoingeckoListInfo(id, mcap);
                            marketCaps.Add(info);
                        }
                        catch
                        {
                            //coin not found in this bundle or there is no market cap for selected coin
                        }
                    }
                }
                catch
                {

                    if (introform != null)
                    {
                        MessageBox.Show($"{i}/{ids.Count - 1} Per daug užklausų. Kriptovaliutų sąrašo nepavyko atsiųsti.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }

                    if (caform != null)
                    {
                        MessageBox.Show($"{i}/{ids.Count - 1} Per daug užklausų.\nKriptovaliutų sąrašo nepavyko atnaujinti.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TerminateDownloading();
                    }

                    break;
                }
            }

            Progress(25);
            await Task.Delay(100);
        }

        private List<string> ReturnFixedIdList(List<string> ids, System.ComponentModel.DoWorkEventArgs e)
        {
            List<string> idList = new List<string>();
            string allids = "";
            var list = ids;

            for (int i = 0; i < list.Count; i++)
            {
                if (i >= 0 && allids == "")
                {
                    allids += list[i];
                }

                if (i > 0 && allids != "")
                {
                    allids += "%2C" + list[i];
                }

                if ((i % 437 == 0 && i != 0) || i == (list.Count - 1)) //437 - toki max skaiciu id priima coingecko
                {
                    idList.Add(allids);
                    allids = "";
                }
            }

            Progress(10);

            return idList;
        }

        private void InsertMcaps(System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < coingeckoList.Count; i++)
            {
                double marketcap = 0;

                try
                {
                    marketcap = marketCaps.Where(x => x.cryptoId == coingeckoList[i].id).Select(x => x.marketCap).First();
                }
                catch
                {
                    //marketcap remains zero
                }

                CoingeckoListInfo info = new CoingeckoListInfo
                    (
                        null,
                        coingeckoList[i].id,
                        coingeckoList[i].symbol,
                        coingeckoList[i].name,
                        marketcap
                    );

                tokenListWithFullData.Add(info);
            }

            Progress(10);
        }

        private async Task DownloadLogos(System.ComponentModel.DoWorkEventArgs e)
        {
            if (introform == null)
            {
                var coins = Connection.db.GetTable<CryptoTable>().Select(x => x.CryptoName).Distinct().ToList();
                var oldlist = Connection.db.GetTable<CoingeckoCryptoList>().ToList();

                await Task.Delay(60000);
                for (int i = 0; i < coins.Count; i++)
                {
                    if (i % 12 == 0 && i != 0)
                    {
                        await Task.Delay(60000);
                    }

                    var split = coins[i].Split('(');
                    var name = split[0].TrimEnd(' ');
                    var symbol = split[1].Trim(')');
                    var id = oldlist.Where(x => x.CryptoName == name && x.CryptoSymbol == symbol).Select(x => x.CryptoId).First();
                    string jsonURL = new WebClient().DownloadString("https://api.coingecko.com/api/v3/coins/markets?vs_currency=eur&ids=" + id + "&order=market_cap_desc&per_page=100&page=1&sparkline=false");
                    var data = JsonConvert.DeserializeObject<dynamic>(jsonURL);
                    string imageLink = (string)data[0]["image"];
                    Image image = DownloadImageFromUrl(imageLink);
                    tokenListWithFullData.Where(x => x.cryptoId == id).ToList().ForEach(x => x.logo = image);
                }
            }

            await Task.Delay(1000);
            Progress(10);
        }

        private System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 3000;

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

        private async void TerminateDownloading()
        {
            worker.CancelAsync();
            workerCancelButtonPressed = true;
            worker.Dispose();
            progressLabel.Text = "Atšaukiama...";
            progressBar.ForeColor = Control.DefaultBackColor;
            await Task.Delay(1000);
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
            caform.Invalidate(true);
            caform.progressBarPanel.Visible = false;
            caform.progressLabel.Text = "";

            if (loadForms)
            {
                Application.Exit();
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
            caform.Invalidate(true);
            
            if (!workerCancelButtonPressed)
            {
                if (introform != null)
                {
                    introform.ChangeProgressLabel("Siuntimas baigtas. Programa paleidžiama iš naujo...");
                }

                InsertListToDatabase();
                DateTime time = DateTime.Now;
                Connection.iwdb.InsertListDate(time);

                if(caform != null && introform == null)
                {
                    caform.AlertPanelControlInstance(21);
                    caform.progressBarPanel.Visible = false;
                }
                
                if(aof != null)
                {
                    aof.UpdateDate();
                }

                if (loadForms)
                {
                    Application.Restart();   
                }
            }
            else
            {
                if(caform != null)
                {
                    caform.progressBarPanel.Visible = false;
                    caform.AlertPanelControlInstance(1);
                }
                else
                {
                    caform.AlertPanelControlInstance(13);
                }
            }

            worker.Dispose();     
        }

        private void InsertListToDatabase()
        {
            if(tokenListWithFullData.Count > 0)
            {
                Connection.iwdb.TruncateTable("CoingeckoCryptoList");
            }

            var list = tokenListWithFullData.OrderByDescending(x => x.marketCap).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Connection.iwdb.InsertCoinGeckoCryptoList(list[i].logo, list[i].cryptoId, list[i].cryptoSymbol.ToUpper(), list[i].cryptoName, list[i].marketCap);
            }
        }

        private void Progress(int progress)
        {
            if (!workerCancelButtonPressed)
            {
                Thread.Sleep(300);
                progressBar.Increment(progress);

                if (caform != null)
                {
                    caform.progressLabel.Text = progressBar.Value.ToString() + " %";
                }

                if (introform != null)
                {
                    introform.ChangeProgressLabel("Siunčiamas kriptovaliutų sąrašas: " + progressBar.Value.ToString() + " %");
                }
            }
        }

    }
}
