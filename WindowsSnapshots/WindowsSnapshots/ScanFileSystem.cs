﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace WindowsSnapshots
{
    public static class ScanFileSystem
    {
        public static string CompareFileSystemFiles(string oldFile, string newFile, Action<string> showStatusAction)
        {
            if (!File.Exists(oldFile))
                throw new Exception($"ERROR! The old file '{Path.GetFileName(oldFile)}' doesn't exist'");
            if (!File.Exists(newFile))
                throw new Exception($"ERROR! The new file '{Path.GetFileName(newFile)}' doesn't exist'");

            var s = Path.GetFileNameWithoutExtension(newFile);
            var i1 = s.IndexOf('_');
            var i2 = s.LastIndexOf('_');
            var diskLabel = s.Substring(i1 + 1, i2 - i1 - 1);
            var differenceFileName = Path.Combine(Path.GetDirectoryName(newFile), $"FileSystemDiff_{diskLabel}_{DateTime.Now:yyyyMMddHHmm}.zip");

            showStatusAction($"Parsing the old file ..");
            var data1 = ParseZipScanFile(oldFile); // 1'879'365

            showStatusAction($"Parsing the new file ..");
            var data2 = ParseZipScanFile(newFile);

            var skipKeys = Settings.FileSystemSkipKeys;
            var difference = new Dictionary<string, (string, string)>();
            foreach (var kvp in data1)
            {
                if (data2.ContainsKey(kvp.Key))
                {
                    if (!string.Equals(data1[kvp.Key], data2[kvp.Key], StringComparison.InvariantCultureIgnoreCase) && !skipKeys.Any(a => kvp.Key.Contains(a, StringComparison.InvariantCultureIgnoreCase)))
                    {
                            difference.Add(kvp.Key, (data1[kvp.Key], data2[kvp.Key]));
                    }
                }
                else if (!skipKeys.Any(a => kvp.Key.Contains(a, StringComparison.InvariantCultureIgnoreCase)))
                    difference.Add(kvp.Key, (data1[kvp.Key], null));
            }

            foreach (var kvp in data2)
            {
                if (!data1.ContainsKey(kvp.Key) && !skipKeys.Any(a => kvp.Key.Contains(a, StringComparison.InvariantCultureIgnoreCase)))
                    difference.Add(kvp.Key, (null, data2[kvp.Key]));
            }

            // Save difference
            showStatusAction($"Saving data ..");
            var data = new List<string>();
            data.Add($"#\tFiles difference: {Path.GetFileName(oldFile)} and {Path.GetFileName(newFile)}");
            data.Add("Type\tName\tDiff\tWritten1\tWritten2\tCreated1\tCreated2\tAccessed1\tAccessed2\tSize1\tSize2");
            data.AddRange(difference.OrderBy(a=>a.Key.Split('\t')[1]).Select(GetDiffLine).Where(a => a != null));
            Helpers.SaveStringsToZipFile(differenceFileName, data);

            showStatusAction($"Data saved into {Path.GetFileName(differenceFileName)}");
            return differenceFileName;

            string GetDiffLine(KeyValuePair<string, (string, string)> kvp)
            {
                var ss1 = kvp.Value.Item1 == null ? new string[0] : kvp.Value.Item1.Split('\t');
                var ss2 = kvp.Value.Item2 == null ? new string[0] : kvp.Value.Item2.Split('\t');
                var s1 = ss1.Length > 2 ? ss1[2] : null;
                var s2 = ss2.Length > 2 ? ss2[2] : null;
                var s3 = ss1.Length > 3 ? ss1[3] : null;
                var s4 = ss2.Length > 3 ? ss2[3] : null;
                var s5 = ss1.Length > 4 ? ss1[4] : null;
                var s6 = ss2.Length > 4 ? ss2[4] : null;
                var s7 = ss1.Length > 5 ? ss1[5] : null;
                var s8 = ss2.Length > 5 ? ss2[5] : null;

                string s0;
                if (ss1.Length == 0 && ss2.Length == 0) s0 = "Denied";
                else if (ss1.Length == 0) s0 = "<NEW>";
                else if (ss2.Length == 0) s0 = "<OLD>";
                else if (ss1.Length > 5 && ss2.Length > 5 && !string.Equals(s7, s8)) s0 = "Size";
                else if (ss1.Length > 2 && ss2.Length > 2 && !string.Equals(s1, s2)) s0 = "Written";
                else if (ss1.Length > 3 && ss2.Length > 3 && !string.Equals(s3, s4)) s0 = "Created";
                else if (ss1.Length > 4 && ss2.Length > 4 && !string.Equals(s5, s6)) s0 = "Accessed";
                else throw new Exception("Check program!");

                if (string.Equals(s0, "Accessed", StringComparison.InvariantCulture)) return null;
                return ($"{kvp.Key}\t{s0}\t{s1}\t{s2}\t{s3}\t{s4}\t{s5}\t{s6}\t{s7}\t{s8}").Trim();
            }
        }

        private static Dictionary<string, string> ParseZipScanFile(string zipFileName)
        {
            var entryName = Path.GetFileNameWithoutExtension(zipFileName) + ".txt";
            var data = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
            {
                var entry = zip.Entries.FirstOrDefault(a => string.Equals(a.Name, entryName));
                if (entry == null)
                    throw new Exception($"Can't find '{entryName}' entry in {Path.GetFileName(zipFileName)} zip file");

                var checkHeader = false;
                foreach (var s in entry.GetLinesOfZipEntry())
                {
                    if (!checkHeader)
                    {
                        if (!string.Equals(s, "Type\tName\tWritten\tCreated\tAccessed\tSize", StringComparison.InvariantCulture))
                            throw new Exception($"Check header of scan file {Path.GetFileName(zipFileName)}");
                        checkHeader = true;
                        continue;
                    }

                    var i1 = s.IndexOf('\t');
                    var i2 = s.IndexOf('\t', i1 + 1);
                    var key = i2 == -1 ? s : s.Substring(0, i2);
                    data.Add(key, s);
                }
            }

            return data;
        }

        public static string SaveFileSystemInfoIntoFile(string dataFolder, string fileId, Action<string> showStatusAction)
        {
            if (!Helpers.IsAdministrator()) throw new Exception("ERROR! To read ALL(!!!) files, please, run program in administrator mode");
            if (!Directory.Exists(dataFolder)) throw new Exception($"ERROR! Data folder {dataFolder} doesn't exist");

            showStatusAction("Started");

            var log = new List<string> { $"Type\tName\tWritten\tCreated\tAccessed\tSize" };
            foreach (var folder in Settings.FoldersForFileScan)
            {
                showStatusAction($"Process folder '{folder}' ..");
                ProcessFolder("\\\\?\\"+folder, log);
            }

            showStatusAction("Saving data ..");
            var zipFileName = Path.Combine(dataFolder, $"FileSystem_{Helpers.GetSystemDriveLabel()}_{fileId}{DateTime.Now:yyyyMMddHHmm}.zip");
            Helpers.SaveStringsToZipFile(zipFileName, log);

            showStatusAction($"Data saved into {Path.GetFileName(zipFileName)}");
            return zipFileName;
        }

        private static void ProcessFolder(string folder, List<string> log)
        {
            var dirInfo = new DirectoryInfo(folder);
            log.Add(GetLogString(dirInfo));

            try
            {
                var tmpFiles = Directory.GetFiles(folder);
                log.AddRange(tmpFiles.Select(a => GetLogString(new FileInfo(a))));
            }
            catch (UnauthorizedAccessException)
            {
                log.Add($"Denied\t{dirInfo.FullName}");
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var tmpFolders = Directory.GetDirectories(folder);
            foreach (var tmpFolder in tmpFolders)
                ProcessFolder(tmpFolder, log);

            //===============================================
            string GetLogString(FileSystemInfo info)
            {
                if (info is FileInfo fi)
                    return $"File\t{GetFileName(info.FullName)}\t{DateToString(info.LastWriteTime)}\t{DateToString(info.CreationTime)}\t{DateToString(info.LastAccessTime)}\t{fi.Length}";
                else
                    return $"Dir\t{GetFileName(info.FullName)}\t{DateToString(info.LastWriteTime)}\t{DateToString(info.CreationTime)}\t{DateToString(info.LastAccessTime)}";

                string DateToString(DateTime dt) => dt.ToString("yyyy-MM-dd HH:mm:ss");
                string GetFileName(string filename) => filename.StartsWith("\\\\?\\") ? filename.Substring(4) : filename;
            }
        }
    }
}
