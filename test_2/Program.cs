using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace test_2
{
    static class Program
    {
        public static string Username;
        public static string StoreName;
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new loginFormN());
        }
    }
}
