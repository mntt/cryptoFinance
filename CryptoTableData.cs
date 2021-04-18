using System;

namespace cryptoFinance
{
    public class CryptoTableData
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public double quantity { get; set; }
        public string operation { get; set; }
        public string wallet { get; set; }
        public double price { get; set; }
        public double fee { get; set; }
        public double sum { get; set; }

        public CryptoTableData(int _id, DateTime _date, string _operation, string _name, double _quantity, string _wallet, double _price, double _fee, double _sum)
        {
            id = _id;   
            date = _date;
            operation = _operation;
            name = _name;
            quantity = _quantity;
            wallet = _wallet;
            price = _price;
            fee = _fee;
            sum = _sum;
        }
    }
}
