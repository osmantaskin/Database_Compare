namespace Database_Compare
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewTabloFarklar = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTabloFarki = new System.Windows.Forms.Label();
            this.lblKolonFarki = new System.Windows.Forms.Label();
            this.listViewKolonFarki = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.cmbKaynakDB = new System.Windows.Forms.ComboBox();
            this.cmbHedefDB = new System.Windows.Forms.ComboBox();
            this.btnBaglanHedef = new System.Windows.Forms.Button();
            this.btnBaglantiyiKopyala = new System.Windows.Forms.Button();
            this.btnSeciliOlanlariGetir = new System.Windows.Forms.Button();
            this.chkConstraint = new System.Windows.Forms.CheckBox();
            this.chkStoredProcedure = new System.Windows.Forms.CheckBox();
            this.chkTable = new System.Windows.Forms.CheckBox();
            this.chkView = new System.Windows.Forms.CheckBox();
            this.chkIndex = new System.Windows.Forms.CheckBox();
            this.btnBaglantiyiKopyalaKaynaga = new System.Windows.Forms.Button();
            this.chkKey = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIpKaynak = new System.Windows.Forms.TextBox();
            this.btnBaglanKaynak = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPasswordKaynak = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserNameKaynak = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtIpHedef = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPasswordHedef = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUserNameHedef = new System.Windows.Forms.TextBox();
            this.btnRunExecSql = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkTileBaslayanlar = new System.Windows.Forms.CheckBox();
            this.txtTableSearch = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewTabloFarklar
            // 
            this.listViewTabloFarklar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader7});
            this.listViewTabloFarklar.FullRowSelect = true;
            this.listViewTabloFarklar.GridLines = true;
            this.listViewTabloFarklar.Location = new System.Drawing.Point(8, 212);
            this.listViewTabloFarklar.Name = "listViewTabloFarklar";
            this.listViewTabloFarklar.Size = new System.Drawing.Size(235, 528);
            this.listViewTabloFarklar.TabIndex = 0;
            this.listViewTabloFarklar.UseCompatibleStateImageBehavior = false;
            this.listViewTabloFarklar.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tablo Adı";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Durumu";
            this.columnHeader7.Width = 100;
            // 
            // lblTabloFarki
            // 
            this.lblTabloFarki.AutoSize = true;
            this.lblTabloFarki.Location = new System.Drawing.Point(568, 168);
            this.lblTabloFarki.Name = "lblTabloFarki";
            this.lblTabloFarki.Size = new System.Drawing.Size(66, 13);
            this.lblTabloFarki.TabIndex = 1;
            this.lblTabloFarki.Text = "Tablo farkı : ";
            // 
            // lblKolonFarki
            // 
            this.lblKolonFarki.AutoSize = true;
            this.lblKolonFarki.Location = new System.Drawing.Point(569, 187);
            this.lblKolonFarki.Name = "lblKolonFarki";
            this.lblKolonFarki.Size = new System.Drawing.Size(66, 13);
            this.lblKolonFarki.TabIndex = 3;
            this.lblKolonFarki.Text = "Kolon farkı : ";
            // 
            // listViewKolonFarki
            // 
            this.listViewKolonFarki.CheckBoxes = true;
            this.listViewKolonFarki.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listViewKolonFarki.FullRowSelect = true;
            this.listViewKolonFarki.GridLines = true;
            this.listViewKolonFarki.Location = new System.Drawing.Point(249, 213);
            this.listViewKolonFarki.Name = "listViewKolonFarki";
            this.listViewKolonFarki.Size = new System.Drawing.Size(659, 527);
            this.listViewKolonFarki.TabIndex = 2;
            this.listViewKolonFarki.UseCompatibleStateImageBehavior = false;
            this.listViewKolonFarki.View = System.Windows.Forms.View.Details;
            this.listViewKolonFarki.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewKolonFarki_ItemChecked);
            this.listViewKolonFarki.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewKolonFarki_MouseClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tablo Adı";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Kolon Adı";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Tipi";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Sıra";
            this.columnHeader4.Width = 30;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Durumu";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Açıklama";
            this.columnHeader6.Width = 240;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(914, 212);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(679, 528);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(914, 12);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(119, 89);
            this.btnCompare.TabIndex = 2;
            this.btnCompare.Text = "Farkları Çıkar";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // cmbKaynakDB
            // 
            this.cmbKaynakDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKaynakDB.FormattingEnabled = true;
            this.cmbKaynakDB.Location = new System.Drawing.Point(79, 135);
            this.cmbKaynakDB.Name = "cmbKaynakDB";
            this.cmbKaynakDB.Size = new System.Drawing.Size(184, 21);
            this.cmbKaynakDB.TabIndex = 0;
            // 
            // cmbHedefDB
            // 
            this.cmbHedefDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHedefDB.FormattingEnabled = true;
            this.cmbHedefDB.Location = new System.Drawing.Point(79, 135);
            this.cmbHedefDB.Name = "cmbHedefDB";
            this.cmbHedefDB.Size = new System.Drawing.Size(184, 21);
            this.cmbHedefDB.TabIndex = 1;
            // 
            // btnBaglanHedef
            // 
            this.btnBaglanHedef.Location = new System.Drawing.Point(79, 106);
            this.btnBaglanHedef.Name = "btnBaglanHedef";
            this.btnBaglanHedef.Size = new System.Drawing.Size(184, 23);
            this.btnBaglanHedef.TabIndex = 14;
            this.btnBaglanHedef.Text = "Bağlan";
            this.btnBaglanHedef.UseVisualStyleBackColor = true;
            this.btnBaglanHedef.Click += new System.EventHandler(this.btnBaglanHedef_Click);
            // 
            // btnBaglantiyiKopyala
            // 
            this.btnBaglantiyiKopyala.Location = new System.Drawing.Point(189, 162);
            this.btnBaglantiyiKopyala.Name = "btnBaglantiyiKopyala";
            this.btnBaglantiyiKopyala.Size = new System.Drawing.Size(74, 23);
            this.btnBaglantiyiKopyala.TabIndex = 15;
            this.btnBaglantiyiKopyala.Text = "=>";
            this.btnBaglantiyiKopyala.UseVisualStyleBackColor = true;
            this.btnBaglantiyiKopyala.Click += new System.EventHandler(this.btnBaglantiyiKopyala_Click);
            // 
            // btnSeciliOlanlariGetir
            // 
            this.btnSeciliOlanlariGetir.Location = new System.Drawing.Point(914, 107);
            this.btnSeciliOlanlariGetir.Name = "btnSeciliOlanlariGetir";
            this.btnSeciliOlanlariGetir.Size = new System.Drawing.Size(119, 45);
            this.btnSeciliOlanlariGetir.TabIndex = 16;
            this.btnSeciliOlanlariGetir.Text = "Seçili olanları getir";
            this.btnSeciliOlanlariGetir.UseVisualStyleBackColor = true;
            this.btnSeciliOlanlariGetir.Click += new System.EventHandler(this.btnSeciliOlanlariGetir_Click);
            // 
            // chkConstraint
            // 
            this.chkConstraint.AutoSize = true;
            this.chkConstraint.Location = new System.Drawing.Point(3, 49);
            this.chkConstraint.Name = "chkConstraint";
            this.chkConstraint.Size = new System.Drawing.Size(73, 17);
            this.chkConstraint.TabIndex = 17;
            this.chkConstraint.Text = "Constraint";
            this.chkConstraint.UseVisualStyleBackColor = true;
            this.chkConstraint.CheckedChanged += new System.EventHandler(this.chkConstraint_CheckedChanged);
            // 
            // chkStoredProcedure
            // 
            this.chkStoredProcedure.AutoSize = true;
            this.chkStoredProcedure.Location = new System.Drawing.Point(4, 95);
            this.chkStoredProcedure.Name = "chkStoredProcedure";
            this.chkStoredProcedure.Size = new System.Drawing.Size(109, 17);
            this.chkStoredProcedure.TabIndex = 18;
            this.chkStoredProcedure.Text = "Stored Procedure";
            this.chkStoredProcedure.UseVisualStyleBackColor = true;
            // 
            // chkTable
            // 
            this.chkTable.AutoSize = true;
            this.chkTable.Checked = true;
            this.chkTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTable.Location = new System.Drawing.Point(3, 3);
            this.chkTable.Name = "chkTable";
            this.chkTable.Size = new System.Drawing.Size(53, 17);
            this.chkTable.TabIndex = 19;
            this.chkTable.Text = "Table";
            this.chkTable.UseVisualStyleBackColor = true;
            this.chkTable.CheckedChanged += new System.EventHandler(this.chkTable_CheckedChanged);
            // 
            // chkView
            // 
            this.chkView.AutoSize = true;
            this.chkView.Location = new System.Drawing.Point(3, 118);
            this.chkView.Name = "chkView";
            this.chkView.Size = new System.Drawing.Size(49, 17);
            this.chkView.TabIndex = 20;
            this.chkView.Text = "View";
            this.chkView.UseVisualStyleBackColor = true;
            // 
            // chkIndex
            // 
            this.chkIndex.AutoSize = true;
            this.chkIndex.Location = new System.Drawing.Point(4, 72);
            this.chkIndex.Name = "chkIndex";
            this.chkIndex.Size = new System.Drawing.Size(52, 17);
            this.chkIndex.TabIndex = 21;
            this.chkIndex.Text = "Index";
            this.chkIndex.UseVisualStyleBackColor = true;
            this.chkIndex.CheckedChanged += new System.EventHandler(this.chkIndex_CheckedChanged);
            // 
            // btnBaglantiyiKopyalaKaynaga
            // 
            this.btnBaglantiyiKopyalaKaynaga.Location = new System.Drawing.Point(79, 162);
            this.btnBaglantiyiKopyalaKaynaga.Name = "btnBaglantiyiKopyalaKaynaga";
            this.btnBaglantiyiKopyalaKaynaga.Size = new System.Drawing.Size(75, 23);
            this.btnBaglantiyiKopyalaKaynaga.TabIndex = 22;
            this.btnBaglantiyiKopyalaKaynaga.Text = "<=";
            this.btnBaglantiyiKopyalaKaynaga.UseVisualStyleBackColor = true;
            this.btnBaglantiyiKopyalaKaynaga.Click += new System.EventHandler(this.btnBaglantiyiKopyalaKaynaga_Click);
            // 
            // chkKey
            // 
            this.chkKey.AutoSize = true;
            this.chkKey.Location = new System.Drawing.Point(3, 26);
            this.chkKey.Name = "chkKey";
            this.chkKey.Size = new System.Drawing.Size(44, 17);
            this.chkKey.TabIndex = 23;
            this.chkKey.Text = "Key";
            this.chkKey.UseVisualStyleBackColor = true;
            this.chkKey.CheckedChanged += new System.EventHandler(this.chkKey_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtIpKaynak);
            this.panel1.Controls.Add(this.btnBaglanKaynak);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnBaglantiyiKopyala);
            this.panel1.Controls.Add(this.txtPasswordKaynak);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtUserNameKaynak);
            this.panel1.Controls.Add(this.cmbKaynakDB);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 194);
            this.panel1.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Kaynak :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Sunucu Ip :";
            // 
            // txtIpKaynak
            // 
            this.txtIpKaynak.Location = new System.Drawing.Point(79, 28);
            this.txtIpKaynak.Name = "txtIpKaynak";
            this.txtIpKaynak.Size = new System.Drawing.Size(184, 20);
            this.txtIpKaynak.TabIndex = 7;
            this.txtIpKaynak.Text = "10.3.20.61";
            // 
            // btnBaglanKaynak
            // 
            this.btnBaglanKaynak.Location = new System.Drawing.Point(79, 106);
            this.btnBaglanKaynak.Name = "btnBaglanKaynak";
            this.btnBaglanKaynak.Size = new System.Drawing.Size(184, 23);
            this.btnBaglanKaynak.TabIndex = 11;
            this.btnBaglanKaynak.Text = "Bağlan";
            this.btnBaglanKaynak.UseVisualStyleBackColor = true;
            this.btnBaglanKaynak.Click += new System.EventHandler(this.btnBaglanKaynak_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Şifre :";
            // 
            // txtPasswordKaynak
            // 
            this.txtPasswordKaynak.Location = new System.Drawing.Point(79, 80);
            this.txtPasswordKaynak.Name = "txtPasswordKaynak";
            this.txtPasswordKaynak.PasswordChar = '*';
            this.txtPasswordKaynak.Size = new System.Drawing.Size(184, 20);
            this.txtPasswordKaynak.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Kullanıcı Adı :";
            // 
            // txtUserNameKaynak
            // 
            this.txtUserNameKaynak.Location = new System.Drawing.Point(79, 54);
            this.txtUserNameKaynak.Name = "txtUserNameKaynak";
            this.txtUserNameKaynak.Size = new System.Drawing.Size(184, 20);
            this.txtUserNameKaynak.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.chkTable);
            this.panel2.Controls.Add(this.chkKey);
            this.panel2.Controls.Add(this.chkConstraint);
            this.panel2.Controls.Add(this.chkView);
            this.panel2.Controls.Add(this.chkIndex);
            this.panel2.Controls.Add(this.chkStoredProcedure);
            this.panel2.Location = new System.Drawing.Point(568, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(165, 140);
            this.panel2.TabIndex = 25;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txtIpHedef);
            this.panel3.Controls.Add(this.btnBaglantiyiKopyalaKaynaga);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.txtPasswordHedef);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.btnBaglanHedef);
            this.panel3.Controls.Add(this.cmbHedefDB);
            this.panel3.Controls.Add(this.txtUserNameHedef);
            this.panel3.Location = new System.Drawing.Point(290, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 194);
            this.panel3.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Hedef :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Sunucu Ip :";
            // 
            // txtIpHedef
            // 
            this.txtIpHedef.Location = new System.Drawing.Point(79, 28);
            this.txtIpHedef.Name = "txtIpHedef";
            this.txtIpHedef.Size = new System.Drawing.Size(184, 20);
            this.txtIpHedef.TabIndex = 7;
            this.txtIpHedef.Text = "10.3.20.61";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 261);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Şifre :";
            // 
            // txtPasswordHedef
            // 
            this.txtPasswordHedef.Location = new System.Drawing.Point(79, 80);
            this.txtPasswordHedef.Name = "txtPasswordHedef";
            this.txtPasswordHedef.PasswordChar = '*';
            this.txtPasswordHedef.Size = new System.Drawing.Size(184, 20);
            this.txtPasswordHedef.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Kullanıcı Adı :";
            // 
            // txtUserNameHedef
            // 
            this.txtUserNameHedef.Location = new System.Drawing.Point(79, 54);
            this.txtUserNameHedef.Name = "txtUserNameHedef";
            this.txtUserNameHedef.Size = new System.Drawing.Size(184, 20);
            this.txtUserNameHedef.TabIndex = 9;
            // 
            // btnRunExecSql
            // 
            this.btnRunExecSql.BackColor = System.Drawing.Color.Red;
            this.btnRunExecSql.Location = new System.Drawing.Point(1476, 147);
            this.btnRunExecSql.Name = "btnRunExecSql";
            this.btnRunExecSql.Size = new System.Drawing.Size(117, 59);
            this.btnRunExecSql.TabIndex = 26;
            this.btnRunExecSql.Text = "Script Çalıştır";
            this.btnRunExecSql.UseVisualStyleBackColor = false;
            this.btnRunExecSql.Click += new System.EventHandler(this.btnRunExecSql_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel4.Controls.Add(this.txtTableSearch);
            this.panel4.Controls.Add(this.chkTileBaslayanlar);
            this.panel4.Location = new System.Drawing.Point(739, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(165, 140);
            this.panel4.TabIndex = 26;
            // 
            // chkTileBaslayanlar
            // 
            this.chkTileBaslayanlar.AutoSize = true;
            this.chkTileBaslayanlar.Checked = true;
            this.chkTileBaslayanlar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTileBaslayanlar.Location = new System.Drawing.Point(3, 3);
            this.chkTileBaslayanlar.Name = "chkTileBaslayanlar";
            this.chkTileBaslayanlar.Size = new System.Drawing.Size(128, 17);
            this.chkTileBaslayanlar.TabIndex = 19;
            this.chkTileBaslayanlar.Text = "T ile başlayan tablolar";
            this.chkTileBaslayanlar.UseVisualStyleBackColor = true;
            // 
            // txtTableSearch
            // 
            this.txtTableSearch.Location = new System.Drawing.Point(3, 26);
            this.txtTableSearch.Name = "txtTableSearch";
            this.txtTableSearch.Size = new System.Drawing.Size(159, 20);
            this.txtTableSearch.TabIndex = 20;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1605, 752);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.btnRunExecSql);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSeciliOlanlariGetir);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lblKolonFarki);
            this.Controls.Add(this.listViewKolonFarki);
            this.Controls.Add(this.lblTabloFarki);
            this.Controls.Add(this.listViewTabloFarklar);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Compare";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewTabloFarklar;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lblTabloFarki;
        private System.Windows.Forms.Label lblKolonFarki;
        private System.Windows.Forms.ListView listViewKolonFarki;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Button btnBaglanHedef;
        private System.Windows.Forms.ComboBox cmbKaynakDB;
        private System.Windows.Forms.ComboBox cmbHedefDB;
        private System.Windows.Forms.Button btnBaglantiyiKopyala;
        private System.Windows.Forms.Button btnSeciliOlanlariGetir;
        private System.Windows.Forms.CheckBox chkConstraint;
        private System.Windows.Forms.CheckBox chkStoredProcedure;
        private System.Windows.Forms.CheckBox chkTable;
        private System.Windows.Forms.CheckBox chkView;
        private System.Windows.Forms.CheckBox chkIndex;
        private System.Windows.Forms.Button btnBaglantiyiKopyalaKaynaga;
        private System.Windows.Forms.CheckBox chkKey;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIpKaynak;
        private System.Windows.Forms.Button btnBaglanKaynak;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPasswordKaynak;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUserNameKaynak;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtIpHedef;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPasswordHedef;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUserNameHedef;
        private System.Windows.Forms.Button btnRunExecSql;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkTileBaslayanlar;
        private System.Windows.Forms.TextBox txtTableSearch;
    }
}

