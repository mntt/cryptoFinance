using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
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
        private List<CoingeckoListInfo> coingeckoList { get; set; }
        private Label progressLabel { get; set; }
        public BackgroundWorker worker { get; set; }
        public bool workerCancelButtonPressed { get; set; }
        private List<CoingeckoListInfo> marketCaps { get; set; }
        private AddOperationForm aof { get; set; }
        private bool loadForms { get; set; }

        public CoingeckoListDownloader()
        {
            
        }

        public void StartCA(CurrentAssets _form, AddOperationForm _aof, ProgressBar _progressBar, Label _progressLabel)
        {
            caform = _form;
            aof = _aof;
            progressBar = _progressBar;
            progressLabel = _progressLabel;
            workerCancelButtonPressed = false;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        public void StartIntro(IntroForm _form, CurrentAssets _caForm, ProgressBar _progressBar, bool _loadForms)
        {
            introform = _form;
            caform = _caForm;
            progressBar = _progressBar;
            loadForms = _loadForms;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
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

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected) 
            {
                CancelOrProgress(e, 0);
                var task1 = Task.Run(() => UpdateListTask(e));
                task1.Wait();
                CancelOrProgress(e, 0);
                var task2 = Task.Run(() => UpdateMarketCapsTask(e));
                task2.Wait();
                CancelOrProgress(e, 0);

                if (introform != null)
                {
                    introform.ChangeProgressLabel("Siuntimas baigtas. Programa paleidžiama iš naujo...");
                }
            }
            else
            {
                if(introform != null)
                {
                    MessageBox.Show("Nėra interneto ryšio. Bandykite vėliau.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                
                if(caform != null)
                {
                    MessageBox.Show("Nėra interneto ryšio. Bandykite vėliau.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TerminateDownloading();
                }
            }
        }

        private void TerminateDownloading()
        {
            worker.CancelAsync();
            workerCancelButtonPressed = true;
            progressLabel.Text = "Atšaukiama...";
            progressBar.ForeColor = Control.DefaultBackColor;
            ProgressBarLabels.ReturnLabel(progressLabel, 0);

            if (loadForms)
            {
                Application.Exit();
            }
        }

        private async Task<MatchCollection> DownloadListData()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            string message = "";
            var reg = new Regex("\".*?\"");
            
            try
            {
                string url = "https://api.coingecko.com/api/v3/coins/list";
                var response = await client.GetAsync(url);
                message = await response.Content.ReadAsStringAsync();
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

            client.Dispose();
            return reg.Matches(message);
        }

        private async Task UpdateListTask(System.ComponentModel.DoWorkEventArgs e)
        {
             int timeout = 30000;
             var timeoutcancel = new CancellationTokenSource();
             var delayTask = Task.Delay(timeout, timeoutcancel.Token);
             var task = DownloadListData();

            if (await Task.WhenAny(task, delayTask) == task)
            {
                timeoutcancel.Cancel();
                var matches = await DownloadListData();
                var coinList = FixListInfo(AddStringsToList(matches));
                AddDataToList(coinList);
                CancelOrProgress(e, 26);
            }
            else
            {
                if (introform != null)
                {
                    MessageBox.Show("Nepavyko pasiekti coingecko API per 30 sekundžių.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }

                if (caform != null)
                {
                    MessageBox.Show("Nepavyko pasiekti coingecko API per 30 sekundžių.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TerminateDownloading();
                }
            }
            
        }

        private List<string> FixListInfo(List<string> _data)
        {
            List<string> listOfIndexes = new List<string>();
            var data = _data;
            int indexSaved = 0;

            for (int i = 0; i < data.Count; i++)
            {
                int testnr = i - 1;
                if(i == 1 || testnr % 6 == 0)
                {
                    listOfIndexes.Add(data[i]);
                    indexSaved = i;
                }

                if(i == indexSaved + 2 || i == indexSaved + 4)
                {
                    listOfIndexes.Add(data[i]);
                }
            }

            return listOfIndexes;
        }

        private List<string> AddStringsToList(MatchCollection matches)
        {
            List<string> data = new List<string>();

            int counter = 0;
            foreach (var item in matches)
            {
                data.Add(item.ToString().Trim('"'));
                counter++;
            }

            return data;
        }

        private void AddDataToList(List<string> coinList)
        {
            coingeckoList = new List<CoingeckoListInfo>();
            int multiplier = 0;

            for (int i = 0; i < (coinList.Count / 3); i++)
            {
                CoingeckoListInfo info = new CoingeckoListInfo
                    (
                        null,
                        coinList[multiplier],
                        coinList[1 + multiplier],
                        coinList[2 + multiplier],
                        0
                    );

                coingeckoList.Add(info);
                multiplier += 3;
            }

        }

        private async Task DownloadMarketCaps(System.ComponentModel.DoWorkEventArgs e)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            var ids = coingeckoList.Select(x => x.cryptoId).ToList();
            var idList = ReturnFixedIdList(ids, e);

            marketCaps = new List<CoingeckoListInfo>();

            var reg = new Regex("\".*?\"");
            List<string> unfixedMarketCapData = new List<string>();

            for (int i = 0; i < idList.Count; i++)
            {
                if (i % (idList.Count / 3) == 0)
                {
                    CancelOrProgress(e, 6);
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                }

                try
                {
                    string url = "https://api.coingecko.com/api/v3/simple/price?ids=" + idList[i] + "&vs_currencies=eur&include_market_cap=true&include_24hr_vol=false&include_24hr_change=false&include_last_updated_at=false";
                    var response = await client.GetAsync(url);
                    var message = await response.Content.ReadAsStringAsync();
                    unfixedMarketCapData = message.Split('}').ToList();
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

                marketCaps.AddRange(ReturnMarketCapsList(unfixedMarketCapData, reg));
            }

            client.Dispose();
        }

        private async Task UpdateMarketCapsTask(System.ComponentModel.DoWorkEventArgs e)
        {
            int timeout = 30000;
            var timeoutcancel = new CancellationTokenSource();
            var delayTask = Task.Delay(timeout, timeoutcancel.Token);
            var task = DownloadMarketCaps(e);

            if (await Task.WhenAny(task, delayTask) == task)
            {
                timeoutcancel.Cancel();
                InsertMcaps(marketCaps, e); 
            }
            else
            {
                if (introform != null)
                {
                    MessageBox.Show("Nepavyko pasiekti coingecko API per 30 sekundžių.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }

                if (caform != null)
                {
                    MessageBox.Show("Nepavyko pasiekti coingecko API per 30 sekundžių.", "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TerminateDownloading();
                }
            }
        }

        private List<string> ReturnFixedIdList(List<string> ids, System.ComponentModel.DoWorkEventArgs e)
        {
            List<string> idList = new List<string>();
            string allids = "";
            var list = ids;

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    allids += list[i];
                }

                if (i > 0)
                {
                    allids += "%2C" + list[i];
                }

                if (i % 437 == 0 || i == (list.Count - 1)) //437 - toki max skaiciu id priima coingecko
                {
                    idList.Add(allids);
                    allids = "";
                }

                if (i % (list.Count / 11) == 0)
                {
                    CancelOrProgress(e, 4);
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                }
            }

            return idList;
        }
    
        private List<CoingeckoListInfo> ReturnMarketCapsList(List<string> unfixedMarketCapData, Regex reg)
        {
            GetCultureInfo info = new GetCultureInfo(".");
            List<CoingeckoListInfo> mcaps = new List<CoingeckoListInfo>();

            for (int i = 0; i < unfixedMarketCapData.Count; i++)
            {
                List<string> tempList = new List<string>();
                string sum = "";
                string id = "";

                try
                {
                    tempList = unfixedMarketCapData[i].Split(':').ToList();
                    sum = tempList[tempList.Count - 1].Trim(' ').Trim('{');
                    var matches = reg.Matches(tempList[0]);
                    id = matches[0].ToString().Trim('"');
                }
                catch
                {
                    //"i" coin doesn't have market cap;
                }

                double n;
                bool isNumeric = double.TryParse(sum, out n);

                if (isNumeric)
                {
                    if (double.Parse(sum) > 0)
                    {
                        CoingeckoListInfo coin = new CoingeckoListInfo(id, double.Parse(sum));
                        mcaps.Add(coin);
                    }
                }
            }

            return mcaps;
        }

        private void InsertMcaps(List<CoingeckoListInfo> mcaps, System.ComponentModel.DoWorkEventArgs e)
        {
            int counter = 0;

            foreach (var item in mcaps)
            {
                coingeckoList.Where(x => x.cryptoId == item.cryptoId).ToList().ForEach(x => x.marketCap = item.marketCap);

                if (counter % (mcaps.Count / 2) == 0)
                {
                    CancelOrProgress(e, 3);
                    if (worker.CancellationPending)
                    {
                        break;
                    }
                }

                counter++;
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if(caform != null && introform == null)
            {
                progressBar.Increment(e.ProgressPercentage);

                if (progressBar.Value == 0)
                {
                    ProgressBarLabels.ReturnLabel(progressLabel, 0);
                }

                if (progressBar.Value >= 20)
                {
                    ProgressBarLabels.ReturnLabel(progressLabel, 1);
                }

                if (progressBar.Value >= 40)
                {
                    ProgressBarLabels.ReturnLabel(progressLabel, 2);
                }

                if (progressBar.Value >= 70)
                {
                    ProgressBarLabels.ReturnLabel(progressLabel, 3);
                }
            } 
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
            caform.Invalidate(true);
            
            if (!workerCancelButtonPressed)
            { 
                InsertListToDatabase(coingeckoList); //problema su šituo, freezina intro forma
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

        private void InsertListToDatabase(List<CoingeckoListInfo> coinList)
        {
            if(coinList.Count > 0)
            {
                Connection.iwdb.TruncateTable("CoingeckoCryptoList");
            }

            var list = coinList.OrderByDescending(x => x.marketCap).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Connection.iwdb.InsertCoinGeckoCryptoList(list[i].logo, list[i].cryptoId, list[i].cryptoSymbol.ToUpper(), list[i].cryptoName, list[i].marketCap);
            }
        }

        private void CancelOrProgress(System.ComponentModel.DoWorkEventArgs e, int progress)
        {
            if (!worker.CancellationPending)
            {
                Thread.Sleep(300);
                //worker.ReportProgress(progress);
                

                progressBar.Increment(progress);

                if(caform != null)
                {
                    caform.progressLabel.Text = progressBar.Value.ToString() + " %";
                }
                
                if(introform != null)
                {
                    introform.ChangeProgressLabel("Siunčiamas kriptovaliutų sąrašas: " + progressBar.Value.ToString() + " %");
                }
            }
            else
            {
                e.Cancel = true;
                progressBar.Value = 0;
            }
        }

    }
}
