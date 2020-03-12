namespace Database_Compare.Model
{
    class GeneralModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Columns { get; set; }

        public bool NewScript = false;
        public bool IsReady = false;

        //public string[] SatirListe;

        public string ValueReplace { get; set; }
    }
}
