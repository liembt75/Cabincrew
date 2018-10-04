namespace Cabincrew.Forms
{
    partial class frmVNCrewNoCrew
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
            this.sbtnCollectInfo = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEdit1
            // 
            this.textEdit1.Size = new System.Drawing.Size(195, 20);
            this.textEdit1.Visible = false;
            // 
            // sbtnLoadExcel
            // 
            this.sbtnLoadExcel.Visible = false;
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Size = new System.Drawing.Size(270, 200);
            // 
            // sbtnCollectInfo
            // 
            this.sbtnCollectInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.sbtnCollectInfo.Appearance.Options.UseFont = true;
            this.sbtnCollectInfo.Location = new System.Drawing.Point(6, 29);
            this.sbtnCollectInfo.Name = "sbtnCollectInfo";
            this.sbtnCollectInfo.Size = new System.Drawing.Size(75, 23);
            this.sbtnCollectInfo.TabIndex = 8;
            this.sbtnCollectInfo.Text = "CollectInfo";
            this.sbtnCollectInfo.Click += new System.EventHandler(this.sbtnCollectInfo_Click);
            // 
            // frmVNCrewNoCrew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.sbtnCollectInfo);
            this.Name = "frmVNCrewNoCrew";
            this.Text = "frmVNCrewNoCrew";
            this.Load += new System.EventHandler(this.frmVNCrewNoCrew_Load);
            this.Controls.SetChildIndex(this.spreadsheetControl1, 0);
            this.Controls.SetChildIndex(this.sbtnLoadExcel, 0);
            this.Controls.SetChildIndex(this.textEdit1, 0);
            this.Controls.SetChildIndex(this.sbtnCollectInfo, 0);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbtnCollectInfo;
    }
}