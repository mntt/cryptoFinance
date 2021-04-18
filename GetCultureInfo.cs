namespace cryptoFinance
{
    public class GetCultureInfo
    {
        public GetCultureInfo(string decimalSeparator)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = decimalSeparator;
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
    }
}
