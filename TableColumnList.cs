using System;

namespace cryptoFinance
{
    public class TableColumnList
    {
        public string name { get; set; }
        public Type type { get; set; }

        public TableColumnList(string _name, Type _type)
        {
            name = _name;
            type = _type;
        }
    }
}
