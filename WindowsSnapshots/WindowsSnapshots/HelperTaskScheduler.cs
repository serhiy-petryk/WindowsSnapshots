using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;

namespace WindowsSnapshots
{
    public static class HelperTaskScheduler
    {
        internal static string GetDataLineKey(string dataLine)
        {
            var ss = dataLine.Split('\t');
            return $"{ss[0]}\t{ss[1]}";
        }

        internal static string GetHeaderStringOfDifference() => "Folder\tName\tDiff\tAuthor1\tAuthor2\tIsActive1\tIsActive2\tEnabled1\tEnabled2\tState1\tState2\tTriggers1\tTriggers2\tActions1\tActions2";

        internal static string GetDataStringOfDifference(string key, string s1, string s2)
        {
            var ss1 = string.IsNullOrEmpty(s1) ? new string[8] : s1.Split('\t');
            var ss2 = string.IsNullOrEmpty(s2) ? new string[8] : s2.Split('\t');
            var diff = ss1[0] == null ? "<NEW>" : (ss2[0] == null ? "<OLD>" : null);
            if (diff != null) { }
            else if (ss1[2] != ss2[2]) diff = "Author";
            else if (ss1[3] != ss2[3]) diff = "IsActive";
            else if (ss1[4] != ss2[4]) diff = "Enabled";
            else if (ss1[5] != ss2[5]) diff = "State";
            else if (ss1[6] != ss2[6]) diff = "Triggers";
            else if (ss1[7] != ss2[7]) diff = "Actions";
            else throw new Exception("Check program!");

            return $"{key}\t{diff}\t{ss1[2]}\t{ss2[2]}\t{ss1[3]}\t{ss2[3]}\t{ss1[4]}\t{ss2[4]}\t{ss1[5]}\t{ss2[5]}\t{ss1[6]}\t{ss2[6]}\t{ss1[7]}\t{ss2[7]}";
        }

        internal static string GetHeaderString() => "Folder\tName\tAuthor\tIsActive\tEnabled\tState\tTriggers\tActions";

        internal static List<string> GetData()
        {
            var data = new List<string> { GetHeaderString() };
            var tasks = TaskService.Instance.AllTasks.OrderBy(a => a.Folder).ThenBy(a => a.Name).ToArray();
            data.AddRange(tasks.Select(GetDataString));
            return data;
        }

        private static string GetDataString(Microsoft.Win32.TaskScheduler.Task t)
        {
            var triggers = t.Definition.Triggers.Count == 0
                ? null
                : string.Join(", ", t.Definition.Triggers.Select(a => a.TriggerType.ToString()).ToArray());
            var actions = string.Join(", ", t.Definition.Actions.Select(a => a.ToString()).ToArray());

            return $"{t.Folder}\t{t.Name}\t{t.Definition.RegistrationInfo.Author}\t{t.IsActive}\t{t.Enabled}\t{t.State}\t{triggers}\t{actions}";
        }
    }
}
