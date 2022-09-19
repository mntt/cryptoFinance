using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace cryptoFinance
{
    public static class GetPrices
    {
        private static decimal price { get; set; }
        private static CancellationTokenSource timeoutcancel { get; set; }

        private static bool IsInternetConnected()
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

        private static async Task DownloadPrice(string id)
        {
            if (!timeoutcancel.IsCancellationRequested)
            {
                GetCultureInfo gci = new GetCultureInfo(".");

                try
                {
                    string jsonURL = new WebClient().DownloadString("https://api.coingecko.com/api/v3/simple/price?ids=" + id + "&vs_currencies=eur");
                    var data = (JObject)JsonConvert.DeserializeObject(jsonURL);
                    string link = $"{id}.eur";
                    price = data.SelectToken(link).Value<decimal>();
                }
                catch
                {
                    price = -2;
                }
            }
        }

        private static async Task FetchPrice(string id)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected)
            {
                int timeout = 2000;
                timeoutcancel = new CancellationTokenSource();
                var delayTask = Task.Delay(timeout, timeoutcancel.Token);
                var task = DownloadPrice(id);

                if (await Task.WhenAny(task, delayTask) == task)
                {
                    timeoutcancel.Cancel();
                }
                else
                {
                    timeoutcancel.Cancel();
                    price = -3;
                }
            }
            else
            {
                price = -1;
            }
        }

        public static decimal ById(string id)
        {
            var task = Task.Run(() => FetchPrice(id));
            task.Wait();

            return price;
        }

    }
}
