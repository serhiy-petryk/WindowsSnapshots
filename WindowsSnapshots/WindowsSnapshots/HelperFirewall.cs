using System.Collections.Generic;
using System.Linq;
using WindowsFirewallHelper;

namespace WindowsSnapshots
{
    public static class HelperFirewall
    {
        internal static List<string> GetData()
        {
            var data = new List<string> { GetHeaderString() };
            var allRules = FirewallManager.Instance.Rules.OrderBy(a => a.Direction).ThenBy(a => a.Name)
                .ThenBy(a => a.Profiles).ThenBy(a => a.Protocol.ToString()).ToArray();
            data.AddRange(allRules.Select(GetDataString));
            return data;
        }

        private static string GetHeaderString() =>
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
