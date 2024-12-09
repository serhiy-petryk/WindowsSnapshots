using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsSnapshots
{
    public static class ScanOtherSnapshots
    {
        public static void Test()
        {

        }

        public static string SaveSnapshotsIntoFile(string dataFolder, Action<string> showStatusAction)
        {
            if (!Helpers.IsAdministrator()) throw new Exception("ERROR! To read ALL(!!!) files, please, run program in administrator mode");
            if (!Directory.Exists(dataFolder)) throw new Exception($"ERROR! Data folder {dataFolder} doesn't exist");

            showStatusAction("Process Firewall");
            var data = new List<string> { "#FIREWALL RULES#" };
            data.AddRange(HelperFirewall.GetData());

            showStatusAction("Process Services");
            data.Add(null);
            data.Add("#SERVICES#");
            data.AddRange(HelperServices.GetData());

            showStatusAction("Process TaskScheduler");
            data.Add(null);
            data.Add("#TASK SCHEDULER LIST#");
            data.AddRange(HelperTaskScheduler.GetData());

            showStatusAction("Saving data ..");
            var zipFileName = Path.Combine(dataFolder, $"Others_{Helpers.GetSystemDriveLabel()}_{DateTime.Now:yyyyMMddHHmm}.zip");
            Helpers.SaveStringsToZipFile(zipFileName, data);

            showStatusAction($"Data saved into {Path.GetFileName(zipFileName)}");
            return zipFileName;
        }
    }
}
