using System.Drawing;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class Design
    {
        public static void SetFormDesign(Form form)
        {
            CenterLoadingBox(form);
            DataGridStyling(form);
            PanelStyling(form);
            LabelStyling(form);
            ButtonStyling(form);
            PieChartStyling(form);
            form.Name = "";
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new System.Drawing.Size(944, 465);
            form.BackColor = Colours.formBackground;

            //form.Icon = new System.Drawing.Icon("Resources/icon.ico");
        }

        private static void CenterLoadingBox(Form form)
        {
            foreach (var item in form.Controls)
            {
                if (item.GetType() == typeof(PictureBox))
                {
                    PictureBox loadingBox = (PictureBox)item;
                    if (loadingBox.Name == "loadingBox")
                    {
                        loadingBox.Location = new System.Drawing.Point((form.ClientSize.Width - loadingBox.Width) / 2, (form.ClientSize.Height - loadingBox.Height) / 2);
                        break;
                    }
                }
            }
        }

        private static void DataGridStyling(Form form)
        {
            foreach (var item in form.Controls)
            {
                if (item.GetType() == typeof(DataGridView))
                {
                    DataGridView datagrid = (DataGridView)item;

                    if(datagrid.Name == "dataGridCurrentAssets")
                    {
                        CurrentAssetsDataGrid(datagrid);
                    }

                    datagrid.BackgroundColor = Colours.formBackground;
                    datagrid.BorderStyle = BorderStyle.None;
                    datagrid.DefaultCellStyle.BackColor = Colours.cellBack;
                    //datagrid.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                    //datagrid.DefaultCellStyle.SelectionForeColor = Color.Transparent;
                    datagrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    datagrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    datagrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    datagrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    datagrid.GridColor = Colours.grid;
                    datagrid.AlternatingRowsDefaultCellStyle.BackColor = Colours.alternateCellBack;
                    datagrid.Font = new Font("Arial Black", 8);
                    datagrid.ForeColor = Colours.labelColor;
                    datagrid.ColumnHeadersDefaultCellStyle.BackColor = Colours.dataGridColumnHeaderBack;
                    datagrid.ColumnHeadersDefaultCellStyle.ForeColor = Colours.labelColor;
                    datagrid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial Black", 9);
                    datagrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                }
            }
        }

        public static void CurrentAssetsDataGrid(DataGridView dataGrid)
        {
            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGrid.Columns[0].Width = 310;
            dataGrid.Columns[1].Width = 160;
            dataGrid.Columns[2].Width = 160;
            dataGrid.Columns[3].Width = 160;
            dataGrid.Columns[1].DefaultCellStyle.Format = "0.00000";
            dataGrid.Columns[2].DefaultCellStyle.Format = "C2";
            dataGrid.Columns[3].DefaultCellStyle.Format = "C2";
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.ReadOnly = true;
            dataGrid.RowHeadersVisible = false;
            dataGrid.ScrollBars = ScrollBars.Vertical;
        }

        private static void PanelStyling(Form form)
        {
            foreach (var formControl in form.Controls)
            {
                if (formControl.GetType() == typeof(Panel))
                {
                    Panel panel = (Panel)formControl;
                    panel.BackColor = Colours.panelBackground;

                    foreach(var panelControl in panel.Controls)
                    {
                        if(panelControl.GetType() == typeof(Button))
                        {
                            Button button = (Button)panelControl;
                            ButtonStyling(button);
                        }

                        if(panelControl.GetType() == typeof(Label))
                        {
                            Label label = (Label)panelControl;
                            LabelStyling(label);
                        }
                    }
                }
            }
        }

        private static void LabelStyling(Form form)
        {
            foreach (var item in form.Controls)
            {
                if (item.GetType() == typeof(Label))
                {
                    Label label = (Label)item;
                    label.ForeColor = Colours.labelColor;
                    label.Font = new Font("Arial Black", 15, FontStyle.Bold);
                }
            }
        }

        private static void LabelStyling(Label label)
        {
            label.ForeColor = Colours.labelColor;
            label.Font = new Font("Arial Black", 15, FontStyle.Bold);
        }

        private static void ButtonStyling(Form form)
        {
            foreach (var item in form.Controls)
            {
                if (item.GetType() == typeof(Button))
                {
                    Button button = (Button)item;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Colours.buttonBorder;
                    button.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;
                }
            }
        }

        private static void ButtonStyling(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Colours.buttonBorder;
            button.FlatAppearance.MouseOverBackColor = Colours.buttonMouseOverBack;
        }

        private static void PieChartStyling(Form form)
        {
            foreach(var item in form.Controls)
            {
                if(item.GetType() == typeof(LiveCharts.WinForms.PieChart))
                {
                    LiveCharts.WinForms.PieChart pieChart = (LiveCharts.WinForms.PieChart)item;
                    pieChart.BackColor = Colours.formBackground;
                    pieChart.ForeColor = Colours.labelColor;
                }
            } 
        }

    }
}
