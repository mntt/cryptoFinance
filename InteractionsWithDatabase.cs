﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class InteractionsWithDatabase
    {
        public SqlConnection sql = new SqlConnection(Connection.connectionString);

        public void TruncateTable(string table)
        {
            sql.Open();
            string querry = "TRUNCATE table " + table + "";
            SqlCommand command = new SqlCommand(querry, sql);
            command.ExecuteNonQuery();
            sql.Close();
        }

        private byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public void InsertCryptoLogo(string cryptoId, Image logo)
        {
            sql.Open();
            string querry = "UPDATE CoingeckoCryptoList SET Logo = @logo WHERE CryptoId = @cryptoId";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@cryptoId", cryptoId);           

            if (logo == null)
            {
                Byte[] imgtype = { 0 };
                command.Parameters.AddWithValue("@logo", logo).Value = imgtype;
            }
            else
            {
                Byte[] bimage = ImageToByte(logo);
                command.Parameters.AddWithValue("@logo", logo).Value = bimage;
            }
  
            command.ExecuteNonQuery();
            sql.Close();
        }

        public Bitmap GetLogo(string cryptoName, string cryptoSymbol)
        {
            sql.Open();
            MemoryStream stream = new MemoryStream();
            string querry = "SELECT Logo FROM CoingeckoCryptoList WHERE CryptoName = '"+cryptoName+"' AND CryptoSymbol = '"+cryptoSymbol+"'";
            SqlCommand command = new SqlCommand(querry, sql);
            byte[] image = (byte[])command.ExecuteScalar();

            sql.Close();

            try
            {
                //if coin logo is found

                if (image.Length == 1)
                {
                    return null;
                }
                else
                {
                    stream.Write(image, 0, image.Length);
                    Bitmap bitmap = new Bitmap(stream);
                    return bitmap;
                }
            }
            catch
            {
                //logo not found
                return null;
            }
        }

        public void InsertCoinGeckoCryptoList(Image logo, string cryptoId, string cryptoSymbol, string cryptoName, double marketCap)
        {
            sql.Open();
            string querry = "INSERT INTO CoingeckoCryptoList(Logo, CryptoId, CryptoSymbol, CryptoName, MarketCap) VALUES (@Logo, @CryptoId, @CryptoSymbol, @CryptoName, @MarketCap)";
            SqlCommand command = new SqlCommand(querry, sql);

            if (logo == null)
            {
                Byte[] imgtype = { 0 };
                command.Parameters.AddWithValue("@Logo", logo).Value = imgtype;
            }
            else
            {
                Byte[] bimage = ImageToByte(logo);
                command.Parameters.AddWithValue("@Logo", logo).Value = bimage;
            }
            
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

                decimal mcap;
                bool isNumeric = decimal.TryParse(marketCap, out mcap);

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

        public void DeleteByOperationID(int id)
        {
            sql.Open();
            string querry = "DELETE FROM CryptoTable WHERE OperationID = @operationID AND Operation <> 'BUY' AND Operation <> 'SELL'";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@operationID", id);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void DeleteByID(int id)
        {
            sql.Open();
            string querry = "DELETE FROM CryptoTable WHERE Id = @id";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void InsertCryptoTable(int operationid, DateTime date, string name, bool customCoin, decimal quantity, string operation, string wallet, decimal sum, decimal price, decimal fee, decimal currentValue)
        {
            sql.Open();
            string querry = "INSERT INTO CryptoTable(OperationID, Date, CryptoName, CustomCoin, CryptoQuantity, Operation, Wallet, Sum, Fee, LastPrice, LastCurrentValue) VALUES (@OperationID, @Date, @CryptoName, @CustomCoin, @CryptoQuantity, @Operation, @Wallet, @Sum, @Fee, @LastPrice, @LastCurrentValue)";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@OperationID", operationid);
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@CryptoName", name);
            command.Parameters.AddWithValue("@CustomCoin", customCoin);
            command.Parameters.AddWithValue("@CryptoQuantity", decimal.Parse(quantity.ToString()));
            command.Parameters.AddWithValue("@Operation", operation);
            command.Parameters.AddWithValue("@Wallet", wallet);
            command.Parameters.AddWithValue("@Sum", decimal.Parse(sum.ToString()));
            command.Parameters.AddWithValue("@LastPrice", decimal.Parse(price.ToString()));
            command.Parameters.AddWithValue("@Fee", decimal.Parse(fee.ToString()));
            command.Parameters.AddWithValue("@LastCurrentValue", currentValue);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void UpdateCryptoTable(int id, int operationid, string operation, string name, DateTime date, decimal quantity, string wallet, decimal price, decimal fee, decimal sum)
        {
            GetCultureInfo gci = new GetCultureInfo(".");

            sql.Open();
            string querry = "UPDATE CryptoTable SET OperationID = @operationid, Operation = @operation, Date = @date, CryptoName = @name, " +
                "CryptoQuantity = @quantity, Wallet = @wallet, LastPrice = @price, Sum = @sum, Fee = @fee WHERE Id = @id";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@operationid", operationid);
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

        public void InsertCurrentAssets(string name, bool customCoin, decimal quantity, DateTime date, decimal price, decimal currentValue)
        {
            sql.Open();
            string querry = "INSERT INTO CurrentAssets(Cryptocurrency, CustomCoin, Quantity, PriceUpdateTime, Price, CurrentValue) VALUES (@Cryptocurrency, @CustomCoin, @Quantity, @PriceUpdateTime, @Price, @CurrentValue)";
            SqlCommand command = new SqlCommand(querry, sql);
            command.Parameters.AddWithValue("@Cryptocurrency", name);
            command.Parameters.AddWithValue("@CustomCoin", customCoin);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@PriceUpdateTime", date);
            command.Parameters.AddWithValue("@Price", price);
            command.Parameters.AddWithValue("@CurrentValue", currentValue);
            command.ExecuteNonQuery();
            sql.Close();
        }

        public void UpdateCurrentAssets(bool updateQuantity, string name, decimal quantity, DateTime date, decimal price, decimal currentValue)
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
                    commandUpdateAll.Parameters.AddWithValue("@CurrentValue", currentValue);
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
                commandUpdateOther.Parameters.AddWithValue("@CurrentValue", currentValue);
                commandUpdateOther.ExecuteNonQuery();
            }
            sql.Close();
        }

        public void UpdatePrice(DateTime date, string name, bool customCoin, decimal quantity, string operation, decimal sum, decimal price, decimal currentValue)
        {
            var findDate = Connection.db.GetTable<CryptoTable>().Where(x => x.CryptoName == name && x.Date == date).ToList();

            sql.Open();
            if(findDate.Count > 0)
            {
                string querry = "UPDATE CryptoTable SET LastPrice = " + decimal.Parse(price.ToString()) + " WHERE CryptoName = '" + name + "' AND Date = '"+ date +"'";
                SqlCommand command = new SqlCommand(querry, sql);
                command.ExecuteNonQuery();
            }
            else
            {
                string querry = "INSERT INTO CryptoTable(OperationID, Date, CryptoName, CustomCoin, CryptoQuantity, Operation, Sum, Fee, LastPrice, LastCurrentValue) VALUES (@OperationID, @Date, @CryptoName, @CustomCoin, @CryptoQuantity, @Operation, @Sum, @Fee, @LastPrice, @LastCurrentValue)";
                SqlCommand command = new SqlCommand(querry, sql);
                command.Parameters.AddWithValue("@OperationID", 0);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@CryptoName", name);
                command.Parameters.AddWithValue("@CustomCoin", customCoin);
                command.Parameters.AddWithValue("@CryptoQuantity", decimal.Parse(quantity.ToString()));
                command.Parameters.AddWithValue("@Operation", operation);
                command.Parameters.AddWithValue("@Sum", sum);
                command.Parameters.AddWithValue("@Fee", 0);
                command.Parameters.AddWithValue("@LastPrice", decimal.Parse(price.ToString()));
                command.Parameters.AddWithValue("@LastCurrentValue", currentValue);
                command.ExecuteNonQuery();
            }
            sql.Close();
        }

    }
}
