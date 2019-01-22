using Database_Compare.Database;
using Database_Compare.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Database_Compare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<TableNames> TableListKaynak = new List<TableNames>();
        List<TableNames> TableListHedef = new List<TableNames>();
        List<Columns> listSQL = new List<Columns>();

        string ScriptSQL = "";
        int kaynaktanHedefeFark = 0;
        int hedeftenKaynagaFark = 0;

        ContextMenuStrip menuStrip;

        private void Form1_Load(object sender, EventArgs e)
        {
            menuStrip = new ContextMenuStrip();
            menuStrip.ItemClicked += menuStrip_ItemClicked;
            menuStrip.Items.Add("Kopyala");
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (cmbHedefDB.Text == "" || cmbKaynakDB.Text == "")
            {
                MessageBox.Show("DB seçiniz.", "Uyarı");
                return;
            }
            CompareRun();
        }

        private void CompareRun()
        {
            kaynaktanHedefeFark = 0;
            hedeftenKaynagaFark = 0;
            ScriptSQL = "";
            richTextBox1.Text = "";
            lblTabloFarki.Text = "Tablo farkı : ";
            lblKolonFarki.Text = "Kolon farkı : ";
            listViewTabloFarklar.Items.Clear();
            listViewKolonFarki.Items.Clear();

            TableListKaynak = new List<TableNames>();
            TableListHedef = new List<TableNames>();
            listSQL = new List<Columns>();

            if (chkTable.Checked)
            {
                #region tablo ve kolonları çekme
                DBTableColumnLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
                DBTableColumnLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);
                #endregion

                TabloFarkVarMi();
                KaynaktanHedefeKolonFarkiVarMi();
                HedeftenKaynagaKolonFarkiVarMi();
            }

            if (chkStoredProcedure.Checked)
            {
                #region prosedürleri çekme
                DBStoredProcedureLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
                DBStoredProcedureLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);
                #endregion

                ProsedurFarkVarMi();
            }

            if (chkView.Checked)
            {
                #region Viewleri çekme
                DBViewsLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
                DBViewsLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);
                #endregion

                ViewFarkVarMi();
            }

            foreach (TableNames item in TableListKaynak)
            {
                if (item.SQL != null)
                {
                    richTextBox1.AppendText(item.SQL);
                }
            }

            lblKolonFarki.Text += " " + kaynaktanHedefeFark.ToString() + " - " + hedeftenKaynagaFark.ToString();

            //richTextBox1.Text = ScriptSQL;
        }

        private void DBTableColumnLoad(string DatabaseName, ref List<TableNames> list, DBModel dbModel)
        {
            string message = "";
            try
            {
                string where = "";
                string whereIndex = "";

                if (chkTileBaslayanlar.Checked)
                {
                    where = "where t.TABLE_NAME like 't%'";
                    whereIndex = "and t.name like 't%'";

                    if (txtTableSearch.Text != "")
                    {
                        where += " and t.TABLE_NAME='" + txtTableSearch.Text + "'";
                        whereIndex += " and t.name='" + txtTableSearch.Text + "'";
                    }
                }
                else
                {
                    if (txtTableSearch.Text != "")
                    {
                        where = "where t.TABLE_NAME='" + txtTableSearch.Text + "'";
                        whereIndex = "and t.name='" + txtTableSearch.Text + "'";
                    }
                }


                string sql = string.Format($"select TABLE_NAME from INFORMATION_SCHEMA.COLUMNS t {where} group by TABLE_NAME order by TABLE_NAME");
                DataTable tblTablesName = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                sql = string.Format(@"select t.TABLE_NAME, t.COLUMN_NAME, ORDINAL_POSITION, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE, IS_NULLABLE, t1.DefaultValue, t1.DefaultConstraintName
                        from INFORMATION_SCHEMA.COLUMNS t
                        left join (SELECT t.name AS TABLE_NAME, c.name AS COLUMN_NAME, SC.COLUMN_DEFAULT AS DefaultValue, dc.name AS DefaultConstraintName
                            FROM
                                sys.all_columns c
                                JOIN sys.tables t ON c.object_id = t.object_id
                                JOIN sys.schemas s ON t.schema_id = s.schema_id
                                LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
                                LEFT JOIN INFORMATION_SCHEMA.COLUMNS SC ON(SC.TABLE_NAME = t.name AND SC.COLUMN_NAME = c.name)
                            WHERE
                            SC.COLUMN_DEFAULT IS NOT NULL) t1 on t.TABLE_NAME = t1.TABLE_NAME and t.COLUMN_NAME = t1.COLUMN_NAME
                        {0} 
                        --and t.TABLE_NAME='tPrefMeta' 
                        order by TABLE_NAME, ORDINAL_POSITION", where);
                DataTable tblColumns = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                sql = string.Format(@"select t.name TABLE_NAME, i.name INDEX_NAME from sys.tables t
                                    inner join sys.schemas s on t.schema_id = s.schema_id
                                    inner join sys.indexes i on i.object_id = t.object_id
                                    inner join sys.index_columns ic on ic.object_id = t.object_id
                                    inner join sys.columns c on c.object_id = t.object_id and ic.column_id = c.column_id
                                    where i.index_id > 0    
                                    and i.type in (1, 2) -- clustered & nonclustered only
                                    and i.is_primary_key = 0 -- do not include PK indexes
                                    and i.is_unique_constraint = 0 -- do not include UQ
                                    and i.is_disabled = 0
                                    and i.is_hypothetical = 0
                                    and ic.key_ordinal > 0
                                    {0} 
                                    group by t.name, i.name
                                    order by t.name", whereIndex);
                DataTable tblIndex = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                sql = string.Format(@"SELECT t.TABLE_NAME, t.CONSTRAINT_NAME
                                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                                    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS t ON TC.CONSTRAINT_NAME = t.CONSTRAINT_NAME 
                                    {0} 
                                    group by t.TABLE_NAME, t.CONSTRAINT_NAME
                                    ORDER BY t.TABLE_NAME, t.CONSTRAINT_NAME", where);
                DataTable tblKeys = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                for (int i = 0; i < tblTablesName.Rows.Count; i++)
                {
                    DataRow row = tblTablesName.Rows[i];

                    TableNames tableNames = new TableNames();
                    tableNames.TABLE_NAME = Convert.ToString(row["TABLE_NAME"]);
                    tableNames.SqlType = "Table";

                    #region Columns Load
                    DataRow[] rowsColumns = tblColumns.Select($"TABLE_NAME = '{tableNames.TABLE_NAME}'");
                    for (int j = 0; j < rowsColumns.Length; j++)
                    {
                        DataRow row1 = rowsColumns[j];

                        Columns columns = new Columns();
                        columns.COLUMN_NAME = Convert.ToString(row1["COLUMN_NAME"]);
                        columns.ORDINAL_POSITION = Convert.ToInt32(row1["ORDINAL_POSITION"]);
                        columns.DATA_TYPE = Convert.ToString(row1["DATA_TYPE"]);

                        if (columns.DATA_TYPE == "nvarchar" || columns.DATA_TYPE == "char" || columns.DATA_TYPE == "nchar")
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "] (" + Convert.ToString(row1["CHARACTER_MAXIMUM_LENGTH"]) + ")";
                        }
                        else if (columns.DATA_TYPE == "tinyint" || columns.DATA_TYPE == "smallint" || columns.DATA_TYPE == "int")
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "]";
                        }
                        else if (columns.DATA_TYPE == "decimal")
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "] (" + Convert.ToString(row1["NUMERIC_PRECISION"]) + "," + Convert.ToString(row1["NUMERIC_SCALE"]) + ")";
                        }
                        else if (columns.DATA_TYPE == "datetime" || columns.DATA_TYPE == "smalldatetime")
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "]";
                        }

                        columns.DefaultValue = Convert.ToString(row1["DefaultValue"]);
                        columns.DefaultConstraintName = Convert.ToString(row1["DefaultConstraintName"]);
                        columns.IS_NULLABLE = (Convert.ToString(row1["IS_NULLABLE"]) == "YES" ? true : false);
                        //if (row1["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                        //    columns.CHARACTER_MAXIMUM_LENGTH = Convert.ToString(row1["CHARACTER_MAXIMUM_LENGTH"]);
                        //if (row1["NUMERIC_PRECISION"] != DBNull.Value)
                        //    columns.NUMERIC_PRECISION = Convert.ToInt32(row1["NUMERIC_PRECISION"]);
                        //if (row1["NUMERIC_SCALE"] != DBNull.Value)
                        //    columns.NUMERIC_SCALE = Convert.ToInt32(row1["NUMERIC_SCALE"]);
                        tableNames.ColumnList.Add(columns);
                    }
                    #endregion

                    #region Index Load
                    DataRow[] rowsIndex = tblIndex.Select($"TABLE_NAME = '{tableNames.TABLE_NAME}'");
                    for (int j = 0; j < rowsIndex.Length; j++)
                    {
                        DataRow row1 = rowsIndex[j];

                        string INDEX_NAME = Convert.ToString(row1["INDEX_NAME"]);
                        tableNames.IndexList.Add(INDEX_NAME);
                    }
                    #endregion

                    #region Key Load
                    DataRow[] rowsKey = tblKeys.Select($"TABLE_NAME = '{tableNames.TABLE_NAME}'");
                    for (int j = 0; j < rowsKey.Length; j++)
                    {
                        DataRow row1 = rowsKey[j];

                        string CONSTRAINT_NAME = Convert.ToString(row1["CONSTRAINT_NAME"]);
                        tableNames.KeyList.Add(CONSTRAINT_NAME);
                    }
                    #endregion


                    list.Add(tableNames);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DBStoredProcedureLoad(string DatabaseName, ref List<TableNames> list, DBModel dbModel)
        {
            string message = "";
            try
            {
                string where = "where ROUTINE_TYPE = 'PROCEDURE' and ROUTINE_NAME not like 'XXX%'";

                string sql = string.Format($"select ROUTINE_NAME, ROUTINE_DEFINITION from INFORMATION_SCHEMA.ROUTINES t {where} order by t.ROUTINE_NAME");
                DataTable tbl = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow row = tbl.Rows[i];

                    TableNames storeProcedure = new TableNames();
                    storeProcedure.Name = Convert.ToString(row["ROUTINE_NAME"]);
                    storeProcedure.SQL = Convert.ToString(row["ROUTINE_DEFINITION"]) + "\r\nGO\r\n\r\n";
                    storeProcedure.SqlType = "StoredProcedure";
                    list.Add(storeProcedure);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DBViewsLoad(string DatabaseName, ref List<TableNames> list, DBModel dbModel)
        {
            string message = "";
            try
            {
                string where = "where TABLE_NAME not like 'XXX%'";

                string sql = string.Format($"select TABLE_NAME, VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS t {where} order by t.TABLE_NAME");
                DataTable tbl = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow row = tbl.Rows[i];

                    TableNames view = new TableNames();
                    view.Name = Convert.ToString(row["TABLE_NAME"]);
                    view.SQL = Convert.ToString(row["VIEW_DEFINITION"]) + "\r\nGO\r\n\r\n";
                    view.SqlType = "View";
                    list.Add(view);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TabloFarkVarMi()
        {
            var tabloKaynak = from COLUMNS in TableListKaynak
                              group COLUMNS by COLUMNS.TABLE_NAME into newGroup
                              orderby newGroup.Key
                              select newGroup;

            var tabloHedef = from COLUMNS in TableListHedef
                             group COLUMNS by COLUMNS.TABLE_NAME into newGroup
                             orderby newGroup.Key
                             select newGroup;

            List<string> tabloAdlariKaynak = new List<string>();
            List<string> tabloAdlariHedef = new List<string>();
            foreach (var item in tabloKaynak)
            {
                foreach (var item1 in item)
                {
                    tabloAdlariKaynak.Add(item1.TABLE_NAME);
                }
            }

            foreach (var item in tabloHedef)
            {
                foreach (var item1 in item)
                {
                    tabloAdlariHedef.Add(item1.TABLE_NAME);
                }
            }

            List<string> KaynaktanHedefeTabloFarki = tabloAdlariKaynak.Except(tabloAdlariHedef).ToList();
            foreach (string item in KaynaktanHedefeTabloFarki)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Create Table");
                li.ForeColor = Color.White;
                li.BackColor = Color.Green;
                listViewTabloFarklar.Items.Add(li);
            }

            List<string> HedeftenKaynagaTabloFarki = tabloAdlariHedef.Except(tabloAdlariKaynak).ToList();
            foreach (string item in HedeftenKaynagaTabloFarki)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Drop Table");
                li.ForeColor = Color.White;
                li.BackColor = Color.Red;
                listViewTabloFarklar.Items.Add(li);
            }


            lblTabloFarki.Text += " " + KaynaktanHedefeTabloFarki.Count.ToString();
            lblTabloFarki.Text += " - " + HedeftenKaynagaTabloFarki.Count.ToString();
        }

        private void ProsedurFarkVarMi()
        {
            List<string> spNamesKaynak = new List<string>();
            foreach (var item in TableListKaynak.Where(f => f.SqlType == "StoredProcedure"))
            {
                spNamesKaynak.Add(item.Name);
            }

            List<string> spNamesHedef = new List<string>();
            foreach (var item in TableListHedef.Where(f => f.SqlType == "StoredProcedure"))
            {
                spNamesHedef.Add(item.Name);
            }

            List<string> KaynaktanHedefeFark = spNamesKaynak.Except(spNamesHedef).ToList();
            foreach (string item in KaynaktanHedefeFark)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Create SP");
                li.ForeColor = Color.White;
                li.BackColor = Color.Green;
                listViewTabloFarklar.Items.Add(li);
            }

            List<string> HedeftenKaynagaFark = spNamesHedef.Except(spNamesKaynak).ToList();
            foreach (string item in HedeftenKaynagaFark)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Drop SP");
                li.ForeColor = Color.White;
                li.BackColor = Color.Red;
                listViewTabloFarklar.Items.Add(li);
            }
        }

        private void ViewFarkVarMi()
        {

            List<string> viewNamesKaynak = new List<string>();
            foreach (var item in TableListKaynak.Where(f => f.SqlType == "View"))
            {
                viewNamesKaynak.Add(item.Name);
            }

            List<string> viewNamesHedef = new List<string>();
            foreach (var item in TableListHedef.Where(f => f.SqlType == "View"))
            {
                viewNamesHedef.Add(item.Name);
            }

            List<string> KaynaktanHedefeFark = viewNamesKaynak.Except(viewNamesHedef).ToList();
            foreach (string item in KaynaktanHedefeFark)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Create View");
                li.ForeColor = Color.White;
                li.BackColor = Color.Green;
                listViewTabloFarklar.Items.Add(li);
            }

            List<string> HedeftenKaynagaFark = viewNamesHedef.Except(viewNamesKaynak).ToList();
            foreach (string item in HedeftenKaynagaFark)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Drop View");
                li.ForeColor = Color.White;
                li.BackColor = Color.Red;
                listViewTabloFarklar.Items.Add(li);
            }
        }

        private void KaynaktanHedefeKolonFarkiVarMi()
        {
            #region sırayla tablolar arasında gezilir
            foreach (TableNames tableNamesKaynak in TableListKaynak)
            {
                #region hedef veritabanında böyle bir tablo var mı diye kontrol edilir
                TableNames tableNamesHedef = TableListHedef.FirstOrDefault(f => f.TABLE_NAME == tableNamesKaynak.TABLE_NAME);
                #endregion
                if (tableNamesHedef != null) //tablo varsa
                {
                    #region sırayla tabloların kolonları arasında gezilir
                    foreach (Columns columnsKaynak in tableNamesKaynak.ColumnList)
                    {
                        #region hedef tabloda böyle bir kolon var mı diye kontrol edilir
                        Columns columnsHedef = tableNamesHedef.ColumnList.FirstOrDefault(f => f.COLUMN_NAME == columnsKaynak.COLUMN_NAME);
                        #endregion
                        if (columnsHedef == null) //kolon yoksa, kolon eklenir
                        {
                            if (chkTable.Checked)
                            {
                                kaynaktanHedefeFark++;

                                string sql = "";
                                sql += string.Format($"ALTER TABLE [dbo].[{tableNamesKaynak.TABLE_NAME}] " + $"ADD [{columnsKaynak.COLUMN_NAME}] {columnsKaynak.DATA_TYPE_NEW} {(columnsKaynak.IS_NULLABLE == true ? "NULL" : "NOT NULL")}");

                                if (columnsKaynak.DefaultConstraintName != "")
                                {
                                    sql += string.Format($" CONSTRAINT [{columnsKaynak.DefaultConstraintName}] DEFAULT {columnsKaynak.DefaultValue}");
                                }
                                sql += ";";

                                columnsKaynak.Status = "Add";
                                if (tableNamesHedef.ColumnList.Count > columnsKaynak.ORDINAL_POSITION)
                                {
                                    columnsKaynak.Explanation = "Yeri değişecek";
                                    sql += "--Yeri değişecek - " + columnsKaynak.ORDINAL_POSITION;
                                }
                                else
                                {
                                    columnsKaynak.Explanation = "";
                                }
                                sql += "\r\n";
                                sql += "GO\r\n";

                                columnsKaynak.SQL = sql;
                                ScriptSQL += columnsKaynak.SQL;
                                ListViewLoad(tableNamesKaynak, columnsKaynak);

                                foreach (Columns item in tableNamesHedef.ColumnList.FindAll(f => f.ORDINAL_POSITION >= columnsKaynak.ORDINAL_POSITION))
                                {
                                    item.ORDINAL_POSITION++;
                                }
                            }
                        }
                        else //kolon varsa diğer kontroller yapılır (sırası aynı mı, tipi aynı mı gibi) 
                        {
                            if (chkTable.Checked)
                            {
                                #region aynı sırada mı diye kontrol edilir
                                if (columnsKaynak.ORDINAL_POSITION != columnsHedef.ORDINAL_POSITION)
                                {

                                    kaynaktanHedefeFark++;

                                    columnsKaynak.Status = "Update";
                                    columnsKaynak.Explanation = "Yeri farklı - Eski yeri(" + columnsHedef.ORDINAL_POSITION + ")";

                                    string sql = "--Yeri değişecek - " + tableNamesKaynak.TABLE_NAME + " " + columnsKaynak.COLUMN_NAME + " Eski yeri(" + columnsHedef.ORDINAL_POSITION + ") - Yeni yeri(" + columnsKaynak.ORDINAL_POSITION + ")\r\n";

                                    columnsKaynak.SQL = sql;
                                    ScriptSQL += columnsKaynak.SQL;
                                    ListViewLoad(tableNamesKaynak, columnsKaynak);

                                    //todo: kldjslfşkajsdşlkfjsşkldfjşslkdfj

                                    if (columnsHedef.ORDINAL_POSITION > columnsKaynak.ORDINAL_POSITION)
                                    {
                                        foreach (Columns item in tableNamesHedef.ColumnList.FindAll(f => f.ORDINAL_POSITION >= columnsKaynak.ORDINAL_POSITION && f.ORDINAL_POSITION < columnsHedef.ORDINAL_POSITION))
                                        {
                                            item.ORDINAL_POSITION++;
                                        }
                                    }
                                }
                                #endregion
                                #region tipleri aynı mı diye kontrol edilir
                                if (columnsKaynak.DATA_TYPE_NEW != columnsHedef.DATA_TYPE_NEW)
                                {
                                    kaynaktanHedefeFark++;

                                    columnsKaynak.Status = "Update";
                                    columnsKaynak.Explanation = "Tipi farklı - " + columnsHedef.DATA_TYPE_NEW;

                                    string sql = "";
                                    if (columnsKaynak.DefaultConstraintName != "")
                                    {
                                        sql += string.Format($"ALTER TABLE [dbo].[{tableNamesKaynak.TABLE_NAME}] DROP CONSTRAINT {columnsKaynak.DefaultConstraintName};\r\n");
                                    }

                                    sql += string.Format($"ALTER TABLE [dbo].[{tableNamesKaynak.TABLE_NAME}] " + $"ALTER COLUMN [{columnsKaynak.COLUMN_NAME}] {columnsKaynak.DATA_TYPE_NEW} {(columnsKaynak.IS_NULLABLE == true ? "NULL" : "NOT NULL")}; --Eski tipi - {columnsHedef.DATA_TYPE_NEW}\r\n");

                                    if (columnsKaynak.DefaultConstraintName != "")
                                    {
                                        sql += string.Format($"ALTER TABLE [dbo].[{tableNamesKaynak.TABLE_NAME}] ADD CONSTRAINT {columnsKaynak.DefaultConstraintName} DEFAULT {columnsKaynak.DefaultValue} FOR [{columnsKaynak.COLUMN_NAME}];\r\n");
                                    }
                                    sql += "GO\r\n";

                                    columnsKaynak.SQL = sql;
                                    ScriptSQL += columnsKaynak.SQL;
                                    ListViewLoad(tableNamesKaynak, columnsKaynak);
                                }
                                #endregion
                                #region hem tipi farklı hem de sırası farklıysa bu işlem yapılır
                                if (columnsKaynak.ORDINAL_POSITION != columnsHedef.ORDINAL_POSITION && columnsKaynak.DATA_TYPE_NEW != columnsHedef.DATA_TYPE_NEW)
                                {
                                    kaynaktanHedefeFark++;

                                    columnsKaynak.Status = "Update";
                                    columnsKaynak.Explanation = "Hem yeri hem tipi farklı - " + columnsHedef.ORDINAL_POSITION + " - " + columnsHedef.DATA_TYPE_NEW;
                                    ListViewLoad(tableNamesKaynak, columnsKaynak);
                                }
                                #endregion
                            }

                            if (chkConstraint.Checked)
                            {
                                #region CONSTRAINT boş değilse
                                if (columnsKaynak.DefaultConstraintName != "")
                                {
                                    if (columnsHedef.DefaultConstraintName != "")
                                    {
                                        #region CONSTRAINT olması gereken şekliyle değilse bu işlem yapılır
                                        string CONSTRAINT = "DF_" + tableNamesKaynak.TABLE_NAME + "_" + columnsKaynak.COLUMN_NAME;
                                        if (CONSTRAINT != columnsHedef.DefaultConstraintName)
                                        {
                                            kaynaktanHedefeFark++;

                                            columnsKaynak.Status = "Update";
                                            columnsKaynak.Explanation = "CONSTRAINT Default farklı";

                                            string sql = string.Format($"exec sp_rename '{columnsHedef.DefaultConstraintName}', '{CONSTRAINT}', 'object'; --{columnsKaynak.COLUMN_NAME}\r\n");
                                            sql += "GO\r\n";

                                            columnsKaynak.SQL = sql;
                                            ScriptSQL += columnsKaynak.SQL;
                                            ListViewLoad(tableNamesKaynak, columnsKaynak);
                                        }
                                        #endregion
                                    }

                                    #region CONSTRAINT farklıysa bu işlem yapılır
                                    //if (columnsKaynak.DefaultConstraintName != columnsHedef.DefaultConstraintName)
                                    //{
                                    //    kaynaktanHedefeFark++;

                                    //    columnsKaynak.Status = "Update";
                                    //    columnsKaynak.Explanation = "CONSTRAINT farklı";

                                    //    string sql = string.Format($"exec sp_rename '{columnsHedef.DefaultConstraintName}', '{columnsKaynak.DefaultConstraintName}', 'object'; --{columnsKaynak.COLUMN_NAME}\r\n");
                                    //    sql += "GO\r\n";

                                    //    columnsKaynak.SQL = sql;
                                    //    ScriptSQL += columnsKaynak.SQL;
                                    //    ListViewLoad(tableNamesKaynak, columnsKaynak);
                                    //}
                                    #endregion
                                }
                                #endregion
                            }
                        }

                        //if (newColumn)
                        //{

                        //}
                    }
                    #endregion

                    if (chkKey.Checked)
                    {
                        if (tableNamesKaynak.KeyList.Count > 0)
                        {
                            List<string> KeyFark = tableNamesKaynak.KeyList.Except(tableNamesHedef.KeyList).ToList();
                            #region Key farklıysa
                            if (KeyFark.Count > 0)
                            {
                                foreach (string item in KeyFark)
                                {
                                    ListViewLoad(tableNamesKaynak, new Columns { Explanation = item, Status = "Key" });
                                }
                            }
                            #endregion
                        }
                    }

                    if (chkIndex.Checked)
                    {
                        if (tableNamesKaynak.IndexList.Count > 0)
                        {
                            List<string> IndexFark = tableNamesKaynak.IndexList.Except(tableNamesHedef.IndexList).ToList();
                            #region İndex farklıysa
                            if (IndexFark.Count > 0)
                            {
                                foreach (string item in IndexFark)
                                {
                                    ListViewLoad(tableNamesKaynak, new Columns { Explanation = item, Status = "Index" });
                                }
                            }
                            #endregion
                        }
                    }
                }
                else //tablo yoksa
                {
                }


            }
            #endregion

            ScriptSQL += "\r\n";
        }

        private void HedeftenKaynagaKolonFarkiVarMi()
        {
            #region sırayla tablolar arasında gezilir
            foreach (TableNames tableNamesKaynak in TableListHedef)
            {
                #region hedef veritabanında böyle bir tablo var mı diye kontrol edilir
                TableNames tableNamesHedef = TableListKaynak.FirstOrDefault(f => f.TABLE_NAME == tableNamesKaynak.TABLE_NAME);
                #endregion
                if (tableNamesHedef != null) //tablo varsa
                {
                    #region sırayla tabloların kolonları arasında gezilir
                    foreach (Columns columnsKaynak in tableNamesKaynak.ColumnList)
                    {
                        #region hedef tabloda böyle bir kolon var mı diye kontrol edilir
                        Columns columnsHedef = tableNamesHedef.ColumnList.FirstOrDefault(f => f.COLUMN_NAME == columnsKaynak.COLUMN_NAME);
                        #endregion
                        if (columnsHedef == null) //kolon yoksa, kolon silinir
                        {
                            if (chkTable.Checked)
                            {
                                hedeftenKaynagaFark++;

                                columnsKaynak.Status = "Delete";
                                columnsKaynak.Explanation = "Kolon silinecek";

                                string sql = "";
                                if (columnsKaynak.DefaultConstraintName != "")
                                {
                                    sql += string.Format($"ALTER TABLE [dbo].[{tableNamesKaynak.TABLE_NAME}] DROP CONSTRAINT [{columnsKaynak.DefaultConstraintName}]; \r\n");
                                }

                                sql += string.Format($"ALTER TABLE [dbo].[{tableNamesKaynak.TABLE_NAME}] DROP COLUMN [{columnsKaynak.COLUMN_NAME}];");
                                sql += "\r\n";
                                sql += "GO\r\n";

                                columnsKaynak.SQL = sql;
                                ScriptSQL += columnsKaynak.SQL;
                                ListViewLoad(tableNamesKaynak, columnsKaynak);
                            }
                        }
                        else //kolon varsa aynı sırada mı kontrol edilir 
                        {

                        }
                    }
                    #endregion
                }
                else //tablo yoksa
                {
                }
            }
            #endregion

            ScriptSQL += "\r\n";
        }

        private void ListViewLoad(TableNames tableNamesKaynak, Columns columnsKaynak)
        {
            listSQL.Add(columnsKaynak);

            tableNamesKaynak.SQL += columnsKaynak.SQL;

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = tableNamesKaynak.TABLE_NAME;
            listViewItem.SubItems.Add(columnsKaynak.COLUMN_NAME);
            listViewItem.SubItems.Add(columnsKaynak.DATA_TYPE_NEW);
            listViewItem.SubItems.Add(columnsKaynak.ORDINAL_POSITION.ToString());
            listViewItem.SubItems.Add(columnsKaynak.Status);
            listViewItem.SubItems.Add(columnsKaynak.Explanation);
            listViewItem.Checked = columnsKaynak.Checked;
            listViewItem.Tag = columnsKaynak.SQL;

            listViewKolonFarki.Items.Add(listViewItem);
        }

        private void btnBaglanKaynak_Click(object sender, EventArgs e)
        {
            //Login login = new Login(cmbKaynakDB);
            //login.ShowDialog();

            string message = "";
            DBModel dbModel = new DBModel();
            dbModel.Ip = txtIpKaynak.Text;
            dbModel.UserName = txtUserNameKaynak.Text;
            dbModel.Password = txtPasswordKaynak.Text;

            string sql = "SELECT name, database_id, create_date FROM sys.databases where database_id>4 order by sys.databases.name; ";
            DataTable tbl = DB.GetTable(sql, "master", dbModel, ref message);
            if (tbl == null)
            {
                MessageBox.Show(message, "Uyarı");
                return;
            }

            cmbKaynakDB.Items.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow row = tbl.Rows[i];
                //DB.DatabaseList.Add(row["name"].ToString());
                cmbKaynakDB.Items.Add(row["name"].ToString());
            }

            cmbKaynakDB.SelectedIndex = 0;

            cmbKaynakDB.Tag = dbModel;
        }

        private void btnBaglanHedef_Click(object sender, EventArgs e)
        {
            //Login login = new Login(cmbHedefDB);
            //login.ShowDialog();

            string message = "";
            DBModel dbModel = new DBModel();
            dbModel.Ip = txtIpHedef.Text;
            dbModel.UserName = txtUserNameHedef.Text;
            dbModel.Password = txtPasswordHedef.Text;

            string sql = "SELECT name, database_id, create_date FROM sys.databases where database_id>4 order by sys.databases.name; ";
            DataTable tbl = DB.GetTable(sql, "master", dbModel, ref message);
            if (tbl == null)
            {
                MessageBox.Show(message, "Uyarı");
                return;
            }

            cmbHedefDB.Items.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow row = tbl.Rows[i];
                //DB.DatabaseList.Add(row["name"].ToString());
                cmbHedefDB.Items.Add(row["name"].ToString());
            }

            cmbHedefDB.SelectedIndex = 0;

            cmbHedefDB.Tag = dbModel;
        }

        private void btnBaglantiyiKopyala_Click(object sender, EventArgs e)
        {
            cmbHedefDB.Tag = cmbKaynakDB.Tag;
            cmbHedefDB.Items.Clear();
            foreach (var item in cmbKaynakDB.Items)
            {
                cmbHedefDB.Items.Add(item);
            }

            cmbHedefDB.Text = cmbKaynakDB.Text;

            txtIpHedef.Text = txtIpKaynak.Text;
            txtUserNameHedef.Text = txtUserNameKaynak.Text;
            txtPasswordHedef.Text = txtPasswordKaynak.Text;
        }

        private void btnBaglantiyiKopyalaKaynaga_Click(object sender, EventArgs e)
        {
            cmbKaynakDB.Tag = cmbHedefDB.Tag;
            cmbKaynakDB.Items.Clear();
            foreach (var item in cmbHedefDB.Items)
            {
                cmbKaynakDB.Items.Add(item);
            }

            cmbKaynakDB.Text = cmbHedefDB.Text;

            txtIpKaynak.Text = txtIpHedef.Text;
            txtUserNameKaynak.Text = txtUserNameHedef.Text;
            txtPasswordKaynak.Text = txtPasswordHedef.Text;
        }

        private void btnSeciliOlanlariGetir_Click(object sender, EventArgs e)
        {
            foreach (Columns item in listSQL)
            {
                item.Checked = false;
            }

            foreach (ListViewItem item in listViewKolonFarki.CheckedItems)
            {
                listSQL.FirstOrDefault(f => f.SQL == item.Tag.ToString()).Checked = item.Checked;
            }

            richTextBox1.Text = "";
            foreach (Columns item in listSQL.Where(f => f.Checked))
            {
                richTextBox1.AppendText(item.SQL);
            }
        }

        private void listViewKolonFarki_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                item = listViewKolonFarki.GetItemAt(e.X, e.Y);
                menuStrip.Show(listViewKolonFarki, e.Location);
            }
        }

        ListViewItem item;
        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Kopyala")
            {
                Clipboard.SetText(item.Tag.ToString());
            }
        }

        private void chkIndex_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIndex.Checked)
            {
                chkTable.Checked = true;
            }
        }

        private void chkConstraint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConstraint.Checked)
            {
                chkTable.Checked = true;
            }
        }

        private void chkKey_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKey.Checked)
            {
                chkTable.Checked = true;
            }
        }

        private void chkTable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkTable.Checked)
            {
                chkIndex.Checked = false;
                chkConstraint.Checked = false;
                chkKey.Checked = false;
            }
        }

        private void listViewKolonFarki_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //foreach (Columns item in listSQL)
            //{
            //    item.Checked = false;
            //}

            //foreach (ListViewItem item in listViewKolonFarki.CheckedItems)
            //{
            //    listSQL.FirstOrDefault(f => f.SQL == item.Tag.ToString()).Checked = item.Checked;
            //}

            //richTextBox1.Text = "";
            //foreach (Columns item in listSQL.Where(f => f.Checked))
            //{
            //    richTextBox1.AppendText(item.SQL);
            //}
        }

        private void btnRunExecSql_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                MessageBox.Show("Çalıştırılacak script bulunamadı.", "Uyarı");
                return;
            }
            DialogResult secenek = MessageBox.Show("Scripti çalıştırmak istediğinize emin misiniz?", "Bilgilendirme Penceresi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (secenek == DialogResult.Yes)
            {
                string message = "";
                bool retVal = DB.ExecSql(richTextBox1.Text, cmbHedefDB.Text, (DBModel)cmbHedefDB.Tag, ref message);
                if (retVal)
                {
                    MessageBox.Show("Script çalıştı", "Uyarı");
                }
                else
                {
                    MessageBox.Show(message, "Uyarı");
                }
            }
            else return;
        }
    }
}
