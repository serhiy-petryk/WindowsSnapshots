﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // ChangeFileDates.XCopy();

            ChangeFileDates.CheckLogFolder();

            // ChangeFileDates.ChangeDates(true, new DateTime(2024,12,12), 1);

            Application.Run(new MainForm());
        }
    }
}