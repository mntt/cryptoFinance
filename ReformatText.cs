using System.Windows.Forms;

namespace cryptoFinance
{
    public static class ReformatText
    {
        public static decimal ReturnNumeric(string text)
        {
            decimal numeric = 0;

            try
            {
                GetCultureInfo gci = new GetCultureInfo(",");
                numeric = decimal.Parse(text.Trim('€'));
            }
            catch
            {
                numeric = 0;
            }
            
            return numeric;
        }
    }
}
