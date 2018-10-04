namespace Cabincrew.Forms
{
    partial class frmThongke
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
            this.SuspendLayout();
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Size = new System.Drawing.Size(890, 200);
            // 
            // sbtnCollectInfo
            // 
            this.sbtnCollectInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.sbtnCollectInfo.Appearance.Options.UseFont = true;
            this.sbtnCollectInfo.Location = new System.Drawing.Point(6, 28);
            this.sbtnCollectInfo.Name = "sbtnCollectInfo";
            this.sbtnCollectInfo.Size = new System.Drawing.Size(75, 23);
            this.sbtnCollectInfo.TabIndex = 8;
            this.sbtnCollectInfo.Text = "Collect Info";
            this.sbtnCollectInfo.Click += new System.EventHandler(this.sbtnCollectInfo_Click);
            // 
            // frmThongke
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 261);
            this.Controls.Add(this.sbtnCollectInfo);
            this.Name = "frmThongke";
            this.Text = "frmThongke";
            this.Load += new System.EventHandler(this.frmThongke_Load);
            this.Controls.SetChildIndex(this.spreadsheetControl1, 0);
            this.Controls.SetChildIndex(this.sbtnCollectInfo, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbtnCollectInfo;
    }
}