using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsSnapshots
{
    class BackupAsterFolder
    {
        private static readonly string Drive =
            Directory.Exists(@"J:\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}")
                ? "J:"
                : "C:";

        public static string Run(string dataFolder, string fileId, Action<string> showStatusAction)
        {
            showStatusAction("Started");
            var oldFolderName = Drive + @"\ProgramData\ASTER Control.{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            var mainFolderName = Path.Combine(dataFolder, $"AsterFolder_{Helpers.GetSystemDriveLabel()}_{fileId}{DateTime.Now:yyyyMMddHHmm}");
            Directory.CreateDirectory(mainFolderName);
            var newFolderName = Path.Combine(mainFolderName, "AsterProgramData");

            var oldFolder = new DirectoryInfo(oldFolderName);
            var newFolder = new DirectoryInfo(newFolderName);

            XCopy(oldFolder, newFolder);

            showStatusAction($"Data saved into {mainFolderName}");

            var zipFileName = mainFolderName + ".zip";
            Helpers.ZipCompressFolder(mainFolderName, zipFileName);
            
            Directory.Delete(mainFolderName, true);

            return zipFileName;
        }

        private static void XCopy(DirectoryInfo oldFolder, DirectoryInfo newFolder)
        {
            if (!newFolder.Exists)
            {
                newFolder.Create();
                newFolder.Refresh();
            }

            var oldFolders = oldFolder.GetDirectories();
            foreach (var folder in oldFolders)
            {
                var newFolder2 = new DirectoryInfo(Path.Combine(newFolder.FullName, folder.Name));
                XCopy(folder, newFolder2);
            }

            var files = oldFolder.GetFiles();
            foreach (var oldFile in files)
            {
                var newFileName = Path.Combine(newFolder.FullName, oldFile.Name);
                oldFile.CopyTo(newFileName, true);
                CopyAttributes(oldFile, new FileInfo(newFileName));
            }

            CopyAttributes(oldFolder, newFolder);
        }

        private static void CopyAttributes(FileSystemInfo fsOld, FileSystemInfo fsNew)
        {
            if (!fsOld.Exists)
                throw new Exception($"Can't find file/folder {fsOld.FullName}");
            if (!fsNew.Exists)
                throw new Exception($"Can't find file/folder {fsNew.FullName}");

            var readOnly = fsNew is FileInfo fi && fi.IsReadOnly;
            if (readOnly) ((FileInfo)fsNew).IsReadOnly = false;

            fsNew.CreationTime = fsOld.CreationTime;
            fsNew.LastWriteTime = fsOld.LastWriteTime;
            fsNew.LastAccessTime = fsOld.LastAccessTime;

            if (readOnly) ((FileInfo)fsNew).IsReadOnly = true;
        }



    }
}
