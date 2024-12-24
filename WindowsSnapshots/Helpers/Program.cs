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

            /*var maxDate = new DateTime(2024, 12, 19, 2, 0, 0);
            var fileDate = new DateTime(2024, 12, 19, 3, 0, 0);
            var d = fileDate.AddDays(-Convert.ToInt32(Math.Floor((fileDate - maxDate).TotalDays)) - 1);

            fileDate = new DateTime(2024, 12, 20, 1, 0, 0);
            d = fileDate.AddDays(-Convert.ToInt32(Math.Floor((fileDate - maxDate).TotalDays)) - 1);

            fileDate = new DateTime(2024, 12, 20, 3, 0, 0);
            d = fileDate.AddDays(-Convert.ToInt32(Math.Floor((fileDate - maxDate).TotalDays)) - 1);

            var a1 = 0;*/

            // ChangeFileDates.XCopy();
            // ChangeFileDates.XCopyOthers();

            // ChangeFileDates.AddMissingLogFile();
            // ChangeFileDates.RenameOldLogFile();

            // ChangeFileDates.CheckLogFolder();
            // ChangeFileDates.CheckDatesOfOthers();
            ChangeFileDates.ChangeMinDateOfOthers();
            ChangeFileDates.ChangeMaxDateOfOthers();
            ChangeFileDates.CheckDatesOfOthers();



            // ChangeFileDates.ChangeDates(true, new DateTime(2024,12,12), 1);

            Application.Run(new MainForm());
        }
    }
}
