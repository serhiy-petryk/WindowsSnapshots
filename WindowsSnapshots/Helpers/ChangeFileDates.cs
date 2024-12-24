using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Helpers
{
    public static class ChangeFileDates
    {
        private const string LogRootFolder = @"E:\Temp\WindowsSnapshots\Data\Tests";
        // private const string LogRootFolder = @"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}";

        private const string OtherFolderTest = @"E:\Temp\WindowsSnapshots\Data\Tests.Others";

        private static readonly string[] OtherFoldersOriginal = new[]
        {
            @"J:\Program Files\ASTER", @"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}",
            @"J:\ProgramData\IBIK Software Ltd\Uninstall",
            @"J:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASTER Control", @"J:\ProgramData\MultiSeat Utils"
        };
        private static readonly string[] OtherFilesOriginal = new[] { @"J:\Windows\System32\drivers\mutenx.sys" };

        private static readonly DateTime LowerDateTime = new DateTime(2024, 1, 1);
        private static readonly DateTime UpperDateTime = new DateTime(2024, 12, 19, 2, 0, 0);

        #region ========  Change minimum dates of other files/folders  ========
        public static void ChangeMinDateOfOthers()
        {
            var minDate = new DateTime(2024, 12, 1);
            var maxDate = new DateTime(2024, 12, 13);
            ChangeMinDateOfOthers(new []{minDate, maxDate});
        }

        private static void ChangeMinDateOfOthers(DateTime[] dateRange)
        {
            var otherFolder = new DirectoryInfo(OtherFolderTest);

            var files = otherFolder.GetFiles();
            var folders = otherFolder.GetDirectories();

            foreach (var file in files)
                ChangeMinDate(file, dateRange);

            foreach (var folder in folders)
                ChangeMinDateOfOthers_FolderRecursive(folder, dateRange);
        }

        private static void ChangeMinDateOfOthers_FolderRecursive(DirectoryInfo folder, DateTime[] dateRange)
        {
            // Change dates of subfolders
            foreach (var dir in folder.GetDirectories())
                ChangeMinDateOfOthers_FolderRecursive(dir, dateRange);

            // Change dates of files
            var files = folder.GetFiles();
            foreach (var file in files)
                ChangeMinDate(file, dateRange);

            // Change dates of this folder
            ChangeMinDate(folder, dateRange);
        }

        private static void ChangeMinDate(FileSystemInfo fsInfo, DateTime[] dateRange)
        {
            var minDate = dateRange[0];
            var maxDate = dateRange[1];

            if (fsInfo.CreationTime >= minDate && fsInfo.CreationTime < maxDate)
                fsInfo.CreationTime = GetNewFileDate(fsInfo.CreationTime);

            if (fsInfo.LastWriteTime >= minDate && fsInfo.LastWriteTime < maxDate)
                fsInfo.LastWriteTime = GetNewFileDate(fsInfo.LastWriteTime);

            if (fsInfo.LastAccessTime >= minDate && fsInfo.LastAccessTime < maxDate)
                fsInfo.LastAccessTime = GetNewFileDate(fsInfo.LastAccessTime);

            DateTime GetNewFileDate(DateTime fileDate) => fileDate.AddDays(Convert.ToInt32(Math.Floor((maxDate - fileDate).TotalDays)) + 1);
        }
        #endregion

        #region ========  Check dates of other files/folders  ========
        public static void CheckDatesOfOthers()
        {
            // var folders = OtherFoldersOriginal;
            // var files = OtherFilesOriginal;
            var folders = Directory.GetDirectories(OtherFolderTest);
            var files = Directory.GetFiles(OtherFolderTest);

            var minMaxDates = new[] {DateTime.MaxValue, DateTime.MinValue};
            var badFiles = new List<string>();
            foreach (var folder in folders)
            {
                var fileAndFolderCount = new[] { 0, 0 };
                RecursiveFolderCheckDates(folder, minMaxDates, fileAndFolderCount, badFiles);
                if (fileAndFolderCount[0] == 0 || fileAndFolderCount[1] == 0)
                    throw new Exception($"No file or folder in {folder}");
            }

            foreach (var file in files)
            {
                var fileDates = GetFileDates(file);
                CheckLowerDate(minMaxDates, fileDates, badFiles, file);
            }
        }

        private static void RecursiveFolderCheckDates(string folder, DateTime[] minMaxDates, int[] fileAndFolderCount, List<string> tempFiles)
        {
            foreach (var dir in Directory.GetDirectories(folder))
            {
                RecursiveFolderCheckDates(dir, minMaxDates, fileAndFolderCount, tempFiles);
            }
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                var fileDates = GetFileDates(file);
                CheckLowerDate(minMaxDates, fileDates, tempFiles, file);
                fileAndFolderCount[0]++;
            }

            var folderDates = GetFolderDates(folder);
            CheckLowerDate(minMaxDates, folderDates, tempFiles, folder);
            fileAndFolderCount[1]++;
        }

        private static void CheckLowerDate(DateTime[] minMaxDates, DateTime[] fileOrFolderDates, List<string> tempFiles, string filename)
        {
            var dates = fileOrFolderDates.Where(d => d >= LowerDateTime).ToArray();
            // var dates = fileOrFolderDates.Where(d => true).ToArray();
            if (dates.Length == 0) return;

            var minDate = dates.Min();
            var maxDate = dates.Max();
            if (minDate < minMaxDates[0]) minMaxDates[0] = minDate;
            if (maxDate > minMaxDates[1]) minMaxDates[1] = maxDate;

            if (maxDate > UpperDateTime)
            {
                tempFiles.Add($"{filename}\t{maxDate:G}");
            }
        }

        #endregion

        #region ==========  RenameOldLogFile  ===========
        public static void RenameOldLogFile() // for 20231215_021335_00040_wp01s.log
        {
            // Save folder dates
            var logFolder = Path.Combine(LogRootFolder, "logs");
            var rootDates = GetFolderDates(LogRootFolder);
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
            SetFolderDates(LogRootFolder, rootDates);
        }
        #endregion

        #region =========  AddMissingLogFile  ===========
        public static void AddMissingLogFile() // add 20241216_021939_00063_wp01s.log file
        {
            var logFolder = Path.Combine(LogRootFolder, "logs");
            var rootDates = GetFolderDates(LogRootFolder);
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
            SetFolderDates(LogRootFolder, rootDates);
        }
        #endregion

        #region =========  ChangeDatesOfLogFolder  ===========
        public static void ChangeDatesOfLogFolder(DateTime oldDate, int dayOffset)
        {
            CheckLogFolder(LogRootFolder);
            var newDate = oldDate.AddDays(dayOffset).Date;

            var rootFolderDates = GetFolderDates(LogRootFolder);
            for (var k = 0; k < rootFolderDates.Length; k++)
            {
                if (rootFolderDates[k] < newDate)
                    rootFolderDates[k] = newDate + rootFolderDates[k].TimeOfDay;
            }

            var logFolder = Path.Combine(LogRootFolder, "Logs");
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

            var files = Directory.GetFiles(LogRootFolder);
            foreach(var file in files)
                ChangeFileDate(file, newDate);

            SetFolderDates(LogRootFolder, rootFolderDates);

            CheckLogFolder(LogRootFolder);
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
        #endregion

        #region ========  CheckLogFolder  =========
        public static void CheckLogFolder()
        {
            CheckLogFolder(LogRootFolder);
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
        #endregion

        #region =============  XCopy others ==============
        public static void XCopyOthers()
        {
            var newFolder = new DirectoryInfo(OtherFolderTest);

            // Delete old data from newFolder
            if (newFolder.Exists)
                RecursiveFolderDelete(newFolder);
            
            newFolder.Create();
            newFolder.Refresh();

            // Copy files
            foreach (var oldFileName in OtherFilesOriginal)
            {
                var oldFile = new FileInfo(oldFileName);
                var newFile = new FileInfo(Path.Combine(newFolder.FullName, oldFile.Name));
                oldFile.CopyTo(newFile.FullName);
                CopyAttributes(oldFile, newFile);
            }

            foreach (var oldFolder in OtherFoldersOriginal)
                XCopyOthers(new DirectoryInfo(oldFolder), new DirectoryInfo(Path.Combine(newFolder.FullName, Path.GetFileName(oldFolder))));
        }

        private static void XCopyOthers(DirectoryInfo oldFolder, DirectoryInfo newFolder)
        {
            if (!newFolder.Exists)
            {
                newFolder.Create();
                newFolder.Refresh();
            }

            var oldFolders = oldFolder.GetDirectories();
            foreach (var folder in oldFolders)
            {
                var newFolder2 = new DirectoryInfo(Path.Combine(newFolder.FullName, folder.Name));
                XCopyOthers(folder, newFolder2);
            }

            var files = oldFolder.GetFiles();
            foreach (var oldFile in files)
            {
                var newFileName = Path.Combine(newFolder.FullName, oldFile.Name);
                oldFile.CopyTo(newFileName);
                CopyAttributes(oldFile, new FileInfo(newFileName));
            }

            CopyAttributes(oldFolder, newFolder);
        }
        #endregion

        #region =============  XCopy  ==============
        public static void XCopy()
        {
            var oldFolderName = @"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            var newFolderName = @"E:\Temp\WindowsSnapshots\Data\Tests";
            // var newFolder = @"E:\Temp\WindowsSnapshots\Data\20D04FE0-3AEA-1069-A2D8-08002B30309D_20241219.Original";

            var oldFolder = new DirectoryInfo(oldFolderName);
            var newFolder = new DirectoryInfo(newFolderName);

            // Delete old data from newFolder
            if (newFolder.Exists)
                RecursiveFolderDelete(newFolder);

            XCopy(oldFolder, newFolder);
        }

        private static void XCopy(DirectoryInfo oldFolder, DirectoryInfo newFolder)
        {
            if (!newFolder.Exists)
            {
                newFolder.Create();
                newFolder.Refresh();
            }

            var oldFolders = oldFolder.GetDirectories();
            foreach (var folder in oldFolders)
            {
                var newFolder2 = new DirectoryInfo(Path.Combine(newFolder.FullName, folder.Name));
                XCopy(folder, newFolder2);
            }

            var files = oldFolder.GetFiles();
            foreach (var oldFile in files)
            {
                var newFileName = Path.Combine(newFolder.FullName, oldFile.Name);
                oldFile.CopyTo(newFileName);
                CopyAttributes(oldFile, new FileInfo(newFileName));
            }

            CopyAttributes(oldFolder, newFolder);
        }

        private static void CopyAttributes(FileSystemInfo fsOld, FileSystemInfo fsNew)
        {
            if (!fsOld.Exists)
                throw new Exception($"Can't find file/folder {fsOld.FullName}");
            if (!fsNew.Exists)
                throw new Exception($"Can't find file/folder {fsNew.FullName}");

            var readOnly = fsNew is FileInfo fi && fi.IsReadOnly;
            if (readOnly) ((FileInfo)fsNew).IsReadOnly = false;

            fsNew.CreationTime = fsOld.CreationTime;
            fsNew.LastWriteTime = fsOld.LastWriteTime;
            fsNew.LastAccessTime = fsOld.LastAccessTime;

            if (readOnly) ((FileInfo)fsNew).IsReadOnly = true;
        }

        private static void RecursiveFolderDelete(DirectoryInfo folder)
        {
            foreach (var dir in folder.GetDirectories())
            {
                RecursiveFolderDelete(dir);
            }
            var files = folder.GetFiles();
            foreach (var file in files)
            {
                if (file.IsReadOnly)
                    file.IsReadOnly = false;
                file.Delete();
            }

            folder.Delete();
            folder.Refresh();
        }
        #endregion

        private static DateTime[] GetFolderDates(string folder) => new DateTime[]
        {
            Directory.GetCreationTime(folder), Directory.GetLastWriteTime(folder), Directory.GetLastAccessTime(folder)
        };

        private static DateTime[] GetFileDates(string fullFileName) => new DateTime[]
        {
            File.GetCreationTime(fullFileName), File.GetLastWriteTime(fullFileName), File.GetLastAccessTime(fullFileName)
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
    }
}
