using System.Linq;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class DecimalBoxRules
    {
        public static void QuantityBox(TextBox quantityBox, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Back) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (quantityBox.Text == "0")
            {
                if ((e.KeyChar != (char)Keys.Back) && e.KeyChar != ',')
                {
                    e.Handled = true;
                }
            }

            if (e.KeyChar == ',' && quantityBox.Text == "")
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && quantityBox.Text != "")
            {
                var chars = quantityBox.Text.ToCharArray();
                var commas = chars.Where(x => x == ',').ToArray();

                if (commas.Length > 0)
                {
                    e.Handled = true;
                }
            }           
        }

        public static void FormatQuantityBox(TextBox quantityBox)
        {
            if (quantityBox.Text != "")
            {
                var comma = quantityBox.Text.Where(x => x == ',').ToList();

                if (comma.Count == 1)
                {
                    var afterComma = quantityBox.Text.Split(',');

                    if (afterComma[1].Length <= 4)
                    {
                        double q = double.Parse(quantityBox.Text);
                        quantityBox.Text = string.Format("{0:F8}", q);
                    }
                }
                else
                {
                    double q = double.Parse(quantityBox.Text);
                    quantityBox.Text = string.Format("{0:F8}", q);
                }
            }
        }

        public static void CurrencyBox(TextBox currencyBox, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Back) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (currencyBox.Text == "0")
            {
                if ((e.KeyChar != (char)Keys.Back) && e.KeyChar != ',')
                {
                    e.Handled = true;
                }
            }

            if (e.KeyChar == ',' && currencyBox.Text == "")
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && currencyBox.Text != "")
            {
                var chars = currencyBox.Text.ToCharArray();
                var commas = chars.Where(x => x == ',').ToArray();

                if (commas.Length > 0)
                {
                    e.Handled = true;
                }
            }
        }

        public static void FormatCurrencyBox(TextBox currencyBox)
        {
            double doubleValue;
            string text = currencyBox.Text.Trim('€').Trim(' ');

            if (double.TryParse(text, out doubleValue))
            {
                currencyBox.Text = string.Format("{0:C2}", doubleValue);
            }
        }

    }
}
