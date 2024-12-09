using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace WindowsSnapshots
{
    public static class HelperServices
    {
        internal static List<string> GetData()
        {
            var data = new List<string> { GetHeaderString() };
            var services = ServiceController.GetServices().OrderBy(a => a.ServiceName).ToArray();
            data.AddRange(services.Select(GetDataString));
            return data;
        }

        private static string GetHeaderString() => "Name\tDisplay name\tStatus\tStart type";

        private static string GetDataString(ServiceController sc) => $"{sc.ServiceName}\t{sc.DisplayName}\t{sc.Status}\t{sc.StartType}";
    }
}
