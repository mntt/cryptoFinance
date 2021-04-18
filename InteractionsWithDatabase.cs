using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace cryptoFinance
{
    public class InteractionsWithDatabase
    {
        private SqlConnection sql = new SqlConnection(Connection.connectionString);

        public void TruncateTable(string table)
        {
            sql.Open();
            string querry = "TRUNCATE table " + table + "";
            SqlCommand command = new SqlCommand(querry, sql);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void InsertCoinGeckoCryptoList(string cryptoId, string cryptoSymbol, string cryptoName, double marketCap)
        {
            sql.Open();
            string querry = "INSERT INTO CoingeckoCryptoList(CryptoId, CryptoSymbol, CryptoName, MarketCap) VALUES (@CryptoId, @CryptoSymbol, @CryptoName, @MarketCap)";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@CryptoId", cryptoId);
            command.Parameters.AddWithValue("@CryptoSymbol", cryptoSymbol);
            command.Parameters.AddWithValue("@CryptoName", cryptoName);
            command.Parameters.AddWithValue("@MarketCap", marketCap);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public List<string> FetchWallets(string text)
        {
            List<string> wallets = new List<string>();

            sql.Open();
            string querry = "SELECT Wallet FROM CryptoTable WHERE Wallet LIKE '" + text + "%'";
            SqlCommand command = new SqlCommand(querry, sql);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string wallet = reader["Wallet".ToString()].ToString();
                wallets.Add(wallet);
            }
            sql.Close();

            return wallets;
        }

        public List<ConstructingLists> FetchCoinNames(string text)
        {
            List<ConstructingLists> coins = new List<ConstructingLists>();

            sql.Open();
            string querry = "SELECT * FROM CoingeckoCryptoList WHERE CryptoSymbol LIKE '" + text + "%' OR CryptoName LIKE '" + text + "%'";
            SqlCommand command = new SqlCommand(querry, sql);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string symbol = reader["CryptoSymbol".ToString()].ToString();
                string name = reader["CryptoName".ToString()].ToString();
                string finalName = name + " " + "(" + symbol + ")";
                string marketCap = reader["MarketCap".ToString()].ToString();

                double mcap;
                bool isNumeric = double.TryParse(marketCap, out mcap);

                if (isNumeric)
                {
                    if (mcap >= 0)
                    {
                        ConstructingLists coin = new ConstructingLists(finalName, mcap);
                        coins.Add(coin);
                    }
                }
            }
            sql.Close();

            return coins;
        }

        public void DeleteOperation(int id)
        {
            sql.Open();
            string querry = "DELETE FROM CryptoTable WHERE Id = @id";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void InsertCryptoTable(DateTime date, string name, double quantity, string operation, string wallet, double sum, double price, double fee, double currentValue)
        {
            sql.Open();
            string querry = "INSERT INTO CryptoTable(Date, CryptoName, CryptoQuantity, Operation, Wallet, Sum, Fee, LastPrice, LastCurrentValue) VALUES (@Date, @CryptoName, @CryptoQuantity, @Operation, @Wallet, @Sum, @Fee, @LastPrice, @LastCurrentValue)";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@CryptoName", name);
            command.Parameters.AddWithValue("@CryptoQuantity", double.Parse(quantity.ToString("0.00000000")));
            command.Parameters.AddWithValue("@Operation", operation);
            command.Parameters.AddWithValue("@Wallet", wallet);
            command.Parameters.AddWithValue("@Sum", double.Parse(sum.ToString("0.00")));
            command.Parameters.AddWithValue("@LastPrice", double.Parse(price.ToString("0.0000")));
            command.Parameters.AddWithValue("@Fee", double.Parse(fee.ToString("0.0000")));
            command.Parameters.AddWithValue("@LastCurrentValue", currentValue);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void UpdateCryptoTable(int id, string operation, string name, DateTime date, double quantity, string wallet, double price, double fee, double sum)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            sql.Open();
            string querry = "UPDATE CryptoTable SET Operation = @operation, Date = @date, CryptoName = @name, " +
                "CryptoQuantity = @quantity, Wallet = @wallet, LastPrice = @price, Sum = @sum, Fee = @fee WHERE Id = @id";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@operation", operation);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@wallet", wallet);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@sum", sum);
            command.Parameters.AddWithValue("@fee", fee);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void InsertListDate(DateTime date)
        {
            sql.Open();
            string querry = "INSERT INTO LastTimeUpdatedList(Date) VALUES (@Date)";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@Date", date);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void InsertCurrentAssets(string name, bool customCoin, double quantity, DateTime date, double price, double currentValue)
        {
            sql.Open();
            string querry = "INSERT INTO CurrentAssets(Cryptocurrency, CustomCoin, Quantity, PriceUpdateTime, Price, CurrentValue) VALUES (@Cryptocurrency, @CustomCoin, @Quantity, @PriceUpdateTime, @Price, @CurrentValue)";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@Cryptocurrency", name);
            command.Parameters.AddWithValue("@CustomCoin", customCoin);
            command.Parameters.AddWithValue("@Quantity", double.Parse(quantity.ToString("0.00000000")));
            command.Parameters.AddWithValue("@PriceUpdateTime", date);
            command.Parameters.AddWithValue("@Price", double.Parse(price.ToString("0.0000")));
            command.Parameters.AddWithValue("@CurrentValue", double.Parse(currentValue.ToString("0.0000")));
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void UpdateCurrentAssets(bool updateQuantity, string name, double quantity, DateTime date, double price, double currentValue)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            sql.Open();
            if (updateQuantity)
            {
                if (quantity == 0)
                {
                    string querryDelete = "DELETE FROM CurrentAssets WHERE Cryptocurrency = '" + name + "'";
                    SqlCommand commandDelete = new SqlCommand(querryDelete, sql);
                    commandDelete.Parameters.AddWithValue("@Cryptocurrency", name);
                    commandDelete.ExecuteNonQuery();
                }
                else
                {
                    string querryUpdateAll = "UPDATE CurrentAssets SET PriceUpdateTime = @PriceUpdateTime, Quantity = @Quantity, Price = @Price, CurrentValue = @CurrentValue WHERE Cryptocurrency = @Cryptocurrency";
                    SqlCommand commandUpdateAll = new SqlCommand(querryUpdateAll, sql);
                    commandUpdateAll.Parameters.AddWithValue("@Cryptocurrency", name);
                    commandUpdateAll.Parameters.AddWithValue("@PriceUpdateTime", date);
                    commandUpdateAll.Parameters.AddWithValue("@Quantity", quantity);
                    commandUpdateAll.Parameters.AddWithValue("@Price", price);
                    commandUpdateAll.Parameters.AddWithValue("@CurrentValue", price);
                    commandUpdateAll.ExecuteNonQuery();
                }
            }
            else
            {
                string querryUpdateOther = "UPDATE CurrentAssets SET PriceUpdateTime = @PriceUpdateTime, Price = @Price, CurrentValue = @CurrentValue WHERE Cryptocurrency = @Cryptocurrency";
                SqlCommand commandUpdateOther = new SqlCommand(querryUpdateOther, sql);
                commandUpdateOther.Parameters.AddWithValue("@Cryptocurrency", name);
                commandUpdateOther.Parameters.AddWithValue("@PriceUpdateTime", date);
                commandUpdateOther.Parameters.AddWithValue("@Price", price);
                commandUpdateOther.Parameters.AddWithValue("@CurrentValue", price);
                commandUpdateOther.ExecuteNonQuery();
            }
            sql.Close();
        }

        public void UpdatePrice(DateTime date, string name, double quantity, string operation, double sum, double price, double currentValue)
        {
            var findDate = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == name && x.Date == date).ToList();

            sql.Open();
            if(findDate.Count > 0)
            {
                string querry = "UPDATE CryptoTable SET LastPrice = " + double.Parse(price.ToString("0.0000")) + " WHERE CryptoName = '" + name + "' AND Date = '"+ date +"'";
                SqlCommand command = new SqlCommand(querry, sql);
                command.ExecuteNonQuery();
            }
            else
            {
                string querry = "INSERT INTO CryptoTable(Date, CryptoName, CryptoQuantity, Operation, Sum, Fee, LastPrice, LastCurrentValue) VALUES (@Date, @CryptoName, @CryptoQuantity, @Operation, @Sum, @Fee, @LastPrice, @LastCurrentValue)";
                SqlCommand command = new SqlCommand(querry, sql);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@CryptoName", name);
                command.Parameters.AddWithValue("@CryptoQuantity", double.Parse(quantity.ToString("0.00000000")));
                command.Parameters.AddWithValue("@Operation", operation);
                command.Parameters.AddWithValue("@Sum", sum);
                command.Parameters.AddWithValue("@Fee", 0);
                command.Parameters.AddWithValue("@LastPrice", double.Parse(price.ToString("0.0000")));
                command.Parameters.AddWithValue("@LastCurrentValue", currentValue);
                command.ExecuteNonQuery();
            }
            sql.Close();
        }

    }
}
