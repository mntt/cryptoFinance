using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cryptoFinance
{
    public static class Design
    {
        public static Font font5 = new Font("Arial Black", 5);
        public static Font font6 = new Font("Arial Black", 6);
        public static Font font7 = new Font("Arial Black", 7);
        public static Font font8 = new Font("Arial Black", 8);
        public static Font font9 = new Font("Arial Black", 9);
        public static Font font15 = new Font("Arial Black", 15);
        public static Font dateFont = new Font("Microsoft Sans Serif", 8);
        public static System.Windows.Media.FontFamily mediaFont = new System.Windows.Media.FontFamily("Arial Black");
        public static Point panelLocation = new Point(100, 105);
        public static Size panelSize = new Size(800, 313);
        public static Point submainPanelLocation = new Point(180, 32);
        public static Size submainPanelSize = new Size(434, 274);
        public static Size optionsPanelSize = new Size(174, 96);
        public static Point datagridLocation = new Point(100, 105);
        public static Size datagridSize = new Size(790, 310);
        public static Point secondarydatagridLocation = new Point(0, 40);
        public static Size secondarydatagridSize = new Size(790, 274);
        public static Point chartLocation = new Point(150, 105);
        public static Size chartSize = new Size(680, 261);
        public static Size buttonSize = new Size(123, 27);
        public static Size smallButtonSize = new Size(22, 21);
        public static Size statsButtonSize = new Size(223, 27);
        public static Size formSize = new Size(944, 465);
        public static Size alertPanelSize = new Size(276, 38);
        public static Point alertPanelLocation = new Point(652, 2);

        public static Point sorting1 = new Point(20, 20);
        public static Point sorting2 = new Point(40, 20);

        public static List<Control> allcontrols;

        public static void SetFormDesign(Form form)
        {
            form.Name = "";
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = Colours.formBackground;

            ListOfControls(form);
            ByFormIndex(form);
            ButtonStyling();
            DataGridStyling();
            PanelStyling();
            PieChartStyling();
            LabelStyling();
            ListViewStyling();
            DatePickerStyling();
            ComboBoxStyling();
            TextBoxStyling();
        }

        private static void ListOfControls(Form form)
        {
            allcontrols = new List<Control>();

            foreach(Control firstLayer in form.Controls)
            {
                allcontrols.Add(firstLayer);
                
                foreach(Control secondLayer in firstLayer.Controls)
                {
                    allcontrols.Add(secondLayer);

                    foreach(Control thirdLayer in secondLayer.Controls)
                    {
                        allcontrols.Add(thirdLayer);

                        foreach(Control fourthLayer in thirdLayer.Controls)
                        {
                            allcontrols.Add(fourthLayer);
                        }
                    }
                }
            }
        }

        private static void ByFormIndex(Form form)
        {
            if (form.Tag.ToString() == "1")
            {
                form.Size = formSize;
            }
        }

        private static void ButtonStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(Button))
                {
                    Buttons(item);
                }
            }
        }

        private static void Buttons(object item)
        {
            Button button = (Button)item;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.MouseOverBackColor = Colours.transparent;
            button.Font = font7;
            button.ForeColor = Colours.labelColor;

            if(button.Tag == "nav")
            {
                button.FlatAppearance.BorderColor = Colours.panelBackground;
                button.FlatAppearance.MouseOverBackColor = Colours.formBackground;
            }

            if(button.Tag == "other")
            {
                button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
                button.Size = buttonSize;
                button.Font = font7;
                button.ForeColor = Colours.buttonFore;
            }

            if(button.Tag == "main")
            {
                button.BackColor = Colours.buttonBack;
                button.ForeColor = Colours.buttonFore;
                button.Size = buttonSize;
                button.Font = font7;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
            }

            if(button.Tag == "options" )
            {
                button.BackColor = Colours.buttonBack;
                button.ForeColor = Colours.buttonFore;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
                button.FlatAppearance.MouseOverBackColor = Colours.formBackground;
                button.Font = font6; 
            }

            if(button.Tag == "small")
            {
                button.Size = smallButtonSize;
                button.BackColor = Colours.buttonBack;
                button.ForeColor = Colours.buttonFore;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
                button.FlatAppearance.MouseOverBackColor = Colours.formBackground;
                button.Font = font6;
            }

            if(button.Tag == "stats")
            {
                button.BackColor = Colours.buttonBack;
                button.ForeColor = Colours.buttonFore;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
                button.Font = font7;
                button.Size = statsButtonSize;
            }

            if(button.Tag == "refresh")
            {
                button.BackColor = Colours.transparent;
                button.FlatAppearance.BorderColor = Colours.formBackground;
                button.FlatAppearance.MouseOverBackColor = Colours.transparent;
                button.FlatAppearance.MouseDownBackColor = Colours.transparent;
            }

            if (button.Tag == "navrefresh")
            {
                button.BackColor = Colours.transparent;
                button.FlatAppearance.BorderColor = Colours.panelBackground;
                button.FlatAppearance.MouseOverBackColor = Colours.transparent;
                button.FlatAppearance.MouseDownBackColor = Colours.transparent;
            }

            if (button.Tag == "cancelUpdate")
            {
                button.BackColor = Colours.buttonBack;
                button.ForeColor = Colours.buttonFore;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
                button.Font = font7;
            }

            if(button.Tag == "time")
            {
                button.BackColor = Colours.buttonBack;
                button.ForeColor = Colours.buttonFore;
                button.FlatAppearance.BorderColor = Colours.buttonBack;
                button.FlatAppearance.MouseOverBackColor = Colours.formBackground;
                button.Font = font7;
            }
        }

        private static void DataGridStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(DataGridView))
                {
                    DefaultDataGrid((DataGridView)item);
                    
                    if(item.Name == "dataGridCurrentAssets")
                    {
                        CurrentAssetsDataGrid((DataGridView)item, false);
                    }

                    if (item.Name == "operationDataGrid")
                    {
                        OperationsDataGrid((DataGridView)item);
                    }

                    if (item.Name == "walletDataGrid")
                    {
                        WalletsDataGrid((DataGridView)item);
                    }
                }
            }
        }

        private static void DefaultDataGrid(DataGridView datagrid)
        {
            foreach (DataGridViewColumn column in datagrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            datagrid.MultiSelect = false;
            datagrid.EnableHeadersVisualStyles = false;
            datagrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(3);
            datagrid.DefaultCellStyle.Padding = new Padding(2);
            datagrid.BackgroundColor = Colours.formBackground;
            datagrid.BorderStyle = BorderStyle.None;
            datagrid.DefaultCellStyle.BackColor = Colours.formBackground;
            datagrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            datagrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            datagrid.GridColor = Colours.grid;
            datagrid.Font = font8;
            datagrid.ForeColor = Colours.labelColor;
            datagrid.ColumnHeadersDefaultCellStyle.BackColor = Colours.labelColor;
            datagrid.ColumnHeadersDefaultCellStyle.ForeColor = Colours.alternateCellBack;
            datagrid.ColumnHeadersDefaultCellStyle.Font = font9;
            datagrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            datagrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            datagrid.AllowUserToAddRows = false;
            datagrid.AllowUserToResizeColumns = false;
            datagrid.AllowUserToResizeRows = false;
            datagrid.ReadOnly = true;
            datagrid.RowHeadersVisible = false;
            datagrid.ScrollBars = ScrollBars.Vertical;
            datagrid.DefaultCellStyle.SelectionBackColor = Colours.selectedItem;
        }

        private static void OperationsDataGrid(DataGridView dataGrid)
        {
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGrid.Size = secondarydatagridSize;
            dataGrid.Location = secondarydatagridLocation;
            dataGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGrid.DefaultCellStyle.BackColor = Colours.panelBackground;
            dataGrid.DefaultCellStyle.SelectionBackColor = Colours.selectedItem;
            
            dataGrid.Font = font7;
            dataGrid.Columns[0].Width = 25;
            dataGrid.Columns[1].Width = 150;
            dataGrid.Columns[2].Width = 138;
            dataGrid.Columns[3].Width = 138;
            dataGrid.Columns[4].Width = 138;
            dataGrid.Columns[5].Width = 138;
            dataGrid.Columns[6].Width = 25;
        }

        public static void CurrentAssetsDataGrid(DataGridView dataGrid, bool loadDefaultSettings)
        {
            if (loadDefaultSettings)
            {
                DefaultDataGrid(dataGrid);
            }

            dataGrid.Size = datagridSize;
            dataGrid.Location = datagridLocation;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colours.labelColor;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Colours.formBackground;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dataGrid.Columns[0].DefaultCellStyle.NullValue = null;
            dataGrid.Columns[0].Width = 40;            
            dataGrid.Columns[1].Width = 312;
            dataGrid.Columns[2].Width = 120;
            dataGrid.Columns[3].Width = 140;
            dataGrid.Columns[4].Width = 140;
            dataGrid.Columns[2].DefaultCellStyle.Format = "N8";
            dataGrid.Columns[3].DefaultCellStyle.Format = "C2";
            dataGrid.Columns[4].DefaultCellStyle.Format = "C2";
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        public static void WalletsDataGrid(DataGridView dataGrid)
        {
            dataGrid.Size = secondarydatagridSize;
            dataGrid.Location = secondarydatagridLocation;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colours.labelColor;
            dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Colours.formBackground;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGrid.RowTemplate.Height = 40;
            dataGrid.Columns[0].DefaultCellStyle.NullValue = null;
            dataGrid.Columns[0].Width = 40;
            dataGrid.Columns[1].Width = 312;
            dataGrid.Columns[2].Width = 200;
            dataGrid.Columns[3].Width = 200;
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private static void PanelStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(Panel))
                {
                    if (item.Tag == "nav")
                    {
                        item.BackColor = Colours.panelBackground;
                    }

                    if(item.Tag == "progress")
                    {
                        item.BackColor = Colours.transparent;
                    }

                    if (item.Tag == "main")
                    {
                        item.BackColor = Colours.transparent;
                        item.Size = panelSize;
                        item.Location = panelLocation;
                    }

                    if(item.Tag == "submain")
                    {
                        item.BackColor = Colours.transparent;
                        item.Size = submainPanelSize;
                        item.Location = submainPanelLocation;
                    }

                    if (item.Tag == "options")
                    {
                        item.BackColor = Colours.panelBackground;
                    }
                    
                    if(item.Tag == "alert")
                    {
                        item.Size = alertPanelSize;
                        item.Location = alertPanelLocation;
                    }

                    if(item.Tag == "time")
                    {
                        item.BackColor = Colours.panelBackground;
                    }

                    if (item.Tag == "maxq" || item.Tag == "sell")
                    {
                        item.BackColor = Colours.formBackground;
                    }
                }
            }
        }

        private static void PieChartStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(LiveCharts.WinForms.PieChart))
                {
                    LiveCharts.WinForms.PieChart pieChart = (LiveCharts.WinForms.PieChart)item;
                    pieChart.Size = chartSize;
                    pieChart.Location = chartLocation;
                    pieChart.BackColor = Colours.formBackground;
                    pieChart.ForeColor = Colours.labelColor;
                }
            }
        }

        private static void LabelStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(Label))
                {
                    Label label = (Label)item;

                    label.ForeColor = Colours.labelColor;
                    label.BackColor = Colours.transparent;
                    label.Font = font7;

                    if (label.Tag == "title")
                    { 
                        label.Font = font15;
                    }
                    
                    if(label.Tag == "progress")
                    {
                        label.Font = font9;
                    }

                    if(label.Tag == "lastUpdated")
                    {
                        label.Font = font6;
                    }

                    if(label.Tag == "alert")
                    {
                        label.ForeColor = Colours.panelBackground;
                        label.Font = font6;
                    }

                    if(label.Tag == "dash")
                    {
                        label.Font = font15;
                        label.TextAlign = ContentAlignment.MiddleCenter;
                    }

                    if(label.Tag == "nodata")
                    {
                        label.Font = font15;
                    }
                }
            }
        }

        private static void ListViewStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(ListView))
                {
                    ListView listview = (ListView)item;

                    if (item.Tag == "stats")
                    {
                        listview.BackColor = Colours.formBackground;
                        listview.ForeColor = Colours.labelColor;
                        listview.Font = font7;
                        listview.BorderStyle = BorderStyle.None;
                    }
                    else if (item.Tag == "filter")
                    {
                        listview.BackColor = Colours.labelColor;
                        listview.Font = font7;
                        listview.BorderStyle = BorderStyle.None;
                    }
                    else
                    {
                        listview.BackColor = Colours.labelColor;
                        listview.Font = font8;
                        listview.BorderStyle = BorderStyle.None;
                    }
                }
            }
        }

        private static void DatePickerStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(CustomDateTimePicker))
                {
                    CustomDateTimePicker datepicker = (CustomDateTimePicker)item;
                    datepicker.Font = font8;
                    datepicker.BorderColor = Colours.labelColor;
                    datepicker.BackColor = Colours.formBackground;
                }
            }
        }

        private static void ComboBoxStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(ComboBox))
                {
                    ComboBox combobox = (ComboBox)item;
                    combobox.Font = font8;
                }
            }
        }

        private static void TextBoxStyling()
        {
            foreach (var item in allcontrols)
            {
                if (item.GetType() == typeof(TextBox))
                {
                    TextBox textbox = (TextBox)item;
                    textbox.Font = font9;
                }
            }
        }

        public static void ProgressBarStyling(ProgressBar pb)
        {
            pb.ForeColor = Colours.progressBar;
            pb.Value = 0;
            pb.Text = "";
            pb.Style = ProgressBarStyle.Blocks;
        }

    }
}
