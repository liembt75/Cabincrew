namespace Cabincrew.Forms
{
    partial class frmGetKTKL
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.edtTungay = new DevExpress.XtraEditors.DateEdit();
            this.edtToingay = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTungay.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTungay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtToingay.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtToingay.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Size = new System.Drawing.Size(795, 200);
            // 
            // sbtnCollectInfo
            // 
            this.sbtnCollectInfo.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.sbtnCollectInfo.Location = new System.Drawing.Point(336, 29);
            this.sbtnCollectInfo.Name = "sbtnCollectInfo";
            this.sbtnCollectInfo.Size = new System.Drawing.Size(75, 23);
            this.sbtnCollectInfo.TabIndex = 8;
            this.sbtnCollectInfo.Text = "Collect Info";
            this.sbtnCollectInfo.Click += new System.EventHandler(this.sbtnCollectInfo_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(6, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(47, 16);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "Từ ngày";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(171, 32);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(49, 16);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "Tới ngày";
            // 
            // edtTungay
            // 
            this.edtTungay.EditValue = null;
            this.edtTungay.Location = new System.Drawing.Point(65, 32);
            this.edtTungay.Name = "edtTungay";
            this.edtTungay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtTungay.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtTungay.Properties.DisplayFormat.FormatString = "dd/MM/yyy";
            this.edtTungay.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtTungay.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.edtTungay.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtTungay.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.edtTungay.Size = new System.Drawing.Size(100, 20);
            this.edtTungay.TabIndex = 13;
            this.edtTungay.EditValueChanged += new System.EventHandler(this.edtTungay_EditValueChanged);
            // 
            // edtToingay
            // 
            this.edtToingay.EditValue = null;
            this.edtToingay.Location = new System.Drawing.Point(226, 31);
            this.edtToingay.Name = "edtToingay";
            this.edtToingay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtToingay.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.edtToingay.Properties.DisplayFormat.FormatString = "dd/MM/yyy";
            this.edtToingay.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtToingay.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.edtToingay.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.edtToingay.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.edtToingay.Size = new System.Drawing.Size(100, 20);
            this.edtToingay.TabIndex = 14;
            // 
            // frmGetKTKL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 261);
            this.Controls.Add(this.edtToingay);
            this.Controls.Add(this.edtTungay);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.sbtnCollectInfo);
            this.Name = "frmGetKTKL";
            this.Text = "frmGetKTKL";
            this.Load += new System.EventHandler(this.frmGetKTKL_Load);
            this.Controls.SetChildIndex(this.spreadsheetControl1, 0);
            this.Controls.SetChildIndex(this.sbtnCollectInfo, 0);
            this.Controls.SetChildIndex(this.labelControl1, 0);
            this.Controls.SetChildIndex(this.labelControl2, 0);
            this.Controls.SetChildIndex(this.edtTungay, 0);
            this.Controls.SetChildIndex(this.edtToingay, 0);
            ((System.ComponentModel.ISupportInitialize)(this.edtTungay.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTungay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtToingay.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtToingay.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbtnCollectInfo;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit edtTungay;
        private DevExpress.XtraEditors.DateEdit edtToingay;
    }
}