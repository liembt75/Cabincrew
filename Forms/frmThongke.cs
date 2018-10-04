using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Cabincrew.Model.HR;
using DevExpress.XtraSpreadsheet;
using DevExpress.Spreadsheet;
using System.IO;


namespace Cabincrew.Forms
{
    public partial class frmThongke : Cabincrew.Forms.shareform
    {
        public frmThongke()
        {
            InitializeComponent();
        }

        private void frmThongke_Load(object sender, EventArgs e)
        {

        }

        private void sbtnCollectInfo_Click(object sender, EventArgs e)
        {
            List<thongke> dstk = new List<thongke>();
            //try {
                using (HREntities hr = new HREntities())
                {
                    var dsns = hr.HoSoGocs.Where(x => x.nghiviec == false).ToList();
                    var toeic = hr.ngoaingus.Where(x => x.ngoaingu_loai == 565 && (x.ngoaingu_bangcap == 669 || x.ngoaingu_bangcap == 670 || x.ngoaingu_bangcap == 671)).ToList();
                    var nnkhac = hr.ngoaingus.Where(x => x.ngoaingu_loai != 565 && x.ngoaingu_bangcap == 3799).ToList();
                    foreach (var ns in dsns)
                    {
                        var item = new thongke();
                        item.id_ns = ns.id;
                        item.mans = ns.mans;
                        item.hoten = ns.tenkodau;
                        var ta  = toeic.Where(x => x.id_ns == ns.id).OrderByDescending(y => y.ngoaingu_ngaycap).FirstOrDefault();
                        var nnk = nnkhac.Where(x => x.id_ns == ns.id).OrderByDescending(y => y.ngoaingu_ngaycap).FirstOrDefault();
                        var cm = hr.nhomchuyenmons.Where(x => x.id_ns == ns.id).FirstOrDefault();
                        
                        if (ta != null)
                        {
                            if(ta.ngoaingu_diemtong!=null)
                                item.diemta = (int)ta.ngoaingu_diemtong;
                            var dm = hr.danhmucs.Where(x => x.id == ta.ngoaingu_bangcap).FirstOrDefault();
                            if (dm != null)
                                item.loaita = dm.TenDanhMuc;
                        }
                        if (nnk != null)
                        {
                            var dm = hr.danhmucs.Where(x => x.id == nnk.ngoaingu_loai).FirstOrDefault();
                            if (dm != null)
                                item.nnkhac = dm.TenDanhMuc;
                        }
                        if (ns.hocvantd >0)
                        {
                            var dm = hr.danhmucs.Where(x => x.id == ns.hocvantd).FirstOrDefault();
                            if (dm != null)
                                item.hocvan = dm.TenDanhMuc;
                        }
                        if (cm != null)
                        {
                            var dm = hr.danhmucs.Where(x => x.id == cm.chuyenmon).FirstOrDefault();
                            if (dm != null)
                                item.chuyenmon = dm.TenDanhMuc;
                            var dm1 = hr.danhmucs.Where(x => x.id == cm.nhomchuyenmon1).FirstOrDefault();
                            if (dm1 != null)
                                item.nhomchuyenmon = dm1.TenDanhMuc;
                        }                        
                        dstk.Add(item);
                    }//foreach
                }//Using
            //}//Try
            //catch(Exception ex){
            //    MessageBox.Show(ex.Message);
            //}
            //Ghi kết quả
            if (dstk.Count > 0)
            {
                Worksheet spreadst = base.spreadsheetControl1.ActiveWorksheet;
                spreadst.Cells[0, 0].Value = "STT";
                spreadst.Cells[0, 1].Value = "Manv";
                spreadst.Cells[0, 2].Value = "Họ và Tên";
                spreadst.Cells[0, 3].Value = "Loai tiếng Anh";
                spreadst.Cells[0, 4].Value = "Điểm tiếng Anh";
                spreadst.Cells[0, 5].Value = "Học vấn";
                spreadst.Cells[0, 6].Value = "Chuyên môn";
                spreadst.Cells[0, 7].Value = "Nhóm chuyên môn";
                spreadst.Cells[0, 8].Value = "Ngoại ngữ khác";
                int dong = 1, stt = 1;
                foreach (var hs in dstk)
                {
                    spreadst.Cells[dong, 0].Value = stt;
                    spreadst.Cells[dong, 1].Value = "'"+hs.mans.Trim();
                    spreadst.Cells[dong, 2].Value = hs.hoten;
                    spreadst.Cells[dong, 3].Value = hs.loaita;
                    spreadst.Cells[dong, 4].Value = hs.diemta;
                    spreadst.Cells[dong, 5].Value = hs.hocvan;
                    spreadst.Cells[dong, 6].Value = hs.chuyenmon;
                    spreadst.Cells[dong, 7].Value = hs.nhomchuyenmon;
                    spreadst.Cells[dong, 8].Value = hs.nnkhac;
                    dong++;
                    stt++;
                }
                //Lưu Kết quả CT
                string filekq;
                filekq = @"f:\temp\tke-diVan.xlsx";
                IWorkbook workbook = base.spreadsheetControl1.Document;
                using (FileStream stream = new FileStream(filekq, FileMode.Create, FileAccess.ReadWrite))
                {
                    workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                }
                MessageBox.Show("Complete!");
            }//
        }
    }
    class thongke
    {
        public int id_ns { get; set; }
        public string mans { get; set; }
        public string hoten { get; set; }
        public string loaita { get; set; }
        public int diemta { get; set; }
        public string nnkhac { get; set; }
        public string hocvan { get; set; }
        public string chuyenmon { get; set; }
        public string nhomchuyenmon { get; set; }
    }
}