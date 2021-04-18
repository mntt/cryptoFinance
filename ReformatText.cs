using System.Windows.Forms;

namespace cryptoFinance
{
    public static class ReformatText
    {
        public static double ReturnNumeric(string text)
        {
            double numeric = 0;

            try
            {
                GetCultureInfo gci = new GetCultureInfo(",");
                numeric = double.Parse(text.Trim('€'));
            }
            catch
            {
                MessageBox.Show("Klaida konvertuojant " + text + " tekstą į skaičių.");
            }
            
            return numeric;
        }
    }
}
