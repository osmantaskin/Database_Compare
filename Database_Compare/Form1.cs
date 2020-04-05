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
        ContextMenuStrip menuStripSolView;

        private void Form1_Load(object sender, EventArgs e)
        {
            menuStrip = new ContextMenuStrip();
            menuStrip.ItemClicked += menuStrip_ItemClicked;
            menuStrip.Items.Add("Run");
            menuStrip.Items.Add("Kopyala");
            menuStrip.Items.Add("Compare");


            menuStripSolView = new ContextMenuStrip();
            menuStripSolView.ItemClicked += menuStripSolView_ItemClicked;
            menuStripSolView.Items.Add("Run");
            menuStripSolView.Items.Add("Kopyala");
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (cmbHedefDB.Text == "" || cmbKaynakDB.Text == "")
            {
                MessageBox.Show("DB seçiniz.", "Uyarı");
                return;
            }
            CompareRun();
            btnRunExecSql.Enabled = true;
        }

        private void CompareRun()
        {
            kaynaktanHedefeFark = 0;
            hedeftenKaynagaFark = 0;
            ScriptSQL = "";
            richTextBox1.Text = "";
            lblTabloFarki.Text = "Tablo farkı : ";
            lblKolonFarki.Text = "Kolon farkı : ";
            lblProsedurFarki.Text = "Prosedür farkı : ";
            lblViewFarki.Text = "View farkı : ";
            lblFunctionFarki.Text = "Function farkı : ";
            lblTriggerFarki.Text = "Trigger farkı : ";
            listViewTabloFarklar.Items.Clear();
            listViewKolonFarki.Items.Clear();
            chkHepsi.Checked = false;

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

            DBAllLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
            DBAllLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);

            //TriggerFarkVarMi();

            if (chkStoredProcedure.Checked)
            {
                #region prosedürleri çekme
                //DBStoredProcedureLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
                //DBStoredProcedureLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);
                #endregion

                ProsedurFarkVarMi();
            }

            if (chkView.Checked)
            {
                #region Viewleri çekme
                //DBViewsLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
                //DBViewsLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);
                #endregion

                ViewFarkVarMi();
            }

            if (chkFunction.Checked)
            {
                #region Function çekme
                //DBFunctionsLoad(cmbKaynakDB.Text, ref TableListKaynak, (DBModel)cmbKaynakDB.Tag);
                //DBFunctionsLoad(cmbHedefDB.Text, ref TableListHedef, (DBModel)cmbHedefDB.Tag);
                #endregion

                FunctionFarkVarMi();
            }

            foreach (TableNames item in TableListKaynak)
            {
                if (item.SQL != null && item.SQL != "" && item.ErrFlag)
                {
                    if (item.SqlType != "StoredProcedure" && item.SqlType != "Function" && item.SqlType != "View")
                    {
                        richTextBox1.AppendText(item.SQL);
                    }
                }
            }

            foreach (TableNames item in TableListHedef)
            {
                if (item.SQL != null && item.SQL != "" && item.ErrFlag)
                {
                    if (item.SqlType != "StoredProcedure" && item.SqlType != "Function" && item.SqlType != "View")
                    {
                        richTextBox1.AppendText(item.SQL);
                    }
                }
            }

            lblKolonFarki.Text += " " + kaynaktanHedefeFark.ToString() + " kolon eklenecek veya değişecek - " + hedeftenKaynagaFark.ToString() + " kolon silinecek veya değişecek";

            //richTextBox1.Text = ScriptSQL;
        }

        private void DBTableColumnLoad(string DatabaseName, ref List<TableNames> list, DBModel dbModel)
        {
            string message = "";
            try
            {
                string where = "";
                string whereIndex = "";
                string whereTrigger = "";
                string whereForeignKey = "";

                //txtTableSearch.Text = "tUser";

                if (chkTileBaslayanlar.Checked)
                {
                    where = "where t.TABLE_NAME like 't%'";
                    whereIndex = "and t.name like 't%'";
                    whereTrigger = "and OBJECT_NAME(parent_obj) like 't%'";
                    whereForeignKey = "where OBJECT_NAME(parent_object_id) like 't%'";

                    if (txtTableSearch.Text != "")
                    {
                        where += " and t.TABLE_NAME='" + txtTableSearch.Text + "'";
                        whereIndex += " and t.name='" + txtTableSearch.Text + "'";
                        whereTrigger += " and OBJECT_NAME(parent_obj)='" + txtTableSearch.Text + "'";
                        whereForeignKey += " and OBJECT_NAME(parent_object_id)='" + txtTableSearch.Text + "'";
                    }
                }
                else
                {
                    if (txtTableSearch.Text != "")
                    {
                        where = "where t.TABLE_NAME='" + txtTableSearch.Text + "'";
                        whereIndex = "and t.name='" + txtTableSearch.Text + "'";
                        whereTrigger = "and OBJECT_NAME(parent_obj)='" + txtTableSearch.Text + "'";
                        whereForeignKey = "where OBJECT_NAME(parent_object_id)='" + txtTableSearch.Text + "'";
                    }
                }


                #region Table
                string sql = string.Format($"select TABLE_NAME from INFORMATION_SCHEMA.COLUMNS t {where} group by TABLE_NAME order by TABLE_NAME");
                DataTable tblTablesName = DB.GetTable(sql, DatabaseName, dbModel, ref message);
                #endregion

                #region Column
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
                        order by TABLE_NAME, ORDINAL_POSITION", where);
                DataTable tblColumns = DB.GetTable(sql, DatabaseName, dbModel, ref message);
                #endregion

                #region Index
                //sql = string.Format(@"select t.name TABLE_NAME, i.name INDEX_NAME from sys.tables t
                //                    inner join sys.schemas s on t.schema_id = s.schema_id
                //                    inner join sys.indexes i on i.object_id = t.object_id
                //                    inner join sys.index_columns ic on ic.object_id = t.object_id and i.index_id=ic.index_id
                //                    inner join sys.columns c on c.object_id = t.object_id and ic.column_id = c.column_id
                //                    where i.index_id > 0    
                //                    and i.type in (1, 2) -- clustered & nonclustered only
                //                    and i.is_primary_key = 0 -- do not include PK indexes
                //                    and i.is_unique_constraint = 0 -- do not include UQ
                //                    and i.is_disabled = 0
                //                    and i.is_hypothetical = 0
                //                    and ic.key_ordinal > 0
                //                    {0} 
                //                    group by t.name, i.name
                //                    order by t.name", whereIndex);
                sql = string.Format(@"SELECT t.name TABLE_NAME, I.name INDEX_NAME, 'CREATE ' +
                                           CASE 
                                                WHEN I.is_unique = 1 THEN ' UNIQUE '
                                                ELSE ''
                                           END +
                                           I.type_desc COLLATE DATABASE_DEFAULT + ' INDEX ' + KeyColumns as KeyColumns, 'CREATE ' +
                                           CASE 
                                                WHEN I.is_unique = 1 THEN ' UNIQUE '
                                                ELSE ''
                                           END +
                                           I.type_desc COLLATE DATABASE_DEFAULT + ' INDEX ' +
                                           I.name + ' ON ' +
                                           SCHEMA_NAME(T.schema_id) + '.' + T.name + ' ( ' +
                                           KeyColumns + ' )  ' +
                                           ISNULL(' INCLUDE (' + IncludedColumns + ' ) ', '') +
                                           ISNULL(' WHERE  ' + I.filter_definition, '') + ' WITH ( ' +
                                           CASE 
                                                WHEN I.is_padded = 1 THEN ' PAD_INDEX = ON '
                                                ELSE ' PAD_INDEX = OFF '
                                           END + ',' +
                                           --'FILLFACTOR = ' + CONVERT(
                                           --    CHAR(5),
                                           --    CASE 
                                           --         WHEN I.fill_factor = 0 THEN 100
                                           --         ELSE I.fill_factor
                                           --    END
                                           --) + ',' +
                                           ---- default value 
                                           'SORT_IN_TEMPDB = OFF ' + ',' +
                                           CASE 
                                                WHEN I.ignore_dup_key = 1 THEN ' IGNORE_DUP_KEY = ON '
                                                ELSE ' IGNORE_DUP_KEY = OFF '
                                           END + ',' +
                                           --CASE 
                                           --     WHEN ST.no_recompute = 0 THEN ' STATISTICS_NORECOMPUTE = OFF '
                                           --     ELSE ' STATISTICS_NORECOMPUTE = ON '
                                           --END + ',' +
                                           ' ONLINE = OFF ' + ',' +
                                           CASE 
                                                WHEN I.allow_row_locks = 1 THEN ' ALLOW_ROW_LOCKS = ON '
                                                ELSE ' ALLOW_ROW_LOCKS = OFF '
                                           END + ',' +
                                           CASE 
                                                WHEN I.allow_page_locks = 1 THEN ' ALLOW_PAGE_LOCKS = ON '
                                                ELSE ' ALLOW_PAGE_LOCKS = OFF '
                                           END + ' ) ON [' +
                                           DS.name + ' ] ' +  CHAR(13) + CHAR(10) [CreateIndexScript]
                                    FROM   sys.indexes I
                                           JOIN sys.tables T
                                                ON  T.object_id = I.object_id
                                           JOIN sys.sysindexes SI
                                                ON  I.object_id = SI.id
                                                AND I.index_id = SI.indid
                                           JOIN (
                                                    SELECT *
                                                    FROM   (
                                                               SELECT IC2.object_id,
                                                                      IC2.index_id,
                                                                      STUFF(
                                                                          (
                                                                              SELECT ' , ' + C.name + CASE 
                                                                                                           WHEN MAX(CONVERT(INT, IC1.is_descending_key)) 
                                                                                                                = 1 THEN 
                                                                                                                ' DESC '
                                                                                                           ELSE 
                                                                                                                ' ASC '
                                                                                                      END
                                                                              FROM   sys.index_columns IC1
                                                                                     JOIN sys.columns C
                                                                                          ON  C.object_id = IC1.object_id
                                                                                          AND C.column_id = IC1.column_id
                                                                                          AND IC1.is_included_column = 
                                                                                              0
                                                                              WHERE  IC1.object_id = IC2.object_id
                                                                                     AND IC1.index_id = IC2.index_id
                                                                              GROUP BY
                                                                                     IC1.object_id,
                                                                                     C.name,
                                                                                     index_id
                                                                              ORDER BY
                                                                                     MAX(IC1.key_ordinal) 
                                                                                     FOR XML PATH('')
                                                                          ),
                                                                          1,
                                                                          2,
                                                                          ''
                                                                      ) KeyColumns
                                                               FROM   sys.index_columns IC2 
                                                                      --WHERE IC2.Object_id = object_id('Person.Address') --Comment for all tables
                                                               GROUP BY
                                                                      IC2.object_id,
                                                                      IC2.index_id
                                                           ) tmp3
                                                )tmp4
                                                ON  I.object_id = tmp4.object_id
                                                AND I.Index_id = tmp4.index_id
                                           JOIN sys.stats ST
                                                ON  ST.object_id = I.object_id
                                                AND ST.stats_id = I.index_id
                                           JOIN sys.data_spaces DS
                                                ON  I.data_space_id = DS.data_space_id
                                           JOIN sys.filegroups FG
                                                ON  I.data_space_id = FG.data_space_id
                                           LEFT JOIN (
                                                    SELECT *
                                                    FROM   (
                                                               SELECT IC2.object_id,
                                                                      IC2.index_id,
                                                                      STUFF(
                                                                          (
                                                                              SELECT ' , ' + C.name
                                                                              FROM   sys.index_columns IC1
                                                                                     JOIN sys.columns C
                                                                                          ON  C.object_id = IC1.object_id
                                                                                          AND C.column_id = IC1.column_id
                                                                                          AND IC1.is_included_column = 
                                                                                              1
                                                                              WHERE  IC1.object_id = IC2.object_id
                                                                                     AND IC1.index_id = IC2.index_id
                                                                              GROUP BY
                                                                                     IC1.object_id,
                                                                                     C.name,
                                                                                     index_id 
                                                                                     FOR XML PATH('')
                                                                          ),
                                                                          1,
                                                                          2,
                                                                          ''
                                                                      ) IncludedColumns
                                                               FROM   sys.index_columns IC2 
                                                                      --WHERE IC2.Object_id = object_id('Person.Address') --Comment for all tables
                                                               GROUP BY
                                                                      IC2.object_id,
                                                                      IC2.index_id
                                                           ) tmp1
                                                    WHERE  IncludedColumns IS NOT NULL
                                                ) tmp2
                                                ON  tmp2.object_id = I.object_id
                                                AND tmp2.index_id = I.index_id
                                    WHERE  I.is_primary_key = 0
                                           AND I.is_unique_constraint = 0	   
                                                {0}
                                               --AND I.Object_id = object_id('Person.Address') --Comment for all tables
                                               --AND I.name = 'IX_Address_PostalCode' --comment for all indexes 
                                    order by t.name, I.name", whereIndex);
                DataTable tblIndex = DB.GetTable(sql, DatabaseName, dbModel, ref message);
                #endregion

                #region PrimaryKey
                //sql = string.Format(@"SELECT t.TABLE_NAME, t.CONSTRAINT_NAME
                //                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                //                    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS t ON TC.CONSTRAINT_NAME = t.CONSTRAINT_NAME 
                //                    {0} 
                //                    group by t.TABLE_NAME, t.CONSTRAINT_NAME
                //                    ORDER BY t.TABLE_NAME, t.CONSTRAINT_NAME", where);
                sql = string.Format(@"--- SCRIPT TO GENERATE THE CREATION SCRIPT OF ALL PK AND UNIQUE CONSTRAINTS.
                                        declare @SchemaName varchar(100)
                                        declare @TableName varchar(256)
                                        declare @IndexName varchar(256)
                                        declare @ColumnName varchar(100)
                                        declare @is_unique_constraint varchar(100)
                                        declare @IndexTypeDesc varchar(100)
                                        declare @FileGroupName varchar(100)
                                        declare @is_disabled varchar(100)
                                        declare @IndexOptions varchar(max)
                                        declare @IndexColumnId int
                                        declare @IsDescendingKey int 
                                        declare @IsIncludedColumn int
                                        declare @TSQLScripCreationIndex varchar(max)
                                        declare @TSQLScripDisableIndex varchar(max)
                                        declare @is_primary_key varchar(100)

                                        create table #t (
	                                        TABLE_NAME nvarchar(50),
	                                        KEY_NAME nvarchar(50),
                                            KEY_COLUMNS nvarchar(150),
	                                        KEY_VALUE nvarchar(max)
                                        )

                                        declare CursorIndex cursor for
                                         select schema_name(t.schema_id) [schema_name], t.name, ix.name,
                                         case when ix.is_unique_constraint = 1 then ' UNIQUE ' else '' END 
                                            ,case when ix.is_primary_key = 1 then ' PRIMARY KEY ' else '' END 
                                         , ix.type_desc,
                                          case when ix.is_padded=1 then 'PAD_INDEX = ON, ' else 'PAD_INDEX = OFF, ' end
                                         + case when ix.allow_page_locks=1 then 'ALLOW_PAGE_LOCKS = ON, ' else 'ALLOW_PAGE_LOCKS = OFF, ' end
                                         + case when ix.allow_row_locks=1 then  'ALLOW_ROW_LOCKS = ON, ' else 'ALLOW_ROW_LOCKS = OFF, ' end
                                         + case when INDEXPROPERTY(t.object_id, ix.name, 'IsStatistics') = 1 then 'STATISTICS_NORECOMPUTE = ON, ' else 'STATISTICS_NORECOMPUTE = OFF, ' end
                                         + case when ix.ignore_dup_key=1 then 'IGNORE_DUP_KEY = ON, ' else 'IGNORE_DUP_KEY = OFF, ' end
                                         --+ 'SORT_IN_TEMPDB = OFF, FILLFACTOR =' + CAST(ix.fill_factor AS VARCHAR(3)) AS IndexOptions
                                         + 'SORT_IN_TEMPDB = OFF' AS IndexOptions
                                         , FILEGROUP_NAME(ix.data_space_id) FileGroupName
                                         from sys.tables t 
                                         inner join sys.indexes ix on t.object_id=ix.object_id
                                         where ix.type>0 and  (ix.is_primary_key=1 or ix.is_unique_constraint=1) --and schema_name(tb.schema_id)= @SchemaName and tb.name=@TableName
                                         and t.is_ms_shipped=0 and t.name<>'sysdiagrams' {0}
                                         order by schema_name(t.schema_id), t.name, ix.name
                                        open CursorIndex
                                        fetch next from CursorIndex into  @SchemaName, @TableName, @IndexName, @is_unique_constraint, @is_primary_key, @IndexTypeDesc, @IndexOptions, @FileGroupName
                                        while (@@fetch_status=0)
                                        begin
                                         declare @IndexColumns varchar(max)
                                         declare @IncludedColumns varchar(max)
                                         set @IndexColumns=''
                                         set @IncludedColumns=''
                                         declare CursorIndexColumn cursor for 
                                         select col.name, ixc.is_descending_key, ixc.is_included_column
                                         from sys.tables tb 
                                         inner join sys.indexes ix on tb.object_id=ix.object_id
                                         inner join sys.index_columns ixc on ix.object_id=ixc.object_id and ix.index_id= ixc.index_id
                                         inner join sys.columns col on ixc.object_id =col.object_id  and ixc.column_id=col.column_id
                                         where ix.type>0 and (ix.is_primary_key=1 or ix.is_unique_constraint=1)
                                         and schema_name(tb.schema_id)=@SchemaName and tb.name=@TableName and ix.name=@IndexName
                                         order by ixc.key_ordinal
                                         open CursorIndexColumn 
                                         fetch next from CursorIndexColumn into  @ColumnName, @IsDescendingKey, @IsIncludedColumn
                                         while (@@fetch_status=0)
                                         begin
                                          if @IsIncludedColumn=0 
                                            set @IndexColumns=@IndexColumns + @ColumnName  + case when @IsDescendingKey=1  then ' DESC, ' else  ' ASC, ' end
                                          else 
                                           set @IncludedColumns=@IncludedColumns  + @ColumnName  +', ' 
     
                                          fetch next from CursorIndexColumn into @ColumnName, @IsDescendingKey, @IsIncludedColumn
                                         end
                                         close CursorIndexColumn
                                         deallocate CursorIndexColumn
                                         set @IndexColumns = substring(@IndexColumns, 1, len(@IndexColumns)-1)
                                         set @IncludedColumns = case when len(@IncludedColumns) >0 then substring(@IncludedColumns, 1, len(@IncludedColumns)-1) else '' end
                                        --  print @IndexColumns
                                        --  print @IncludedColumns

                                        set @TSQLScripCreationIndex =''
                                        set @TSQLScripDisableIndex =''
                                        set  @TSQLScripCreationIndex='ALTER TABLE '+  QUOTENAME(@SchemaName) +'.'+ QUOTENAME(@TableName)+ ' ADD CONSTRAINT ' +  QUOTENAME(@IndexName) + @is_unique_constraint + @is_primary_key + +@IndexTypeDesc +  '('+@IndexColumns+') '+ 
                                         case when len(@IncludedColumns)>0 then CHAR(13) +'INCLUDE (' + @IncludedColumns+ ')' else '' end + 'WITH (' + @IndexOptions+ ') ON ' + QUOTENAME(@FileGroupName) + ';'  

                                        --print @TSQLScripCreationIndex
                                        --print @TSQLScripDisableIndex

                                        insert into #t
                                        select @TableName, @IndexName, @IndexColumns, @TSQLScripCreationIndex

                                        fetch next from CursorIndex into  @SchemaName, @TableName, @IndexName, @is_unique_constraint, @is_primary_key, @IndexTypeDesc, @IndexOptions, @FileGroupName

                                        end
                                        close CursorIndex
                                        deallocate CursorIndex

                                        select * from #t

                                        drop table #t
                                        ", whereIndex);
                DataTable tblPrimaryKeys = DB.GetTable(sql, DatabaseName, dbModel, ref message);
                #endregion

                #region ForeignKey
                //sql = string.Format(@"SELECT t.TABLE_NAME, t.CONSTRAINT_NAME
                //                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                //                    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS t ON TC.CONSTRAINT_NAME = t.CONSTRAINT_NAME 
                //                    {0} 
                //                    group by t.TABLE_NAME, t.CONSTRAINT_NAME
                //                    ORDER BY t.TABLE_NAME, t.CONSTRAINT_NAME", where);
                sql = string.Format(@"--- SCRIPT TO GENERATE THE CREATION SCRIPT OF ALL FOREIGN KEY CONSTRAINTS
                                        declare @ForeignKeyID int
                                        declare @ForeignKeyName varchar(4000)
                                        declare @ParentTableName varchar(4000)
                                        declare @ParentColumn varchar(4000)
                                        declare @ReferencedTable varchar(4000)
                                        declare @ReferencedColumn varchar(4000)
                                        declare @StrParentColumn varchar(max)
                                        declare @StrReferencedColumn varchar(max)
                                        declare @ParentTableSchema varchar(4000)
                                        declare @ReferencedTableSchema varchar(4000)
                                        declare @TSQLCreationFK varchar(max)
                                        declare @TableName varchar(4000)
                                        declare @IndexName varchar(4000)

                                        create table #t (
	                                        TABLE_NAME nvarchar(150),
	                                        KEY_NAME nvarchar(150),
                                            KEY_COLUMNS nvarchar(150),
	                                        KEY_VALUE nvarchar(max)
                                        )

                                        --Written by Percy Reyes www.percyreyes.com
                                        declare CursorFK cursor for select object_id, object_name( parent_object_id) , name
                                        from sys.foreign_keys
                                        {0}
                                        order by object_name( parent_object_id) , name
                                        open CursorFK
                                        fetch next from CursorFK into @ForeignKeyID, @TableName, @IndexName
                                        while (@@FETCH_STATUS=0)
                                        begin
                                         set @StrParentColumn=''
                                         set @StrReferencedColumn=''
                                         declare CursorFKDetails cursor for
                                          select  fk.name ForeignKeyName, schema_name(t1.schema_id) ParentTableSchema,
                                          object_name(fkc.parent_object_id) ParentTable, c1.name ParentColumn,schema_name(t2.schema_id) ReferencedTableSchema,
                                           object_name(fkc.referenced_object_id) ReferencedTable,c2.name ReferencedColumn
                                          from --sys.tables t inner join 
                                          sys.foreign_keys fk 
                                          inner join sys.foreign_key_columns fkc on fk.object_id=fkc.constraint_object_id
                                          inner join sys.columns c1 on c1.object_id=fkc.parent_object_id and c1.column_id=fkc.parent_column_id 
                                          inner join sys.columns c2 on c2.object_id=fkc.referenced_object_id and c2.column_id=fkc.referenced_column_id 
                                          inner join sys.tables t1 on t1.object_id=fkc.parent_object_id 
                                          inner join sys.tables t2 on t2.object_id=fkc.referenced_object_id 
                                          where fk.object_id=@ForeignKeyID
                                         open CursorFKDetails
                                         fetch next from CursorFKDetails into  @ForeignKeyName, @ParentTableSchema, @ParentTableName, @ParentColumn, @ReferencedTableSchema, @ReferencedTable, @ReferencedColumn
                                         while (@@FETCH_STATUS=0)
                                         begin    
                                          set @StrParentColumn=@StrParentColumn + ', ' + quotename(@ParentColumn)
                                          set @StrReferencedColumn=@StrReferencedColumn + ', ' + quotename(@ReferencedColumn)
  
                                             fetch next from CursorFKDetails into  @ForeignKeyName, @ParentTableSchema, @ParentTableName, @ParentColumn, @ReferencedTableSchema, @ReferencedTable, @ReferencedColumn
                                         end
                                         close CursorFKDetails
                                         deallocate CursorFKDetails

                                         set @StrParentColumn=substring(@StrParentColumn,2,len(@StrParentColumn)-1)
                                         set @StrReferencedColumn=substring(@StrReferencedColumn,2,len(@StrReferencedColumn)-1)
                                         set @TSQLCreationFK='ALTER TABLE '+quotename(@ParentTableSchema)+'.'+quotename(@ParentTableName)+' WITH CHECK ADD CONSTRAINT '+quotename(@ForeignKeyName)
                                         + ' FOREIGN KEY('+ltrim(@StrParentColumn)+') '+ char(13) +'REFERENCES '+quotename(@ReferencedTableSchema)+'.'+quotename(@ReferencedTable)+' ('+ltrim(@StrReferencedColumn)+') ' + char(13)
 
                                         --print @TSQLCreationFK

                                        insert into #t
                                        select @TableName, @IndexName, ltrim(@StrParentColumn), @TSQLCreationFK

                                        fetch next from CursorFK into @ForeignKeyID, @TableName, @IndexName 
                                        end
                                        close CursorFK
                                        deallocate CursorFK


                                        select * from #t

                                        drop table #t
                                        ", whereForeignKey);
                DataTable tblForeignKeys = DB.GetTable(sql, DatabaseName, dbModel, ref message);
                #endregion

                #region Trigger
                //sql = string.Format(@"select t.name as TABLE_NAME, trig.name as TRIGGERNAME from sys.triggers trig 
                //                    JOIN sys.tables t ON trig.parent_id = t.object_id 
                //                    {0} 
                //                    order by t.name, trig.name", whereIndex);
                sql = string.Format(@"select OBJECT_NAME(parent_obj) as TABLE_NAME, t.name as TRIGGERNAME, OBJECT_DEFINITION(id) AS TRIGGER_DEFINITION from sysobjects t 
                                    WHERE t.type = 'TR' {0} 
                                    order by TABLE_NAME, TRIGGERNAME", whereTrigger);
                DataTable tblTrigger = DB.GetTable(sql, DatabaseName, dbModel, ref message);
                #endregion

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
                        else if (columns.DATA_TYPE == "tinyint" || columns.DATA_TYPE == "smallint" || columns.DATA_TYPE == "int" || columns.DATA_TYPE == "datetime" || columns.DATA_TYPE == "smalldatetime")
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "]";
                        }
                        else if (columns.DATA_TYPE == "decimal")
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "] (" + Convert.ToString(row1["NUMERIC_PRECISION"]) + "," + Convert.ToString(row1["NUMERIC_SCALE"]) + ")";
                        }
                        else
                        {
                            columns.DATA_TYPE_NEW = "[" + Convert.ToString(row1["DATA_TYPE"]) + "]";
                        }

                        columns.DefaultValue = Convert.ToString(row1["DefaultValue"]);
                        columns.DefaultConstraintName = Convert.ToString(row1["DefaultConstraintName"]);
                        columns.IS_NULLABLE = (Convert.ToString(row1["IS_NULLABLE"]) == "YES" ? true : false);
                        if (row1["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                            columns.CHARACTER_MAXIMUM_LENGTH = Convert.ToInt32(row1["CHARACTER_MAXIMUM_LENGTH"]);
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
                        string CreateIndexScript = Convert.ToString(row1["CreateIndexScript"]).TrimStart().TrimEnd();
                        string KEY_COLUMNS = Convert.ToString(row1["KeyColumns"]);
                        //tableNames.IndexList.Add(INDEX_NAME);

                        //string COLUMN_NAME = Convert.ToString(row1["COLUMN_NAME"]);
                        //string INDEX_NAME = Convert.ToString(row1["INDEX_NAME"]);

                        GeneralModel generalModel = new GeneralModel();
                        generalModel.Name = INDEX_NAME;
                        generalModel.Value = CreateIndexScript + "\r\n";
                        generalModel.Columns = KEY_COLUMNS;

                        tableNames.IndexListGeneral.Add(generalModel);
                    }
                    #endregion

                    #region PrimaryKey Load
                    DataRow[] rowsKeyPrimary = tblPrimaryKeys.Select($"TABLE_NAME = '{tableNames.TABLE_NAME}'");
                    for (int j = 0; j < rowsKeyPrimary.Length; j++)
                    {
                        DataRow row1 = rowsKeyPrimary[j];

                        string KEY_NAME = Convert.ToString(row1["KEY_NAME"]);
                        string KEY_VALUE = Convert.ToString(row1["KEY_VALUE"]);
                        string KEY_COLUMNS = Convert.ToString(row1["KEY_COLUMNS"]);
                        //tableNames.KeyList.Add(CONSTRAINT_NAME);

                        GeneralModel generalModel = new GeneralModel();
                        generalModel.Name = KEY_NAME;
                        generalModel.Value = KEY_VALUE + "\r\n";
                        generalModel.Columns = KEY_COLUMNS;

                        tableNames.PrimaryKeyListGeneral.Add(generalModel);
                    }
                    #endregion

                    #region Foreign Load
                    DataRow[] rowsKeyForeign = tblForeignKeys.Select($"TABLE_NAME = '{tableNames.TABLE_NAME}'");
                    for (int j = 0; j < rowsKeyForeign.Length; j++)
                    {
                        DataRow row1 = rowsKeyForeign[j];

                        string KEY_NAME = Convert.ToString(row1["KEY_NAME"]);
                        string KEY_VALUE = Convert.ToString(row1["KEY_VALUE"]);
                        string KEY_COLUMNS = Convert.ToString(row1["KEY_COLUMNS"]);
                        //tableNames.KeyList.Add(CONSTRAINT_NAME);

                        GeneralModel generalModel = new GeneralModel();
                        generalModel.Name = KEY_NAME;
                        generalModel.Value = KEY_VALUE + "\r\n";
                        generalModel.Columns = KEY_COLUMNS;

                        tableNames.ForeignKeyListGeneral.Add(generalModel);
                    }
                    #endregion

                    #region Trigger Load
                    DataRow[] rowsTrigger = tblTrigger.Select($"TABLE_NAME = '{tableNames.TABLE_NAME}'");
                    for (int j = 0; j < rowsTrigger.Length; j++)
                    {
                        DataRow row1 = rowsTrigger[j];

                        string TABLE_NAME = Convert.ToString(row1["TABLE_NAME"]);
                        string TRIGGERNAME = Convert.ToString(row1["TRIGGERNAME"]);
                        //tableNames.TriggerNameList.Add(TRIGGERNAME);

                        string TRIGGER_DEFINITION = Convert.ToString(row1["TRIGGER_DEFINITION"]).TrimStart().TrimEnd();
                        //tableNames.TriggerDefinitionList.Add(TRIGGER_DEFINITION);
                        //int start = TRIGGER_DEFINITION.IndexOf("/******");
                        //int finish = TRIGGER_DEFINITION.IndexOf("******/");

                        //if (start != -1 && finish != -1)
                        //{
                        //    TRIGGER_DEFINITION = TRIGGER_DEFINITION.Remove(start, finish + 7);
                        //}

                        int finish = TRIGGER_DEFINITION.IndexOf("CREATE TRIGGER");

                        TRIGGER_DEFINITION = TRIGGER_DEFINITION.Remove(0, finish);

                        GeneralModel generalModel = new GeneralModel();
                        generalModel.Name = TRIGGERNAME;
                        generalModel.Value = TRIGGER_DEFINITION + "\r\n";
                        //generalModel.SatirListe = TRIGGER_DEFINITION.Split('\n');
                        generalModel.ValueReplace = TRIGGER_DEFINITION.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("dbo." + TABLE_NAME, "[dbo].[" + TABLE_NAME + "]");

                        tableNames.TriggerDefinitionListGeneral.Add(generalModel);

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

        private void DBAllLoad(string DatabaseName, ref List<TableNames> list, DBModel dbModel)
        {
            string message = "";
            try
            {
                //string where = "where ROUTINE_TYPE = 'PROCEDURE' and ROUTINE_NAME not like 'XXX%'";
                //string sql = string.Format($"select ROUTINE_NAME, ROUTINE_DEFINITION from INFORMATION_SCHEMA.ROUTINES t {where} order by t.ROUTINE_NAME");

                string where = "and obj.name not like 'XXX%'";
                string sql = string.Format(@"select RTRIM(obj.type) SqlType, obj.name AS ROUTINE_NAME, code.definition AS ROUTINE_DEFINITION from sys.objects as obj
                                            join sys.sql_modules as code on code.object_id = obj.object_id
                                            join sys.schemas as sch on sch.schema_id = obj.schema_id
                                            where obj.type in ('P', 'FN', 'TF', 'V', 'TR') {0} order by obj.name", where);

                DataTable tbl = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow row = tbl.Rows[i];

                    TableNames all = new TableNames();
                    all.Name = Convert.ToString(row["ROUTINE_NAME"]);
                    all.SQL = Convert.ToString(row["ROUTINE_DEFINITION"]) + "\r\nGO\r\n\r\n";
                    all.SQLReplace = all.SQL.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                    string SqlType = Convert.ToString(row["SqlType"]);
                    if (SqlType == "P") all.SqlType = "StoredProcedure";
                    else if (SqlType == "FN" || SqlType == "TF") all.SqlType = "Function";
                    else if (SqlType == "V") all.SqlType = "View";
                    else if (SqlType == "TR") all.SqlType = "Trigger";
                    list.Add(all);
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
                //string where = "where ROUTINE_TYPE = 'PROCEDURE' and ROUTINE_NAME not like 'XXX%'";
                //string sql = string.Format($"select ROUTINE_NAME, ROUTINE_DEFINITION from INFORMATION_SCHEMA.ROUTINES t {where} order by t.ROUTINE_NAME");

                string where = "and obj.name not like 'XXX%'";
                string sql = string.Format(@"select obj.name AS ROUTINE_NAME, code.definition AS ROUTINE_DEFINITION from sys.objects as obj
                                            join sys.sql_modules as code on code.object_id = obj.object_id
                                            join sys.schemas as sch on sch.schema_id = obj.schema_id
                                            where obj.type = 'P' {0} order by obj.name", where);

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
                //string where = "where TABLE_NAME not like 'XXX%'";
                //string sql = string.Format($"select TABLE_NAME, VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS t {where} order by t.TABLE_NAME");

                string where = "and obj.name not like 'XXX%'";
                string sql = string.Format(@"select obj.name AS TABLE_NAME, code.definition AS VIEW_DEFINITION from sys.objects as obj
                                            join sys.sql_modules as code on code.object_id = obj.object_id
                                            join sys.schemas as sch on sch.schema_id = obj.schema_id
                                            where obj.type = 'V' {0} order by obj.name", where);

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

        private void DBFunctionsLoad(string DatabaseName, ref List<TableNames> list, DBModel dbModel)
        {
            string message = "";
            try
            {
                //string where = "where TABLE_NAME not like 'XXX%'";
                //string sql = string.Format($"select TABLE_NAME, VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS t {where} order by t.TABLE_NAME");

                string where = "and obj.name not like 'XXX%'";
                string sql = string.Format(@"select obj.name AS TABLE_NAME, code.definition AS VIEW_DEFINITION from sys.objects as obj
                                            join sys.sql_modules as code on code.object_id = obj.object_id
                                            join sys.schemas as sch on sch.schema_id = obj.schema_id
                                            where obj.type in ('FN', 'TF') {0} order by obj.name", where);

                DataTable tbl = DB.GetTable(sql, DatabaseName, dbModel, ref message);

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow row = tbl.Rows[i];

                    TableNames view = new TableNames();
                    view.Name = Convert.ToString(row["TABLE_NAME"]);
                    view.SQL = Convert.ToString(row["VIEW_DEFINITION"]) + "\r\nGO\r\n\r\n";
                    view.SqlType = "Function";
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
                li.ForeColor = Color.Green;
                //li.BackColor = Color.Green;
                listViewTabloFarklar.Items.Add(li);
            }

            List<string> HedeftenKaynagaTabloFarki = tabloAdlariHedef.Except(tabloAdlariKaynak).ToList();
            foreach (string item in HedeftenKaynagaTabloFarki)
            {
                ListViewItem li = new ListViewItem();
                li.Text = item;
                li.SubItems.Add("Drop Table");
                li.ForeColor = Color.Red;
                //li.BackColor = Color.Red;
                listViewTabloFarklar.Items.Add(li);
            }


            lblTabloFarki.Text += " " + KaynaktanHedefeTabloFarki.Count.ToString() + " tablo eklenecek";
            lblTabloFarki.Text += " - " + HedeftenKaynagaTabloFarki.Count.ToString() + " tablo silinecek";
        }

        private void ProsedurFarkVarMi()
        {
            foreach (TableNames item in TableListKaynak.Where(f => f.SqlType == "StoredProcedure"))
            {
                TableNames table = TableListHedef.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item.Name);

                if (table == null)//sp yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Create SP");
                    li.ForeColor = Color.Green;
                    //li.BackColor = Color.Green;
                    item.Explanation = "Create SP";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item.Name).ErrFlag = true;
                }
                else if (table != null && table.SQLReplace != item.SQLReplace)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Update SP");
                    li.ForeColor = Color.Blue;
                    //li.BackColor = Color.Blue;
                    item.Explanation = "Update SP";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item.Name).ErrFlag = true;
                }
            }

            foreach (TableNames item in TableListHedef.Where(f => f.SqlType == "StoredProcedure"))
            {
                TableNames table = TableListKaynak.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item.Name);

                if (table == null)//sp yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Drop SP");
                    li.ForeColor = Color.Red;
                    //li.BackColor = Color.Red;
                    item.Explanation = "Drop SP";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListHedef.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item.Name).ErrFlag = true;
                }
            }

            lblProsedurFarki.Text += " " + TableListKaynak.Where(f => f.SqlType == "StoredProcedure" && f.ErrFlag).Count().ToString() + " prosedür eklenecek veya değişecek";
            lblProsedurFarki.Text += " - " + TableListHedef.Where(f => f.SqlType == "StoredProcedure" && f.ErrFlag).Count().ToString() + " prosedür silinecek";

            return;

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

            List<TableNames> KaynaktanHedefeFarkTablo = TableListKaynak.Where(f => f.SqlType == "StoredProcedure").Except(TableListHedef.Where(f => f.SqlType == "StoredProcedure")).ToList();
            List<string> KaynaktanHedefeFark = spNamesKaynak.Except(spNamesHedef).ToList();

            foreach (string item in KaynaktanHedefeFark)
            {
                TableNames table = KaynaktanHedefeFarkTablo.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item);

                if (table != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item;
                    li.SubItems.Add("Create SP");
                    li.ForeColor = Color.White;
                    li.BackColor = Color.Green;
                    table.Explanation = "Create SP";
                    li.Tag = table;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item).ErrFlag = true;
                }
            }

            List<TableNames> HedeftenKaynagaFarkTablo = TableListHedef.Where(f => f.SqlType == "StoredProcedure").Except(TableListKaynak.Where(f => f.SqlType == "StoredProcedure")).ToList();
            List<string> HedeftenKaynagaFark = spNamesHedef.Except(spNamesKaynak).ToList();

            foreach (string item in HedeftenKaynagaFark)
            {
                TableNames table = HedeftenKaynagaFarkTablo.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item);

                if (table != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item;
                    li.SubItems.Add("Drop SP");
                    li.ForeColor = Color.White;
                    li.BackColor = Color.Red;
                    table.Explanation = "Drop SP";
                    li.Tag = table;
                    listViewTabloFarklar.Items.Add(li);

                    TableListHedef.FirstOrDefault(f => f.SqlType == "StoredProcedure" && f.Name == item).ErrFlag = true;
                }
            }

            lblProsedurFarki.Text += " " + KaynaktanHedefeFark.Count.ToString() + " prosedür eklenecek";
            lblProsedurFarki.Text += " - " + HedeftenKaynagaFark.Count.ToString() + " prosedür silinecek";
        }

        private void ViewFarkVarMi()
        {
            foreach (TableNames item in TableListKaynak.Where(f => f.SqlType == "View"))
            {
                TableNames table = TableListHedef.FirstOrDefault(f => f.SqlType == "View" && f.Name == item.Name);

                if (table == null)//View yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Create View");
                    li.ForeColor = Color.Green;
                    //li.BackColor = Color.Green;
                    item.Explanation = "Create View";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "View" && f.Name == item.Name).ErrFlag = true;
                }
                else if (table != null && table.SQLReplace != item.SQLReplace)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Update View");
                    li.ForeColor = Color.Blue;
                    //li.BackColor = Color.Blue;
                    item.Explanation = "Update View";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "View" && f.Name == item.Name).ErrFlag = true;
                }
            }

            foreach (TableNames item in TableListHedef.Where(f => f.SqlType == "View"))
            {
                TableNames table = TableListKaynak.FirstOrDefault(f => f.SqlType == "View" && f.Name == item.Name);

                if (table == null)//View yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Drop View");
                    li.ForeColor = Color.Red;
                    //li.BackColor = Color.Red;
                    item.Explanation = "Drop View";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListHedef.FirstOrDefault(f => f.SqlType == "View" && f.Name == item.Name).ErrFlag = true;
                }
            }

            lblViewFarki.Text += " " + TableListKaynak.Where(f => f.SqlType == "View" && f.ErrFlag).Count().ToString() + " view eklenecek veya değişecek";
            lblViewFarki.Text += " - " + TableListHedef.Where(f => f.SqlType == "View" && f.ErrFlag).Count().ToString() + " view silinecek";

            return;

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

            List<TableNames> KaynaktanHedefeFarkTablo = TableListKaynak.Where(f => f.SqlType == "View").Except(TableListHedef.Where(f => f.SqlType == "View")).ToList();
            List<string> KaynaktanHedefeFark = viewNamesKaynak.Except(viewNamesHedef).ToList();

            foreach (string item in KaynaktanHedefeFark)
            {
                TableNames table = KaynaktanHedefeFarkTablo.FirstOrDefault(f => f.SqlType == "View" && f.Name == item);

                if (table != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item;
                    li.SubItems.Add("Create View");
                    li.ForeColor = Color.White;
                    li.BackColor = Color.Green;
                    table.Explanation = "Create View";
                    li.Tag = table;
                    listViewTabloFarklar.Items.Add(li);
                }
            }

            List<TableNames> HedeftenKaynagaFarkTablo = TableListHedef.Where(f => f.SqlType == "View").Except(TableListKaynak.Where(f => f.SqlType == "View")).ToList();
            List<string> HedeftenKaynagaFark = viewNamesHedef.Except(viewNamesKaynak).ToList();

            foreach (string item in HedeftenKaynagaFark)
            {
                TableNames table = HedeftenKaynagaFarkTablo.FirstOrDefault(f => f.SqlType == "View" && f.Name == item);

                if (table != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item;
                    li.SubItems.Add("Drop View");
                    li.ForeColor = Color.White;
                    li.BackColor = Color.Red;
                    table.Explanation = "Drop View";
                    li.Tag = table;
                    listViewTabloFarklar.Items.Add(li);
                }
            }

            lblViewFarki.Text += " " + KaynaktanHedefeFark.Count.ToString() + " view eklenecek";
            lblViewFarki.Text += " - " + HedeftenKaynagaFark.Count.ToString() + " view silinecek";
        }

        private void FunctionFarkVarMi()
        {
            foreach (TableNames item in TableListKaynak.Where(f => f.SqlType == "Function"))
            {
                TableNames table = TableListHedef.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item.Name);

                if (table == null)//Function yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Create Function");
                    li.ForeColor = Color.Green;
                    //li.BackColor = Color.Green;
                    item.Explanation = "Create Function";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item.Name).ErrFlag = true;
                }
                else if (table != null && table.SQLReplace != item.SQLReplace)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Update Function");
                    li.ForeColor = Color.Blue;
                    //li.BackColor = Color.Blue;
                    item.Explanation = "Update Function";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item.Name).ErrFlag = true;
                }
            }

            foreach (TableNames item in TableListHedef.Where(f => f.SqlType == "Function"))
            {
                TableNames table = TableListKaynak.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item.Name);

                if (table == null)//Function yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Drop Function");
                    li.ForeColor = Color.Red;
                    //li.BackColor = Color.Red;
                    item.Explanation = "Drop Function";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListHedef.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item.Name).ErrFlag = true;
                }
            }

            lblFunctionFarki.Text += " " + TableListKaynak.Where(f => f.SqlType == "Function" && f.ErrFlag).Count().ToString() + " function eklenecek veya değişecek";
            lblFunctionFarki.Text += " - " + TableListHedef.Where(f => f.SqlType == "Function" && f.ErrFlag).Count().ToString() + " function silinecek";

            return;

            List<string> functionNamesKaynak = new List<string>();
            foreach (var item in TableListKaynak.Where(f => f.SqlType == "Function"))
            {
                functionNamesKaynak.Add(item.Name);
            }

            List<string> functionNamesHedef = new List<string>();
            foreach (var item in TableListHedef.Where(f => f.SqlType == "Function"))
            {
                functionNamesHedef.Add(item.Name);
            }

            List<TableNames> KaynaktanHedefeFarkTablo = TableListKaynak.Where(f => f.SqlType == "Function").Except(TableListHedef.Where(f => f.SqlType == "Function")).ToList();
            List<string> KaynaktanHedefeFark = functionNamesKaynak.Except(functionNamesHedef).ToList();

            foreach (string item in KaynaktanHedefeFark)
            {
                TableNames table = KaynaktanHedefeFarkTablo.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item);

                if (table != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item;
                    li.SubItems.Add("Create Function");
                    li.ForeColor = Color.White;
                    li.BackColor = Color.Green;
                    table.Explanation = "Create Function";
                    li.Tag = table;
                    listViewTabloFarklar.Items.Add(li);
                }
            }

            List<TableNames> HedeftenKaynagaFarkTablo = TableListHedef.Where(f => f.SqlType == "Function").Except(TableListKaynak.Where(f => f.SqlType == "Function")).ToList();
            List<string> HedeftenKaynagaFark = functionNamesHedef.Except(functionNamesKaynak).ToList();

            foreach (string item in HedeftenKaynagaFark)
            {
                TableNames table = HedeftenKaynagaFarkTablo.FirstOrDefault(f => f.SqlType == "Function" && f.Name == item);

                if (table != null)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item;
                    li.SubItems.Add("Drop Function");
                    li.ForeColor = Color.White;
                    li.BackColor = Color.Red;
                    table.Explanation = "Drop Function";
                    li.Tag = table;
                    listViewTabloFarklar.Items.Add(li);
                }
            }

            lblFunctionFarki.Text += " " + KaynaktanHedefeFark.Count.ToString() + " function eklenecek";
            lblFunctionFarki.Text += " - " + HedeftenKaynagaFark.Count.ToString() + " function silinecek";
        }

        private void TriggerFarkVarMi()
        {
            foreach (TableNames item in TableListKaynak.Where(f => f.SqlType == "Trigger"))
            {
                TableNames table = TableListHedef.FirstOrDefault(f => f.SqlType == "Trigger" && f.Name == item.Name);

                if (table == null)//Trigger yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Create Trigger");
                    li.ForeColor = Color.Green;
                    //li.BackColor = Color.Green;
                    item.Explanation = "Create Trigger";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "Trigger" && f.Name == item.Name).ErrFlag = true;
                }
                else if (table != null && table.SQLReplace != item.SQLReplace)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Update Trigger");
                    li.ForeColor = Color.Blue;
                    //li.BackColor = Color.Blue;
                    item.Explanation = "Update Trigger";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListKaynak.FirstOrDefault(f => f.SqlType == "Trigger" && f.Name == item.Name).ErrFlag = true;
                }
            }

            foreach (TableNames item in TableListHedef.Where(f => f.SqlType == "Trigger"))
            {
                TableNames table = TableListKaynak.FirstOrDefault(f => f.SqlType == "Trigger" && f.Name == item.Name);

                if (table == null)//Trigger yoksa
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = item.Name;
                    li.SubItems.Add("Drop Trigger");
                    li.ForeColor = Color.Red;
                    //li.BackColor = Color.Red;
                    item.Explanation = "Drop Trigger";
                    li.Tag = item;
                    listViewTabloFarklar.Items.Add(li);

                    TableListHedef.FirstOrDefault(f => f.SqlType == "Trigger" && f.Name == item.Name).ErrFlag = true;
                }
            }

            lblTriggerFarki.Text += " " + TableListKaynak.Where(f => f.SqlType == "Trigger" && f.ErrFlag).Count().ToString() + " trigger eklenecek veya değişecek";
            lblTriggerFarki.Text += " - " + TableListHedef.Where(f => f.SqlType == "Trigger" && f.ErrFlag).Count().ToString() + " trigger silinecek";
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

                                columnsKaynak.Status = "AddColumn";
                                if (tableNamesHedef.ColumnList.Count >= columnsKaynak.ORDINAL_POSITION)
                                {
                                    columnsKaynak.Explanation = "Yeri değişecek";
                                    sql += "--Yeri değişecek - " + columnsKaynak.ORDINAL_POSITION;
                                }
                                else
                                {
                                    columnsKaynak.Explanation = "";
                                }
                                sql += "\r\n";
                                //sql += "GO\r\n";

                                columnsKaynak.SQL = sql;
                                ScriptSQL += columnsKaynak.SQL;
                                ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);

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

                                    columnsKaynak.Status = "UpdateColumn";
                                    columnsKaynak.Explanation = "Yeri farklı - Eski yeri(" + columnsHedef.ORDINAL_POSITION + ")";

                                    string sql = "--Yeri değişecek - " + tableNamesKaynak.TABLE_NAME + " " + columnsKaynak.COLUMN_NAME + " Eski yeri(" + columnsHedef.ORDINAL_POSITION + ") - Yeni yeri(" + columnsKaynak.ORDINAL_POSITION + ")\r\n";

                                    columnsKaynak.SQL = sql;
                                    ScriptSQL += columnsKaynak.SQL;
                                    ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);

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
                                    if (chkKolonBoyutuBuyukse.Checked && columnsKaynak.CHARACTER_MAXIMUM_LENGTH > columnsHedef.CHARACTER_MAXIMUM_LENGTH)
                                    {
                                        //if (columnsKaynak.CHARACTER_MAXIMUM_LENGTH > columnsHedef.CHARACTER_MAXIMUM_LENGTH)
                                        //{
                                        kaynaktanHedefeFark++;

                                        columnsKaynak.Status = "UpdateColumn";
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
                                        //sql += "GO\r\n";

                                        columnsKaynak.SQL = sql;
                                        ScriptSQL += columnsKaynak.SQL;
                                        ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);
                                        //}
                                    }
                                    else
                                    {
                                        kaynaktanHedefeFark++;

                                        columnsKaynak.Status = "UpdateColumn";
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
                                        //sql += "GO\r\n";

                                        columnsKaynak.SQL = sql;
                                        ScriptSQL += columnsKaynak.SQL;
                                        ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);
                                    }


                                }
                                #endregion
                                #region hem tipi farklı hem de sırası farklıysa bu işlem yapılır
                                if (columnsKaynak.ORDINAL_POSITION != columnsHedef.ORDINAL_POSITION && columnsKaynak.DATA_TYPE_NEW != columnsHedef.DATA_TYPE_NEW)
                                {
                                    kaynaktanHedefeFark++;

                                    columnsKaynak.Status = "UpdateColumn";
                                    columnsKaynak.Explanation = "Hem yeri hem tipi farklı - " + columnsHedef.ORDINAL_POSITION + " - " + columnsHedef.DATA_TYPE_NEW;
                                    ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);
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

                                            columnsKaynak.Status = "UpdateConstraint";
                                            columnsKaynak.Explanation = "CONSTRAINT Default farklı";

                                            string sql = string.Format($"exec sp_rename '{columnsHedef.DefaultConstraintName}', '{CONSTRAINT}', 'object'; --{columnsKaynak.COLUMN_NAME}\r\n");
                                            //sql += "GO\r\n";

                                            columnsKaynak.SQL = sql;
                                            ScriptSQL += columnsKaynak.SQL;
                                            ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);
                                        }
                                        #endregion
                                    }

                                    #region CONSTRAINT farklıysa bu işlem yapılır
                                    //if (columnsKaynak.DefaultConstraintName != columnsHedef.DefaultConstraintName)
                                    //{
                                    //    kaynaktanHedefeFark++;

                                    //    columnsKaynak.Status = "UpdateConstraint";
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
                        //if (tableNamesKaynak.KeyList.Count > 0)
                        //{
                        //    List<string> KeyFark = tableNamesKaynak.KeyList.Except(tableNamesHedef.KeyList).ToList();
                        //    #region Key farklıysa
                        //    if (KeyFark.Count > 0)
                        //    {
                        //        foreach (string item in KeyFark)
                        //        {
                        //            ListViewLoad(tableNamesKaynak, new Columns { Explanation = item, Status = "Key" }, tableNamesHedef);
                        //        }
                        //    }
                        //    #endregion
                        //}

                        if (tableNamesKaynak.PrimaryKeyListGeneral.Count > 0)
                        {
                            foreach (GeneralModel item in tableNamesKaynak.PrimaryKeyListGeneral)
                            {
                                GeneralModel generalHedef = tableNamesHedef.PrimaryKeyListGeneral.FirstOrDefault(f => f.Columns == item.Columns || f.Name == item.Name);

                                if (generalHedef == null)
                                {
                                    ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value, Status = "AddKey", SQL = item.Value }, tableNamesHedef);
                                    item.NewScript = true;
                                }
                                else
                                {
                                    string hedefValue = generalHedef.Value;
                                    if (item.Value != hedefValue && item.Columns != generalHedef.Columns)
                                    {
                                        ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value + "~" + hedefValue, Status = "UpdateKey" }, tableNamesHedef);
                                        item.NewScript = true;
                                    }
                                }
                            }
                        }

                        if (tableNamesKaynak.ForeignKeyListGeneral.Count > 0)
                        {
                            foreach (GeneralModel item in tableNamesKaynak.ForeignKeyListGeneral)
                            {
                                GeneralModel generalHedef = tableNamesHedef.ForeignKeyListGeneral.FirstOrDefault(f => f.Columns == item.Columns || f.Name == item.Name);

                                if (generalHedef == null)
                                {
                                    ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value, Status = "AddKey", SQL = item.Value }, tableNamesHedef);
                                    item.NewScript = true;
                                }
                                else
                                {
                                    string hedefValue = generalHedef.Value;
                                    if (item.Value != hedefValue && item.Columns != generalHedef.Columns)
                                    {
                                        ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value + "~" + hedefValue, Status = "UpdateKey" }, tableNamesHedef);
                                        item.NewScript = true;
                                    }
                                }
                            }
                        }
                    }

                    if (chkIndex.Checked)
                    {
                        //if (tableNamesKaynak.IndexListGeneral.Count > 0)
                        //{
                        //    List<string> IndexFark = tableNamesKaynak.IndexListGeneral.Except(tableNamesHedef.IndexListGeneral).ToList();
                        //    #region İndex farklıysa
                        //    if (IndexFark.Count > 0)
                        //    {
                        //        foreach (string item in IndexFark)
                        //        {
                        //            //EXEC sp_rename N'tEquip.IX_tEquip_3', N'IX_tEquip_Disabled', N'INDEX'

                        //            //string sql = string.Format($"exec sp_rename '{tableNamesKaynak.TABLE_NAME}.', '{item}', N'INDEX'; --{item}\r\n");
                        //            //sql += "GO\r\n";

                        //            //ScriptSQL += sql;
                        //            ListViewLoad(tableNamesKaynak, new Columns { Explanation = item, Status = "Index" }, tableNamesHedef);
                        //        }
                        //    }
                        //    #endregion
                        //}

                        if (tableNamesKaynak.IndexListGeneral.Count > 0)
                        {
                            foreach (GeneralModel item in tableNamesKaynak.IndexListGeneral)
                            {
                                GeneralModel generalHedef = tableNamesHedef.IndexListGeneral.FirstOrDefault(f => (f.Columns == item.Columns || f.Name == item.Name) && f.IsReady == false);

                                if (generalHedef == null)//index yoksa
                                {
                                    ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value, Status = "AddIndex", SQL = item.Value }, tableNamesHedef);
                                    item.NewScript = true;
                                }
                                else
                                {
                                    string hedefValue = generalHedef.Value;
                                    if (item.Value != hedefValue && item.Columns != generalHedef.Columns)
                                    {
                                        ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value + "~" + hedefValue, Status = "UpdateIndex" }, tableNamesHedef);
                                        item.NewScript = true;
                                    }
                                    else if (item.Value != hedefValue && item.Columns == generalHedef.Columns && item.Name.Substring(0, 6) != generalHedef.Name.Substring(0, 6))
                                    {
                                        string rename = string.Format("EXEC sp_rename N'{0}.{1}', N'{2}', N'INDEX'\r\n", tableNamesKaynak.TABLE_NAME, generalHedef.Name, item.Name);
                                        ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value + "~" + hedefValue, Status = "RenameIndex", SQL = rename }, tableNamesHedef);
                                        item.NewScript = true;
                                    }
                                }

                                item.IsReady = true;
                            }
                        }

                        //if (tableNamesKaynak.IndexList.Count > 0)
                        //{
                        //    foreach (GeneralModel item in tableNamesKaynak.IndexList)
                        //    {
                        //        GeneralModel generalHedef = tableNamesHedef.IndexList.FirstOrDefault(f => f.Name == item.Name);

                        //        if (generalHedef == null)
                        //        {
                        //            ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value, Status = "AddIndex" }, tableNamesHedef);
                        //        }
                        //        else
                        //        {
                        //            string hedefValue = generalHedef.Value;
                        //            if (item.Name == generalHedef.Name && item.Value != hedefValue)
                        //            {
                        //                string sql = string.Format($"exec sp_rename '{tableNamesKaynak.TABLE_NAME}.{generalHedef.Value}', '{item.Value}', N'INDEX'; --{generalHedef.Value}\r\n");
                        //                sql += "GO\r\n";

                        //                ScriptSQL += sql;
                        //                //columnsKaynak.SQL = sql;
                        //                //ScriptSQL += columnsKaynak.SQL;
                        //                ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value + "~" + hedefValue, Status = "UpdateIndex" }, tableNamesHedef);
                        //            }
                        //        }
                        //    }
                        //}
                    }

                    if (chkTrigger.Checked)
                    {
                        //if (tableNamesKaynak.TriggerNameList.Count > 0)
                        //{
                        //    List<string> TriggerFark = tableNamesKaynak.TriggerNameList.Except(tableNamesHedef.TriggerNameList).ToList();
                        //    #region Trigger farklıysa
                        //    if (TriggerFark.Count > 0)
                        //    {
                        //        foreach (string item in TriggerFark)
                        //        {
                        //            ListViewLoad(tableNamesKaynak, new Columns { Explanation = item, Status = "AddTrigger" }, tableNamesHedef);
                        //        }
                        //    }
                        //    #endregion
                        //}

                        //if (tableNamesKaynak.TriggerDefinitionList.Count > 0)
                        //{
                        //    List<string> TriggerFark = tableNamesKaynak.TriggerDefinitionList.Except(tableNamesHedef.TriggerDefinitionList).ToList();
                        //    #region Trigger farklıysa
                        //    if (TriggerFark.Count > 0)
                        //    {
                        //        foreach (string item in TriggerFark)
                        //        {
                        //            ListViewLoad(tableNamesKaynak, new Columns { Explanation = item, Status = "UpdateTrigger" }, tableNamesHedef);
                        //        }
                        //    }
                        //    #endregion
                        //}

                        if (tableNamesKaynak.TriggerDefinitionListGeneral.Count > 0)
                        {
                            foreach (GeneralModel item in tableNamesKaynak.TriggerDefinitionListGeneral)
                            {
                                GeneralModel generalHedef = tableNamesHedef.TriggerDefinitionListGeneral.FirstOrDefault(f => f.Name == item.Name);

                                if (generalHedef == null)
                                {
                                    ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value, Status = "AddTrigger", SQL = item.Value }, tableNamesHedef);
                                    item.NewScript = true;
                                }
                                else
                                {
                                    //string hedefValue = generalHedef.Value;
                                    //int compare = item.Value.Trim().CompareTo(hedefValue.Trim());
                                    //int compare1 = String.Compare(item.Value.Trim(), hedefValue.Trim(), StringComparison.OrdinalIgnoreCase);
                                    //int compare2 = String.CompareOrdinal(item.Value.Trim(), hedefValue.Trim());
                                    //bool areEqual = item.SatirListe.SequenceEqual(generalHedef.SatirListe);
                                    //if (compare == 1)
                                    //if (item.Value != hedefValue)
                                    //if (compare1 < 0)
                                    if (item.ValueReplace != generalHedef.ValueReplace)
                                    {
                                        ListViewLoad(tableNamesKaynak, new Columns { COLUMN_NAME = item.Name, Explanation = item.Value + "~" + generalHedef.Value, Status = "UpdateTrigger", SQL = item.Value.Replace("CREATE TRIGGER", "ALTER TRIGGER") }, tableNamesHedef);
                                        item.NewScript = true;
                                    }
                                }
                            }
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
                                //sql += "GO\r\n";

                                columnsKaynak.SQL = sql;
                                ScriptSQL += columnsKaynak.SQL;
                                ListViewLoad(tableNamesKaynak, columnsKaynak, tableNamesHedef);
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

        private void ListViewLoad(TableNames tableNamesKaynak, Columns columnsKaynak, TableNames tableNamesHedef)
        {
            listSQL.Add(columnsKaynak);

            if (columnsKaynak.Status != "AddTrigger" && columnsKaynak.Status != "UpdateTrigger" && columnsKaynak.Status != "Delete")
            {
                tableNamesKaynak.SQL += columnsKaynak.SQL;
                tableNamesKaynak.ErrFlag = true;
            }

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = tableNamesKaynak.TABLE_NAME;
            listViewItem.SubItems.Add(columnsKaynak.COLUMN_NAME);
            listViewItem.SubItems.Add(columnsKaynak.DATA_TYPE_NEW);
            listViewItem.SubItems.Add(columnsKaynak.ORDINAL_POSITION.ToString());
            listViewItem.SubItems.Add(columnsKaynak.Status);
            listViewItem.SubItems.Add(columnsKaynak.Explanation);
            listViewItem.Checked = columnsKaynak.Checked;

            TableNames tableNamesTag = new TableNames();
            if (columnsKaynak.Status == "UpdateTrigger" || columnsKaynak.Status == "AddTrigger" || columnsKaynak.Status == "UpdateIndex" || columnsKaynak.Status == "AddIndex" || columnsKaynak.Status == "UpdateKey" || columnsKaynak.Status == "AddKey")
            {
                tableNamesTag.SQL = columnsKaynak.Explanation;
            }
            else
            {
                tableNamesTag.SQL = columnsKaynak.SQL;
            }
            tableNamesTag.SqlType = columnsKaynak.Status;

            listViewItem.Tag = tableNamesTag;

            if (columnsKaynak.Status == "AddColumn")
            {
                listViewItem.ForeColor = Color.Green;
                //listViewItem.BackColor = Color.Green;
            }
            else if (columnsKaynak.Status == "UpdateColumn")
            {
                listViewItem.ForeColor = Color.Blue;
                //listViewItem.BackColor = Color.Blue;
            }
            else if (columnsKaynak.Status == "Delete")
            {
                listViewItem.ForeColor = Color.Red;
                //listViewItem.BackColor = Color.Red;
            }

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
            if (listViewKolonFarki.CheckedItems.Count == 0)
            {
                MessageBox.Show("Seçili satır yok", "Uyarı");
                return;
            }

            foreach (Columns item in listSQL)
            {
                item.Checked = false;
            }

            foreach (ListViewItem item in listViewKolonFarki.CheckedItems)
            {
                TableNames tableNamesTag = (TableNames)item.Tag;
                if (listSQL.FirstOrDefault(f => f.SQL == tableNamesTag.SQL.ToString()) != null) listSQL.FirstOrDefault(f => f.SQL == tableNamesTag.SQL.ToString()).Checked = item.Checked;
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
                itemKolonFarki = listViewKolonFarki.GetItemAt(e.X, e.Y);
                menuStrip.Show(listViewKolonFarki, e.Location);
            }
        }

        private void listViewTabloFarklar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                itemTabloFarki = listViewTabloFarklar.GetItemAt(e.X, e.Y);
                menuStripSolView.Show(listViewTabloFarklar, e.Location);
            }
        }


        ListViewItem itemKolonFarki;
        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TableNames tableNames = (TableNames)itemKolonFarki.Tag;

            if (tableNames == null)
            {
                MessageBox.Show("İşlem yapılamaıyor", "Uyarı");
                return;
            }

            if (e.ClickedItem.Text == "Run")
            {
                try
                {
                    string message = "";
                    string SQL = "";

                    if (tableNames.SqlType == "AddTrigger" || tableNames.SqlType == "AddIndex" || tableNames.SqlType == "AddKey")
                    {
                        SQL = tableNames.SQL.ToString().Split('~')[0];
                    }
                    else if (tableNames.SqlType == "UpdateTrigger")
                    {
                        SQL = tableNames.SQL.ToString().Split('~')[0].Replace("CREATE TRIGGER", "ALTER TRIGGER");
                    }
                    else if (tableNames.SqlType == "AddColumn" || tableNames.SqlType == "UpdateColumn" || tableNames.SqlType == "RenameIndex")//kolonlar ve RenameIndex için burası çalışıyor
                    {
                        //SQL = tableNames.SQL.Replace("\r\nGO", "");
                        SQL = tableNames.SQL;
                    }
                    else
                    {
                        MessageBox.Show(tableNames.SqlType + " tipi için çalışma yapılmadı", "Uyarı");
                        return;
                    }

                    DialogResult secenek = MessageBox.Show("Seçili olan satırı çalıştırmak istediğinize emin misiniz?", "Bilgilendirme Penceresi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (secenek == DialogResult.Yes)
                    {
                        bool retVal = DB.ExecSql(SQL, cmbHedefDB.Text, (DBModel)cmbHedefDB.Tag, ref message);
                        if (retVal)
                        {
                            MessageBox.Show("Seçili olan satır çalıştı", "Uyarı");
                        }
                        else
                        {
                            MessageBox.Show(message, "Uyarı");
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            else if (e.ClickedItem.Text == "Kopyala" && (tableNames.SqlType == "AddColumn" || tableNames.SqlType == "UpdateColumn"))
            {
                if (tableNames != null) Clipboard.SetText(tableNames.SQL.ToString());
            }
            else if (e.ClickedItem.Text == "Kopyala" && (tableNames.SqlType == "AddIndex" || tableNames.SqlType == "UpdateIndex"))
            {
                if (tableNames != null) Clipboard.SetText(tableNames.SQL.ToString().Split('~')[0]);
            }
            else if (e.ClickedItem.Text == "Compare" && (tableNames.SqlType == "UpdateTrigger" || tableNames.SqlType == "AddTrigger"))
            {
                try
                {
                    string kaynak = tableNames.SQL.ToString().Split('~')[0];
                    string hedef = tableNames.SQL.ToString().Split('~')[1];
                    CompareForm form = new CompareForm();
                    form.richTextBoxKaynak.Text = kaynak;
                    form.richTextBoxHedef.Text = hedef;
                    form.ShowDialog();

                    //CompareForm form = new CompareForm();
                    //form.richTextBoxKaynak.Text = kaynak;
                    ////form.richTextBoxHedef.Text = hedef;
                    //for (int i = 0; i < form.richTextBoxKaynak.Lines.Length; i++)
                    //{
                    //    string lineKaynak = form.richTextBoxKaynak.Lines[i];
                    //    string lineHedef = form.richTextBoxKaynak.Lines[i];

                    //    if (lineKaynak == lineHedef)
                    //    {
                    //        form.richTextBoxHedef.AppendText(lineHedef + "\n");
                    //    }
                    //    else
                    //    {
                    //        form.richTextBoxHedef.AppendText(lineHedef + "\n");
                    //    }
                    //}
                    ////form.richTextBoxKaynak.Text = kaynak;
                    ////form.richTextBoxHedef.Text = hedef;
                    //form.ShowDialog();
                }
                catch (Exception)
                {

                }
            }
        }

        ListViewItem itemTabloFarki;
        private void menuStripSolView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            TableNames tableNames = (TableNames)itemTabloFarki.Tag;

            if (tableNames == null)
            {
                MessageBox.Show("İşlem yapılamaıyor", "Uyarı");
                return;
            }

            if (e.ClickedItem.Text == "Run")
            {
                if (tableNames.Explanation == "Create SP" || tableNames.Explanation == "Create View" || tableNames.Explanation == "Create Function")
                {
                    DialogResult secenek = MessageBox.Show("Seçili olan satırı çalıştırmak istediğinize emin misiniz?", "Bilgilendirme Penceresi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (secenek == DialogResult.Yes)
                    {
                        try
                        {
                            string message = "";
                            string SQL = "";

                            if (tableNames.SqlType == "StoredProcedure" || tableNames.SqlType == "View" || tableNames.SqlType == "Function")
                            {
                                SQL = tableNames.SQL.Replace("\r\nGO", "");
                                //SQL = tableNames.SQL;

                                bool retVal = DB.ExecSql(SQL, cmbHedefDB.Text, (DBModel)cmbHedefDB.Tag, ref message);
                                if (retVal)
                                {
                                    MessageBox.Show("Seçili olan satır çalıştı", "Uyarı");
                                }
                                else
                                {
                                    MessageBox.Show(message, "Uyarı");
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                else
                {
                    MessageBox.Show(tableNames.Explanation + " tipi için çalışma yapılmadı", "Uyarı");
                }
            }
            else if (e.ClickedItem.Text == "Kopyala" && tableNames.SqlType == "StoredProcedure")
            {
                if (tableNames != null) Clipboard.SetText(tableNames.SQL.ToString());
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

        private void chkTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrigger.Checked)
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
                chkTrigger.Checked = false;
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
                btnRunExecSql.Enabled = false;
                string message = "";
                string sql = richTextBox1.Text;
                bool retVal = DB.ExecSql(sql, cmbHedefDB.Text, (DBModel)cmbHedefDB.Tag, ref message);
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

        private void btnConstraintUpdate_Click(object sender, EventArgs e)
        {
            DialogResult secenek = MessageBox.Show("Toplu Constraint güncellemek istediğinize emin misiniz?", "Bilgilendirme Penceresi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (secenek == DialogResult.Yes)
            {
                string message = "";
                string sql = @"-- Update Default Contrain
                            declare @sql varchar(max)

                            set @sql = ''
                            select @sql = @sql + 'exec sp_rename ''' + d.name + ''', ''DF_' + o.name + '_' + c.name + ''',''object'';'
                            from sysobjects o
                            inner join dbo.syscolumns c on o.id = c.id
                            inner join dbo.sysobjects d on c.cdefault = d.id
                            where d.name <> 'DF_' + o.name + '_' + c.name

                            select @sql
                            exec (@sql)";
                bool retVal = DB.ExecSql(sql, cmbHedefDB.Text, (DBModel)cmbHedefDB.Tag, ref message);
                if (retVal)
                {
                    MessageBox.Show("Constraintler güncellendi", "Uyarı");
                }
                else
                {
                    MessageBox.Show(message, "Uyarı");
                }
            }
        }

        private void chkHepsi_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHepsi.Checked)
            {
                for (int i = 0; i < listViewKolonFarki.Items.Count; i++)
                {
                    ListViewItem l = listViewKolonFarki.Items[i];
                    l.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < listViewKolonFarki.Items.Count; i++)
                {
                    ListViewItem l = listViewKolonFarki.Items[i];
                    l.Checked = false;
                }
            }
        }
    }
}
