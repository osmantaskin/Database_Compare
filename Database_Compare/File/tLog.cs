using Database_Compare.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Compare.File
{
    class tLog
    {
        internal static bool LogType(string Message, string Type)
        {
            try
            {
                string file = Environment.CurrentDirectory + @"\Logs\";
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }

                string path = "";
                if (Type == "ERROR")
                {
                    path = Environment.CurrentDirectory + @"\Logs\" + DateTime.Now.ToShortDateString() + "_" + Type + ".log";
                }
                else
                {
                    path = Environment.CurrentDirectory + @"\Logs\" + Type + ".log";
                }
                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write("\r\nDate:" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " | " + Message + "\r\n");
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
