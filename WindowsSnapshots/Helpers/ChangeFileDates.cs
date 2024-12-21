using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Helpers
{
    public static class ChangeFileDates
    {
        private const string TestLogFilesFolder = @"E:\Temp\WindowsSnapshots\Data\Tests";
        private const string OriginalLogFilesFolder = @"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}";

        public static void RenameOldLogFile() // for 20231215_021335_00040_wp01s.log
        {
            // Save folder dates
            var rootFolder = GetRootFolder(true);
            var logFolder = Path.Combine(rootFolder, "logs");
            var rootDates = GetFolderDates(rootFolder);
            var logDates = GetFolderDates(logFolder);

            var oldFileName = @"20231215_021335_00040_wp01s.log";
            var newFileName = @"20241215_021335_00040_wp01s.log";
            var oldFullFileName = Path.Combine(logFolder, oldFileName);
            var newFullFileName = Path.Combine(logFolder, newFileName);
            var newFileDate = new DateTime(2024, 12, 15) + File.GetCreationTime(oldFullFileName).TimeOfDay;
            var oldDateKey = "2023-Dec-15";
            var newDateKey = "2024-Dec-15";

            // Change content of new file
            var content = File.ReadAllText(oldFullFileName);
            content = content.Replace(oldDateKey, newDateKey);
            File.WriteAllText(oldFullFileName, content);
            File.Move(oldFullFileName, newFullFileName);

            // Set dates of new file
            File.SetCreationTime(newFullFileName, newFileDate);
            File.SetLastWriteTime(newFullFileName, newFileDate);
            if (File.GetLastAccessTime(newFullFileName) < newFileDate)
                File.SetLastAccessTime(newFullFileName, newFileDate);

            // Restore folder dates
            SetFolderDates(logFolder, logDates);
            SetFolderDates(rootFolder, rootDates);
        }

        public static void AddMissingLogFile() // add 20241216_021939_00063_wp01s.log file
        {
            var rootFolder = GetRootFolder(true);
            var logFolder = Path.Combine(rootFolder, "logs");
            var rootDates = GetFolderDates(rootFolder);
            var logDates = GetFolderDates(logFolder);

            var sourceFilenames = Directory.GetFiles(logFolder, "20241216_021939_00062_wp01s.log");
            if (sourceFilenames.Length != 1)
                throw new Exception("AAAAAAAA");
            var targetFileName = Path.Combine(logFolder, "20241216_022939_00063_wp01s.log");

            var content = File.ReadAllText(sourceFilenames[0]).Replace("02:19:39", "02:29:39");
            File.WriteAllText(targetFileName, content);

            var dateTime = new DateTime(2024, 12, 16, 2, 29, 39);
            File.SetCreationTime(targetFileName, dateTime);
            File.SetLastWriteTime(targetFileName, dateTime);
            File.SetLastAccessTime(targetFileName, dateTime.AddHours(2));

            SetFolderDates(logFolder, logDates);
            SetFolderDates(rootFolder, rootDates);
        }

        private static DateTime[] GetFolderDates(string folder) => new DateTime[]
        {
            Directory.GetCreationTime(folder), Directory.GetLastWriteTime(folder), Directory.GetLastAccessTime(folder)
        };

        private static void SetFolderDates(string folder, DateTime[] dates)
        {
            if (Directory.GetCreationTime(folder) != dates[0])
                Directory.SetCreationTime(folder, dates[0]);
            if (Directory.GetLastWriteTime(folder) != dates[1])
                Directory.SetLastWriteTime(folder, dates[1]);
            if (Directory.GetLastAccessTime(folder) != dates[2])
                Directory.SetLastAccessTime(folder, dates[2]);
        }

        // =========================
        // =========================
        // =========================

        public static void ChangeDates(bool isTestMode, DateTime oldDate, int dayOffset)
        {
            var rootFolder = GetRootFolder(isTestMode);
            CheckLogFolder(rootFolder);
            var newDate = oldDate.AddDays(dayOffset).Date;

            var rootFolderDates = GetFolderDates(rootFolder);
            for (var k = 0; k < rootFolderDates.Length; k++)
            {
                if (rootFolderDates[k] < newDate)
                    rootFolderDates[k] = newDate + rootFolderDates[k].TimeOfDay;
            }

            var logFolder = Path.Combine(rootFolder, "Logs");
            var logFolderDates = GetFolderDates(logFolder);
            for (var k = 0; k < logFolderDates.Length; k++)
            {
                if (logFolderDates[k] < newDate)
                    logFolderDates[k] = newDate + logFolderDates[k].TimeOfDay;
            }

            var oldDateKey = oldDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture);
            var newDateKey = newDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture);
            var logFiles = Directory.GetFiles(logFolder, $"{oldDate:yyyyMMdd}_*.*");
            foreach (var logFileName in logFiles)
            {
                var newFileName = Path.Combine(logFolder,
                    newDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture) +
                    Path.GetFileName(logFileName).Substring(8));
                var fileDate = File.GetCreationTimeUtc(logFileName).AddDays(dayOffset);

                // Change content of new file
                var content = File.ReadAllText(logFileName);
                content = content.Replace(oldDateKey, newDateKey);
                File.WriteAllText(logFileName, content);
                File.Move(logFileName, newFileName);

                // Set dates of new file
                File.SetCreationTimeUtc(newFileName, fileDate);
                File.SetLastWriteTimeUtc(newFileName, fileDate);
                if (File.GetLastAccessTimeUtc(newFileName) < fileDate)
                    File.SetLastAccessTimeUtc(newFileName, fileDate);
            }
            SetFolderDates(logFolder, logFolderDates);

            var files = Directory.GetFiles(rootFolder);
            foreach(var file in files)
                ChangeFileDate(file, newDate);

            SetFolderDates(rootFolder, rootFolderDates);

            CheckLogFolder(rootFolder);
        }

        private static void ChangeFileDate(string filename, DateTime newDate)
        {
            var fileDate = File.GetCreationTime(filename);
            if (fileDate < newDate)
                File.SetCreationTime(filename, newDate + fileDate.TimeOfDay);

            fileDate = File.GetLastWriteTime(filename);
            if (fileDate < newDate)
                File.SetLastWriteTime(filename, newDate + fileDate.TimeOfDay);

            fileDate = File.GetLastAccessTime(filename);
            if (fileDate < newDate)
                File.SetLastAccessTime(filename, newDate + fileDate.TimeOfDay);
        }

        // ==============================
        // ==============================
        // ==============================
        public static void CheckLogFolder()
        {
            CheckLogFolder(TestLogFilesFolder);
        }

        private static void CheckLogFolder(string logFilesFolder)
        {
            var files = Directory.GetFiles(logFilesFolder);
            var logFiles = Directory.GetFiles(Path.Combine(logFilesFolder, "Logs"));
            foreach (var logFileName in logFiles)
            {
                if (!CheckLogFile(logFileName))
                {
                    MessageBox.Show($"Check log file {logFileName}");
                    return;
                }
            }
        }

        private static bool CheckLogFile(string fullFilename)
        {
            var extension = Path.GetExtension(fullFilename);
            if (!string.Equals(extension, ".log"))
                throw new Exception($"Invalid file extension: {fullFilename}");

            // Compare file dates and filename
            var ss = Path.GetFileNameWithoutExtension(fullFilename).Split('_');
            var year = int.Parse(ss[0].Substring(0, 4));
            var month = int.Parse(ss[0].Substring(4, 2));
            var day = int.Parse(ss[0].Substring(6, 2));
            var hour = int.Parse(ss[1].Substring(0, 2));
            var minute = int.Parse(ss[1].Substring(2, 2));
            var second = int.Parse(ss[1].Substring(4, 2));
            var no = int.Parse(ss[2]);

            var date = new DateTime(year, month, day, hour, minute, second);

            var fileDate = File.GetCreationTime(fullFilename);
            var difference = fileDate - date;
            if (difference > TimeSpan.FromSeconds(2))
                throw new Exception($"Invalid file creation date: {fullFilename}");

            fileDate = File.GetLastWriteTime(fullFilename);
            difference = fileDate - date;
            if (difference > TimeSpan.FromMinutes(1))
                throw new Exception($"Invalid file last write date: {fullFilename}");

            var content = File.ReadAllText(fullFilename);
            var dateKey = fileDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture);
            if (!content.Contains(dateKey))
                throw new Exception($"Invalid content of log file: {fullFilename}");

            return true;
        }


        //==============================
        //==============================
        //==============================

        public static void XCopy()
        {
            var oldFolder = OriginalLogFilesFolder;
            var newFolder = TestLogFilesFolder;
            // var newFolder = @"E:\Temp\WindowsSnapshots\Data\20D04FE0-3AEA-1069-A2D8-08002B30309D_20241220";


            // Delete old data from newFolder
            if (Directory.Exists(newFolder))
                RecursiveFolderDelete(newFolder);

            XCopy(oldFolder,newFolder);
        }

        private static void XCopy(string oldFolder, string newFolder)
        {
            if (!Directory.Exists(newFolder))
                Directory.CreateDirectory(newFolder);

            var oldFolders = Directory.GetDirectories(oldFolder);
            foreach (var folder in oldFolders)
            {
                var newFolder2 = Path.Combine(newFolder, Path.GetFileName(folder));
                XCopy(folder, newFolder2);
            }

            var files = Directory.GetFiles(oldFolder);
            foreach (var oldFile in files)
            {
                var newFileName = Path.Combine(newFolder, Path.GetFileName(oldFile));
                File.Copy(oldFile, newFileName);
                CopyFileAttributes(oldFile, newFileName);
            }

            CopyFileAttributes(oldFolder, newFolder);
        }

        private static void CopyFileAttributes(string oldFilename, string newFilename)
        {
            if (File.Exists(oldFilename))
            {
                File.SetCreationTimeUtc(newFilename, File.GetCreationTimeUtc(oldFilename));
                File.SetLastWriteTimeUtc(newFilename, File.GetLastWriteTimeUtc(oldFilename));
                File.SetLastAccessTimeUtc(newFilename, File.GetLastAccessTimeUtc(oldFilename));
            }
            else if (Directory.Exists(oldFilename))
            {
                Directory.SetCreationTimeUtc(newFilename, Directory.GetCreationTimeUtc(oldFilename));
                Directory.SetLastWriteTimeUtc(newFilename, Directory.GetLastWriteTimeUtc(oldFilename));
                Directory.SetLastAccessTimeUtc(newFilename, Directory.GetLastAccessTimeUtc(oldFilename));
            }
            else
                throw new Exception($"Can't find file {oldFilename}");
        }

        private static void RecursiveFolderDelete(string folder)
        {
            if (!Directory.Exists(folder))
                return;

            foreach (var dir in Directory.GetDirectories(folder))
            {
                RecursiveFolderDelete(dir);
            }
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                var fi = new FileInfo(file);
                if (fi.IsReadOnly)
                    fi.IsReadOnly = false;
                File.Delete(file);
            }
            
            Directory.Delete(folder);
        }

        //==============================
        //==============================
        //==============================

        private static string GetRootFolder(bool isTestMode) => isTestMode? TestLogFilesFolder : OriginalLogFilesFolder;
    }
}
