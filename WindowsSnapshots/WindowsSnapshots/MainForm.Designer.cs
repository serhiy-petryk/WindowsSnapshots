
namespace WindowsSnapshots
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtDataFolder = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.btnFileSystemSnapshot = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCompareFileSystemSnapshots = new System.Windows.Forms.Button();
            this.btnSelectNewFileSystemSnapshotFile = new System.Windows.Forms.Button();
            this.btnSelectOldFileSystemSnapshotFile = new System.Windows.Forms.Button();
            this.txtNewFileSystemSnapshotFile = new System.Windows.Forms.TextBox();
            this.txtOldFileSystemSnapshotFile = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCompareRegistrySnapshots = new System.Windows.Forms.Button();
            this.btnSelectNewRegistrySnapshotFile = new System.Windows.Forms.Button();
            this.btnSelectOldRegistrySnapshotFile = new System.Windows.Forms.Button();
            this.txtNewRegistrySnapshotFile = new System.Windows.Forms.TextBox();
            this.txtOldRegistrySnapshotFile = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCompareOthersSnapshots = new System.Windows.Forms.Button();
            this.btnSelectNewOthersSnapshotFile = new System.Windows.Forms.Button();
            this.btnSelectOldServicesSnapshotFile = new System.Windows.Forms.Button();
            this.txtNewServicesSnapshotFile = new System.Windows.Forms.TextBox();
            this.txtOldOthersSnapshotFile = new System.Windows.Forms.TextBox();
            this.lblUsersFolderPath = new System.Windows.Forms.TextBox();
            this.btnOthersSnapshot = new System.Windows.Forms.Button();
            this.lblRegistryFileName = new System.Windows.Forms.TextBox();
            this.btnRegistrySnapshot = new System.Windows.Forms.Button();
            this.txtFileId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAsterFolder = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 368);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(762, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(70, 17);
            this.lblStatus.Text = "Status Label";
            // 
            // txtDataFolder
            // 
            this.txtDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataFolder.Location = new System.Drawing.Point(10, 35);
            this.txtDataFolder.Name = "txtDataFolder";
            this.txtDataFolder.Size = new System.Drawing.Size(472, 20);
            this.txtDataFolder.TabIndex = 1;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFolder.Location = new System.Drawing.Point(487, 35);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(100, 22);
            this.btnSelectFolder.TabIndex = 2;
            this.btnSelectFolder.Text = "Select Data folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // btnFileSystemSnapshot
            // 
            this.btnFileSystemSnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileSystemSnapshot.Location = new System.Drawing.Point(589, 35);
            this.btnFileSystemSnapshot.Name = "btnFileSystemSnapshot";
            this.btnFileSystemSnapshot.Size = new System.Drawing.Size(156, 22);
            this.btnFileSystemSnapshot.TabIndex = 3;
            this.btnFileSystemSnapshot.Text = "Make file system snapshot";
            this.btnFileSystemSnapshot.UseVisualStyleBackColor = true;
            this.btnFileSystemSnapshot.Click += new System.EventHandler(this.btnFileSystemSnapshot_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCompareFileSystemSnapshots);
            this.groupBox1.Controls.Add(this.btnSelectNewFileSystemSnapshotFile);
            this.groupBox1.Controls.Add(this.btnSelectOldFileSystemSnapshotFile);
            this.groupBox1.Controls.Add(this.txtNewFileSystemSnapshotFile);
            this.groupBox1.Controls.Add(this.txtOldFileSystemSnapshotFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 78);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Compare file system snapshots";
            // 
            // btnCompareFileSystemSnapshots
            // 
            this.btnCompareFileSystemSnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompareFileSystemSnapshots.Location = new System.Drawing.Point(583, 19);
            this.btnCompareFileSystemSnapshots.Name = "btnCompareFileSystemSnapshots";
            this.btnCompareFileSystemSnapshots.Size = new System.Drawing.Size(156, 45);
            this.btnCompareFileSystemSnapshots.TabIndex = 6;
            this.btnCompareFileSystemSnapshots.Text = "Compare file system snapshots";
            this.btnCompareFileSystemSnapshots.UseVisualStyleBackColor = true;
            this.btnCompareFileSystemSnapshots.Click += new System.EventHandler(this.btnCompareFileSystemSnapshots_Click);
            // 
            // btnSelectNewFileSystemSnapshotFile
            // 
            this.btnSelectNewFileSystemSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectNewFileSystemSnapshotFile.Location = new System.Drawing.Point(477, 44);
            this.btnSelectNewFileSystemSnapshotFile.Name = "btnSelectNewFileSystemSnapshotFile";
            this.btnSelectNewFileSystemSnapshotFile.Size = new System.Drawing.Size(100, 22);
            this.btnSelectNewFileSystemSnapshotFile.TabIndex = 5;
            this.btnSelectNewFileSystemSnapshotFile.Text = "Select new file";
            this.btnSelectNewFileSystemSnapshotFile.UseVisualStyleBackColor = true;
            this.btnSelectNewFileSystemSnapshotFile.Click += new System.EventHandler(this.btnSelectNewFileSystemSnapshotFile_Click);
            // 
            // btnSelectOldFileSystemSnapshotFile
            // 
            this.btnSelectOldFileSystemSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectOldFileSystemSnapshotFile.Location = new System.Drawing.Point(477, 19);
            this.btnSelectOldFileSystemSnapshotFile.Name = "btnSelectOldFileSystemSnapshotFile";
            this.btnSelectOldFileSystemSnapshotFile.Size = new System.Drawing.Size(100, 22);
            this.btnSelectOldFileSystemSnapshotFile.TabIndex = 4;
            this.btnSelectOldFileSystemSnapshotFile.Text = "Select old file";
            this.btnSelectOldFileSystemSnapshotFile.UseVisualStyleBackColor = true;
            this.btnSelectOldFileSystemSnapshotFile.Click += new System.EventHandler(this.btnSelectOldFileSystemSnapshotFile_Click);
            // 
            // txtNewFileSystemSnapshotFile
            // 
            this.txtNewFileSystemSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewFileSystemSnapshotFile.Location = new System.Drawing.Point(5, 44);
            this.txtNewFileSystemSnapshotFile.Name = "txtNewFileSystemSnapshotFile";
            this.txtNewFileSystemSnapshotFile.Size = new System.Drawing.Size(467, 20);
            this.txtNewFileSystemSnapshotFile.TabIndex = 3;
            // 
            // txtOldFileSystemSnapshotFile
            // 
            this.txtOldFileSystemSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOldFileSystemSnapshotFile.Location = new System.Drawing.Point(5, 19);
            this.txtOldFileSystemSnapshotFile.Name = "txtOldFileSystemSnapshotFile";
            this.txtOldFileSystemSnapshotFile.Size = new System.Drawing.Size(467, 20);
            this.txtOldFileSystemSnapshotFile.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnCompareRegistrySnapshots);
            this.groupBox2.Controls.Add(this.btnSelectNewRegistrySnapshotFile);
            this.groupBox2.Controls.Add(this.btnSelectOldRegistrySnapshotFile);
            this.groupBox2.Controls.Add(this.txtNewRegistrySnapshotFile);
            this.groupBox2.Controls.Add(this.txtOldRegistrySnapshotFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(743, 78);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Compare registry snapshots";
            // 
            // btnCompareRegistrySnapshots
            // 
            this.btnCompareRegistrySnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompareRegistrySnapshots.Location = new System.Drawing.Point(583, 19);
            this.btnCompareRegistrySnapshots.Name = "btnCompareRegistrySnapshots";
            this.btnCompareRegistrySnapshots.Size = new System.Drawing.Size(156, 45);
            this.btnCompareRegistrySnapshots.TabIndex = 6;
            this.btnCompareRegistrySnapshots.Text = "Compare registry snapshots";
            this.btnCompareRegistrySnapshots.UseVisualStyleBackColor = true;
            this.btnCompareRegistrySnapshots.Click += new System.EventHandler(this.btnCompareRegistrySnapshots_Click);
            // 
            // btnSelectNewRegistrySnapshotFile
            // 
            this.btnSelectNewRegistrySnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectNewRegistrySnapshotFile.Location = new System.Drawing.Point(477, 43);
            this.btnSelectNewRegistrySnapshotFile.Name = "btnSelectNewRegistrySnapshotFile";
            this.btnSelectNewRegistrySnapshotFile.Size = new System.Drawing.Size(100, 22);
            this.btnSelectNewRegistrySnapshotFile.TabIndex = 5;
            this.btnSelectNewRegistrySnapshotFile.Text = "Select new file";
            this.btnSelectNewRegistrySnapshotFile.UseVisualStyleBackColor = true;
            this.btnSelectNewRegistrySnapshotFile.Click += new System.EventHandler(this.btnSelectNewRegistrySnapshotFile_Click);
            // 
            // btnSelectOldRegistrySnapshotFile
            // 
            this.btnSelectOldRegistrySnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectOldRegistrySnapshotFile.Location = new System.Drawing.Point(477, 18);
            this.btnSelectOldRegistrySnapshotFile.Name = "btnSelectOldRegistrySnapshotFile";
            this.btnSelectOldRegistrySnapshotFile.Size = new System.Drawing.Size(100, 22);
            this.btnSelectOldRegistrySnapshotFile.TabIndex = 4;
            this.btnSelectOldRegistrySnapshotFile.Text = "Select old file";
            this.btnSelectOldRegistrySnapshotFile.UseVisualStyleBackColor = true;
            this.btnSelectOldRegistrySnapshotFile.Click += new System.EventHandler(this.btnSelectOldRegistrySnapshotFile_Click);
            // 
            // txtNewRegistrySnapshotFile
            // 
            this.txtNewRegistrySnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewRegistrySnapshotFile.Location = new System.Drawing.Point(5, 44);
            this.txtNewRegistrySnapshotFile.Name = "txtNewRegistrySnapshotFile";
            this.txtNewRegistrySnapshotFile.Size = new System.Drawing.Size(467, 20);
            this.txtNewRegistrySnapshotFile.TabIndex = 3;
            // 
            // txtOldRegistrySnapshotFile
            // 
            this.txtOldRegistrySnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOldRegistrySnapshotFile.Location = new System.Drawing.Point(5, 19);
            this.txtOldRegistrySnapshotFile.Name = "txtOldRegistrySnapshotFile";
            this.txtOldRegistrySnapshotFile.Size = new System.Drawing.Size(467, 20);
            this.txtOldRegistrySnapshotFile.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnCompareOthersSnapshots);
            this.groupBox3.Controls.Add(this.btnSelectNewOthersSnapshotFile);
            this.groupBox3.Controls.Add(this.btnSelectOldServicesSnapshotFile);
            this.groupBox3.Controls.Add(this.txtNewServicesSnapshotFile);
            this.groupBox3.Controls.Add(this.txtOldOthersSnapshotFile);
            this.groupBox3.Location = new System.Drawing.Point(7, 280);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(743, 78);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Compare others snapshots (firewall, services, task scheduler)";
            // 
            // btnCompareOthersSnapshots
            // 
            this.btnCompareOthersSnapshots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompareOthersSnapshots.Location = new System.Drawing.Point(582, 19);
            this.btnCompareOthersSnapshots.Name = "btnCompareOthersSnapshots";
            this.btnCompareOthersSnapshots.Size = new System.Drawing.Size(156, 45);
            this.btnCompareOthersSnapshots.TabIndex = 6;
            this.btnCompareOthersSnapshots.Text = "Compare others snapshots";
            this.btnCompareOthersSnapshots.UseVisualStyleBackColor = true;
            this.btnCompareOthersSnapshots.Click += new System.EventHandler(this.btnCompareOthersSnapshots_Click);
            // 
            // btnSelectNewOthersSnapshotFile
            // 
            this.btnSelectNewOthersSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectNewOthersSnapshotFile.Location = new System.Drawing.Point(477, 43);
            this.btnSelectNewOthersSnapshotFile.Name = "btnSelectNewOthersSnapshotFile";
            this.btnSelectNewOthersSnapshotFile.Size = new System.Drawing.Size(100, 22);
            this.btnSelectNewOthersSnapshotFile.TabIndex = 5;
            this.btnSelectNewOthersSnapshotFile.Text = "Select new file";
            this.btnSelectNewOthersSnapshotFile.UseVisualStyleBackColor = true;
            this.btnSelectNewOthersSnapshotFile.Click += new System.EventHandler(this.btnSelectNewOthersSnapshotFile_Click);
            // 
            // btnSelectOldServicesSnapshotFile
            // 
            this.btnSelectOldServicesSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectOldServicesSnapshotFile.Location = new System.Drawing.Point(477, 18);
            this.btnSelectOldServicesSnapshotFile.Name = "btnSelectOldServicesSnapshotFile";
            this.btnSelectOldServicesSnapshotFile.Size = new System.Drawing.Size(100, 22);
            this.btnSelectOldServicesSnapshotFile.TabIndex = 4;
            this.btnSelectOldServicesSnapshotFile.Text = "Select old file";
            this.btnSelectOldServicesSnapshotFile.UseVisualStyleBackColor = true;
            this.btnSelectOldServicesSnapshotFile.Click += new System.EventHandler(this.btnSelectOldOthersSnapshotFile_Click);
            // 
            // txtNewServicesSnapshotFile
            // 
            this.txtNewServicesSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewServicesSnapshotFile.Location = new System.Drawing.Point(5, 44);
            this.txtNewServicesSnapshotFile.Name = "txtNewServicesSnapshotFile";
            this.txtNewServicesSnapshotFile.Size = new System.Drawing.Size(467, 20);
            this.txtNewServicesSnapshotFile.TabIndex = 3;
            // 
            // txtOldOthersSnapshotFile
            // 
            this.txtOldOthersSnapshotFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOldOthersSnapshotFile.Location = new System.Drawing.Point(5, 19);
            this.txtOldOthersSnapshotFile.Name = "txtOldOthersSnapshotFile";
            this.txtOldOthersSnapshotFile.Size = new System.Drawing.Size(467, 20);
            this.txtOldOthersSnapshotFile.TabIndex = 2;
            // 
            // lblUsersFolderPath
            // 
            this.lblUsersFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblUsersFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsersFolderPath.Location = new System.Drawing.Point(15, 10);
            this.lblUsersFolderPath.Name = "lblUsersFolderPath";
            this.lblUsersFolderPath.ReadOnly = true;
            this.lblUsersFolderPath.Size = new System.Drawing.Size(467, 13);
            this.lblUsersFolderPath.TabIndex = 9;
            this.lblUsersFolderPath.Text = "Label";
            // 
            // btnOthersSnapshot
            // 
            this.btnOthersSnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOthersSnapshot.Location = new System.Drawing.Point(589, 60);
            this.btnOthersSnapshot.Name = "btnOthersSnapshot";
            this.btnOthersSnapshot.Size = new System.Drawing.Size(156, 48);
            this.btnOthersSnapshot.TabIndex = 12;
            this.btnOthersSnapshot.Text = "Make others snapshot (firewall, services, task scheduler)";
            this.btnOthersSnapshot.UseVisualStyleBackColor = true;
            this.btnOthersSnapshot.Click += new System.EventHandler(this.btnOthersSnapshot_Click);
            // 
            // lblRegistryFileName
            // 
            this.lblRegistryFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblRegistryFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRegistryFileName.Location = new System.Drawing.Point(15, 93);
            this.lblRegistryFileName.Name = "lblRegistryFileName";
            this.lblRegistryFileName.ReadOnly = true;
            this.lblRegistryFileName.Size = new System.Drawing.Size(467, 13);
            this.lblRegistryFileName.TabIndex = 13;
            this.lblRegistryFileName.Text = "lblRegistryFileName";
            // 
            // btnRegistrySnapshot
            // 
            this.btnRegistrySnapshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistrySnapshot.Location = new System.Drawing.Point(589, 5);
            this.btnRegistrySnapshot.Name = "btnRegistrySnapshot";
            this.btnRegistrySnapshot.Size = new System.Drawing.Size(156, 22);
            this.btnRegistrySnapshot.TabIndex = 14;
            this.btnRegistrySnapshot.Text = "Make registry snapshot";
            this.btnRegistrySnapshot.UseVisualStyleBackColor = true;
            this.btnRegistrySnapshot.Click += new System.EventHandler(this.btnRegistrySnapshot_Click);
            // 
            // txtFileId
            // 
            this.txtFileId.Location = new System.Drawing.Point(12, 61);
            this.txtFileId.Name = "txtFileId";
            this.txtFileId.Size = new System.Drawing.Size(161, 20);
            this.txtFileId.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Additional file identificator";
            // 
            // btnAsterFolder
            // 
            this.btnAsterFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAsterFolder.Location = new System.Drawing.Point(428, 73);
            this.btnAsterFolder.Name = "btnAsterFolder";
            this.btnAsterFolder.Size = new System.Drawing.Size(156, 22);
            this.btnAsterFolder.TabIndex = 17;
            this.btnAsterFolder.Text = "Backup Aster folder";
            this.btnAsterFolder.UseVisualStyleBackColor = true;
            this.btnAsterFolder.Click += new System.EventHandler(this.btnAsterFolder_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 390);
            this.Controls.Add(this.btnAsterFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileId);
            this.Controls.Add(this.btnRegistrySnapshot);
            this.Controls.Add(this.lblRegistryFileName);
            this.Controls.Add(this.btnOthersSnapshot);
            this.Controls.Add(this.lblUsersFolderPath);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnFileSystemSnapshot);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.txtDataFolder);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Windows Snapshots";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.TextBox txtDataFolder;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Button btnFileSystemSnapshot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCompareFileSystemSnapshots;
        private System.Windows.Forms.Button btnSelectOldFileSystemSnapshotFile;
        private System.Windows.Forms.TextBox txtNewFileSystemSnapshotFile;
        private System.Windows.Forms.TextBox txtOldFileSystemSnapshotFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCompareRegistrySnapshots;
        private System.Windows.Forms.Button btnSelectNewRegistrySnapshotFile;
        private System.Windows.Forms.Button btnSelectOldRegistrySnapshotFile;
        private System.Windows.Forms.TextBox txtNewRegistrySnapshotFile;
        private System.Windows.Forms.TextBox txtOldRegistrySnapshotFile;
        private System.Windows.Forms.Button btnSelectNewFileSystemSnapshotFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCompareOthersSnapshots;
        private System.Windows.Forms.Button btnSelectNewOthersSnapshotFile;
        private System.Windows.Forms.Button btnSelectOldServicesSnapshotFile;
        private System.Windows.Forms.TextBox txtNewServicesSnapshotFile;
        private System.Windows.Forms.TextBox txtOldOthersSnapshotFile;
        private System.Windows.Forms.TextBox lblUsersFolderPath;
        private System.Windows.Forms.Button btnOthersSnapshot;
        private System.Windows.Forms.TextBox lblRegistryFileName;
        private System.Windows.Forms.Button btnRegistrySnapshot;
        private System.Windows.Forms.TextBox txtFileId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAsterFolder;
    }
}

