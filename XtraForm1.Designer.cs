namespace Cabincrew
{
    partial class XtraForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spreadsheetControl1 = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            this.sbtnSelectFile = new DevExpress.XtraEditors.SimpleButton();
            this.lblFilename = new DevExpress.XtraEditors.LabelControl();
            this.sbtnGetInfo = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spreadsheetControl1.Location = new System.Drawing.Point(0, 59);
            this.spreadsheetControl1.Name = "spreadsheetControl1";
            this.spreadsheetControl1.Size = new System.Drawing.Size(927, 278);
            this.spreadsheetControl1.TabIndex = 1;
            this.spreadsheetControl1.Text = "spreadsheetControl1";
            this.spreadsheetControl1.Click += new System.EventHandler(this.spreadsheetControl1_Click);
            // 
            // sbtnSelectFile
            // 
            this.sbtnSelectFile.Location = new System.Drawing.Point(9, 1);
            this.sbtnSelectFile.Name = "sbtnSelectFile";
            this.sbtnSelectFile.Size = new System.Drawing.Size(90, 23);
            this.sbtnSelectFile.TabIndex = 2;
            this.sbtnSelectFile.Text = "Select File Excel";
            this.sbtnSelectFile.Click += new System.EventHandler(this.sbtnSelectFile_Click);
            // 
            // lblFilename
            // 
            this.lblFilename.Location = new System.Drawing.Point(106, 9);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(0, 13);
            this.lblFilename.TabIndex = 3;
            this.lblFilename.Click += new System.EventHandler(this.labelControl1_Click);
            // 
            // sbtnGetInfo
            // 
            this.sbtnGetInfo.Enabled = false;
            this.sbtnGetInfo.Location = new System.Drawing.Point(9, 30);
            this.sbtnGetInfo.Name = "sbtnGetInfo";
            this.sbtnGetInfo.Size = new System.Drawing.Size(90, 23);
            this.sbtnGetInfo.TabIndex = 4;
            this.sbtnGetInfo.Text = "Get Info";
            this.sbtnGetInfo.Click += new System.EventHandler(this.sbtnGetInfo_Click);
            // 
            // XtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(927, 337);
            this.Controls.Add(this.sbtnGetInfo);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.sbtnSelectFile);
            this.Controls.Add(this.spreadsheetControl1);
            this.Name = "XtraForm1";
            this.Text = "XtraForm1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.XtraForm1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraSpreadsheet.SpreadsheetControl spreadsheetControl1;
        private DevExpress.XtraEditors.SimpleButton sbtnSelectFile;
        private DevExpress.XtraEditors.LabelControl lblFilename;
        private DevExpress.XtraEditors.SimpleButton sbtnGetInfo;
    }
}