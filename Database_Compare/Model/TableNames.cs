using System.Collections.Generic;

namespace Database_Compare.Model
{
    class TableNames
    {
        public string TABLE_NAME { get; set; }
        public List<Columns> ColumnList = new List<Columns>();

        //birden fazla kolon için index kullanıldığı için buraya alındı
        public List<string> IndexList = new List<string>();
        public List<string> KeyList = new List<string>();



        public string SqlType { get; set; } //Table, StoredProcedure, View
        public string Name { get; set; } //SP - View Name
        public string SQL { get; set; }
    }
}
