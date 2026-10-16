using System;
using System.Windows.Forms;

namespace GooseFighterApp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // "--daotests" opens the DAO test screen instead of the login screen
            if (args.Length > 0 && args[0] == "--daotests")
            {
                Application.Run(new DaoTestForm());
            }
            else
            {
                Application.Run(new LoginForm());
            }
        }
    }
}