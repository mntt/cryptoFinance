using System;
using System.Linq;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class CoinQuantity
    {
        public double GetCoinQuantityByWallet(string coinName, string wallet)
        {
            double q = 0;

            try
            {
                var bought = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Operation == "BUY" && x.CryptoName == coinName && x.Wallet == wallet)
                .Select(x => x.CryptoQuantity).ToList();
                var sold = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "SELL" && x.CryptoName == coinName && x.Wallet == wallet)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedIn = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_IN" && x.CryptoName == coinName && x.Wallet == wallet)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedOut = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_OUT" && x.CryptoName == coinName && x.Wallet == wallet)
                    .Select(x => x.CryptoQuantity).ToList();
                var fees = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "FEES" && x.CryptoName == coinName && x.Wallet == wallet)
                    .Select(x => x.CryptoQuantity).ToList();

                q = bought.Sum() - sold.Sum() + transferedIn.Sum() - transferedOut.Sum() - fees.Sum();
            }
            catch
            {
                MessageBox.Show("Nenumatyta klaida. Nepavyko užkrauti kiekio.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return q;
        }

        public double GetCoinQuantityByWalletAndDate(string coinName, string wallet, DateTime maxDate)
        {
            double q = 0;

            try
            {
                var bought = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Operation == "BUY" && x.CryptoName == coinName && x.Wallet == wallet && x.Date <= maxDate)
                .Select(x => x.CryptoQuantity).ToList();
                var sold = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "SELL" && x.CryptoName == coinName && x.Wallet == wallet && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedIn = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_IN" && x.CryptoName == coinName && x.Wallet == wallet && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedOut = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_OUT" && x.CryptoName == coinName && x.Wallet == wallet && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();
                var fees = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "FEES" && x.CryptoName == coinName && x.Wallet == wallet && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();

                q = bought.Sum() - sold.Sum() + transferedIn.Sum() - transferedOut.Sum() - fees.Sum();
            }
            catch
            {
                MessageBox.Show("Nenumatyta klaida. Nepavyko užkrauti kiekio.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return q;
        }

        public double GetCoinQuantityByName(string coinName)
        {
            double q = 0;

            try
            {
                var bought = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Operation == "BUY" && x.CryptoName == coinName)
                .Select(x => x.CryptoQuantity).ToList();
                var sold = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "SELL" && x.CryptoName == coinName)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedIn = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_IN" && x.CryptoName == coinName)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedOut = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_OUT" && x.CryptoName == coinName)
                    .Select(x => x.CryptoQuantity).ToList();
                var fees = Connection.db.GetTable<CryptoTable>() //nebera operation FEES, fees skaiciuojama su BUY , SELL operacijomis
                    .Where(x => x.Operation == "FEES" && x.CryptoName == coinName)
                    .Select(x => x.CryptoQuantity).ToList();

                q = bought.Sum() - sold.Sum() + transferedIn.Sum() - transferedOut.Sum() - fees.Sum();
            }
            catch
            {
                MessageBox.Show("Nenumatyta klaida. Nepavyko užkrauti kiekio.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return q;
        }

        public double GetCoinQuantityByNameAndDate(string coinName, DateTime maxDate)
        {
            double q = 0;

            try
            {
                var bought = Connection.db.GetTable<CryptoTable>()
                .Where(x => x.Operation == "BUY" && x.CryptoName == coinName && x.Date <= maxDate)
                .Select(x => x.CryptoQuantity).ToList();
                var sold = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "SELL" && x.CryptoName == coinName && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedIn = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_IN" && x.CryptoName == coinName && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();
                var transferedOut = Connection.db.GetTable<CryptoTable>()
                    .Where(x => x.Operation == "TRANSFER_OUT" && x.CryptoName == coinName && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();
                var fees = Connection.db.GetTable<CryptoTable>() //nebera operation FEES, fees skaiciuojama su BUY , SELL operacijomis
                    .Where(x => x.Operation == "FEES" && x.CryptoName == coinName && x.Date <= maxDate)
                    .Select(x => x.CryptoQuantity).ToList();

                q = bought.Sum() - sold.Sum() + transferedIn.Sum() - transferedOut.Sum() - fees.Sum();
            }
            catch
            {
                MessageBox.Show("Nenumatyta klaida. Nepavyko užkrauti kiekio.", "Pranešimas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return q;
        }


    }
}
