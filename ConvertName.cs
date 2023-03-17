using System.Linq;

namespace cryptoFinance
{
    static public class ConvertName
    {
        static public string ToUpperId(string name)
        {
            var split = name.Split('(');
            var cryptoName = split[0].TrimEnd(' ');
            var cryptoSymbol = split[1].Trim(')').ToUpper();
            string id = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => x.CryptoName == cryptoName && x.CryptoSymbol == cryptoSymbol).Select(x => x.CryptoId).First();

            return id;
        }

        static public string ToLowerId(string name)
        {
            var split = name.Split('(');
            var cryptoName = split[0].TrimEnd(' ');
            var cryptoSymbol = split[1].Trim(')').ToLower();
            string id = Connection.db.GetTable<CoingeckoCryptoList>().Where(x => x.CryptoName == cryptoName && x.CryptoSymbol == cryptoSymbol).Select(x => x.CryptoId).First();

            return id;
        }

        static public string ToJustName(string name)
        {
            var split = name.Split('(');
            var cryptoName = split[0].TrimEnd(' ');

            return cryptoName;
        }

        static public string ToJustSymbol(string name)
        {
            var split = name.Split('(');
            var cryptoSymbol = split[1].Trim(')');

            return cryptoSymbol;
        }

    }
}
