using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace cryptoFinance
{
    public static class GetPrices
    {
        private static double price { get; set; }
        private static CancellationTokenSource timeoutcancel { get; set; }

        private static async Task DownloadPrice(string id)
        {
            if (!timeoutcancel.IsCancellationRequested)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                GetCultureInfo gci = new GetCultureInfo(".");

                try
                {
                    string url = "https://api.coingecko.com/api/v3/simple/price?ids=" + id + "&vs_currencies=eur";
                    var response = await client.GetAsync(url);
                    var message = await response.Content.ReadAsStringAsync();
                    var tempList = message.Split(':').ToList();
                    price = double.Parse(tempList[2].Trim('}'));
                }
                catch
                {
                    price = -1;
                }
            }            
        }

        private static async Task FetchPrice(string id)
        {
            int timeout = 3000;
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
                price = -2;
            }
        }

        public static double ById(string id)
        {
            var task = Task.Run(() => FetchPrice(id));
            task.Wait();

            return price;
        }

    }
}
