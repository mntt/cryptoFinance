using System.Drawing;
using System.Windows.Forms;

namespace cryptoFinance
{
    public class AdvancedComboBox : System.Windows.Forms.ComboBox
    {
        new public System.Windows.Forms.DrawMode DrawMode { get; set; }
        public Color HighlightColor { get; set; }

        public AdvancedComboBox()
        {
            base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.HighlightColor = Colours.SelectedItemColor();
            this.DrawItem += new DrawItemEventHandler(AdvancedComboBox_DrawItem);
        }

        private void AdvancedComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            System.Windows.Forms.ComboBox combo = sender as System.Windows.Forms.ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(new SolidBrush(HighlightColor),
                                         e.Bounds);
            else
                e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                         e.Bounds);

            e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                  new SolidBrush(combo.ForeColor),
                                  new Point(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

    }
}
