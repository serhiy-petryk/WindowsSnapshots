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

        public static string CompareFiles(string oldFile, string newFile, Action<string> showStatusAction)
        {
            if (!File.Exists(oldFile))
                throw new Exception($"ERROR! The old file '{Path.GetFileName(oldFile)}' doesn't exist'");
            if (!File.Exists(newFile))
                throw new Exception($"ERROR! The new file '{Path.GetFileName(newFile)}' doesn't exist'");

            var s = Path.GetFileNameWithoutExtension(oldFile);
            var i1 = s.IndexOf('_');
            var i2 = s.LastIndexOf('_');
            var diskLabel = s.Substring(i1 + 1, i2 - i1 - 1);
            var differenceFileName = Path.Combine(Path.GetDirectoryName(oldFile),
                $"OthersDiff_{diskLabel}_{DateTime.Now:yyyyMMddHHmm}.zip");

            showStatusAction($"Parsing the old file ..");
            var fileLines1 = ReadZipFile(oldFile);
            var fileLines2 = ReadZipFile(newFile);

            var printData = new List<string>
            {
                $"OTHERS DIFFERENCES\t{Path.GetFileNameWithoutExtension(oldFile)}\t{Path.GetFileNameWithoutExtension(newFile)}"
            };

            // ======  FIREWALL  =======
            var firewallData1 = GetDataLines(fileLines1, "FIREWALL", HelperFirewall.GetHeaderString(),
                (HelperFirewall.GetDataLineKey));
            var firewallData2 = GetDataLines(fileLines2, "FIREWALL", HelperFirewall.GetHeaderString(),
                (HelperFirewall.GetDataLineKey));

            var difference = GetDifference(firewallData1, firewallData2);
            printData.Add($"#FIREWALL ({difference.Count} rows)#");
            printData.Add(HelperFirewall.GetHeaderStringOfDifference());
            foreach(var kvp in difference)
                printData.Add(HelperFirewall.GetDataStringOfDifference(kvp.Key, kvp.Value.Item1, kvp.Value.Item2));

            //=======  SERVICES  ========
            var servicesData1 = GetDataLines(fileLines1, "SERVICES", HelperServices.GetHeaderString(),
                (HelperServices.GetDataLineKey));
            var servicesData2 = GetDataLines(fileLines2, "SERVICES", HelperServices.GetHeaderString(),
                (HelperServices.GetDataLineKey));

            difference = GetDifference(servicesData1, servicesData2);
            printData.Add(null);
            printData.Add($"#SERVICES ({difference.Count} rows)#");
            printData.Add(HelperServices.GetHeaderStringOfDifference());
            foreach (var kvp in difference)
                printData.Add(HelperServices.GetDataStringOfDifference(kvp.Key, kvp.Value.Item1, kvp.Value.Item2));

            //========  TASK SCHEDULER  =========
            var taskScheduler1 = GetDataLines(fileLines1, "TASK SCHEDULER", HelperTaskScheduler.GetHeaderString(),
                (HelperTaskScheduler.GetDataLineKey));
            var taskScheduler2 = GetDataLines(fileLines2, "TASK SCHEDULER", HelperTaskScheduler.GetHeaderString(),
                (HelperTaskScheduler.GetDataLineKey));

            difference = GetDifference(taskScheduler1, taskScheduler2);
            printData.Add(null);
            printData.Add($"#TASK SCHEDULER LIST ({difference.Count} rows)#");
            printData.Add(HelperTaskScheduler.GetHeaderStringOfDifference());
            foreach (var kvp in difference)
              printData.Add(HelperTaskScheduler.GetDataStringOfDifference(kvp.Key, kvp.Value.Item1, kvp.Value.Item2));

            // =====  Save difference  ======
            showStatusAction($"Saving data ..");
            Helpers.SaveStringsToZipFile(differenceFileName, printData);

            showStatusAction($"Data saved into {Path.GetFileName(differenceFileName)}");
            return differenceFileName;
        }

        private static Dictionary<string, (string, string)> GetDifference(Dictionary<string, string> data1, Dictionary<string, string> data2)
        {
            var difference = new Dictionary<string, (string, string)>();
            foreach (var kvp in data1)
            {
                if (data2.ContainsKey(kvp.Key))
                {
                    if (!object.Equals(data1[kvp.Key], data2[kvp.Key]))
                    {
                        difference.Add(kvp.Key, (data1[kvp.Key], data2[kvp.Key]));
                    }
                }
                else
                    difference.Add(kvp.Key, (data1[kvp.Key], null));
            }

            foreach (var kvp in data2)
            {
                if (!data1.ContainsKey(kvp.Key))
                    difference.Add(kvp.Key, (null, data2[kvp.Key]));
            }

            return difference;
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
