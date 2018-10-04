namespace Cabincrew.Forms
{
    partial class frmVNCrewFlightDelete
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
            this.sbtnCheck = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEdit1
            // 
            this.textEdit1.Size = new System.Drawing.Size(648, 20);
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Size = new System.Drawing.Size(723, 200);
            // 
            // sbtnCheck
            // 
            this.sbtnCheck.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.sbtnCheck.Appearance.Options.UseFont = true;
            this.sbtnCheck.Location = new System.Drawing.Point(7, 30);
            this.sbtnCheck.Name = "sbtnCheck";
            this.sbtnCheck.Size = new System.Drawing.Size(75, 23);
            this.sbtnCheck.TabIndex = 8;
            this.sbtnCheck.Text = "Check";
            this.sbtnCheck.Click += new System.EventHandler(this.sbtnCheck_Click);
            // 
            // frmVNCrewFlightDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 261);
            this.Controls.Add(this.sbtnCheck);
            this.Name = "frmVNCrewFlightDelete";
            this.Text = "frmVNCrewFlightDelete";
            this.Load += new System.EventHandler(this.frmVNCrewFlightDelete_Load);
            this.Controls.SetChildIndex(this.spreadsheetControl1, 0);
            this.Controls.SetChildIndex(this.sbtnLoadExcel, 0);
            this.Controls.SetChildIndex(this.textEdit1, 0);
            this.Controls.SetChildIndex(this.sbtnCheck, 0);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbtnCheck;
    }
}