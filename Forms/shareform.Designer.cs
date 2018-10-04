namespace Cabincrew.Forms
{
    partial class shareform
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
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.sbtnLoadExcel = new DevExpress.XtraEditors.SimpleButton();
            this.spreadsheetControl1 = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(85, 3);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(765, 20);
            this.textEdit1.TabIndex = 7;
            // 
            // sbtnLoadExcel
            // 
            this.sbtnLoadExcel.Location = new System.Drawing.Point(4, 1);
            this.sbtnLoadExcel.Name = "sbtnLoadExcel";
            this.sbtnLoadExcel.Size = new System.Drawing.Size(75, 23);
            this.sbtnLoadExcel.TabIndex = 6;
            this.sbtnLoadExcel.Text = "Load Excel";
            this.sbtnLoadExcel.Click += new System.EventHandler(this.sbtnLoadExcel_Click);
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Location = new System.Drawing.Point(6, 58);
            this.spreadsheetControl1.Name = "spreadsheetControl1";
            this.spreadsheetControl1.Size = new System.Drawing.Size(844, 200);
            this.spreadsheetControl1.TabIndex = 4;
            this.spreadsheetControl1.Text = "spreadsheetControl1";
            // 
            // shareform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 261);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.sbtnLoadExcel);
            this.Controls.Add(this.spreadsheetControl1);
            this.Name = "shareform";
            this.Text = "shareform";
            this.Load += new System.EventHandler(this.shareform_Load);
            this.SizeChanged += new System.EventHandler(this.shareform_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit textEdit1;
        public DevExpress.XtraEditors.SimpleButton sbtnLoadExcel;
        public DevExpress.XtraSpreadsheet.SpreadsheetControl spreadsheetControl1;
    }
}