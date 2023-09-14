using System.Linq;
using System.Windows.Documents;

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

            if(split.Length > 2)                    //if we send (REMOVED) coin here, length will be more than 2
            {
                return "removed";
            }
            else
            {
                return fixedName;
            }
        }

        public static string GetSymbol(string name)
        {
            var split = name.Split('(');
            var cryptoSymbol = split[1].Trim(')');

            if (split.Length > 2)                    //if we send (REMOVED) coin here, length will be more than 2
            {
                return "removed";
            }
            else
            {
                return cryptoSymbol;
            }
        }

    }
}
