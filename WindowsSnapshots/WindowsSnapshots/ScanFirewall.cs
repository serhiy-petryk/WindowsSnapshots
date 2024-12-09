using System.Linq;
using WindowsFirewallHelper;

namespace WindowsSnapshots
{
    public static class ScanFirewall
    {
        public static void Test()
        {
            var allRules = FirewallManager.Instance.Rules.ToArray();
            var r1 = allRules.Where(a => string.Equals(a.Name, "Core Networking - Router Advertisement (ICMPv6-Out)")).ToArray();
            var r2 = allRules.Where(a => string.Equals(a.Name, "BranchCache Peer Discovery (WSD-In)")).ToArray();
            var r3 = allRules.Where(a => string.Equals(a.Name, "Connect to a Network Projector (TCP-In)")).ToArray();
            var r4 = allRules.Where(a => string.Equals(a.Name, "Core Networking - IPHTTPS (TCP-Out)")).ToArray();
            var rr = allRules.Where(a => !string.Equals(a.Name, a.FriendlyName)).ToArray();
            var s1 = r3[0].Profiles.ToString().Replace(" | ", ", ");
            var a1 = r3[0].Protocol;
        }

        /*private static Dictionary<string, string> ParseZipScanFile(string zipFileName)
        {
            var entryName = Path.GetFileNameWithoutExtension(zipFileName) + ".txt";
            var data = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            using (var zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read))
                foreach (var entry in zip.Entries.Where(a => string.Equals(a.Name, entryName)))
                {
                    var checkHeader = false;
                    foreach (var s in entry.GetLinesOfZipEntry())
                    {
                        if (!checkHeader)
                        {
                            if (!string.Equals(s, GetHeaderString()))
                                throw new Exception($"Check header of scan file {Path.GetFileName(zipFileName)}");
                            checkHeader = true;
                            continue;
                        }

                        var ss = s.Split('\t');
                        data.Add(ss[1].Trim(), s);
                    }
                }
            return data;
        }*/

        /*public static string SaveFirewallRulesIntoFile(string dataFolder, Action<string> showStatusAction)
        {
            if (!Directory.Exists(dataFolder)) throw new Exception($"ERROR! Data folder {dataFolder} doesn't exist");

            showStatusAction("Started");

            var log = new List<string> { GetHeaderString() };
            var allRules = FirewallManager.Instance.Rules.OrderBy(a => a.Direction).ThenBy(a => a.Name)
                .ThenBy(a => a.Profiles).ThenBy(a => a.Protocol.ToString()).ToArray();
            foreach (var rule in allRules)
                log.Add(GetDataString(rule));

            showStatusAction("Saving data ..");
            var zipFileName = Path.Combine(dataFolder, $"Firewall_{Helpers.GetSystemDriveLabel()}_{DateTime.Now:yyyyMMddHHmm}.zip");
            Helpers.SaveStringsToZipFile(zipFileName, log);

            showStatusAction($"Data saved into {Path.GetFileName(zipFileName)}");
            return zipFileName;
        }*/
    }
}
