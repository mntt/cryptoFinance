using System.Linq;

namespace cryptoFinance
{
    public static class ConvertName
    {
        public static string ToUpperId(string name)
        {
            var split = name.Split('(');
            var cryptoName = split[0].TrimEnd(' ');
            var cryptoSymbol = split[1].Trim(')').ToUpper();
            string id = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => x.CryptoName == cryptoName && x.CryptoSymbol == cryptoSymbol).Select(x => x.CryptoId).First();

            return id;
        }

        public static string ToLowerId(string name)
        {
            var split = name.Split('(');
            var cryptoName = split[0].TrimEnd(' ');
            var cryptoSymbol = split[1].Trim(')').ToLower();
            string id = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => x.CryptoName == cryptoName && x.CryptoSymbol == cryptoSymbol).Select(x => x.CryptoId).First();

            return id;
        }

        public static string GetName(string name)
        {
            var split = name.Split('(');
            var fixedName = split[0].TrimEnd(' ');

            return fixedName;
        }

        public static string GetSymbol(string name)
        {
            var split = name.Split('(');
            var cryptoSymbol = split[1].Trim(')');

            return cryptoSymbol;
        }

    }
}
