using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class FormValidation
    {
        public static bool ReturnValidation(Control.ControlCollection controls)
        {
            List<Control> controlList = new List<Control>();

            foreach(var item in controls)
            {
                controlList.Add((Control)item);
            }

            bool validation = CheckForEmptyBoxes(controlList);
            return validation; 
        }

        public static bool ReturnValidation(List<Control> controlList)
        {
            bool validation = CheckForEmptyBoxes(controlList);
            return validation;
        }

        private static bool CheckZeroQuantity(string quantity)
        {
            bool validation = true;

            if(decimal.Parse(quantity) <= 0)
            {
                validation = false;
            }

            return validation;
        }

        public static bool ReturnValidation(List<Control> controlList, string maxQuantity, string quantity)
        {
            bool validation = CheckForEmptyBoxes(controlList) && CompareMaxQuantityToQuantity(maxQuantity, quantity) && CheckZeroQuantity(quantity);
            return validation;
        }

        public static bool ReturnValidation(Control.ControlCollection controls, string maxQuantity, string quantity)
        {
            List<Control> controlList = new List<Control>();

            foreach (var item in controls)
            {
                controlList.Add((Control)item);
            }

            bool validation = CheckForEmptyBoxes(controlList) && CompareMaxQuantityToQuantity(maxQuantity, quantity) && CheckZeroQuantity(quantity);
            return validation;
        }

        private static bool CheckForEmptyBoxes(List<Control> controlList)
        {
            bool validation = true;

            foreach (var item in controlList)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox thebox = (TextBox)item;
                    if (thebox.Text == "" && thebox.Visible)
                    {
                        validation = false;
                        break;
                    }

                    if(thebox.Name == "quantityBox" && decimal.Parse(thebox.Text) <= 0)
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
                if (decimal.Parse(maxQuantity) < decimal.Parse(quantity))
                {
                    validation = false;
                }
            }
            catch
            {
                validation = false;
            }

            return validation;
        }

    }


}
