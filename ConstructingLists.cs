using System;

namespace cryptoFinance
{
    public class ConstructingLists
    {
        public int operationID { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public bool customCoin { get; set; }
        public string id { get; set; }
        public decimal quantity { get; set; }
        public string wallet { get; set; }
        public decimal price { get; set; }
        public decimal totalSum { get; set; }

        public ConstructingLists(string _name, decimal _quantity)
        {
            name = _name;
            quantity = _quantity;
        }

        public ConstructingLists(DateTime _date, string _name, decimal _quantity)
        {
            date = _date;
            name = _name;
            quantity = _quantity;
        }

        public ConstructingLists(int _operationID, string _name, bool _customCoin, decimal _quantity, string _wallet)
        {
            operationID = _operationID;
            name = _name;
            customCoin = _customCoin;
            quantity = _quantity;
            wallet = _wallet;
        }

        public ConstructingLists(string _name, decimal _quantity, string _wallet, decimal _price, decimal _totalSum)
        {
            name = _name;
            quantity = _quantity;
            wallet = _wallet;
            price = _price;
            totalSum = _totalSum;
        }

        public ConstructingLists(DateTime _date, string _name, bool _customCoin, string _id, decimal _quantity, decimal _price, decimal _totalSum)
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
