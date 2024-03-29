﻿using Newtonsoft.Json;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;

namespace cryptoFinance
{
    public static class GetPrices
    {
        private static decimal price { get; set; }
        private static CancellationTokenSource timeoutcancel { get; set; }
        private static List<ConstructingLists> coins { get; set; }

        private static bool jsonSuccess = true;

        private static int maxStringNumber = 437; //437 cia daugiausiai tiek paima simboliu coingecko API

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

        private static async Task DownloadPrice(List<ConstructingLists> coins)
        {
            if (!timeoutcancel.IsCancellationRequested)
            {
                List<ConstructingLists> listOfIds = new List<ConstructingLists>();
                var oldlist = Connection.db.GetTable<CoingeckoCryptoList>().ToList();
                int arrayNumber = coins.Count > maxStringNumber ? coins.Count / maxStringNumber : 0;
                string requestString = "";
                string[] requestStrings = new string[arrayNumber + 1];

                for (int i = 0; i < coins.Count; i++)
                {
                    int counter = 0;
                    listOfIds.Add(coins[i]);
                    requestString += coins[i].cryptoId + "%2C";

                    if (i > 0 && i % maxStringNumber == 0)
                    {
                        requestStrings[counter] = requestString;
                        counter += 1;
                        requestString = "";
                    }
                    else if (i == coins.Count - 1)
                    {
                        requestStrings[arrayNumber] = requestString;
                    }
                }

                for (int i = 0; i < requestStrings.Length; i++)
                {
                    string jsonURL = "";
                    dynamic data = null;

                    try
                    {
                        jsonURL = new WebClient().DownloadString("https://api.coingecko.com/api/v3/simple/price?ids=" + requestStrings[i] + "&vs_currencies=eur");
                        data = JsonConvert.DeserializeObject<dynamic>(jsonURL);
                    }
                    catch
                    {
                        jsonSuccess = false;
                    }

                    var orderedIds = listOfIds.Skip(maxStringNumber * i).Take(maxStringNumber).ToList();

                    for (int j = 0; j < orderedIds.Count; j++)
                    {
                        try
                        {
                            GetCultureInfo gci = new GetCultureInfo(".");
                            string link = $"{orderedIds[j].cryptoId}.eur";
                            price = (decimal)data.SelectToken(link);
                        }
                        catch
                        {
                            price = orderedIds[j].customCoin == true ? Connection.db.GetTable<CurrentAssetsDB>()
                                    .Where(x => x.CryptoId == orderedIds[j].cryptoId)
                                    .Select(x => x.Price)
                                    .ToList()
                                    .First() : -2;
                        }

                        coins.Where(x => x.cryptoId == orderedIds[j].cryptoId).ToList().ForEach(x => x.price = price);
                    }
                }
            }

            await Task.Delay(100);
        }

        private static async Task FetchPrice(List<ConstructingLists> coins)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected)
            {
                int timeout = 2000;
                timeoutcancel = new CancellationTokenSource();
                var delayTask = Task.Delay(timeout, timeoutcancel.Token);
                var task = DownloadPrice(coins);

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

        public static List<ConstructingLists> ById(List<ConstructingLists> _coins)
        {
            jsonSuccess = true;
            coins = _coins;
            var task = Task.Run(() => FetchPrice(coins));
            task.Wait();

            return coins;
        }

        public static decimal ById(string cryptoId)
        {
            jsonSuccess = true;
            ConstructingLists coin = new ConstructingLists(DateTime.Now, "not defined", false, cryptoId, 0, 0, 0);
            
            coins = new List<ConstructingLists>
            {
                coin
            };

            var task = Task.Run(() => FetchPrice(coins));
            task.Wait();

            if (!jsonSuccess)
            {
                price = -2;
            }

            return price;
        }

    }
}
