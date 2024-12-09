using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace WindowsSnapshots
{
    public static class ScanOtherSnapshots
    {
        public static void Test()
        {

        }

        public static string CompareFiles(string firstFile, string secondFile, Action<string> showStatusAction)
        {
            if (!File.Exists(firstFile))
                throw new Exception($"ERROR! The first file '{Path.GetFileName(firstFile)}' doesn't exist'");
            if (!File.Exists(secondFile))
                throw new Exception($"ERROR! The second file '{Path.GetFileName(secondFile)}' doesn't exist'");

            var s = Path.GetFileNameWithoutExtension(firstFile);
            var i1 = s.IndexOf('_');
            var i2 = s.LastIndexOf('_');
            var diskLabel = s.Substring(i1 + 1, i2 - i1 - 1);
            var differenceFileName = Path.Combine(Path.GetDirectoryName(firstFile),
                $"OthersDiff_{diskLabel}_{DateTime.Now:yyyyMMddHHmm}.zip");

            showStatusAction($"Parsing the first file ..");
            var fileLines1 = ReadZipFile(firstFile);
            var fileLines2 = ReadZipFile(secondFile);

            var firewallData1 = GetDataLines(fileLines1, "FIREWALL", HelperFirewall.GetHeaderString(),
                (HelperFirewall.GetDataLineKey));
            var firewallData2 = GetDataLines(fileLines2, "FIREWALL", HelperFirewall.GetHeaderString(),
                (HelperFirewall.GetDataLineKey));

            var difference = new Dictionary<string, (string, string)>();
            foreach (var kvp in firewallData1)
            {
                if (firewallData2.ContainsKey(kvp.Key))
                {
                    if (!object.Equals(firewallData1[kvp.Key], firewallData2[kvp.Key]))
                    {
                        difference.Add(kvp.Key, (firewallData1[kvp.Key], firewallData2[kvp.Key]));
                    }
                }
                else
                    difference.Add(kvp.Key, (firewallData1[kvp.Key], null));
            }

            foreach (var kvp in firewallData2)
            {
                if (!firewallData1.ContainsKey(kvp.Key))
                    difference.Add(kvp.Key, (null, firewallData2[kvp.Key]));
            }

            return null;
        }

        private static Dictionary<string, string> GetDataLines(string[] fileLines, string headerId, string header, Func<string, string> fnKey)
        {
            var isDataArea = false;
            var isHeaderChecked = false;
            var data = new Dictionary<string, string>();
            foreach (var s in fileLines)
            {
                if (string.IsNullOrEmpty(s) && isDataArea) return data;
                if (isDataArea)
                {
                    if (isHeaderChecked)
                        data.Add(fnKey(s), s);
                    else if (string.Equals(s, header)) isHeaderChecked = true;
                    else
                        throw new Exception($"Check header for {header}");
                }
                else if (s.StartsWith($"#{headerId}", StringComparison.InvariantCultureIgnoreCase) && s.EndsWith("#"))
                {
                    isDataArea = true;
                    isHeaderChecked = false;
                }
            }

            return data;
        }

        private static string[] ReadZipFile(string zipFileName)
        {
            var entryName = Path.GetFileNameWithoutExtension(zipFileName) + ".txt";
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
            {
                var entry = zip.Entries.FirstOrDefault(a => string.Equals(a.Name, entryName));
                if (entry == null)
                    throw new Exception($"Check content of zip file. File '{entryName}' is missing");
                return entry.GetLinesOfZipEntry().ToArray();
            }
        }

        public static string SaveSnapshotsIntoFile(string dataFolder, Action<string> showStatusAction)
        {
            if (!Helpers.IsAdministrator()) throw new Exception("ERROR! To read ALL(!!!) files, please, run program in administrator mode");
            if (!Directory.Exists(dataFolder)) throw new Exception($"ERROR! Data folder {dataFolder} doesn't exist");

            showStatusAction("Process Firewall");
            var d = HelperFirewall.GetData();
            var data = new List<string> { $"#FIREWALL RULES ({d.Count} rows)#" };
            data.AddRange(d);

            showStatusAction("Process Services");
            data.Add(null);
            d = HelperServices.GetData();
            data.Add($"#SERVICES ({d.Count} rows)#");
            data.AddRange(d);

            showStatusAction("Process TaskScheduler");
            data.Add(null);
            d = HelperTaskScheduler.GetData();
            data.Add($"#TASK SCHEDULER LIST ({d.Count} rows)#");
            data.AddRange(d);

            showStatusAction("Saving data ..");
            var zipFileName = Path.Combine(dataFolder, $"Others_{Helpers.GetSystemDriveLabel()}_{DateTime.Now:yyyyMMddHHmm}.zip");
            Helpers.SaveStringsToZipFile(zipFileName, data);

            showStatusAction($"Data saved into {Path.GetFileName(zipFileName)}");
            return zipFileName;
        }
    }
}
