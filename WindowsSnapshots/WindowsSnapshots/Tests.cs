using System;
using System.Diagnostics;
using System.Linq;

namespace WindowsSnapshots
{
    public static class Tests
    {
        private static readonly string[] _searchStrings = new[] { "IBIK", "ASTER", "mutectl", "mutesv" };
        public static void GetAsterEntries()
        {
            var zipFileName = @"E:\Temp\WindowsSnapshots\Registry_HDD_100_AfterActivation3_202412121612.zip";
            var data1 = ScanRegistry.ParseZipRegistryFile(zipFileName)
                .Where(
                    a => _searchStrings.Any(s => (a.Key.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) != -1)
                                                 || (a.Value != null && a.Value.IndexOf(s,
                                                     StringComparison.InvariantCultureIgnoreCase) != -1)))
                .OrderBy(a => a.Key.StartsWith("@") ? a.Key.Substring(1) : a.Key)
                .ToDictionary(a => a.Key, a => a.Value);

            zipFileName = @"E:\Temp\WindowsSnapshots\Registry_HDD_100_AfterInstall_202412120134.zip";
            var data2 = ScanRegistry.ParseZipRegistryFile(zipFileName)
                .Where(
                    a => _searchStrings.Any(s => (a.Key.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) != -1)
                                                 || (a.Value != null && a.Value.IndexOf(s,
                                                     StringComparison.InvariantCultureIgnoreCase) != -1)))
                .OrderBy(a => a.Key.StartsWith("@") ? a.Key.Substring(1) : a.Key)
                .ToDictionary(a => a.Key, a => a.Value);

            /*Debug.Print($"Type\tKey\tValue");
            foreach (var kvp in data1)
            {
                var s = data2.ContainsKey(kvp.Key) ? "" : "NEW";
                var value = kvp.Value != null && kvp.Value.Length > 10000 ? kvp.Value.Substring(0, 10000) : kvp.Value;
                Debug.Print($"{s}\t{kvp.Key}\t{value}");
            }*/
            Debug.Print($"Key\tOldValue\tNewValue");
            foreach (var kvp in data1)
            {
                if (data2.ContainsKey(kvp.Key) && !string.Equals(kvp.Value, data2[kvp.Key]))
                {
                    var value1 = data2[kvp.Key] != null && data2[kvp.Key].Length > 10000
                        ? data2[kvp.Key].Substring(0, 10000)
                        : data2[kvp.Key];
                    var value2 = kvp.Value != null && kvp.Value.Length > 10000
                        ? kvp.Value.Substring(0, 10000)
                        : kvp.Value;
                    Debug.Print($"{kvp.Key}\t{value1}\t{value2}");
                }
            }
        }
    }
}
