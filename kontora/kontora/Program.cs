using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace kontora
{
    static class Globals
    {
        public static SqlConnection connection = null;
    }
    static class Program
    {
    /// <summary>
    /// Главная точка входа для приложения.
    /// </summary>
    [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
