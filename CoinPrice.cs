namespace cryptoFinance
{
    public class CoinPrice
    {
        public string name { get; set; }
        public decimal price { get; set; }

        public CoinPrice(string _name, decimal _price)
        {
            name = _name;
            price = _price;
        }
    }
}
