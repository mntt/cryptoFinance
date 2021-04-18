using System.Windows.Forms;

namespace cryptoFinance
{
    public static class DataGridViewSettings
    {
        public static void TurnOffSorting(DataGridView dataGrid)
        {
            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public static void FormatCurrentAssets(DataGridView datagrid)
        {
            TurnOffSorting(datagrid);

            datagrid.Columns[0].Width = 202;
            datagrid.Columns[1].Width = 100;
            datagrid.Columns[2].Width = 100;
            datagrid.Columns[3].Width = 100;
            datagrid.Columns[1].DefaultCellStyle.Format = "0.00000";
            datagrid.Columns[2].DefaultCellStyle.Format = "C2";
            datagrid.Columns[3].DefaultCellStyle.Format = "C2";
            datagrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            datagrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;  
        }

    }
}
