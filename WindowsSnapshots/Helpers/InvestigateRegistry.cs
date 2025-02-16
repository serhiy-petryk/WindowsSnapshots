using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Helpers
{
    public class InvestigateRegistry
    {
        private List<string> AllKeys = new List<string>();
        private List<string> NotAllKeys = new List<string>();

        public override string ToString() => AllKeys.Count + " " + NotAllKeys.Count;

        /// <summary>
        /// Print the number of keys
        /// </summary>
        public static void ParseZipRegistryFile()
        {
            /*
ALL                 1339736
HKEY_CLASSES_ROOT	262112
HKEY_CURRENT_CONFIG	53
HKEY_CURRENT_USER	146097
HKEY_LOCAL_MACHINE 	1069081
HKEY_USERS	        270655
             */
            const string zipFileName = @"D:\Temp\WindowsSnapshots\Registry_SSD_240_Test_202502030145.zip";
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

                    if (!string.IsNullOrEmpty(mainKey))
                        SaveValue(mainKey, key, valueStrings, data);
                    mainKey = null;
                    key = null;

                    Debug.Print($"{entry.Name}\t{data.Count}");
                    Debug.Print($"Classes:\t{entry.Name}\t{data.Count(a => a.Key.StartsWith(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes"))}");
                    Debug.Print($"User:\t{entry.Name}\t{data.Count(a => a.Key.StartsWith(@"HKEY_USERS\S-1-5-21-1388931996-740669986-2123317917-1000"))}");
                    data.Clear();
                }
        }


        public static void CompareRegistries()
        {
            const string zipFileName = @"D:\Temp\WindowsSnapshots\Registry_SSD_240_Test_202502030145.zip";
            var data = new Dictionary<string, InvestigateRegistry>();
            string mainKey = null;
            string key = null;
            var valueStrings = new List<string>();
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
                foreach (var entry in zip.Entries.Where(a => a.Length > 0))
                {
                    var isAll = entry.Name.IndexOf("_ALL_", StringComparison.InvariantCultureIgnoreCase) != -1;
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
                                SaveValue_Compare(mainKey, key, valueStrings, data, isAll);

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
                                    SaveValue_Compare("@" + mainKey, null, valueStrings, data, isAll);
                                }
                                else
                                    throw new Exception("Check registry parser");
                            }
                            else
                            {
                                if (s.StartsWith("@="))
                                {
                                    if (key != null)
                                        SaveValue_Compare(mainKey, key, valueStrings, data, isAll);

                                    key = "@";
                                    valueStrings.Add(GetValue(s.Substring(2).Trim()));
                                }
                                else if (s.StartsWith("\"") &&
                                         s.IndexOf("\"=", StringComparison.CurrentCultureIgnoreCase) != -1)
                                {
                                    if (key != null)
                                        SaveValue_Compare(mainKey, key, valueStrings, data, isAll);

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

                    if (!string.IsNullOrEmpty(mainKey))
                        SaveValue_Compare(mainKey, key, valueStrings, data, isAll);

                    mainKey = null;
                    key = null;
                }

            /*foreach (var item in data.Values)
            {
                var regKeys = item.AllKeys.ToArray();
                foreach (var regKey in regKeys)
                {
                    if (item.NotAllKeys.Contains(regKey))
                    {
                        item.NotAllKeys.Remove(regKey);
                        item.AllKeys.Remove(regKey);
                    }
                }
            }

            var data0 = data.Where(a => a.Value.AllKeys.Count != 0 || a.Value.NotAllKeys.Count != 0)
                .ToDictionary(a => a.Key, a => a.Value);*/

            var data1 = data.Where(a => a.Value.AllKeys.Count == 0).ToDictionary(a => a.Key, a => a.Value);
            var data2 = data.Where(a => a.Value.NotAllKeys.Count == 0).ToDictionary(a => a.Key, a => a.Value);

            var regKeys = data2.SelectMany(a => a.Value.AllKeys).ToArray();

            foreach (var regKey in regKeys)
            {
                foreach (var value in data1.Values)
                {
                    if (value.NotAllKeys.Contains(regKey))
                    {
                        value.NotAllKeys.Remove(regKey);
                        var allRegKey = data2.Select(a => a.Value).Where(a => a.AllKeys.Contains(regKey)).ToArray();
                        allRegKey[0].AllKeys.Remove(regKey);
                    }
                }
            }

            var data11 = data1.Where(a => a.Value.NotAllKeys.Count != 0).ToDictionary(a => a.Key, a => a.Value);
            var data21 = data2.Where(a => a.Value.AllKeys.Count != 0).ToDictionary(a => a.Key, a => a.Value);

        }

        private static string GetValue(string value)
        {
            if (value.EndsWith("\\")) return value.Substring(0, value.Length - 1);
            return value;
        }
        private static void SaveValue(string mainKey, string key, List<string> valueStrings, Dictionary<string, string> data)
        {
            if (mainKey.StartsWith("@")) return;

            var dataKey = mainKey + ConvertKeyToString(key);
            if (valueStrings.Count == 0)
                data.Add(dataKey, null);
            else
                data.Add(dataKey, string.Join(null, valueStrings));
            valueStrings.Clear();
        }

        private static void SaveValue_Compare(string mainKey, string key, List<string> valueStrings, Dictionary<string, InvestigateRegistry> data, bool isAll)
        {
            var dataKey = mainKey + ConvertKeyToString(key);
            if (valueStrings.Count != 0)
            {
                var value = string.Join(null, valueStrings);
                if (!data.ContainsKey(value))
                    data.Add(value, new InvestigateRegistry());

                if (isAll)
                    data[value].AllKeys.Add(dataKey);
                else
                    data[value].NotAllKeys.Add(dataKey);
            }

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
