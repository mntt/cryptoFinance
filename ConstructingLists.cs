using System;

namespace cryptoFinance
{
    public class ConstructingLists
    {
        public DateTime date { get; set; }
        public string name { get; set; }
        public bool customCoin { get; set; }
        public string id { get; set; }
        public double quantity { get; set; }
        public string wallet { get; set; }
        public double price { get; set; }
        public double totalSum { get; set; }

        public ConstructingLists(string _name, double _quantity)
        {
            name = _name;
            quantity = _quantity;
        }

        public ConstructingLists(DateTime _date, string _name, double _quantity)
        {
            date = _date;
            name = _name;
            quantity = _quantity;
        }

        public ConstructingLists(string _name, double _quantity, string _wallet)
        {
            name = _name;
            quantity = _quantity;
            wallet = _wallet;
        }

        public ConstructingLists(string _name, double _quantity, string _wallet, double _price, double _totalSum)
        {
            name = _name;
            quantity = _quantity;
            wallet = _wallet;
            price = _price;
            totalSum = _totalSum;
        }

        public ConstructingLists(DateTime _date, string _name, bool _customCoin, string _id, double _quantity, double _price, double _totalSum)
        {
            date = _date;
            name = _name;
            customCoin = _customCoin;
            id = _id;
            quantity = _quantity;
            price = _price;
            totalSum = _totalSum;
        }
    }
}
