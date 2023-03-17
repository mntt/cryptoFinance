using System;

namespace cryptoFinance
{
    public class ConstructingLists
    {
        public int operationID { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public bool customCoin { get; set; }
        public string cryptoId { get; set; }
        public decimal quantity { get; set; }
        public string wallet { get; set; }
        public decimal price { get; set; }
        public decimal totalSum { get; set; }

        public ConstructingLists(string _name, decimal _price)
        {
            name = _name;
            price = _price;
        }

        public ConstructingLists(DateTime _date, string _name, decimal _quantity)
        {
            date = _date;
            name = _name;
            quantity = _quantity;
        }

        public ConstructingLists(int _operationID, string _cryptoId, string _name, bool _customCoin, decimal _quantity, string _wallet)
        {
            operationID = _operationID;
            cryptoId = _cryptoId;
            name = _name;
            customCoin = _customCoin;
            quantity = _quantity;
            wallet = _wallet;
        }

        public ConstructingLists(DateTime _date, string _name, bool _customCoin, string _cryptoId, decimal _quantity, decimal _price, decimal _totalSum)
        {
            date = _date;
            name = _name;
            customCoin = _customCoin;
            cryptoId = _cryptoId;
            quantity = _quantity;
            price = _price;
            totalSum = _totalSum;
        }

        public ConstructingLists(string _name, bool _customCoin)
        {
            name = _name;
            customCoin = _customCoin;
        }
    }
}
