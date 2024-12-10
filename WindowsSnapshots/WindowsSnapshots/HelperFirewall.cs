using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper;

namespace WindowsSnapshots
{
    public static class HelperFirewall
    {
        internal static string GetDataLineKey(string dataLine)
        {
            var ss = dataLine.Split('\t');
            return $"{ss[0]}\t{ss[1]}\t{ss[2]}\t{ss[5]}\t{ss[8]}\t{ss[9]}\t{ss[10]}";
        }

        internal static string GetHeaderStringOfDifference() =>
            "Direction\tName\tProfile\tProtocol\tProgram\tLocalPort\tRemotePort\tDiff\tEnabled1\tEnabled2\tAction1\tAction2\tLocal Address1\tLocal Address2\tRemote Address1\tRemote Address2";

        internal static string GetDataStringOfDifference(string key, string s1, string s2)
        {
            var keys = key.Split('\t');
            var ss1 = string.IsNullOrEmpty(s1) ? new string[11] : s1.Split('\t');
            var ss2 = string.IsNullOrEmpty(s2) ? new string[11] : s2.Split('\t');
            var diff = ss1[0] == null ? "<NEW>" : (ss2[0] == null ? "<OLD>" : null);
            if (diff != null) { }
            else if (ss1[3] != ss2[3]) diff = "Enabled";
            else if (ss1[4] != ss2[4]) diff = "Action";
            else if (ss1[6] != ss2[6]) diff = "LocalAddress";
            else if (ss1[7] != ss2[7]) diff = "RemoteAddress";
            else throw new Exception("Check program!");

            return $"{keys[0]}\t{keys[1]}\t{keys[2]}\t{keys[4]}\t{keys[3]}\t{keys[5]}\t{keys[6]}\t{diff}\t{ss1[3]}\t{ss2[3]}\t{ss1[4]}\t{ss2[4]}\t{ss1[6]}\t{ss2[6]}\t{ss1[7]}\t{ss2[7]}";
        }

        internal static List<string> GetData()
        {
            var data = new List<string> { GetHeaderString() };
            var allRules = FirewallManager.Instance.Rules.OrderBy(a => a.Direction).ThenBy(a => a.Name)
                .ThenBy(a => a.Profiles).ThenBy(a => a.Protocol.ToString()).ToArray();
            data.AddRange(allRules.Select(GetDataString));
            return data;
        }

        internal static string GetHeaderString() =>
            "Direction\tName\tProfile\tEnabled\tAction\tProgram\tLocal Address\tRemote Address\tProtocol\tLocal Port\tRemote Port";

        private static string GetDataString(IFirewallRule rule)
        {
            var s = rule.Protocol.ToString();
            var protocol = s.StartsWith("{") && s.EndsWith("}") ? s.Substring(1, s.Length - 2) : s;
            var direction = rule.Direction == FirewallDirection.Inbound ? "In" : "Out";
            var profiles = rule.Profiles.ToString().Replace(" | ", ", ");
            var enabled = rule.IsEnable ? "Yes" : "No";

            return
                $"{direction}\t{rule.Name}\t{profiles}\t{enabled}\t{rule.Action}\t{rule.ApplicationName}" +
                $"\t{GetAddresses(rule.LocalAddresses)}\t{GetAddresses(rule.RemoteAddresses)}\t{protocol}" +
                $"\t{GetPorts(rule.LocalPorts)}\t{GetPorts(rule.RemotePorts)}";

            string GetAddresses(IAddress[] addresses) => (addresses.Length == 0 ? "Any" : string.Join(", ", addresses.Select(a => a.ToString()))).Replace("*", "Any");
            string GetPorts(ushort[] ports) => ports.Length == 0 ? "Any" : string.Join(", ", ports.Select(a => a.ToString()));
        }
    }
}
