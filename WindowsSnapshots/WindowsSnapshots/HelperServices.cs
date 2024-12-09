using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace WindowsSnapshots
{
    public static class HelperServices
    {
        internal static string GetDataLineKey(string dataLine)
        {
            var ss = dataLine.Split('\t');
            return $"{ss[0]}";
        }

        internal static string GetHeaderStringOfDifference() => "Name\tDiff\tDisplayName1\tDisplayName2\tStatus1\tStatus2\tStartType1\tStartType2";

        internal static string GetDataStringOfDifference(string key, string s1, string s2)
        {
            var ss1 = string.IsNullOrEmpty(s1) ? new string[4] : s1.Split('\t');
            var ss2 = string.IsNullOrEmpty(s2) ? new string[4] : s2.Split('\t');
            var diff = ss1[0] == null ? "New" : (ss2[0] == null ? "Old" : null);
            if (diff != null){}
            else if (ss1[1] != ss2[1]) diff = "DisplayName";
            else if (ss1[2] != ss2[2]) diff = "Status";
            else if (ss1[3] != ss2[3]) diff = "StartType";
            else throw new Exception("Check program!");

            return $"{key}\t{diff}\t{ss1[1]}\t{ss2[1]}\t{ss1[2]}\t{ss2[2]}\t{ss1[3]}\t{ss2[3]}";
        }

        internal static List<string> GetData()
        {
            var data = new List<string> { GetHeaderString() };
            var services = ServiceController.GetServices().OrderBy(a => a.ServiceName).ToArray();
            data.AddRange(services.Select(GetDataString));
            return data;
        }

        internal static string GetHeaderString() => "Name\tDisplay name\tStatus\tStart type";

        private static string GetDataString(ServiceController sc) => $"{sc.ServiceName}\t{sc.DisplayName}\t{sc.Status}\t{sc.StartType}";
    }
}
