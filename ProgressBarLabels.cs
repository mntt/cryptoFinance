using System.Windows.Forms;

namespace cryptoFinance
{
    public static class ProgressBarLabels
    {
        public static Label ReturnLabel(Label label, int labelCode)
        {
            label.SetBounds(0, 17, 29, 13);

            if (labelCode == 0)
            {
                label.Text = "Siunčiamas kriptovaliutų sąrašas...";
            }

            return label;
        }
    }
}
