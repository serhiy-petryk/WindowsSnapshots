namespace WindowsSnapshots
{
    public static class ScanTaskScheduler
    {
        public static void Test()
        {
            /*var data = ParseZipScanFile(@"E:\Temp\FileSystemAndRegistrySnapshots\Tasks_SSD_240_202412082004.zip");
            return;
            var aa1 = TaskService.Instance.AllTasks.OrderBy(a=>a.Folder).ThenBy(a=>a.Name).ToArray();
            var a1 = aa1.Where(a => !a.Enabled).ToArray();
            var a2 = aa1.Where(a => a.IsActive).ToArray();
            var a31 = aa1.Where(a => a.State == TaskState.Disabled).ToArray();
            var a32 = aa1.Where(a => a.State == TaskState.Queued).ToArray();
            var a33 = aa1.Where(a => a.State == TaskState.Ready).ToArray();
            var a34 = aa1.Where(a => a.State == TaskState.Running).ToArray();
            var a35 = aa1.Where(a => a.State == TaskState.Unknown).ToArray();
            var s0 = GetDataString(aa1[0]);
            var s1 = GetDataString(aa1[1]);
            var s2 = GetDataString(aa1[2]);

            var aa = aa1.Select(a=>GetDataString( a)).ToArray();
            foreach(var a in aa)
                Debug.Print(a);*/
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

        /*public static string SaveTaskSchedulerIntoFile(string dataFolder, Action<string> showStatusAction)
        {
            if (!Directory.Exists(dataFolder)) throw new Exception($"ERROR! Data folder {dataFolder} doesn't exist");

            showStatusAction("Started");

            var tasks = TaskService.Instance.AllTasks.OrderBy(a => a.Folder).ThenBy(a => a.Name).ToArray();

            var log = new List<string> { GetHeaderString() };
            log.AddRange(tasks.Select(GetDataString));


            showStatusAction("Saving data ..");
            var zipFileName = Path.Combine(dataFolder, $"Tasks_{Helpers.GetSystemDriveLabel()}_{DateTime.Now:yyyyMMddHHmm}.zip");
            Helpers.SaveStringsToZipFile(zipFileName, log);

            showStatusAction($"Data saved into {Path.GetFileName(zipFileName)}");
            return zipFileName;
        }*/
    }
}
