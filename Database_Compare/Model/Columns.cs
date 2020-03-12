namespace Database_Compare.Model
{
    class Columns
    {
        public string COLUMN_NAME { get; set; }
        public int ORDINAL_POSITION { get; set; }
        public string DATA_TYPE { get; set; }
        public string DATA_TYPE_NEW { get; set; }
        public int CHARACTER_MAXIMUM_LENGTH { get; set; }
        //public int NUMERIC_PRECISION { get; set; }
        //public int NUMERIC_SCALE { get; set; }
        public string DefaultValue { get; set; }
        //CONSTRAINT name bilgisi
        public string DefaultConstraintName { get; set; }

        public bool IS_NULLABLE { get; set; }

        public string Status { get; set; }
        public string Explanation { get; set; }

        public string SQL { get; set; }

        public bool Checked = false;
    }
}
