using System.Drawing;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class SearchBoxSettings
    {
        public static void OnLeavingFocus(TextBox textBox)
        {
            textBox.Text = "paieška";
            textBox.ForeColor = Color.Silver;
            textBox.Font = new Font(textBox.Font, FontStyle.Italic);
        }

        public static void OnFocus(TextBox textBox)
        {
            textBox.Text = "";
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font(textBox.Font, FontStyle.Regular);
        }

        public static void ShowRegular(TextBox textBox)
        {
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font(textBox.Font, FontStyle.Regular);
        }

    }
}
