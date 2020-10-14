using System;
using System.Windows.Forms;

namespace CraftopiaRNGTool
{
    public static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        public static void Launch()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            form1 = form;
            form.Show();
        }

        public static bool isExit = false;
        public static bool isLoading = true;
        public static Form1 form1;
    }
}
