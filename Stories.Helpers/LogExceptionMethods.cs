using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Stories.Helpers
{
    public static class LogExceptionMethods
    {
        public static string Style(this Exception ex)
        {
            return $"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{ex.InnerException}";
        }

        public static void Log(this string message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "log\\";

            if (Directory.Exists(path))
            {
                var now = DateTime.Now;
                string file = now.ToPersianDateString().Replace('/', '-') + "_" + now.ToString("HH;mm;ss;fff") + ".txt";
                path = Path.Combine(path, file);
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(message);
                    sw.Close();
                }
            }
        }
    }
}
