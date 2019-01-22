using Database_Compare.File;
using Database_Compare.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Compare.Database
{
    class DB
    {
        //public static string Ip;
        //public static string UserName;
        //public static string Password;
        //public static List<string> DatabaseList = new List<string>();

        public static string DateFormat = "set dateformat dmy;";
        static public DataTable GetTable(string sql, string DatabaseName, DBModel dbModel, ref string ErrMsg)
        {
            if (sql == "") return null;
            string message = "";
            string Conn = string.Format($"Server={dbModel.Ip};Database={DatabaseName};UID={dbModel.UserName};PWD={dbModel.Password};MultipleActiveResultSets=True;Connection Timeout=10;");
            SqlDataAdapter adapter = null;
            SqlConnection conn = new SqlConnection(Conn);
            DataTable dt = null;

            try
            {
                adapter = new SqlDataAdapter(DateFormat + sql, conn);
                dt = new DataTable();
                adapter.Fill(dt);
                Clipboard.SetText(sql);
            }
            catch (Exception ex)
            {
                message = "Sql= " + sql + "\r\nMessage= " + ex.Message + "\r\nStack Trace= " + ex.StackTrace + "\r\nIp= " + dbModel.Ip;
                ErrMsg = "Sql= " + sql + "\r\nMessage= " + ex.Message + "\r\nIp= " + dbModel.Ip;
                tLog.LogType(message, "ERROR");
                dt = null;
                Clipboard.SetText(ErrMsg);
            }

            adapter.Dispose(); adapter = null; conn.Close(); conn.Dispose(); conn = null;
            return dt;
        }

        static public bool ExecSql(string sql, string DatabaseName, DBModel dbModel, ref string ErrMsg)
        {
            bool status = false;
            string Conn = string.Format($"Server={dbModel.Ip};Database={DatabaseName};UID={dbModel.UserName};PWD={dbModel.Password};MultipleActiveResultSets=True;Connection Timeout=10;");
            SqlConnection connection = new SqlConnection(Conn);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            //SqlTransaction transaction;
            //transaction = connection.BeginTransaction("SampleTransaction");
            command.Connection = connection;
            //command.Transaction = transaction;
            command.CommandTimeout = 9999;
            try
            {
                try
                {
                    command.CommandText = DateFormat + sql;
                    command.ExecuteNonQuery();
                    //transaction.Commit();
                    status = true;
                }
                catch (Exception ex)
                {
                    string msg = sql + "\r\nMessage= " + ex.Message + "\r\nStack Trace= " + ex.StackTrace + ex.StackTrace + "\r\nIp= " + dbModel.Ip;
                    ErrMsg = "Sql= " + sql + "\r\nMessage= " + ex.Message + "\r\nIp= " + dbModel.Ip;
                    tLog.LogType(msg, "ERROR");
                    status = false;
                    try
                    {
                        //transaction.Rollback();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = sql + "\r\nMessage= " + ex.Message + "\r\nStack Trace= " + ex.StackTrace + ex.StackTrace + "\r\nIp= " + dbModel.Ip;
                ErrMsg = "Sql= " + sql + "\r\nMessage= " + ex.Message + "\r\nIp= " + dbModel.Ip;
                tLog.LogType(msg, "ERROR");
                status = false;
            }
            command.Dispose(); command = null; connection.Close(); connection.Dispose(); connection = null;
            return status;
        }
    }
}
