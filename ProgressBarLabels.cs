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

            if(labelCode == 1)
            {
                label.Text = "Ruošiami duomenys market cap duomenų atsisiuntimui...";
            }

            if(labelCode == 2)
            {
                label.Text = "Siunčiami kriptovaliutų market cap duomenys...";
            }

            if(labelCode == 3)
            {
                label.Text = "Market cap duomenys įrašomi į duomenų bazę...";
            }

            if (labelCode == 4)
            {
                label.Text = "Atliekami baigiamieji darbai...";
            }

            return label;
        }
    }
}
