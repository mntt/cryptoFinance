using System;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class ListViewSettings
    {
        private static Timer listViewTimer = new Timer();
        private static int listViewLastIndex { get; set; }
        private static int listViewCurrentIndex { get; set; }
        private static ListView theListView { get; set; }

        public static void WhenDownButtonPressed(ListView listView, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && listView.Focused)
            {
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Down && listView.Visible && !listView.Focused)
            {
                bool selectedItemFound = false;
                int index = -1;

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].BackColor == Colours.SelectedItemColor())
                    {
                        selectedItemFound = true;
                        index = i;
                        break;
                    }
                }

                if (selectedItemFound && index >= 0 && index + 1 < listView.Items.Count)
                {
                    listView.Items[index].BackColor = Colours.Transparent();
                    listView.Items[index + 1].BackColor = Colours.SelectedItemColor();
                    listView.EnsureVisible(index + 1);
                }
                else if (!selectedItemFound)
                {
                    listView.Items[0].BackColor = Colours.SelectedItemColor();
                }
            }
        }

        public static void WhenUpButtonPressed(ListView listView, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                bool selectedItemFound = false;
                int index = -1;

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].BackColor == Colours.SelectedItemColor())
                    {
                        selectedItemFound = true;
                        index = i;
                        break;
                    }
                }

                if (selectedItemFound && index - 1 >= 0 && index < listView.Items.Count)
                {
                    listView.Items[index].BackColor = Colours.Transparent();
                    listView.Items[index - 1].BackColor = Colours.SelectedItemColor();
                    listView.EnsureVisible(index - 1);
                }
            }
        }

        public static void OnEnterButtonPressedWhenInvesting(AddOperation form, ListView listView, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool selectedItemFound = false;
                int index = -1;

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].BackColor == Colours.SelectedItemColor())
                    {
                        listView.Items[i].BackColor = Colours.Transparent();
                        selectedItemFound = true;
                        index = i;
                        break;
                    }
                }

                if (selectedItemFound)
                {
                    form.AssignNameToTextBox(FixLabelName.ReturnName(listView.Items[index].Text));
                }
            }
        }

        public static void WhenEnterButtonPressed(ListView listView, KeyEventArgs e, TextBox boxToFill, Control controlToSelect)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool selectedItemFound = false;
                int index = -1;

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].BackColor == Colours.SelectedItemColor())
                    {
                        selectedItemFound = true;
                        index = i;
                        break;
                    }
                }

                if (selectedItemFound)
                {
                    boxToFill.Text = listView.Items[index].Text;
                    controlToSelect.Select();
                    listView.Visible = false;
                }
            }
        }

        public static void WhenItIsNotFocused(ListView listView, KeyEventArgs e)
        {
            if (listView.Focused)
            {
                e.Handled = true;
            }
        }

        public static void WhenMouseMoves(ListView _listView, int _lastIndex, MouseEventArgs e)
        {
            ListViewItem lvi = _listView.GetItemAt(e.X, e.Y);

            if (lvi != null && _lastIndex == -1)
            {
                _listView.Items[lvi.Index].BackColor = Colours.SelectedItemColor();
                listViewLastIndex = lvi.Index;
            }

            if (lvi != null && _lastIndex != -1)
            {
                theListView = _listView;
                listViewCurrentIndex = lvi.Index;
                SetTimerSettings();
                listViewTimer.Start();
            }
        }

        private static void SetTimerSettings()
        {
            listViewTimer.Tick += new EventHandler(ListViewTimer_Tick);
            listViewTimer.Interval = 40;
        }

        private static void ListViewTimer_Tick(object sender, EventArgs e)
        {
            theListView.BeginUpdate();
            try
            {
                theListView.Items[listViewLastIndex].BackColor = Colours.Transparent();
                theListView.Items[listViewCurrentIndex].BackColor = Colours.SelectedItemColor();
                listViewLastIndex = listViewCurrentIndex;
            }
            catch
            {
                //do nothing
            }
            finally
            {
                listViewTimer.Stop();
            }
            theListView.EndUpdate();
        }

        public static int ReturnLastIndex()
        {
            return listViewLastIndex;
        }

        public static void WhenMouseIsOutOfBounds(ListView listView)
        {
            if (Cursor.Clip != listView.Bounds)
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].BackColor == Colours.SelectedItemColor())
                    {
                        listView.Items[i].BackColor = Colours.Transparent();
                    }
                }
            }
        }

        public static void Format(ListView listView)
        {
            ColumnHeader header = new ColumnHeader(); 
            header.Text = "";
            header.Name = "col1";
            listView.Columns.Add(header);
            listView.FullRowSelect = true;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.MultiSelect = false;
            listView.HideSelection = true;
            listView.View = View.Details;
        }

        public static void SetColumnWidth(ListView listView, int correction, int listViewHeight, int items)
        {
            //i 108 heighto telpa 7 itemai su defaultiniu srifto dydziu
            var maxItems = (listViewHeight * 7 / 108).ToString().Split('.');

            if (items > int.Parse(maxItems[0]))
            {
                listView.Columns[0].Width = listView.Width - correction - SystemInformation.VerticalScrollBarWidth;
            }
            else if(items <= int.Parse(maxItems[0]))
            {
                listView.Columns[0].Width = listView.Width - correction;
            }
        }

    }
}
