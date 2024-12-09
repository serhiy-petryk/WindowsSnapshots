﻿using System;
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
            txtDataFolder.Text = Settings.DataFolder;

            var dataFolder = GetDataFolder();
            if (Directory.Exists(dataFolder))
            {
                var files = Directory.GetFiles(dataFolder, "FileSystem_*.zip")
                    .OrderByDescending(a => new FileInfo(a).CreationTime).Take(2).ToArray();
                if (files.Length == 1) txtFirstFileSystemSnapshotFile.Text = files[0];
                else if (files.Length > 1)
                {
                    txtFirstFileSystemSnapshotFile.Text = files[1];
                    txtSecondFileSystemSnapshotFile.Text = files[0];
                }

                files = Directory.GetFiles(dataFolder, "Registry_*.zip")
                    .OrderByDescending(a => new FileInfo(a).CreationTime).Take(2).ToArray();
                if (files.Length == 1) txtFirstRegistrySnapshotFile.Text = files[0];
                else if (files.Length > 1)
                {
                    txtFirstRegistrySnapshotFile.Text = files[1];
                    txtSecondRegistrySnapshotFile.Text = files[0];
                }

                files = Directory.GetFiles(dataFolder, "Services_*.zip")
                    .OrderByDescending(a => new FileInfo(a).CreationTime).Take(2).ToArray();
                if (files.Length == 1) txtFirstServicesSnapshotFile.Text = files[0];
                else if (files.Length > 1)
                {
                    txtFirstServicesSnapshotFile.Text = files[1];
                    txtSecondServicesSnapshotFile.Text = files[0];
                }
            }
        }

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
                var task = ScanFileSystem.SaveFileSystemInfoIntoFile(GetDataFolder(), ShowStatus);
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

        private void btnSelectFirstFileSystemSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtFirstFileSystemSnapshotFile.Text,
                    @"file system zip files (*.zip)|FileSystem_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtFirstFileSystemSnapshotFile.Text = fn;
            }
        }

        private void btnSelectSecondFileSystemSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtSecondFileSystemSnapshotFile.Text,
                    @"file system zip files (*.zip)|FileSystem_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtSecondFileSystemSnapshotFile.Text = fn;
            }
        }

        private async void btnCompareFileSystemSnapshots_Click(object sender, EventArgs e)
        {
            btnCompareFileSystemSnapshots.Enabled = false;
            try
            {
                var task = ScanFileSystem.CompareFileSystemFiles(txtFirstFileSystemSnapshotFile.Text, txtSecondFileSystemSnapshotFile.Text, ShowStatus);
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
        private void btnSelectFirstRegistrySnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtFirstRegistrySnapshotFile.Text,
                    @"registry zip files (*.zip)|Registry_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtFirstRegistrySnapshotFile.Text = fn;
            }
        }

        private void btnSelectSecondRegistrySnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtSecondRegistrySnapshotFile.Text,
                    @"registry zip files (*.zip)|Registry_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtSecondRegistrySnapshotFile.Text = fn;
            }
        }

        private async void btnCompareRegistrySnapshots_Click(object sender, EventArgs e)
        {
            btnCompareRegistrySnapshots.Enabled = false;
            try
            {
                var task = ScanRegistry.CompareRegistryFiles(txtFirstRegistrySnapshotFile.Text, txtSecondRegistrySnapshotFile.Text, ShowStatus);
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

        #region ==========  Services  ===========
        private async void btnServicesSnapshot_Click(object sender, EventArgs e)
        {
            btnServicesSnapshot.Enabled = false;
            try
            {
                var task = ScanServices.SaveServiceInfosIntoFile(GetDataFolder(), ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New ServiceList snapshot file is {task}");
                MessageBox.Show($"New ServiceList snapshot file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnServicesSnapshot.Enabled = true;
        }

        private void btnSelectFirstServicesSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtFirstServicesSnapshotFile.Text,
                    @"service list zip files (*.zip)|Services_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtFirstServicesSnapshotFile.Text = fn;
            }
        }

        private void btnSelectSecondServicesSnapshotFile_Click(object sender, EventArgs e)
        {
            if (Helpers.OpenFileSystemZipFileDialog(GetDataFolder(), txtSecondServicesSnapshotFile.Text,
                    @"service list zip files (*.zip)|Services_*.zip") is string fn && !string.IsNullOrWhiteSpace(fn))
            {
                txtSecondServicesSnapshotFile.Text = fn;
            }
        }

        private async void btnCompareServicesSnapshots_Click(object sender, EventArgs e)
        {
            btnCompareServicesSnapshots.Enabled = false;
            try
            {
                var task = ScanServices.CompareServicesFiles(txtFirstServicesSnapshotFile.Text, txtSecondServicesSnapshotFile.Text, ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New registry difference file is {task}");
                MessageBox.Show($"New registry difference file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnCompareServicesSnapshots.Enabled = true;

            Helpers.ClearMemory();
        }
        #endregion

        #region =========  Firewall rules  ============
        private async void btnFirewallRulesSnapshot_Click(object sender, EventArgs e)
        {
            /*btnFirewallRulesSnapshot.Enabled = false;
            try
            {
                var task = ScanFirewall.SaveFirewallRulesIntoFile(GetDataFolder(), ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New FirewallRules snapshot file is {task}");
                MessageBox.Show($"New FirewallRules snapshot file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnFirewallRulesSnapshot.Enabled = true;*/
        }
        #endregion

        private async void btnTasksSnapshot_Click(object sender, EventArgs e)
        {
            btnTasksSnapshot.Enabled = false;
            try
            {
                var task = ScanOtherSnapshots.SaveSnapshotsIntoFile(GetDataFolder(), ShowStatus);
                await Task.Factory.StartNew(() => task);
                ShowStatus($"New OtherSnapshots file is {task}");
                MessageBox.Show($"New OtherSnapshots file is {task}");
            }
            catch (Exception exception)
            {
                ShowStatus(exception.Message);
                MessageBox.Show(exception.Message);
            }

            btnTasksSnapshot.Enabled = true;

        }
    }
}
