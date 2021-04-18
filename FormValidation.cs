using System;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class FormValidation
    {
        public static bool ReturnValidation(Control.ControlCollection controls)
        {
            bool validation = CheckForEmptyBoxes(controls);
            return validation;
        }

        public static bool ReturnValidation(Control.ControlCollection controls, string maxQuantity, string quantity)
        {
            bool validation = CheckForEmptyBoxes(controls) && CompareMaxQuantityToQuantity(maxQuantity, quantity);
            return validation;
        }

        private static bool CheckForEmptyBoxes(Control.ControlCollection controls)
        {
            bool validation = true;

            foreach (var item in controls)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox thebox = (TextBox)item;
                    if (thebox.Text == "" && thebox.Visible)
                    {
                        validation = false;
                        break;
                    }
                }

                if (item.GetType() == typeof(ComboBox))
                {
                    ComboBox thebox = (ComboBox)item;
                    if (thebox.Text == "" && thebox.Visible)
                    {
                        validation = false;
                        break;
                    }
                }
            }

            return validation;
        }

        private static bool CompareMaxQuantityToQuantity(string maxQuantity, string quantity)
        {
            bool validation = true;

            try
            {
                if (double.Parse(maxQuantity) < double.Parse(quantity))
                {
                    validation = false;
                }
            }
            catch
            {
                validation = false;
            }

            MessageBox.Show("compaarison: " + validation + "");
            return validation;
        }

    }


}
