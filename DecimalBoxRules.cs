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

            if (quantityBox.Text == "0" && ((e.KeyChar != (char)Keys.Back) && e.KeyChar != ','))
            {
                e.Handled = true;
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

        public static string FormatLabel(string label)
        {
            string fixedstring = label;

            if (fixedstring != "")
            {
                var comma = fixedstring.Where(x => x == ',').ToList();

                if (comma.Count == 1)
                {
                    var afterComma = fixedstring.Split(',');

                    if (afterComma[1].Length <= 4)
                    {
                        decimal q = decimal.Parse(fixedstring);
                        fixedstring = string.Format("{0:F4}", q);
                    }
                    else if (afterComma[1].Length > 4)
                    {
                        decimal q = decimal.Parse(fixedstring);
                        fixedstring = string.Format("{0:F8}", q);
                    }
                }
                else
                {
                    decimal q = decimal.Parse(fixedstring);
                    fixedstring = string.Format("{0:n}", q);
                }
            }

            return fixedstring;
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
                        decimal q = decimal.Parse(quantityBox.Text);
                        quantityBox.Text = string.Format("{0:F4}", q);
                    }
                    else if (afterComma[1].Length > 4)
                    {
                        decimal q = decimal.Parse(quantityBox.Text);
                        quantityBox.Text = string.Format("{0:F8}", q);
                    }
                }
                else
                {
                    decimal q = decimal.Parse(quantityBox.Text);
                    quantityBox.Text = string.Format("{0:n}", q);
                }
            }
        }

        public static void CurrencyBox(TextBox currencyBox, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Back) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (currencyBox.Text == "0" && ((e.KeyChar != (char)Keys.Back) && e.KeyChar != ','))
            {
                e.Handled = true;
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
            decimal decimalValue;
            string text = currencyBox.Text.Trim('€').Trim(' ');

            if (currencyBox.Name == "priceBox")
            {
                var comma = text.Where(x => x == ',').ToList();

                if (comma.Count == 1)
                {
                    var afterComma = text.Split(',');

                    if (afterComma[1].Length <= 4)
                    {
                        decimal c = decimal.Parse(text);
                        currencyBox.Text = string.Format("{0:C4}", c);
                    }
                    else if (afterComma[1].Length > 4)
                    {
                        decimal c = decimal.Parse(text);
                        currencyBox.Text = string.Format("{0:C8}", c);
                    }
                }
                else
                {
                    if (decimal.TryParse(text, out decimalValue))
                    {
                        currencyBox.Text = string.Format("{0:C2}", decimalValue);
                    }
                }
            }
            else
            {
                if (decimal.TryParse(text, out decimalValue))
                {
                    currencyBox.Text = string.Format("{0:C2}", decimalValue);
                }
            }
            
        }
    }
}
