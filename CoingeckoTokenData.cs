namespace cryptoFinance
{
    public class CoingeckoTokenData
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }


        public CoingeckoTokenData(string _id, string _symbol, string _name)
        {
            this.id = _id;
            this.symbol = _symbol;
            this.name = _name;
        }
    }
}
