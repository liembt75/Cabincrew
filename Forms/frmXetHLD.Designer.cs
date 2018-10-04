namespace Cabincrew
{
    partial class frmXetHDLD
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.sbtnLoadExcel = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Location = new System.Drawing.Point(-1, 60);
            this.spreadsheetControl1.Name = "spreadsheetControl1";
            this.spreadsheetControl1.Size = new System.Drawing.Size(400, 200);
            this.spreadsheetControl1.TabIndex = 0;
            this.spreadsheetControl1.Text = "spreadsheetControl1";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(4, 31);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Collect Info";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // sbtnLoadExcel
            // 
            this.sbtnLoadExcel.Location = new System.Drawing.Point(4, 2);
            this.sbtnLoadExcel.Name = "sbtnLoadExcel";
            this.sbtnLoadExcel.Size = new System.Drawing.Size(75, 23);
            this.sbtnLoadExcel.TabIndex = 2;
            this.sbtnLoadExcel.Text = "Load Excel";
            this.sbtnLoadExcel.Click += new System.EventHandler(this.sbtnLoadExcel_Click);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(85, 4);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(825, 20);
            this.textEdit1.TabIndex = 3;
            // 
            // frmXetHDLD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 261);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.sbtnLoadExcel);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.spreadsheetControl1);
            this.Name = "frmXetHDLD";
            this.Text = "Lấy thông tin xét chuyển HĐLĐ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmXetHDLD_Load);
            this.SizeChanged += new System.EventHandler(this.frmXetHDLD_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraSpreadsheet.SpreadsheetControl spreadsheetControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton sbtnLoadExcel;
        private DevExpress.XtraEditors.TextEdit textEdit1;



    }
}