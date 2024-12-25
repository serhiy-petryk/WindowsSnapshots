using System;
using System.Windows.Forms;

namespace Helpers
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

            if (!Others.IsAdministrator())
            {
                MessageBox.Show("Run after administrator right");
                return;
            }

            ChangeFileDates.XCopy();
            // ChangeFileDates.XCopyOthers();

            // ChangeFileDates.AddMissingLogFile();
            // ChangeFileDates.RenameOldLogFile();

            // ChangeFileDates.ChangeDatesOfLogFolder(new DateTime(2024, 12, 12), 1);

            // ChangeFileDates.CheckLogFolder();
            // ChangeFileDates.CheckDatesOfOthers();
            // ChangeFileDates.ChangeMinDateOfOthers();
            // ChangeFileDates.ChangeMaxDateOfOthers();
            // ChangeFileDates.CheckDatesOfOthers();



            // ChangeFileDates.ChangeDates(true, new DateTime(2024,12,12), 1);

            Application.Run(new MainForm());
        }
    }
}
