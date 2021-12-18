using System;
using System.Windows.Forms;

namespace cryptoFinance
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CurrentAssets ca = new CurrentAssets();
            Application.Run(new IntroForm(ca));
        }
    }
}
