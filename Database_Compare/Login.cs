using Database_Compare.Database;
using Database_Compare.Model;
using System;
using System.Data;
using System.Windows.Forms;

namespace Database_Compare
{
    public partial class Login : Form
    {
        ComboBox cmbBox;
        public Login(ComboBox ACmbBox)
        {
            InitializeComponent();
            this.cmbBox = ACmbBox;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (cmbBox.Tag != null)
            {
                DBModel dBModel = (DBModel)cmbBox.Tag;
                txtIp.Text = dBModel.Ip;
                txtUserName.Text = dBModel.UserName;
                txtPassword.Text = dBModel.Password;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string message = "";
            DBModel dbModel = new DBModel();
            dbModel.Ip = txtIp.Text;
            dbModel.UserName = txtUserName.Text;
            dbModel.Password = txtPassword.Text;

            string sql = "SELECT name, database_id, create_date FROM sys.databases where database_id>4 order by sys.databases.name; ";
            DataTable tbl = DB.GetTable(sql, "master", dbModel, ref message);
            if (tbl == null)
            {
                MessageBox.Show(message, "Uyarı");
                return;
            }

            cmbBox.Items.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow row = tbl.Rows[i];
                //DB.DatabaseList.Add(row["name"].ToString());
                cmbBox.Items.Add(row["name"].ToString());
            }

            cmbBox.Tag = dbModel;

            //Form1 main = new Form1();
            //main.ShowDialog();
            this.Close();
        }
    }
}
