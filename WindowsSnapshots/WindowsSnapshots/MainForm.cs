using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WindowsSnapshots
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            if (Helpers.IsAdministrator()) this.Text += " (ADMINISTRATOR mode)";
            lblUsersFolderPath.Text = $"Users folder is {Helpers.GetUsersFolderPath()}";
            lblStatus.Text = "";
            lblRegistryFileName.Text = "";
            txtDataFolder.Text = Settings.DataFolder;

            var dataFolder = GetDataFolder();
            if (Directory.Exists(dataFolder))
            {
                var files = Directory.GetFiles(dataFolder, "FileSystem_*.zip")
                    .OrderByDescending(a => new FileInfo(a).CreationTime).Take(2).ToArray();
                if (files.Length == 1) txtOldFileSystemSnapshotFile.Text = files[0];
                else if (files.Length > 1)
                {
                    txtOldFileSystemSnapshotFile.Text = files[1];
                    txtNewFileSystemSnapshotFile.Text = files[0];
                }

                files = Directory.GetFiles(dataFolder, "Registry_*.zip")
                    .OrderByDescending(a => new FileInfo(a).CreationTime).Take(2).ToArray();
                if (files.Length == 1) txtOldRegistrySnapshotFile.Text = files[0];
                else if (files.Length > 1)
                {
                    txtOldRegistrySnapshotFile.Text = files[1];
                    txtNewRegistrySnapshotFile.Text = files[0];
                }

                files = Directory.GetFiles(dataFolder, "Others_*.zip")
                    .OrderByDescending(a => new FileInfo(a).CreationTime).Take(2).ToArray();
                if (files.Length == 1) txtOldOthersSnapshotFile.Text = files[0];
                else if (files.Length > 1)
                {
                    txtOldOthersSnapshotFile.Text = files[1];
                    txtNewServicesSnapshotFile.Text = files[0];
                }
            }
        }

        private string GetFileId() => string.IsNullOrEmpty(txtFileId.Text) ? "" : $"{txtFileId.Text}_";

        private void ShowStatus(string message)
        {
            if (statusStrip1.InvokeRequired)
                Invoke(new MethodInvoker(delegate { ShowStatus(message); }));
            else
                lblStatus.Text = message;

            Application.DoEvents();
        }

        private string GetDataFolder()
        {
            if (Directory.Exists(txtDataFolder.Text)) return txtDataFolder.Text;
            if (Directory.Exists(Settings.DataFolder)) return Settings.DataFolder;
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        private void btnSelectFolder_Click(object sender, System.EventArgs e)
        {
            using (var folderBrowser = new CommonOpenFileDialog { IsFolderPicker = true, InitialDirectory = GetDataFolder() })
            {
                if (folderBrowser.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (Directory.Exists(folderBrowser.FileName))
                        txtDataFolder.Text = folderBrowser.FileName;
                }
            }
        }

        #region =============  File System  ============
        private async void btnFileSystemSnapshot_Click(object sender, EventArgs e)
        {
            btnFileSystemSnapshot.Enabled = false;
            try
            {
                var task = ScanFileSystem.SaveFileSystemInfoIntoFile(GetDataFolder(), GetFileId(), ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New FileSystem snapshot file is {task}");
                MessageBox.Show($"New FileSystem snapshot file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnFileSystemSnapshot.Enabled = true;
        }

        private void btnSelectOldFileSystemSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtOldFileSystemSnapshotFile.Text,
                    @"file system zip files (*.zip)|FileSystem_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtOldFileSystemSnapshotFile.Text = fn;
            }
        }

        private void btnSelectNewFileSystemSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtNewFileSystemSnapshotFile.Text,
                    @"file system zip files (*.zip)|FileSystem_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtNewFileSystemSnapshotFile.Text = fn;
            }
        }

        private async void btnCompareFileSystemSnapshots_Click(object sender, EventArgs e)
        {
            btnCompareFileSystemSnapshots.Enabled = false;
            try
            {
                var task = ScanFileSystem.CompareFileSystemFiles(txtOldFileSystemSnapshotFile.Text, txtNewFileSystemSnapshotFile.Text, ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New FileSystem difference file is {task}");
                MessageBox.Show($"New FileSystem difference file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            Helpers.ClearMemory();
            btnCompareFileSystemSnapshots.Enabled = true;
        }
        #endregion;

        #region ============  Registry  =============
        private void btnRegistrySnapshot_Click(object sender, EventArgs e)
        {
            var fileName = Path.Combine(GetDataFolder(), $"Registry_{Helpers.GetSystemDriveLabel()}_{GetFileId()}{DateTime.Now:yyyyMMddHHmm}.reg");
            MessageBox.Show(
                $"To create registry data, please, run 'regedit.exe' program, make export data in '{fileName}' and zip the newly created text file.");
            lblRegistryFileName.Text = fileName;
        }

        private void btnSelectOldRegistrySnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtOldRegistrySnapshotFile.Text,
                    @"registry zip files (*.zip)|Registry_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtOldRegistrySnapshotFile.Text = fn;
            }
        }

        private void btnSelectNewRegistrySnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtNewRegistrySnapshotFile.Text,
                    @"registry zip files (*.zip)|Registry_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtNewRegistrySnapshotFile.Text = fn;
            }
        }

        private async void btnCompareRegistrySnapshots_Click(object sender, EventArgs e)
        {
            btnCompareRegistrySnapshots.Enabled = false;
            try
            {
                var task = ScanRegistry.CompareRegistryFiles(txtOldRegistrySnapshotFile.Text, txtNewRegistrySnapshotFile.Text, ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New registry difference file is {task}");
                MessageBox.Show($"New registry difference file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            Helpers.ClearMemory();
            btnCompareRegistrySnapshots.Enabled = true;
        }
        #endregion

        #region ============  Others Snapshots  ===========
        private async void btnOthersSnapshot_Click(object sender, EventArgs e)
        {
            btnOthersSnapshot.Enabled = false;
            try
            {
                var task = ScanOtherSnapshots.SaveSnapshotsIntoFile(GetDataFolder(), GetFileId(), ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New OtherSnapshots file is {task}");
                MessageBox.Show($"New OtherSnapshots file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnOthersSnapshot.Enabled = true;
        }

        private void btnSelectOldOthersSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtOldOthersSnapshotFile.Text,
                    @"others zip files (*.zip)|Others_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtOldOthersSnapshotFile.Text = fn;
            }
        }

        private void btnSelectNewOthersSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtNewServicesSnapshotFile.Text,
                    @"others zip files (*.zip)|Others_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtNewServicesSnapshotFile.Text = fn;
            }
        }

        private async void btnCompareOthersSnapshots_Click(object sender, EventArgs e)
        {
            btnCompareOthersSnapshots.Enabled = false;
            try
            {
                var task = ScanOtherSnapshots.CompareFiles(txtOldOthersSnapshotFile.Text, txtNewServicesSnapshotFile.Text, ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New registry difference file is {task}");
                MessageBox.Show($"New registry difference file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnCompareOthersSnapshots.Enabled = true;

            Helpers.ClearMemory();
        }
        #endregion

        private async void btnAsterFolder_Click(object sender, EventArgs e)
        {
            btnAsterFolder.Enabled = false;
            try
            {
                var task = BackupAsterFolder.Run(GetDataFolder(), GetFileId(), ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"Backup Aster folder is {task}");
                MessageBox.Show($"Backup Aster folder is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnAsterFolder.Enabled = true;

            Helpers.ClearMemory();
        }
    }
}
