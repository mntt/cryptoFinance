using System.Drawing;

namespace cryptoFinance
{
    public static class Colours
    {
        public static Color labelColor = Color.FromArgb(255, 255, 255);
        public static Color panelBackground = Color.FromArgb(29, 33, 41);
        public static Color formBackground = Color.FromArgb(59, 68, 85);
        public static Color cellBack = Color.FromArgb(80, 91, 114);
        public static Color grid = Color.FromArgb(116, 201, 204);
        public static Color alternateCellBack = Color.FromArgb(99, 114, 141);
        public static Color dataGridColumnHeaderBack = Color.FromArgb(29, 33, 41);
        public static Color buttonBorder = Color.FromArgb(59, 68, 85);
        public static Color buttonMouseOverBack = Color.FromArgb(54, 62, 78);
        public static System.Windows.Media.Color chartGrid = System.Windows.Media.Color.FromRgb(255, 255, 255);
        public static System.Windows.Media.Color chartLabels = System.Windows.Media.Color.FromRgb(255, 255, 255);
        public static System.Windows.Media.Color tooltipLabels = System.Windows.Media.Color.FromRgb(59, 68, 85);

        public static Color Transparent()
        {
            return Color.Transparent;
        }

        public static Color SelectedItemColor()
        {
            return Color.Aquamarine;
        }

    }
}

