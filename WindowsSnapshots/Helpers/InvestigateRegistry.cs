using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Helpers
{
    public class InvestigateRegistry
    {
        public static void CompareRegistries()
        {
            const string zipFileName = @"E:\Temp\WindowsSnapshots\Registry_SSD_240_Test_202502030145.zip";
            var data = new Dictionary<string, string>();
            string mainKey = null;
            string key = null;
            var valueStrings = new List<string>();
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
                foreach (var entry in zip.Entries.Where(a => a.Length > 0))
                {
                    var checkHeader = false;
                    foreach (var s in GetLinesOfZipEntry(entry))
                    {
                        if (!checkHeader)
                        {
                            if (!s.StartsWith("Windows Registry Editor Version"))
                                throw new Exception($"Check header of registry file {Path.GetFileName(zipFileName)}");
                            checkHeader = true;
                            continue;
                        }

                        if (s.StartsWith("Windows Registry Editor Version")) continue;

                        if (string.IsNullOrEmpty(s))
                        {
                            if (valueStrings.Count > 0 && valueStrings[0].StartsWith("\"") &&
                                !valueStrings[valueStrings.Count - 1].EndsWith("\""))
                            {
                                valueStrings.Add("\n");
                                continue;
                            }

                            if (!string.IsNullOrEmpty(mainKey) && !string.IsNullOrEmpty(key))
                                SaveValue(mainKey, key, valueStrings, data);

                            mainKey = null;
                            key = null;
                        }
                        else
                        {
                            if (mainKey == null)
                            {
                                if (s.StartsWith("[") && s.EndsWith("]"))
                                {
                                    mainKey = s.Substring(1, s.Length - 2);
                                    SaveValue("@" + mainKey, null, valueStrings, data);
                                }
                                else
                                    throw new Exception("Check registry parser");
                            }
                            else
                            {
                                if (s.StartsWith("@="))
                                {
                                    if (key != null)
                                        SaveValue(mainKey, key, valueStrings, data);

                                    key = "@";
                                    valueStrings.Add(GetValue(s.Substring(2).Trim()));
                                }
                                else if (s.StartsWith("\"") &&
                                         s.IndexOf("\"=", StringComparison.CurrentCultureIgnoreCase) != -1)
                                {
                                    if (key != null)
                                        SaveValue(mainKey, key, valueStrings, data);

                                    var i1 = s.IndexOf("\"=", StringComparison.CurrentCultureIgnoreCase);
                                    key = s.Substring(0, i1 + 1).Trim();
                                    valueStrings.Add(GetValue(s.Substring(i1 + 2).Trim()));
                                }
                                else
                                {
                                    if (key == null)
                                        throw new Exception("Check registry parser");

                                    valueStrings.Add(GetValue(s.TrimStart()));
                                }
                            }
                        }
                    }
                }

            if (!string.IsNullOrEmpty(mainKey))
                SaveValue(mainKey, key, valueStrings, data);



        }

        private static string GetValue(string value)
        {
            if (value.EndsWith("\\")) return value.Substring(0, value.Length - 1);
            return value;
        }
        private static void SaveValue(string mainKey, string key, List<string> valueStrings, Dictionary<string, string> data)
        {
            var dataKey = mainKey + ConvertKeyToString(key);
            if (valueStrings.Count == 0)
                data.Add(dataKey, null);
            else
                data.Add(dataKey, string.Join(null, valueStrings));
            valueStrings.Clear();
        }

        private static string ConvertKeyToString(string key)
        {
            if (key == null || key == "@") return "\\";

            if (key.StartsWith("\"") || key.EndsWith("\""))
                key = key.Substring(1, key.Length - 2);
            else
                throw new Exception("Check registry parser");

            var newKey = key.Replace("\\\"", ((char)1).ToString()).Replace("\\\\", ((char)2).ToString());
            newKey = newKey.Replace(((char)1).ToString(), "\"").Replace(((char)2).ToString(), "\\");

            return "\\" + newKey;
        }

        public static IEnumerable<string> GetLinesOfZipEntry(ZipArchiveEntry entry)
        {
            using (var entryStream = entry.Open())
            using (var reader = new StreamReader(entryStream, System.Text.Encoding.UTF8, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }


    }
}
