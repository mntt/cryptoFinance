using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                    var search = tempList[2].Where(x => x == 'e').ToList();

                    if(search.Count == 1)
                    {
                        var finalprice = tempList[2].Split('e').ToList();
                        var powerchars = finalprice[1].Trim('}').ToCharArray();

                        /*string text = "";
                        foreach(var item in powerchars)
                        {
                            text += item + " ";
                        }*/

                        double secondnumber = 1;
                        if(powerchars[0] == '-')
                        {
                            string number = "-" + powerchars[2];
                            secondnumber = Math.Pow(10, double.Parse(number));
                        }
                        else if(powerchars[0] == '+')
                        {
                            secondnumber = Math.Pow(10, double.Parse(powerchars[2].ToString()));
                        }

                        price = decimal.Parse(finalprice[0]) * (decimal)secondnumber;
                    }
                    else
                    {
                        price = decimal.Parse(tempList[2].Trim('}'));
                    }
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
