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

        public static void ChangeDates(bool isTestMode, DateTime oldDate, int dayOffset)
        {
            var logFilesFolder = isTestMode ? TestLogFilesFolder : OriginalLogFilesFolder;
            var files = Directory.GetFiles(logFilesFolder);
            var logFiles = Directory.GetFiles(Path.Combine(logFilesFolder, "Logs"), $"{oldDate:yyyyMMdd}_*.*");
            foreach (var logFileName in logFiles)
            {
                if (!CheckLogFile(logFileName, oldDate))
                {
                    MessageBox.Show($"Check log file {logFileName}");
                    return;
                }
            }
        }

        private static bool CheckLogFile(string filename, DateTime oldDate)
        {
            var dateKey = oldDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            if (!filename.Contains(dateKey + "_")) return false;

            dateKey = oldDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture);
            var content = File.ReadAllText(filename);
            if (!content.Contains(dateKey)) return false;
            return true;
        }

        private static void ChangeFileDate(string filename, DateTime newDate)
        {
            var fileDate = File.GetCreationTimeUtc(filename);
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
    }
}
