using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem
{
    public class Utilities
    {
    }
    public static class Extensions
    {
        public static void WriteLog(this string str)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\logs.txt", true);
            sw.WriteLine(str + " " + DateTime.Now);
            sw.Close();

        }
    }
}
