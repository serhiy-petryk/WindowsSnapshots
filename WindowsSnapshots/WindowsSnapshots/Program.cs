using System;
using System.IO;
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

            /*var files = Directory.GetFiles(@"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
            foreach (var file in files)
            {
                var bytes = File.ReadAllBytes(file);
                var text = File.ReadAllText(file);
            }
            var folders = Directory.GetDirectories(@"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
            foreach (var folder in folders)
            {
                var files2 = Directory.GetFiles(folder);
                foreach (var file in files2)
                {
                    var bytes = File.ReadAllBytes(file);
                    var text = File.ReadAllText(file);
                }
            }*/


            // Tests.GetAsterEntries();

            Application.Run(new MainForm());
        }
    }
}
