using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;

namespace WindowsSnapshots
{
    public static class HelperTaskScheduler
    {
        internal static List<string> GetData()
        {
            var data = new List<string> { GetHeaderString() };
            var tasks = TaskService.Instance.AllTasks.OrderBy(a => a.Folder).ThenBy(a => a.Name).ToArray();
            data.AddRange(tasks.Select(GetDataString));
            return data;
        }

        private static string GetHeaderString() => "Folder\tName\tAuthor\tIsActive\tEnabled\tState\tTriggers\tActions";

        private static string GetDataString(Microsoft.Win32.TaskScheduler.Task t)
        {
            var triggers = t.Definition.Triggers.Count == 0
                ? null
                : string.Join(", ", t.Definition.Triggers.ToArray().Select(a => a.TriggerType.ToString()).ToArray());
            var actions = string.Join(", ", t.Definition.Actions.ToArray().Select(a => a.ToString()).ToArray());

            return $"{t.Folder}\t{t.Name}\t{t.Definition.RegistrationInfo.Author}\t{t.IsActive}\t{t.Enabled}\t{t.State}\t{triggers}\t{actions}";
        }
    }
}
