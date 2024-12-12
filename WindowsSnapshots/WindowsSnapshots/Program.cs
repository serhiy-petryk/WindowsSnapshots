using System;
using System.Windows.Forms;

namespace WindowsSnapshots
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

            Settings.SetZipLibrary();

            Tests.GetAsterEntries();

            Application.Run(new MainForm());
        }
    }
}
