﻿using System.Drawing;

namespace cryptoFinance
{
    public class CoingeckoListInfo
    {
        public Image logo { get; set; }
        public string cryptoId { get; set; }
        public string cryptoSymbol { get; set; }
        public string cryptoName { get; set; }
        public double marketCap { get; set; }

        public CoingeckoListInfo(Image _logo, string _cryptoId, string _cryptoSymbol, string _cryptoName, double _marketCap)
        {
            logo = _logo;
            cryptoId = _cryptoId;
            cryptoSymbol = _cryptoSymbol;
            cryptoName = _cryptoName;
            marketCap = _marketCap;
        }

        public CoingeckoListInfo(string _cryptoId, double _marketCap)
        {
            cryptoId = _cryptoId;
            marketCap = _marketCap;
        }
    }

}
