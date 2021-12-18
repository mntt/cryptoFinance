using System;

namespace cryptoFinance
{
    public class CryptoTableData
    {
        public int id { get; set; }
        public int operationID { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public decimal quantity { get; set; }
        public string operation { get; set; }
        public string wallet { get; set; }
        public decimal price { get; set; }
        public decimal fee { get; set; }
        public decimal sum { get; set; }

        public CryptoTableData(int _id, int _operationID, DateTime _date, string _operation, string _name, decimal _quantity, string _wallet, decimal _price, decimal _fee, decimal _sum)
        {
            id = _id;
            operationID = _operationID;
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
