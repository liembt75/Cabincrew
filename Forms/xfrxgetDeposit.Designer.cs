namespace Cabincrew
{
    partial class xfrxgetDeposit
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
            this.sbtnGetInfo = new DevExpress.XtraEditors.SimpleButton();
            this.sbtnSelectFile = new DevExpress.XtraEditors.SimpleButton();
            this.spreadsheetControl1 = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            this.lblFilename = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // sbtnGetInfo
            // 
            this.sbtnGetInfo.Enabled = false;
            this.sbtnGetInfo.Location = new System.Drawing.Point(10, 32);
            this.sbtnGetInfo.Name = "sbtnGetInfo";
            this.sbtnGetInfo.Size = new System.Drawing.Size(90, 23);
            this.sbtnGetInfo.TabIndex = 7;
            this.sbtnGetInfo.Text = "Get Info";
            this.sbtnGetInfo.Click += new System.EventHandler(this.sbtnGetInfo_Click);
            // 
            // sbtnSelectFile
            // 
            this.sbtnSelectFile.Location = new System.Drawing.Point(10, 3);
            this.sbtnSelectFile.Name = "sbtnSelectFile";
            this.sbtnSelectFile.Size = new System.Drawing.Size(90, 23);
            this.sbtnSelectFile.TabIndex = 6;
            this.sbtnSelectFile.Text = "Select File Excel";
            this.sbtnSelectFile.Click += new System.EventHandler(this.sbtnSelectFile_Click);
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Location = new System.Drawing.Point(3, 105);
            this.spreadsheetControl1.Name = "spreadsheetControl1";
            this.spreadsheetControl1.Size = new System.Drawing.Size(913, 253);
            this.spreadsheetControl1.TabIndex = 5;
            this.spreadsheetControl1.Text = "spreadsheetControl1";
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(106, 8);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(0, 13);
            this.lblFilename.TabIndex = 8;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Enabled = false;
            this.simpleButton1.Location = new System.Drawing.Point(120, 32);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(90, 23);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "Update Info";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // xfrxgetDeposit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 363);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.sbtnGetInfo);
            this.Controls.Add(this.sbtnSelectFile);
            this.Controls.Add(this.spreadsheetControl1);
            this.Name = "xfrxgetDeposit";
            this.Text = "Get Deposit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbtnGetInfo;
        private DevExpress.XtraEditors.SimpleButton sbtnSelectFile;
        private DevExpress.XtraSpreadsheet.SpreadsheetControl spreadsheetControl1;
        private System.Windows.Forms.Label lblFilename;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}