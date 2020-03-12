using System.Collections.Generic;

namespace Database_Compare.Model
{
    class TableNames
    {
        public string TABLE_NAME { get; set; }
        public List<Columns> ColumnList = new List<Columns>();

        //birden fazla kolon için index kullanıldığı için buraya alındı
        //public List<string> IndexList = new List<string>();
        public List<GeneralModel> IndexListGeneral = new List<GeneralModel>();
        //public List<string> KeyList = new List<string>();
        public List<GeneralModel> PrimaryKeyListGeneral = new List<GeneralModel>();
        public List<GeneralModel> ForeignKeyListGeneral = new List<GeneralModel>();
        //public List<string> TriggerNameList = new List<string>();
        //public List<string> TriggerDefinitionList = new List<string>();
        public List<GeneralModel> TriggerDefinitionListGeneral = new List<GeneralModel>();



        public string SqlType { get; set; } //Table, StoredProcedure, View
        public string Name { get; set; } //SP - View Name
        public string SQL { get; set; }
        public string SQLReplace { get; set; }

        public string Explanation { get; set; }

        public bool ErrFlag { get; set; }
    }
}
