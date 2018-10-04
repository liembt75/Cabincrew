using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cabincrew.Model.HR;
using Cabincrew.Model.SMS;
using Cabincrew.Model.CCSK;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using NDbfReader;
using DotNetDBF;
using System.Runtime.InteropServices;
using Domino;
using Cabincrew.Utils;
using LinqToExcel;
using Cabincrew.Forms;
//using Cabincrew.AVES;

namespace Cabincrew
{
    public partial class MainForm : Form
    {
        partial class donvihc
        {
            public string ma_tt { get; set; }
            public string tinh_thanh { get; set; }
            public string ma_qh { get; set; }
            public string quan_huyen { get; set; }
            public string ma_px { get; set; }
            public string phuong_xa { get; set; }
        };
        public MainForm()
        {
            InitializeComponent();
        }

        private void diLanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime d1, d2;
            string manv,hoten, constring;
            constring = "Data Source=10.105.2.252;Initial Catalog=VietnamRedant;Persist Security Info=True;User ID=liembt;Password=Ong@Gia!2017";
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataAdapter sda;
            DataTable dt = new DataTable();
            //DataSet dt = new DataSet();
            conn = new SqlConnection(constring);
            //cmd.CommandType = CommandType.Text;

            d1 = new DateTime(1980,01,01);
            d2 = new DateTime(2017, 11, 30);
            Excel.Application xlApp;
            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlSrcSheet, xlDesSheet;

            xlApp = new Excel.Application();
            //string filename = @"f:\downloads\S-P1-Aug2017.xlsx";
            //string filename = @"f:\downloads\Y-B-Aug2017.xlsx";
            //string filename = @"f:\downloads\THI NANG BAC C 08.11.2017 HAN.xlsx";
            string filename = @"f:\temp\THI CBT TVTB1 07.12.2017 HAN.xlsx";
            xlWorkbook= xlApp.Workbooks.Open(filename);
            xlApp.Visible = true;
            xlSrcSheet=xlWorkbook.Worksheets.get_Item(1);
            xlDesSheet=xlWorkbook.Worksheets.get_Item(2);
            xlDesSheet.Columns[4].Columnwidth = 255;
            int k = 2;
            
            for (int i = 14; i <= 17; i++)
            {
                manv = xlSrcSheet.Cells[i, 2].value;
                hoten = xlSrcSheet.Cells[i, 3].value.Trim()+" "+ xlSrcSheet.Cells[i, 4].value.Trim(); 
                if (manv != null)
                {
                    cmd = new SqlCommand("SELECT * FROM [VietnamRedant].[dbo].[PView_ktkl] where mans=@mans and ktkl_ngayqd>=@tungay and ktkl_ngayqd<=@toingay order by kyluat,ktkl_ngayqd", conn);
                    cmd.Parameters.Add("@mans", SqlDbType.Char);
                    cmd.Parameters["@mans"].Value = manv;
                    cmd.Parameters.Add("@tungay", SqlDbType.Date);
                    cmd.Parameters["@tungay"].Value = d1;
                    cmd.Parameters.Add("@toingay", SqlDbType.Date);
                    cmd.Parameters["@toingay"].Value = d2;                    
                    sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0) //có kết quả thì ghi ra excel
                    {
                        xlDesSheet.Cells[k, 1].value = "'"+manv;
                        xlDesSheet.Cells[k, 2].value = hoten;
                        k++;
                        foreach (DataRow dr in dt.Rows)
                        {
                            //xlDesSheet.Cells[k, 1].value = k - 1;
                            //xlDesSheet.Cells[k,2].value= "'"+dr["mans"].ToString();
                            //xlDesSheet.Cells[k, 4].value = "'" + dr["ktkl_ngay"].ToString().Substring(0,10);
                            xlDesSheet.Cells[k, 2].value = "'" + dr["ktkl_ngayqd"].ToString().Substring(0, 10);
                            xlDesSheet.Cells[k, 3].value = dr["TenDanhMuc"].ToString();
                            xlDesSheet.Cells[k, 4].value = dr["ktkl_ndung"].ToString().Replace("\n"," ").Replace("\r"," ");
                            k++;
                        }
                    }
                    //Xoa trắng
                    cmd.Parameters.Clear();
                    dt.Clear();
                    cmd.Dispose();                    
                    sda.Dispose();
                }
            }
            conn.Dispose();
            MessageBox.Show("Done!");
        }

        private void updateDBHCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Excel.Application xlApp;
            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlSrcSheet, xlDesSheet;
            xlApp = new Excel.Application();
            string filename = @"f:\downloads\DANH MUC DIA BAN HANH CHINH %281%2921.9.17.xlsx";
            xlWorkbook = xlApp.Workbooks.Open(filename);
            xlApp.Visible = true;
            xlSrcSheet = xlWorkbook.Worksheets.get_Item(1);
            List<donvihc> dsdvhc =new List<donvihc>();
            #region Lấy số liệu địa bàn hành chính
            for (int i = 5; i <= 11940; i++)
            {
                donvihc dv= new donvihc();
                                
                dv.ma_qh = xlSrcSheet.Cells[i, 5].value;
                if (dv.ma_qh != null)
                {

                    if (int.Parse(dv.ma_qh) > 0)
                    {
                        dv.phuong_xa = xlSrcSheet.Cells[i, 2].value;
                        dv.ma_px = xlSrcSheet.Cells[i, 1].value;
                        dv.quan_huyen = xlSrcSheet.Cells[i, 6].value;
                        dv.tinh_thanh = xlSrcSheet.Cells[i, 8].value;
                        dv.ma_tt = xlSrcSheet.Cells[i, 7].value;
                        dsdvhc.Add(dv);
                    }
                }
            }
            xlWorkbook.Close();
            #endregion 
            #region Cập nhật file thiếu
            filename = @"F:\Temp\DS_DBHC_L3.xlsx";
            xlWorkbook = xlApp.Workbooks.Open(filename);
            xlSrcSheet = xlWorkbook.Worksheets.get_Item(1);
            for (int j = 2; j <= 416; j++)
            {
                donvihc dv = new donvihc();
                dv.tinh_thanh = xlSrcSheet.Cells[j, 7].value;
                if (dv.tinh_thanh.ToUpper() == "HÀ NỘI")
                {
                    dv.tinh_thanh = "Thành phố Hà Nội";
                }
                else 
                    if(dv.tinh_thanh.ToUpper()=="TP HCM"){
                        dv.tinh_thanh = "Thành phố Hồ Chí Minh";
                    }
                else
                        if (dv.tinh_thanh.ToUpper() == "HẢI PHÒNG")
                        {
                            dv.tinh_thanh = "Thành phố Hải Phòng";
                        }
                        else
                            if (dv.tinh_thanh.ToUpper() == "ĐÀ NẴNG")
                            {
                                dv.tinh_thanh = "Thành phố Đà Nẵng";
                            }
                            else
                                if (dv.tinh_thanh.ToUpper() == "CẦN THƠ")
                                {
                                    dv.tinh_thanh = "Thành phố Cần Thơ";
                                }
                                else 
                                {
                                    dv.tinh_thanh = "Tỉnh " + dv.tinh_thanh;
                                 }
                
                dv.quan_huyen = xlSrcSheet.Cells[j, 8].value.ToString();
                dv.phuong_xa = xlSrcSheet.Cells[j, 9].value.ToString();
                try
                {
                    donvihc dv1;
                    //dv1 = dsdvhc.FirstOrDefault(o => o.tinh_thanh.ToUpper() == dv.tinh_thanh.ToUpper());
                    
                    dv1 = dsdvhc.Where(o => (o.tinh_thanh.ToUpper().Contains(dv.tinh_thanh.ToUpper())) && 
                                            (o.quan_huyen.ToUpper().Contains(dv.quan_huyen.ToUpper())) && 
                                            (o.phuong_xa.ToUpper().Contains(dv.phuong_xa.ToUpper()))).FirstOrDefault();
                    xlSrcSheet.Cells[j, 10].value = "'"+dv1.ma_tt;
                    xlSrcSheet.Cells[j, 11].value = "'" + dv1.ma_qh;
                    xlSrcSheet.Cells[j, 12].value = "'" + dv1.ma_px;
                }
                catch(Exception ex)
                {

                }
            }
            #endregion 
            MessageBox.Show("Done!");
        }

        private void pTCLDToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Lấy thông tin hồ sơ chuyên cơ
        private void chuyênCơToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string f_mau = @"f:\fox_app\chuyenco\hoso.docx";
            string f_ex = @"f:\fox_app\chuyenco\goc\Danh sach chuyen di Thai Lan.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\Danh sach chuyen di Nhat Ban.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\Danh sach chuyen di Canada- Argentina.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\Doi nguoi-Nhat Ban.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\Bosung-22May2018.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\DS-06Jun2018.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\DS-08Jun2018.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\Chuyenco_13Aug2018.xlsx";
            f_ex = @"f:\fox_app\chuyenco\goc\Chuyenco_13Aug2018_2.xlsx";

            string f_hs;
            Excel.Application xlApp;
            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlSrcSheet;

            xlApp = new Excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(f_ex);
            //xlApp.Visible = true;
            xlSrcSheet = xlWorkbook.Worksheets.get_Item(1);
            xlSrcSheet.Cells[1,19].value = "Ngày sinh";
            xlSrcSheet.Cells[1,20].value="HCPT Số";
            xlSrcSheet.Cells[1,21].value="HCPT Cấp";
            xlSrcSheet.Cells[1,22].value="HCPT Hết Hạn";
            xlSrcSheet.Cells[1,23].value="HCCV Số";
            xlSrcSheet.Cells[1,24].value="HCCV Cấp";
            xlSrcSheet.Cells[1,25].value="HCCV Hết hạn";
            xlSrcSheet.Cells[1,26].value="Giờ bay 321";
            xlSrcSheet.Cells[1,27].value="Giờ bay 350";
            xlSrcSheet.Cells[1,28].value="Giờ bay 787";
            xlSrcSheet.Cells[1, 29].value = "Tổng giờ bay";
            xlSrcSheet.Cells[1, 30].value = "TOEIC";


            //DBFReader reader = new DBFReader();

            Word.Application oApp;
            Word.Document oDoc;
            oApp = new Word.Application();
            
            HREntities db = new HREntities();
            SataHRMEntities dbsms = new SataHRMEntities();
            CCSKEntities dbsk = new CCSKEntities();
            //Danh sách tiếp viên
            FileStream fstv = new FileStream(@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\dm_tvien.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable tv = Table.Open(fstv).AsDataTable();
            //Giấy tờ bay
            FileStream fsgtb = new FileStream(@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\prgkb\giaytobay.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable gtb = Table.Open(fsgtb).AsDataTable();
            //Tổng hợp giờ bay
            FileStream fsgb = new FileStream(@"f:\fox_app\chuyenco\dscuoi.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable gb = Table.Open(fsgb).AsDataTable();
            //Đào tạo
            FileStream fs = new FileStream(@"\\10.100.8.30\foxapp\hldt\data\process.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //FileStream fs = new FileStream(@"f:\temp\process.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable hldt = Table.Open(fs).AsDataTable();
            //Hộ chiếu
            FileStream fs1 = new FileStream(@"\\10.100.8.30\foxapp\hldt\data\cctv0.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable cctv0 = Table.Open(fs1).AsDataTable();            
            
            List<PView_toeic> diemtoeic;
            diemtoeic=(from y in db.PView_toeics select y).ToList();
            
            List<View_Healthcare> dscc;
            dscc = (from cc in dbsk.View_Healthcare select cc).ToList();
            
            HoSoGoc hs;
            danhmuc dm;
            chucvu cv;
            smsAddressBook contact;
            View_Healthcare ccsk;
            int i = 1;
            int dongtrong=0;
            string _manv = "";
            short manv;
            
            while(dongtrong<20){
                _manv = xlSrcSheet.Cells[i, 1].value;
                if (_manv == null)
                {
                    dongtrong++;
                    i++;
                    continue;
                }
                if (!Int16.TryParse(_manv,out manv))
                {
                    dongtrong++;
                    i++;
                    continue;
                }
                dongtrong = 0;
#region xu_ly_tung_nguoi
                
                //oApp.Visible = true;
                hs = db.HoSoGocs.Where(o => o.mans.Trim() == _manv).FirstOrDefault();
                f_hs = @"f:\fox_app\chuyenco\hoso\" +hs.Tenkd.Trim()+ ".docx";
                File.Copy(f_mau, f_hs, true);

                oDoc = oApp.Documents.Open(f_hs);
                ((Word.FormField)oDoc.FormFields.get_Item("txtHovaTen")).Result = hs.ns_ho.Trim()+" "+hs.ns_ten.Trim();
                oDoc.FormFields.get_Item("txtstt").Result = hs.ns_stt;
                oDoc.FormFields.get_Item("txtNgaysinh").Result = hs.ngaysinh.ToString("dd/MM/yyyy");
                dm = db.danhmucs.Where(m => m.id == hs.quequan_tinhtp).FirstOrDefault();
                oDoc.FormFields.get_Item("txtQuequan").Result = hs.quequan_dc.Trim()+", "+dm.TenDanhMuc.Trim();
                dm = db.danhmucs.Where(m => m.id == hs.noio_tinhtp).FirstOrDefault();
                oDoc.FormFields.get_Item("TxtDiachi").Result = hs.noio_dc + ", " + dm.TenDanhMuc.Trim();

                contact = dbsms.smsAddressBooks.Where(obj => obj.ContactCode.Trim() == _manv).FirstOrDefault();
                oDoc.FormFields.get_Item("txtDienthoai").Result = contact.MobilePhone;
                
                cv = db.chucvus.Where(v => v.id_ns == hs.id).FirstOrDefault();
                dm = db.danhmucs.Where(m => m.id == cv.chucvu1).FirstOrDefault();
                oDoc.FormFields.get_Item("txtChucvucq").Result = dm.TenDanhMuc;

                
                if (hs.dang_ngaykn != null)
                {
                    dm = db.danhmucs.Where(m => m.id == hs.dang_chucvu).FirstOrDefault();
                    if (dm != null)
                    {
                        oDoc.FormFields.get_Item("txtChucvuDang").Result = dm.TenDanhMuc.Trim();                      
                    }
                    else
                    {
                        oDoc.FormFields.get_Item("txtChucvuDang").Result = "";
                    
                    }
                    oDoc.FormFields.get_Item("txtNgayvaodang").Result = hs.dang_ngaykn.ToString();
                }                    

                dm = db.danhmucs.Where(m => m.id == hs.bophanlamviec).FirstOrDefault();
                oDoc.FormFields.get_Item("txtBophan").Result = dm.TenDanhMuc;
                
                oDoc.FormFields.get_Item("txtNgayvn").Result = hs.bienche_tct.ToString();

                dm = db.danhmucs.Where(m => m.id == hs.hocvantd).FirstOrDefault();
                oDoc.FormFields.get_Item("txtTrinhdo").Result = dm.TenDanhMuc.Trim();
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = dt1.AddMonths(-12);

                var ktkl = from kk in db.khenkluats
                       where (kk.kyluat==false) && (kk.ktkl_ngayqd<=dt1) && (kk.ktkl_ngayqd>=dt2) && (kk.id_ns==hs.id) && (kk.ktkl_hinhthuc!=null)
                       select kk;
                int solan = 0, sl,nht;
                string kq = "",kq1,kq2,ht="";
                foreach (khenkluat kl in ktkl)
                {
                    solan++;                    
                    nht = (int)kl.ktkl_hinhthuc;
                    if(nht!=3677 && nht!=3679 && nht!=3687 && nht!=4020){
                        ht = kl.ktkl_hinhthuc.ToString().Trim();
                        if (kq.Contains(ht))
                        {
                            kq1 = kq.Substring(0, kq.IndexOf(ht) + ht.Length + 1);
                            kq2 = kq.Substring(kq.IndexOf(ht) + ht.Length + 1);
                            sl = Int16.Parse(kq2.Substring(0, kq2.IndexOf(';')));
                            kq = kq1 + (sl + 1).ToString().Trim() + ";" + kq2.Substring(kq2.IndexOf(';') + 1);
                        }
                        else
                        {
                            if (kq == "")
                            {
                                kq = ht + ":1;";
                            }
                            else
                            {
                                kq = kq.Trim() + ht + ":1;";
                            }

                        }
                    }                       
                    
                }//foreach
                if(solan>0){
                    kq1 = solan.ToString().Trim()+" (";
                    kq2 = "";
                    while (kq != "")
                    {
                        nht = kq.IndexOf(':');
                        ht = kq.Substring(0, nht); //hình thức
                        kq=kq.Substring(nht+1);
                        nht = kq.IndexOf(';');
                        kq2 =kq.Substring(0, nht); //số lượng
                        kq = kq.Substring(nht + 1);
                        solan=Int16.Parse(ht);
                        dm = db.danhmucs.Where(m => m.id == solan).FirstOrDefault();
                        if (kq1.EndsWith("("))
                        {
                            kq1 = kq1.Trim() + kq2 + " " + dm.TenDanhMuc;
                        }
                        else
                        {
                            kq1 = kq1.Trim() + "+" + kq2 + " " + dm.TenDanhMuc;
                        }                      
                        
                    }
                    kq1 = kq1.Trim() + ")";
                    oDoc.FormFields.get_Item("txtKT").Result = kq1;
                }
                else {
                    oDoc.FormFields.get_Item("txtKT").Result = "";
                }
                ///Phan ky luat
                ktkl = from kk in db.khenkluats
                       where (kk.kyluat == true) && (kk.ktkl_ngayqd <= dt1) && (kk.ktkl_ngayqd >= dt2) && (kk.id_ns == hs.id) && (kk.ktkl_hinhthuc != null)
                           select kk;
                solan = 0;
                foreach (khenkluat kl in ktkl)
                {
                    solan++;
                    nht = (int)kl.ktkl_hinhthuc;
                    if (nht != 3677 && nht != 3679 && nht != 3687 && nht != 4020)
                    {
                        ht = kl.ktkl_hinhthuc.ToString().Trim();
                        if (kq.Contains(ht))
                        {
                            kq1 = kq.Substring(0, kq.IndexOf(ht) + ht.Length + 1);
                            kq2 = kq.Substring(kq.IndexOf(ht) + ht.Length + 1);
                            sl = Int16.Parse(kq2.Substring(0, kq2.IndexOf(';')));
                            kq = kq1 + (sl + 1).ToString().Trim() + ";" + kq2.Substring(kq2.IndexOf(';') + 1);
                        }
                        else
                        {
                            if (kq == "")
                            {
                                kq = ht + ":1;";
                            }
                            else
                            {
                                kq = kq.Trim() + ht + ":1;";
                            }

                        }
                    }

                }//foreach
                if (solan > 0)
                {
                    kq1 = solan.ToString().Trim() + " (";
                    kq2 = "";
                    while (kq != "")
                    {
                        nht = kq.IndexOf(':');
                        ht = kq.Substring(0, nht); //hình thức
                        kq = kq.Substring(nht + 1);
                        nht = kq.IndexOf(';');
                        kq2 = kq.Substring(0, nht); //số lượng
                        kq = kq.Substring(nht + 1);
                        solan = Int16.Parse(ht);
                        dm = db.danhmucs.Where(m => m.id == solan).FirstOrDefault();
                        if (kq1.EndsWith("("))
                        {
                            kq1 = kq1.Trim() + kq2 + " " + dm.TenDanhMuc;
                        }
                        else
                        {
                            kq1 = kq1.Trim() + "+" + kq2 + " " + dm.TenDanhMuc;
                        }

                    }
                    kq1 = kq1.Trim() + ")";
                    oDoc.FormFields.get_Item("txtKL").Result = kq1;
                }
                else
                {
                    oDoc.FormFields.get_Item("txtKL").Result = "Không có";
                }
                //Chứng chỉ sức khỏe
                
 
                ccsk=dscc.Where(cc => cc.Code_tv == hs.mans.Trim() && cc.Expired != null).FirstOrDefault();
                if (ccsk != null)
                {
                    oDoc.FormFields.get_Item("txtLoaiSK").Result = "Nhóm 2";
                    oDoc.FormFields.get_Item("txtCCSKCap").Result = ccsk.Dotkham.ToString();
                    oDoc.FormFields.get_Item("txtCCSKHH").Result = ccsk.Expired.ToString();
                }
                //Hộ Chiếu
               var pp = (from v in tv.AsEnumerable()
                          where v.Field<string>("code_tv") == hs.mans.Trim()
                          select new
                          {
                              codetv = v.Field<string>("code_tv"),
                              sohc = v.Field<string>("pport_no")
                          }).FirstOrDefault();
                var hc = (from gt in gtb.AsEnumerable()
                      where (gt.Field<string>("loaigt") == "PAPT") && (gt.Field<string>("sogt") == pp.sohc) && (gt.Field<string>("code_tv") == hs.mans.Trim())
                      select new
                      {
                          sohc = gt.Field<string>("sogt"),
                          cap = gt.Field<string>("ngaycap"),
                          hethan = gt.Field<string>("ngayhh")
                      }).FirstOrDefault();
                
                oDoc.FormFields.get_Item("txtHochieu").Result = hc.sohc;
                oDoc.FormFields.get_Item("txtHCCap").Result = hc.cap;
                oDoc.FormFields.get_Item("txtHCHH").Result = hc.hethan;
                // Chứng chỉ An toàn bay
                
                var atb=(from rec in hldt.AsEnumerable()
                         where rec.Field<string>("objcode") == "REC" && rec.Field<string>("status") == "OK" && rec.Field<string>("Editstat") != "Delete" && rec.Field<string>("paxcode") == hs.mans.Trim() 
                             select rec).OrderByDescending(x=>x.Field<DateTime>("testdate")).First();

                oDoc.FormFields.get_Item("txtDtCap").Result = atb.Field<DateTime>("testdate").ToString("dd/MM/yyyy");
                oDoc.FormFields.get_Item("txtDtHH").Result = atb.Field<DateTime>("expiredate").ToString("dd/MM/yyyy");
                //Phần ghi hộ chiếu, giờ lên file gốc
                xlSrcSheet.Cells[i,20].value=hc.sohc;
                xlSrcSheet.Cells[i, 21].value = "'"+hc.cap;
                xlSrcSheet.Cells[i, 22].value = "'"+hc.hethan;
                try { 
                    var hccv = (from pcv in cctv0.AsEnumerable()
                            where pcv.Field<string>("manv") == hs.mans.Trim()
                            select new
                            {
                                socv=pcv.Field<string>("pportno"),
                                cvcap = pcv.Field<DateTime>("isspport"),
                                cvhh = pcv.Field<DateTime>("exppport")
                            }).FirstOrDefault();
                
                    xlSrcSheet.Cells[i, 23].value = hccv.socv;
                    xlSrcSheet.Cells[i, 24].value = "'" + hccv.cvcap.ToString("dd/MM/yyyy");
                    xlSrcSheet.Cells[i, 25].value = "'" + hccv.cvhh.ToString("dd/MM/yyyy");
                }
                catch(Exception ex){

                }
                var gbtv = (from pgb in gb.AsEnumerable()
                            where pgb.Field<string>("manv") == hs.mans.Trim()
                            select new
                            {
                                a321 = pgb.Field<decimal>("f_321") / 60,
                                a350 = pgb.Field<decimal>("f_350") / 60,
                                b787 = pgb.Field<decimal>("f_787") / 60,
                                tonggb = pgb.Field<decimal>("tong") / 60
                             

                            }).FirstOrDefault();
                xlSrcSheet.Cells[i, 26].value = gbtv.a321;
                xlSrcSheet.Cells[i, 27].value = gbtv.a350;
                xlSrcSheet.Cells[i, 28].value = gbtv.b787;
                xlSrcSheet.Cells[i, 29].value = gbtv.tonggb;
                xlSrcSheet.Cells[i, 19].value = hs.ngaysinh.ToString("dd/MM/yyyy");
                
                var toeic = (from av in diemtoeic
                             where av.manv==hs.mans.Trim()
                             select av
                             ).FirstOrDefault();

                xlSrcSheet.Cells[i, 30].value = toeic.diem;
#endregion 
                oApp.ActiveDocument.Save();
                oApp.ActiveDocument.Close();
                i++;
            }            
            //Dong fiel excel
            xlWorkbook.Save();
            xlWorkbook.Close();
            xlApp.Quit();
            oApp.Quit();
            xlApp = null;
            oApp = null;
            Console.ReadLine();
            db.Dispose();
            dbsms.Dispose();
            dbsk.Dispose();
            fs.Dispose();
            fs1.Dispose();
            fsgb.Dispose();
            fsgtb.Dispose();
            fstv.Dispose();
            GC.Collect();
            MessageBox.Show("Complete!");
        }

        private void kiểmTraManvtênFileWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Word.Application oApp;
            Word.Document oDoc;
            oApp = new Word.Application();
            oDoc = oApp.Documents.Open(@"F:\Downloads\DANH SÁCH TIẾP VIÊN VNA  ĐĐKSK NHÓM 2 – GĐSK ĐỊNH KỲ THÁNG 04-2018  TẠI TTYTHK – BB 140.docx");
            oApp.Visible = true;
            string _manv,hoten;
            int k = oDoc.Tables.Count;
            Word.Table tbl;// in oDoc.Tables
            using (HREntities db=new HREntities()){
                for (int j = 2; j <= k; j++)
                {
                    tbl = oDoc.Tables[j];
                    int cnt = tbl.Rows.Count;
                    for (int i = 1; i <= cnt; i++)
                    {
                        _manv = tbl.Cell(i, 6).Range.Text.Substring(0,4);
                        hoten = tbl.Cell(i, 2).Range.Text;
                        hoten = hoten.Substring(0,hoten.IndexOf("\r")).Trim();
                        var nv = (from ns in db.HoSoGocs
                                  where ns.mans.Trim() == _manv
                                  select new
                                  {
                                      manv = ns.mans.Trim(),
                                      hoten = ns.ns_ho.Trim() + " " + ns.ns_ten.Trim() + " " + ns.ns_stt.Trim()
                                  }).FirstOrDefault();
                        if (hoten.Trim() != nv.hoten.ToUpper())
                        {
                            tbl.Cell(i, 8).Range.Text = nv.hoten.ToUpper();
                        }
                        else
                        {
                            tbl.Cell(i, 8).Range.Text = "";
                        }

                    }
                }
            }
            oApp.Documents.Save();
            oDoc.Close(false);
            oApp.Quit(false);
            Marshal.ReleaseComObject(oApp);
            MessageBox.Show("Xong!");
        }

        private void convertHRTCTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (HREntities db = new HREntities())
            {
                Cursor.Current = Cursors.WaitCursor;
                var dshd = (from ld in db.Laodongs
                            group ld by ld.id_ns
                                into groupld
                                select new
                                {
                                    id_ns = groupld.Key,
                                    hd_ngayhieuluc = groupld.Max(x => x.hd_ngayhieuluc),
                                }).AsEnumerable();

                var dsld = (from ld in db.Laodongs
                            join hd in dshd
                            on new { ld.id_ns, ld.hd_ngayhieuluc } equals new { hd.id_ns, hd.hd_ngayhieuluc }
                            select new
                            {
                                id = ld.id,
                                id_ns = ld.id_ns,
                                hd_ngaykyhd = ld.hd_ngaykyhd,
                                hd_ngaychamdut = ld.hd_ngaychamdut,
                                hd_ngayhieuluc = ld.hd_ngayhieuluc,
                                hd_ngayhlchamduthd = ld.hd_ngayhlchamduthd
                            }).AsEnumerable();

                var dsnv = (from hs in db.HoSoGocs.Where(x => x.nghiviec == false)
                            join hd in dsld on hs.id equals hd.id_ns into fg
                            from fgi in fg.DefaultIfEmpty()
                            where (fgi.hd_ngaychamdut == null)
                            select new
                            {
                                manv = hs.mans,
                                ngaysinh = hs.ngaysinh,
                                hodem = hs.ns_ho,
                                ten = hs.ns_ten.Trim() + " " + hs.ns_stt.Trim(),
                                nghiviec = hs.nghiviec
                            }).ToList();

                Excel.Application oApp = new Excel.Application();
                Excel.Workbook owb = oApp.Workbooks.Add();
                Excel.Worksheet ost = owb.Sheets[1];
                ost.Cells[1, 1].Value = "STT";
                ost.Cells[1, 2].Value = "Manv";
                ost.Cells[1, 3].Value = "Họ đệm";
                ost.Cells[1, 4].Value = "Tên";
                ost.Cells[1, 5].Value = "Note Họ đệm";
                ost.Cells[1, 6].Value = "Note Tên";
                oApp.Visible = true;

                NotesSession session = new NotesSession();
                NotesDatabase dbase;
                NotesView view;
                NotesDocument doc;
                string msnv, hodem, ten;
                int i = 2;
                session.Initialize("btliem");
                dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
                view = dbase.GetView("Nhan su\\Theo ma so");
                foreach (var nv in dsnv)
                {
                    ost.Cells[i, 1].value = i - 1;
                    ost.Cells[i, 2].value = "'" + nv.manv.Substring(0, 4);
                    ost.Cells[i, 3].value = nv.hodem;
                    ost.Cells[i, 4].value = nv.ten;
                    ost.Cells[i, 8].value = "'" + nv.ngaysinh.ToString("dd/MM/yyyy");
                    ost.Cells[i, 9].value = nv.nghiviec;
                    //ost.Cells[i, 10].value = nv.hd_ngayhieuluc;
                    //ost.Cells[i, 11].value = nv.hd_ngaychamdut;
                    doc = view.GetDocumentByKey(nv.manv.Substring(0, 4));
                    if (doc != null)
                    {
                        msnv = doc.GetItemValue("MSNV")[0];
                        hodem = doc.GetItemValue("Hodem")[0];
                        ten = doc.GetItemValue("ten")[0];
                        //ngaysinh = doc.GetItemValue("Ngaysinh")[0];
                        hodem = Utils.Utils.TCVN3ToUnicode(hodem);
                        ten = Utils.Utils.TCVN3ToUnicode(ten);

                        ost.Cells[i, 5].value = hodem.ToLower();
                        ost.Cells[i, 6].value = ten.ToLower();


                        if (ten.Trim().ToUpper() != nv.ten.Trim().ToUpper())
                            ost.Cells[i, 7].value = "Sai tên!";
                    }

                    i++;
                }
                //Note có con kiến không có
                doc = view.GetFirstDocument();
                while (doc != null)
                {
                    msnv = doc.GetItemValue("MSNV")[0];
                    var nv = (from hs in dsnv
                              where hs.manv.Trim() == msnv
                              select hs).FirstOrDefault();
                    if (nv == null)
                    {
                        hodem = doc.GetItemValue("Hodem")[0];
                        ten = doc.GetItemValue("ten")[0];

                        hodem = Utils.Utils.TCVN3ToUnicode(hodem);
                        ten = Utils.Utils.TCVN3ToUnicode(ten);
                        ost.Cells[i, 1].value = i - 1;
                        ost.Cells[i, 2].value = "'" + msnv;
                        ost.Cells[i, 5].value = hodem.ToLower();
                        ost.Cells[i, 6].value = ten.ToLower();
                        i++;
                    }
                    doc = view.GetNextDocument(doc);
                }
                MessageBox.Show("Xong!");
                Cursor.Current = Cursors.Default;
            }
        }

        private void layDSDTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlSrcSheet;

            xlApp = new Excel.Application();
            xlWorkbook = xlApp.Workbooks.Add();
            xlApp.Visible = true;
            xlSrcSheet = xlWorkbook.Worksheets.get_Item(1);
            xlSrcSheet.Cells[1, 1].value = "So TT";
            xlSrcSheet.Cells[1, 2].value = "Manv";
            xlSrcSheet.Cells[1, 3].value = "Dien thoai";
            
        }

        private void layDSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] listfile = Directory.GetFiles(@"F:\Downloads\Phan_bo_LD_2018", "*.docx");
            Word.Application oApp = new Word.Application();
            Word.Document oDoc;
            Word.Table oTable;
            Excel.Application oExcel = new Excel.Application();
            Excel.Workbook owb = oExcel.Workbooks.Add();
            Excel.Worksheet ost = owb.Sheets[1];
            ost.Cells[1, 1].value = "STT";
            ost.Cells[1, 2].value = "Manv";
            ost.Cells[1, 3].value = "New Group";
            oApp.Visible = true;
            oExcel.Visible = true;
            int sodong,msnv, i,k=2;
            string _manv, grp;
            foreach (string filename in listfile)
            {
                oDoc = oApp.Documents.Open(filename);
                oTable = oDoc.Tables[1];
                sodong=oTable.Rows.Count;
                for (i = 1; i <= sodong; i++)
                {
                    _manv = oTable.Cell(i, 1).Range.Text;
                    if (_manv.Length < 4)
                        continue;
                    _manv = _manv.Substring(0, 4);
                    if (_manv == null)
                        continue;
                    int.TryParse(_manv,out msnv);
                    if (msnv==0)
                        continue;

                    grp = oTable.Cell(i, 6).Range.Text;
                    ost.Cells[k, 1].value = k-1;
                    ost.Cells[k, 2].value ="'"+_manv;
                    ost.Cells[k, 3].value = grp.Substring(0,grp.Length-1);
                    k++;
                }
                oDoc.Close();
            }
            MessageBox.Show("Complete");
        }

        private void layOldIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application oExcel = new Excel.Application();
            //Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\Downloads\Phan_bo_LD_2018\Tong hop.xlsx");
            Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\Downloads\Phan_bo_LD_2018\Danh sach NVMD _01.06.2018.xlsx");
            Excel.Worksheet ost = owb.Sheets[1];
            oExcel.Visible = true;
            string _manv;
            int newID;
            using (HREntities db = new HREntities())
            {
                for (int i = 2; i <=34; i++)
                {
                    _manv = ost.Cells[i, 2].value;
                    if (_manv == null)
                        continue;
                    newID = (int)ost.Cells[i, 8].value;
                    var ns=(from hs in db.HoSoGocs
                                where hs.mans ==_manv
                                select hs).FirstOrDefault();
                    if (ns != null)
                    {
                        ost.Cells[i, 9].value = ns.bophanlamviec;
                        ns.bophanlamviec = newID;
                    }
                    else
                        ost.Cells[i, 20].value = "Mất";
                }
                db.SaveChanges();
            }
            MessageBox.Show("Complete!");
        }

        private void updateNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;
            
            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");
            
            Excel.Application oExcel = new Excel.Application();
            //Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\Downloads\Phan_bo_LD_2018\Tong hop.xlsx");
            Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\Downloads\Phan_bo_LD_2018\Danh sach NVMD _01.06.2018.xlsx");
            Excel.Worksheet ost = owb.Sheets[1];
            oExcel.Visible = true;
            string _manv,NewBP;
            for (int i = 2; i <= 34; i++)//2950
            {
                _manv = ost.Cells[i, 2].value;
                if (_manv == null)
                    continue;
                NewBP = ost.Cells[i, 10].value;
                doc = view.GetDocumentByKey(_manv);
                if (doc != null)
                {
                    ost.Cells[i, 11].value = doc.GetItemValue("Bophan");
                    doc.ReplaceItemValue("Bophan", NewBP);
                    doc.Save(true,true);
                }
            }
            
            MessageBox.Show("Complete");
        }

        private void updateRedAntToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateCDCVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;

            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");

            Excel.Application oExcel = new Excel.Application();
            Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\Downloads\Phan_bo_LD_2018\Chuc danh.xls");
            Excel.Worksheet ost = owb.Sheets[1];
            oExcel.Visible = true;
            string _manv, NewBP;
            DateTime ngaycv;
            for (int i = 2; i <= 4897; i++)
            {
                NewBP=ost.Cells[i,4].value;
                if (NewBP != null)
                    continue;
                _manv = ost.Cells[i, 1].value;
                doc = view.GetDocumentByKey(_manv);
                if (doc == null)
                    continue;
                NewBP = doc.GetItemValue("ChucvuTV")[0];
                if (NewBP != null && NewBP!="")
                {
                    ngaycv = (DateTime)doc.GetItemValue("ngaycvtv")[0];
                    ost.Cells[i, 15].value = NewBP;
                    ost.Cells[i, 16].value = ngaycv;
                }
            }
            MessageBox.Show("Complete");
        }

       

        private void hSNVToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Excel.Application oExcel = new Excel.Application();
            //Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\HR - TCT\TVC_VNA_Template chuyen doi DL -21 05 2018\TVC_VNA_Template chuyen doi DL -21 05 2018.xls");
            Excel.Workbook owb = oExcel.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet ost = owb.Sheets["HSNV"];
            Excel.Worksheet stkn = owb.Sheets["QT Kiem nhiem"];
            oExcel.Visible = true;

            var dtn = new ExcelQueryFactory(@"F:\HR - TCT\DANH SACH DOAN VIEN.xlsx");
            var dsdtn = dtn.Worksheet("Danh sach DVTN 2018").ToList();
            var axmanv = new ExcelQueryFactory(@"F:\HR - TCT\Ma ID TCT\DTV-ID.xlsx");
            var matct=axmanv.Worksheet("Sheet1").ToList();

            FileStream fs = new FileStream(@"F:\Cac van de ve Mail TCT\dbmail.dbf",FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            DataTable mail = Table.Open(fs).AsDataTable();
            FileStream fstv = new FileStream(@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\dm_tvien.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable tv = Table.Open(fstv).AsDataTable();
            FileStream fsgtb = new FileStream(@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\prgkb\giaytobay.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable gtb = Table.Open(fsgtb).AsDataTable();

            SataHRMEntities smsdb = new SataHRMEntities();
            smsAddressBook contact;
            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;

            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");
            DateTime maxcd = new DateTime(1900, 1, 1);
            DateTime maxcv = new DateTime(1900, 1, 1);
            int doiso,i = 10,ikn=2;
            string tentinh,hoten,bpnote,tennote,tenso,chuoi="",chuoi1="",tmp="",list_tv_cb;
            bool flagkn;
            list_tv_cb = "0044*0598*0143*0762*0730*0149*0104*1407*0129*0686*0156*0272*0823*0248*0487*0157*0090";
            using (HREntities db = new HREntities())
            {
                
                foreach (var hs in db.HoSoGocs)
                {
                    if (hs.nghiviec == true)
                        continue;
                    //if (hs.mans.Trim() != "0060")
                    //    continue;
                    var ld = (from ttld in db.Laodongs
                              where ttld.id_ns == hs.id
                              select ttld).FirstOrDefault();
                    if (ld==null || ld.hd_ngayhlchamduthd == null)
                    {
                        doc = view.GetDocumentByKey(hs.mans.Trim());
                        if (doc == null)
                            continue;
                        if (list_tv_cb.IndexOf(hs.mans.Trim()) > 0)
                            flagkn = true;
                        else
                            flagkn = false;
                        #region Họ Tên
                        tennote = doc.GetItemValue("Ten")[0];
                        //Kiểm tra số trùng tên giữa Note và RedAnt
                        if (tennote.Trim().LastIndexOf(" ") != -1)
                        {
                            tenso = tennote.Substring(tennote.Trim().LastIndexOf(" ")+1);
                            //Có phải là số trùng tên
                            if (int.TryParse(tenso, out doiso))
                            {
                                if(tenso.Trim()==hs.ns_stt.Trim())
                                    hoten= hs.ns_ho.Trim() + " " + hs.ns_ten.Trim() + " " + hs.ns_stt;
                                else //RedAnt bị mất số
                                {
                                    hoten = hs.ns_ho.Trim() + " " + hs.ns_ten.Trim() + " " + tenso;
                                    //hs.ns_stt = tenso;
                                }
                            }
                            else
                                hoten = hs.ns_ho.Trim() + " " + hs.ns_ten.Trim() + " " + hs.ns_stt;
                        }
                        else
                        {
                            hoten= hs.ns_ho.Trim() + " " + hs.ns_ten.Trim() + " " + hs.ns_stt;
                        }
                        #endregion 
                        ost.Rows[i].insert();
                        ost.Range[ost.Cells[i, 1], ost.Cells[i, 111]].font.color = 0;
                        ost.Cells[i, 1].value = i - 9;
                        ost.Cells[i, 4].value = "'"+hs.mans.Trim();
                        ost.Cells[i, 5].value = hoten;
                        if (flagkn)
                        {
                            ikn++;
                            stkn.Cells[ikn, 1].value = ikn - 2;
                            stkn.Cells[ikn, 3].value = hoten;
                            stkn.Cells[ikn, 5].value = "Đoàn Tiếp viên";
                            stkn.Cells[ikn, 8].value = "Đoàn Tiếp viên";
                            stkn.Cells[ikn, 10].value = "Tiếp viên trưởng bậc 2";
                        }
                        //Mã TCT
                        var idtct = (from x in matct
                                     where x["ID cũ do đơn vị cấp"] == hs.mans.Trim()
                                     select x).FirstOrDefault();
                        
                        if (idtct != null)
                        {
                            ost.Cells[i, 3].value = idtct["ID-TCT"];
                            if(flagkn)
                                stkn.Cells[ikn, 2].value = idtct["ID-TCT"];
                        }   
                        //Giới tính
                        if (hs.gioitinh == 2536)
                            ost.Cells[i, 8].value = "Nam";
                        if (hs.gioitinh == 2537)
                            ost.Cells[i, 8].value = "Nữ";
                        //Ngày sinh
                        if (hs.ngaysinh != null)
                        {
                            ost.Cells[i, 9].value = "'" + hs.ngaysinh.ToString("dd/MM/yyyy");
                            if(flagkn)
                                stkn.Cells[ikn, 4].value = "'" + hs.ngaysinh.ToString("dd/MM/yyyy");
                        }
                            
                        //Đơn vị cấp 1
                        ost.Cells[i, 10].value = "Đoàn Tiếp viên";
                        #region Chức danh
                        var dscv = db.chucvus.Where(y => y.id_ns == hs.id);
                        if(dscv.Any())
                            maxcv=dscv.Max(x=>x.chucvu_ngay);    
                        
                        //var maxcv = db.chucvus.Where(y => y.id_ns == hs.id).Max(x => x.chucvu_ngay);
                        var chucvu=(from cv in db.chucvus
                                   where cv.id_ns==hs.id && cv.chucvu_ngay==maxcv
                                   select cv.chucvu1).FirstOrDefault();

                        //var maxcd = db.chucdanhs.Where(y => y.id_ns == hs.id).Max(k => k.chucdanh_ngay)??new DateTime(1900,1,1);
                        var dscd = db.chucdanhs.Where(y => y.id_ns == hs.id);
                        if (dscd.Any())
                            maxcd = dscd.Max(y => y.chucdanh_ngay);
                        var chucdanh = (from cd in db.chucdanhs
                                        where cd.id_ns == hs.id && cd.chucdanh_ngay==maxcd   
                                        select cd.chucdanh1).FirstOrDefault();

                        
                        switch (chucvu)
                        {
                            case 2663:  //Nhân viên
                                switch (chucdanh)
                                {
                                    case 390:
                                        chuoi = "Chuyên viên";
                                        chuoi1="";
                                        break;
                                    case 465:
                                        chuoi = "Cán sự";
                                        chuoi1="";
                                        break;
                                    case 469:
                                        chuoi = "Nhân viên";
                                        chuoi1="Nhân viên Lái xe";
                                        break;
                                    case 472:
                                        chuoi = "Nhân viên";
                                        chuoi1="Nhân viên Lễ tân";
                                        break;
                                    case 474: //Nhân viên nghiệp vụ 1
                                        chuoi = "Nhân viên";
                                        chuoi1="";
                                        break;
                                    case 478:
                                        chuoi = "Tiếp viên hạng Y";
                                        chuoi1="Tiếp viên phục vụ hạng phổ thông";
                                        break;
                                    case 480://Chuyên viên trực OCC
                                        chuoi = "Chuyên viên";
                                        chuoi1="";
                                        break;
                                    case 483://TV VASCO
                                        chuoi = "Tiếp viên VASCO";
                                        ost.Cells[i, 14].font.color = 255;
                                        chuoi1="";
                                        break;
                                    case 3658://TVT Bậc 2
                                        chuoi = "Tiếp viên trưởng";
                                        chuoi1 = "Tiếp viên trưởng bậc 2";
                                        ost.Cells[i, 15].font.color = 255;
                                        break;
                                    case 3659:
                                        chuoi = "Tiếp viên hạng C";
                                        chuoi1="Tiếp viên phục vụ hạng thương gia";
                                        break;
                                    case 3660://TV Phó
                                        chuoi = "Tiếp viên phó";
                                        chuoi1 = "Tiếp viên phó";
                                        ost.Cells[i, 14].font.color = 255;
                                        ost.Cells[i, 15].font.color = 255;
                                        break;
                                    case 3661://TVP-TVT1
                                        chuoi = "Tiếp viên phó";
                                        chuoi1 = "Tiếp viên phó";
                                        ost.Cells[i, 14].font.color = 255;
                                        ost.Cells[i, 15].font.color = 255;
                                        break;
                                    case 3663://TVT Bậc 1
                                        chuoi = "Tiếp viên trưởng";
                                        chuoi1 = "Tiếp viên trưởng bậc 1";
                                        ost.Cells[i, 15].font.color = 255;
                                        break;
                                    case 3664://TVT ATR
                                        chuoi = "Tiếp viên trưởng";
                                        chuoi1 = "Tiếp viên trưởng ATR";
                                        ost.Cells[i, 15].font.color = 255;
                                        break;
                                    case 4016:
                                        chuoi = "Tiếp viên hạng Y";
                                        chuoi1="Tiếp viên phục vụ hạng phổ thông";
                                        break;
                                    case 4022://TVP-Hạng C
                                        chuoi = "Tiếp viên phó";
                                        chuoi1 = "Tiếp viên phó";
                                        ost.Cells[i, 14].font.color = 255;
                                        ost.Cells[i, 15].font.color = 255;
                                        break;
                                    case 4668:
                                        chuoi = "Nhân viên";
                                        chuoi1="Nhân viên Bảo trì điện, nước";
                                        break;
                                    case 4669://NV nghiệp vụ 2
                                        chuoi = "Nhân viên";
                                        chuoi1="";
                                        break;
                                    case 0: //MĐ thuê ngoài
                                        tmp= doc.GetItemValue("loaidoitac")[0];
                                        if(tmp.Trim()=="2a") //Mặt đất
                                        {
                                            chuoi = "Nhân viên";
                                            chuoi1="";
                                        }
                                        else
                                        {
                                            if (tmp.Trim() == "5")
                                            {
                                                chuoi = "Học viên";
                                                chuoi1 = "Học viên";
                                                ost.Cells[i, 14].font.color = 255;
                                                ost.Cells[i, 15].font.color = 255;
                                            }
                                            else
                                            {
                                                if (tmp.Trim() == "7") //TV thời vụ
                                                {
                                                    chuoi = "Tiếp viên hạng Y";
                                                    chuoi1 = "Tiếp viên phục vụ hạng phổ thông";
                                                }
                                                else
                                                {
                                                    if(tmp.Trim()=="3c"){
                                                        chuoi = "Tiếp viên VASCO";
                                                        chuoi1 = "";
                                                        ost.Cells[i, 14].font.color = 255;
                                                    }
                                                    else 
                                                    {
                                                        chuoi = "2663";
                                                        chuoi1 = chucdanh.ToString();
                                                    }
                                                }
                                            }
                                        }                                        
                                        break;
                                    default:
                                        chuoi = chucvu.ToString();
                                        chuoi1 = chucdanh.ToString();                                        
                                        break;
                                }
                                break;
                            case 2656://Đoàn phó
                                chuoi = "Đoàn phó";
                                chuoi1 = "Đoàn phó";
                                ost.Cells[i,14].font.color=255;
                                ost.Cells[i, 15].font.color = 255;
                                break;
                            case 2657://Đoàn trưởng
                                chuoi = "Đoàn trưởng";
                                chuoi1 = "Đoàn trưởng";
                                ost.Cells[i, 14].font.color = 255;
                                ost.Cells[i, 15].font.color = 255;
                                break;
                            case 2658://Đội phó
                                chuoi = "Đội phó";
                                chuoi1 = "";
                                break;
                            case 2659://Đội trưởng
                                chuoi = "Đội trưởng";
                                chuoi1 = "";
                                break;
                            case 2661://Liên đội phó
                                chuoi = "Phó phòng";
                                chuoi1 = "Liên đội phó";
                                break;
                            case 2662://Liên đội trưởng
                                chuoi = "Trưởng phòng";
                                chuoi1 = "Liên đội trưởng";
                                break;
                            case 2664://Nhóm trưởng
                                if (chucdanh == 3658)
                                {
                                    chuoi = "Tiếp viên trưởng";
                                    chuoi1 = "Tiếp viên trưởng bậc 2";
                                    ost.Cells[i, 15].font.color = 255;
                                }
                                else 
                                {
                                    chuoi = "Nhóm trưởng";
                                    chuoi1 = "Nhóm trưởng";                                   
                                }

                                break;
                            case 2665://Phó Chánh Văn phòng ĐT
                                chuoi = "Phó phòng";
                                chuoi1 = "Phó Chánh Văn phòng";
                                ost.Cells[i, 15].font.color = 255;
                                break;
                            case 2666://Phó phòng
                                chuoi = "Phó phòng";
                                chuoi1 = "";
                                break;
                            case 2667://Tổ phó
                                chuoi = "Nhân viên";
                                chuoi1 = "Nhân viên Lái xe";
                                break;
                            case 2668://Tổ trưởng
                                if (chucdanh == 390)
                                {
                                    chuoi = "Chuyên viên";
                                    chuoi1 = "";
                                }
                                else
                                    if (chucdanh == 465)
                                    {
                                        chuoi = "Cán sự";
                                        chuoi1 = "";
                                    }
                                else
                                        if (chucdanh == 474)
                                        {
                                            chuoi = "Nhân viên";
                                            chuoi1 = "";
;                                        }
                                        else
                                        {
                                            chuoi = "";
                                            chuoi1 = "";
                                        }                                
                                break;
                            case 2669://Trưởng phòng
                                chuoi = "Trưởng phòng";
                                chuoi1 = "";
                                break;
                            case 4650://Trạm trưởng trạm y tế
                                chuoi = "Phó phòng";
                                chuoi1 = "Trạm trưởng Trạm Y tế";
                                break;
                            case 4651://Trạm phó trạm y tế
                                chuoi = "Đội trưởng";
                                chuoi1 = "Trạm phó trạm y tế";
                                ost.Cells[i, 14].font.color = 255;
                                ost.Cells[i, 15].font.color = 255;
                                break;
                            case 0:
                                tmp= doc.GetItemValue("loaidoitac")[0];
                                if(tmp.Trim()=="2a") //Mặt đất
                                {
                                    chuoi = "Nhân viên";
                                    chuoi1="";
                                }
                                else
                                {
                                    if (tmp.Trim() == "5")
                                    {
                                        chuoi = "Học viên";
                                        chuoi1 = "Học viên";
                                        ost.Cells[i, 14].font.color = 255;
                                        ost.Cells[i, 15].font.color = 255;
                                    }
                                    else
                                    {
                                        if (tmp.Trim() == "7") //TV thời vụ
                                        {
                                            chuoi = "Tiếp viên hạng Y";
                                            chuoi1 = "Tiếp viên phục vụ hạng phổ thông";
                                        }
                                        else
                                            if (tmp.Trim() == "3c")
                                            {
                                                chuoi = "Tiếp viên VASCO";
                                                chuoi1 = "";
                                                ost.Cells[i, 14].font.color = 255;
                                            }
                                            else 
                                            {
                                                chuoi = chucvu.ToString();
                                                chuoi1 = chucdanh.ToString();
                                            }
                                    }
                                }
                                break;
                            default:
                                chuoi=chucvu.ToString();
                                chuoi1=chucdanh.ToString();
                                break;
                        }
                       ost.Cells[i, 14].value = chuoi;
                       ost.Cells[i, 15].value = chuoi1;
                       if(flagkn)
                           stkn.Cells[ikn,7].value=chuoi1;
                        #endregion
                        //Biên chế
                        if (hs.bienche_ngay!=null)
                            ost.Cells[i, 19].value = "'"+hs.bienche_ngay.ToString().Substring(0,10);
                        //Ngày vào ngành
                        if(hs.bienche_tct!=null)
                            ost.Cells[i, 20].value = "'" + hs.bienche_tct.ToString().Substring(0, 10);
                        //Nơi sinh
                        tentinh = Utils.Utils.tinhthanh(hs.noisinh_tinhtp);
                        if (tentinh.Substring(0, 1) == "1")
                        {
                            tentinh = tentinh.Substring(1);
                            ost.Cells[i, 21].font.color = 255;
                        }                            
                        ost.Cells[i, 21].value = tentinh;
                        #region Quốc tịch
                        //Quốc tịch 
                        switch (hs.quoctich)
                        {
                            case 38:
                                ost.Cells[i, 22].value = "Việt Nam";
                                break;
                            case 576:
                                ost.Cells[i, 22].value = "Nhật Bản";
                                break;
                            case 577:
                                ost.Cells[i, 22].value = "Hàn Quốc";
                                break;
                            case 375:
                                ost.Cells[i, 22].value = "";
                                break;
                            default:
                                ost.Cells[i, 22].value = hs.quoctich;
                                break;
                        }
                        #endregion
                        //Số CMND
                        ost.Cells[i, 24].value = "'" + hs.cmnd_so.Trim();
                        ost.Cells[i, 25].value = "'" + hs.cmnd_ngaycap.ToString("dd/MM/yyyy");
                        tentinh = Utils.Utils.tinhthanh(hs.cmnd_noicap_tinhtp);
                        if (tentinh.Substring(0, 1) == "1")
                        {
                            tentinh = tentinh.Substring(1);
                            ost.Cells[i, 26].font.color = 255;
                        }
                        ost.Cells[i, 26].value = tentinh;
                        #region Hộ chiếu
                        var sohc = (from ptv in tv.AsEnumerable()
                                    where ptv.Field<string>("code_tv") == hs.mans.Trim()
                                    select ptv.Field<string>("pport_no")).FirstOrDefault();
                        if (sohc != null)
                        {
                            ost.Cells[i, 27].value = sohc;
                            var hethan = (from pgtb in gtb.AsEnumerable()
                                          where pgtb.Field<string>("code_tv") == hs.mans.Trim() && pgtb.Field<string>("sogt") == sohc.Trim()
                                          select pgtb.Field<string>("ngayhh")).FirstOrDefault();
                            if (hethan != null)
                            {
                                ost.Cells[i, 28].value = "'" + hethan;
                            }
                        }
                        #endregion

                        #region Dân tộc
                        //Dân tộc
                        switch (hs.dantoc)
                        {
                            case 136:
                                chuoi = "Kinh";
                                break;
                            case 137:
                                chuoi = "Mường";
                                break;
                            case 138:
                                chuoi = "Hoa";
                                break;
                            case 139:
                                chuoi = "Tày";
                                break;
                            case 3159:
                                chuoi = "Thái Trắng";
                                break;
                            case 3160:
                                chuoi = "Nùng";
                                break;
                            case 3161:
                                chuoi = "Lào";
                                break;
                            default:
                                ost.Cells[i, 29].font.color = 255;
                                var dt = db.danhmucs.Where(x => x.id == hs.dantoc).FirstOrDefault();
                                chuoi = dt.TenDanhMuc;
                                break;                            
                        }
                        ost.Cells[i, 29].value = chuoi;
                        #endregion

                        #region Tôn giao
                        //Tôn giáo
                        switch (hs.tongiao)
                        {
                            case 350:
                                chuoi = "Phật giáo";
                                break;
                            case 351:
                                chuoi = "Thiên chúa giáo";
                                break;
                            case 352:
                                chuoi = "Thiên chúa giáo";
                                break;                            
                            case 3167:
                                chuoi = "Cao đài";
                                break;                            
                            case 3170:
                            case 431:
                                chuoi = "Không";
                                break;
                            default:
                                var tg = db.danhmucs.Where(x => x.id == hs.tongiao).FirstOrDefault();
                                chuoi=tg.TenDanhMuc;
                                ost.Cells[i, 30].font.color = 255;
                                break;
                        }
                        ost.Cells[i, 30].value = chuoi;
#endregion

                        //Nguyên quán
                        tentinh = Utils.Utils.tinhthanh(hs.quequan_tinhtp);
                        if (tentinh.Substring(0, 1) == "1")
                        {
                            tentinh = tentinh.Substring(1);
                            ost.Cells[i, 31].font.color = 255;
                        }                            
                        ost.Cells[i, 31].value = tentinh;
                        //Thường trú
                        tentinh=Utils.Utils.tinhthanh(hs.ttru_tinhtp);
                        if (tentinh.Substring(0, 1) == "1")
                        {
                            tentinh = tentinh.Substring(1);
                            ost.Cells[i, 33].font.color = 255;
                        }
                            
                        ost.Cells[i, 32].value =hs.ttru_dc;
                        ost.Cells[i, 33].value =tentinh;
                        ost.Cells[i, 34].value ="";
                        ost.Cells[i, 35].value = "";
                        //Nơi ở
                        tentinh = Utils.Utils.tinhthanh(hs.noio_tinhtp);
                        if (tentinh.Substring(0, 1) == "1")
                        {
                            tentinh = tentinh.Substring(1);
                            ost.Cells[i, 37].font.color = 255;
                        }
                            
                        ost.Cells[i, 36].value = hs.noio_dc;
                        ost.Cells[i, 37].value = tentinh;
                        ost.Cells[i, 38].value = "";
                        ost.Cells[i, 39].value = "";
                        #region Trình độ
                        //Trình độ
                        switch (hs.hocvantd)
                        {
                            case 611:
                                chuoi = "Tiến sỹ";
                                break;
                            case 612:
                                chuoi = "Thạc sỹ";
                                break;
                            case 613:
                                chuoi = "Đại học";
                                break;
                            case 614:
                                chuoi = "Cao đẳng";
                                break;
                            case 615:
                                chuoi = "Trung cấp";
                                break;
                            case 616:
                            case 3330:
                                chuoi = "THPT";
                                break;
                            case 376:
                                chuoi = "Khác";
                                break;
                            default:
                                chuoi = hs.hocvantd.ToString();
                                break;
                        }
                        ost.Cells[i, 40].value = chuoi;
                        #endregion
                        #region Ngoại ngữ
                        //Ngoại ngữ
                        var ccnn = from nn in db.ngoaingus
                                   where nn.id_ns == hs.id && nn.ngoaingu_loai==565
                                   select nn;
                        if (ccnn.Any()) //Có Tiếng Anh
                        {
                            var ngaymoi = ccnn.Max(x => x.ngoaingu_ngaycap);
                            var ccmoi = (from cc in ccnn
                                         where cc.id_ns == hs.id && cc.ngoaingu_ngaycap == ngaymoi
                                         select cc).FirstOrDefault();
                            if (ccmoi != null)
                            {
                                ost.Cells[i, 44].value = "Tiếng Anh";
                                if(ccmoi.ngoaingu_diemtong!=null)
                                    ost.Cells[i, 46].value = ccmoi.ngoaingu_diemtong;
                                var tencc = db.danhmucs.Where(x => x.id == ccmoi.ngoaingu_bangcap).FirstOrDefault();
                                if(tencc!=null)
                                    ost.Cells[i, 45].value = tencc.TenDanhMuc;
                                
                            }
                        }
                        else
                        {
                            var nnkhac = (from nn in db.ngoaingus
                                       where nn.id_ns == hs.id && nn.ngoaingu_loai != 565
                                       select nn);
                            if (nnkhac.Any())
                            {
                                var nnk = (from nn1 in nnkhac
                                           select nn1).FirstOrDefault();
                                ost.Cells[i, 44].value = nnk.ngoaingu_loai;
                            }
                        }
                        #endregion

                        #region Tình trạng hôn nhân
                        //Tình trạng hôn nhân
                        switch (hs.tthonnhan)
                        {
                            case 587:
                                chuoi = "";
                                break;
                            case 588:
                                chuoi = "Độc thân";
                                break;
                            case 589:
                                chuoi = "Đã kết hôn";
                                break;
                            case 590:
                                chuoi = "Đã ly dị";
                                break;
                            default:
                                chuoi = hs.tthonnhan.ToString();
                                break;
                        }
                        ost.Cells[i, 48].value = chuoi;
#endregion
                        //E-mail
                        var dcmail = (from email in mail.AsEnumerable()
                                      where email.Field<string>("manv") == hs.mans.Trim()
                                      select email).FirstOrDefault();
                        if (dcmail!=null)
                            ost.Cells[i, 49].value = dcmail.Field<string>("mail").Trim();
                        //Số điện thoại
                        contact = (from sms in smsdb.smsAddressBooks
                                   where sms.ContactCode == hs.mans.Trim()
                                   select sms).FirstOrDefault();
                        if(contact!=null)
                            ost.Cells[i, 50].value = "'"+contact.MobilePhone.Trim();
                        //Mã số thuế
                        ost.Cells[i, 51].value = "'"+hs.masothue;
                        //Tài khoản ngân hàng
                        if(hs.taikhoan!=null)
                            ost.Cells[i, 53].value = "'"+hs.taikhoan;
                        if (hs.nganhang != null)
                        {
                            if(hs.nganhang==609)
                                ost.Cells[i, 54].value = "Ngân hàng Vietcombank";
                            else 
                                if (hs.nganhang==610)
                                    ost.Cells[i, 54].value = "Ngân hàng Techcombank";
                                else 
                                    ost.Cells[i, 54].value = "";
                        }
                        
                        //Chiều cao, cân nặng, nhóm máu
                        if(hs.chieucao>0)
                            ost.Cells[i, 59].value = hs.chieucao;
                        if(hs.cannang>0)
                            ost.Cells[i, 60].value = hs.cannang;
                        if (hs.nhommau != 424)
                        {
                            var mau = db.danhmucs.Where(x => x.id == hs.nhommau).FirstOrDefault();
                            if(mau!=null)
                                ost.Cells[i, 63].value = mau.TenDanhMuc;
                        }
                        #region Gia đình chính sách
                        //Gia đình chính sách
                        if (hs.tpgiadinh > 0)
                        {
                            tmp = "";
                            switch (hs.tpgiadinh)
                            {
                                case 3365:
                                    tmp = "Liệt sĩ";
                                    break;
                                case 3378:
                                case 3385:
                                    tmp = "Thương binh liệt sĩ";
                                    break;
                                case 3384:
                                    tmp = "Thương binh";
                                    break;
                            }
                                ost.Cells[i, 93].value =tmp;
                        }
                        #endregion
                        //Trình độ lý luận chính trị
                        if(hs.llchinhtri>0)
                        {
                            tmp = "";
                            switch (hs.llchinhtri)
                            {
                                case 3519:
                                    tmp = "Cao cấp";
                                    break;
                                case 3520:
                                    tmp = "Trung cấp";
                                    break;
                                case 3521:
                                    tmp = "Sơ cấp";
                                    break;
                            }
                            ost.Cells[i, 95].value = tmp;
                        }
                        
                        //Trình độ quản lý hành chính nhà nước
                        var ql = db.quanlyhcnns.Where(x => x.id_ns == hs.id).FirstOrDefault();
                        tmp = "";
                        if(ql!=null)
                            switch (ql.qlhcnn)
                            {
                                case 637:
                                    tmp = "Cao cấp";
                                    break;
                                case 638:
                                    tmp = "Trung cấp";
                                    break;
                                case 639:
                                    tmp = "Sơ cấp";
                                    break;
                                case 3523:
                                    tmp = "Chuyên viên";
                                    break;
                                case 3524:
                                    tmp = "Chuyên viên chính";
                                    break;
                                case 3525:
                                    tmp = "Chuyên viên cao cấp";
                                    break;
                            }
                        ost.Cells[i, 96].value =tmp;
                        
                        //Đoàn viên
                        var dv = (from a in dsdtn
                                  where a["MNV"] == hs.mans.Trim()
                                  select a).FirstOrDefault();
                        
                        if (dv!=null)
                        {
                            ost.Cells[i, 99].value = "Có";
                            ost.Cells[i, 100].value="";
                            ost.Cells[i, 102].value =dv["CHỨC VỤ ĐOÀN"];
                        }                        
                        
                        //Đảng viên
                        if (hs.dangvien == true)
                        {
                            ost.Cells[i, 103].value = "Có";
                            ost.Cells[i, 104].value = "";
                            if (hs.dang_ngaykn != null) 
                                ost.Cells[i, 105].value = "'"+hs.dang_ngaykn.ToString().Substring(0,10);
                            if(hs.dang_ngaychuyen!=null)
                                ost.Cells[i, 108].value = "'" + hs.dang_ngaychuyen.ToString().Substring(0, 10); ;
                            var cvdang = db.danhmucs.Where(x => x.id == hs.dang_chucvu).FirstOrDefault();
                            if(cvdang!=null)
                                ost.Cells[i, 113].value = cvdang.TenDanhMuc;
                        }
                        
                        #region Bộ phận
                        //Bộ phận
                        var dm=(from ttdm in db.danhmucs
                               where ttdm.id==hs.bophanlamviec
                               select ttdm).FirstOrDefault();

                        switch (hs.bophanlamviec)
                        {
                            case 4679:
                            case 4680:
                            case 4681:
                            case 4682:
                            case 4683:
                            case 4684:
                            case 4685:
                            case 4686:
                            case 4687:
                            case 4688:
                            case 4689:
                            case 4690:
                            case 4691:
                            case 4692:
                            case 4693:
                            case 4694:
                            case 4695:
                            case 4696:
                            case 4697:
                            //case 4698:
                            //case 2637:
                                ost.Cells[i, 11].value = "Liên đội TV1";
                                if (dm != null)
                                {
                                    if(int.TryParse(dm.TenDanhMuc.Substring(4),out doiso))
                                    {
                                        ost.Cells[i, 13].value = "'" + dm.TenDanhMuc.Substring(4);
                                        ost.Cells[i, 12].value = "SGN";
                                    }
                                    else 
                                        ost.Cells[i, 12].value = "'" + dm.TenDanhMuc.Substring(4);//DAD+CXR+JP+KR
                                        
                                }
                                break; //LĐTV1
                            case 3484: //Mợ Hân
                                {
                                    ost.Cells[i, 11].value = "Liên đội TV1";
                                    ost.Cells[i, 12].value = "SGN";
                                }
                                break;
                            case 4699:
                            case 4700:
                            case 4701:
                            case 4702:
                            case 4703:
                            case 4704:
                            case 4705:
                            case 4706:
                            case 4707:
                            case 4708:
                            case 4709:
                            case 4710:
                            case 4711:
                            case 4712:
                            case 4713:
                            case 4714:
                            case 4715:
                            case 4716:
                            case 4717:
                            case 4718:
                            //case 2631:
                                ost.Cells[i, 11].value = "Liên đội TV2";
                                ost.Cells[i, 12].value = "SGN";
                                if (dm != null)
                                {
                                    if (int.TryParse(dm.TenDanhMuc.Substring(4), out doiso))
                                    {
                                        ost.Cells[i, 13].value = "'" + dm.TenDanhMuc.Substring(4);
                                        ost.Cells[i, 12].value = "SGN";
                                    }
                                    else
                                        ost.Cells[i, 12].value = "'" + dm.TenDanhMuc.Substring(4);//DAD+CXR
                                        
                                }
                                break; //LĐTV2
                            case 4719:
                            case 4720:
                            case 4721:
                            case 4722:
                            case 4723:
                            case 4724:
                            case 4725:
                            case 4726:
                            case 4727:
                            case 4728:
                            case 4729:
                            case 4730:
                            case 4731:
                            case 4732:
                            case 4733:
                            case 4734:
                            case 4735:
                            case 4736:
                            case 4737:
                            case 4738:
                            //case 2647: 
                                ost.Cells[i, 11].value = "Liên đội TV3";
                                ost.Cells[i, 12].value = "SGN";
                                if (dm != null)
                                {
                                    if (int.TryParse(dm.TenDanhMuc.Substring(4), out doiso))
                                    {
                                        ost.Cells[i, 13].value = "'" + dm.TenDanhMuc.Substring(4);
                                        ost.Cells[i, 12].value = "SGN";
                                    }
                                    else
                                        ost.Cells[i, 12].value = "'" + dm.TenDanhMuc.Substring(4);//DAD+CXR
                                        
                                }
                                break; //LĐTV3
                            case 4739:
                            case 4740:
                            case 4741:
                            case 4742:
                            case 4743:
                            case 4744:
                            case 4745:
                            case 4746:
                            case 4747:
                            case 4748:
                            case 4749:
                            case 4750:
                            case 4751:
                            case 4752:
                            case 4753:
                            case 4754:
                            case 4755:
                            case 4756:
                            case 4757:
                            case 4758:
                                ost.Cells[i, 11].value = "Liên đội TV4";
                                ost.Cells[i, 12].value = "HAN";
                                if (dm != null)
                                {
                                    if (int.TryParse(dm.TenDanhMuc.Substring(4), out doiso))
                                    {
                                        ost.Cells[i, 13].value = "'" + dm.TenDanhMuc.Substring(4);
                                        ost.Cells[i, 12].value = "HAN";
                                    }
                                    else
                                        ost.Cells[i, 12].value = "'" + dm.TenDanhMuc.Substring(4);//DAD+CXR
                                        
                                }
                                break; //LĐTV4
                            case 4759:
                            case 4760:
                            case 4761:
                            case 4762:
                            case 4763:
                            case 4764:
                            case 4765:
                            case 4766:
                            case 4767:
                            case 4768:
                            case 4769:
                            case 4770:
                            case 4771:
                            case 4772:
                            case 4773:
                            case 4774:
                            case 4775:
                            case 4776:
                            case 4777:
                            case 4778:
                            case 4799:
                                ost.Cells[i, 11].value = "Liên đội TV5";
                                ost.Cells[i, 12].value = "HAN";
                                if (dm != null)
                                {
                                    if (int.TryParse(dm.TenDanhMuc.Substring(4), out doiso))
                                    {
                                        ost.Cells[i, 13].value = "'" + dm.TenDanhMuc.Substring(4);
                                        ost.Cells[i, 12].value = "HAN";
                                    }
                                    else
                                        ost.Cells[i, 12].value = "'" + dm.TenDanhMuc.Substring(4);//DAD+CXR
                                        
                                }
                                break; //LĐTV5
                            case 4779:
                            case 4780:
                            case 4781:
                            case 4782:
                            case 4783:
                            case 4784:
                            case 4785:
                            case 4786:
                            case 4787:
                            case 4788:
                            case 4789:
                            case 4790:
                            case 4791:
                            case 4792:
                            case 4793:
                            case 4794:
                            case 4795:
                            case 4796:
                            case 4797:
                            case 4798:
                                ost.Cells[i, 11].value = "Liên đội TV6";
                                ost.Cells[i, 12].value = "HAN";
                                if (dm != null)
                                {
                                    if (int.TryParse(dm.TenDanhMuc.Substring(4), out doiso))
                                    {
                                        ost.Cells[i, 13].value = "'" + dm.TenDanhMuc.Substring(4);
                                        ost.Cells[i, 12].value = "HAN";
                                    }
                                    else
                                        ost.Cells[i, 12].value = "'" + dm.TenDanhMuc.Substring(4); //DAD+CXR                                       
                                }
                                break; //LĐTV6
                            case 2616:
                            case 2617:
                                ost.Cells[i, 11].value = "Phòng Kế hoạch hành chính";                                
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length-2,2)=="PN")
                                        ost.Cells[i, 12].value = "SGN";                                    
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //KHHC
                            case 2614:
                            case 2615:
                                ost.Cells[i, 11].value = "Phòng Kế toán";
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length - 2, 2) == "PN")
                                        ost.Cells[i, 12].value = "SGN";
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //KT
                            case 2624:
                            case 2625:                            
                                ost.Cells[i, 11].value = "Phòng An toàn chất lượng";
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length - 2, 2) == "PN")
                                        ost.Cells[i, 12].value = "SGN";
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //ATCL
                            case 2622:
                            case 2623:
                                ost.Cells[i, 11].value = "Phòng Phục vụ hành khách";
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length - 2, 2) == "PN")
                                        ost.Cells[i, 12].value = "SGN";
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //PVHK
                            case 2620:
                            case 2621:
                                ost.Cells[i, 11].value = "Phòng Đào tạo";
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length - 2, 2) == "PN")
                                        ost.Cells[i, 12].value = "SGN";
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //ĐT
                            case 2626:
                            case 2627:
                                ost.Cells[i, 11].value = "Văn phòng Đảng Đoàn thể";
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length - 2, 2) == "PN")
                                        ost.Cells[i, 12].value = "SGN";
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //VPĐĐT
                            case 2618:
                            case 2619:
                                ost.Cells[i, 11].value = "Phòng Nhân lực";
                                if (dm != null)
                                {
                                    if (dm.TenDanhMuc.Substring(dm.TenDanhMuc.Length - 2, 2) == "PN")
                                        ost.Cells[i, 12].value = "SGN";
                                    else
                                        ost.Cells[i, 12].value = "HAN";
                                }
                                break; //VPĐĐT  
                            default: //Các trường hợp còn lại xét theo Note
                                chuoi = doc.GetItemValue("loaidoitac")[0];
                                if(chuoi.Trim()=="5") //Học viên
                                {
                                    ost.Cells[i, 11].value = "Học viên";
                                    ost.Cells[i, 11].font.color = 255;
                                }
                                else 
                                {
                                    bpnote = doc.GetItemValue("Bophan")[0];
                                    tenso = bpnote.Substring(2, 1);
                                    if (int.TryParse(tenso, out doiso))
                                    {
                                        ost.Cells[i, 11].value = "Liên đội TV" + tenso;
                                        if((bpnote.Substring(3,1)=="H") || (bpnote.Substring(3,2)=="PB"))
                                            ost.Cells[i, 12].value = "HAN";
                                        else
                                            ost.Cells[i, 12].value = "SGN";
                                    }
                                    else
                                    {
                                        ost.Cells[i, 11].font.color = 255;
                                        if (bpnote.Trim() == "VASCO")
                                            ost.Cells[i, 11].value = "Tiếp viên VASCO";
                                        else
                                            if (bpnote.Trim() == "LDAO")
                                                ost.Cells[i, 11].Value = "Lãnh đạo";
                                            else 
                                                ost.Cells[i, 11].value = bpnote;                                        
                                    }
                                }
                                chuoi = "";
                                break;
                        }
                        if (flagkn)
                            stkn.Cells[ikn, 9].value = ost.Cells[i, 11].value;
                        #endregion 
                        
                        i++;
                    }
                }
                //db.SaveChanges();
            }            
            smsdb.Dispose();
            dtn.Dispose();
            axmanv.Dispose();
            MessageBox.Show("Complete!");
        }

        private void thannhanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Danh sách nhân sự
            var fdsnv = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsnv = fdsnv.Worksheet("HSNV").ToList();
            // Ánh xạ manv DTV <-> manv TCT
            var fax = new ExcelQueryFactory(@"F:\HR - TCT\Ma ID TCT\DTV-ID.xlsx");
            var ax=fax.Worksheet("Sheet1").ToList();
            // Ánh xạ ID vé giảm <-> manv dtv
            FileStream fs = new FileStream(@"F:\HR - TCT\idnv-manv.DBF", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable dsid = Table.Open(fs).AsDataTable();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["TT_nhan_than"];
            xlApp.Visible = true;
            Excel.Workbook xlveid = xlApp.Workbooks.Open(@"F:\HR - TCT\Cap nhat DS mua ve ID.xlsx");
            Excel.Worksheet veid = xlveid.Sheets["Sheet1"];
            int ive = 3,idvp;
            string hoten;
           for(int i=2;i<=10393;i++)
           {
               hoten = veid.Cells[i, 2].value;
               if (hoten == null)
                   continue;
               idvp = (int)veid.Cells[i, 1].value;
               var manv = (from x in dsid.AsEnumerable()
                          where x.Field<decimal>("id_nv") == idvp
                          select x).FirstOrDefault();

               if(manv==null){
                   veid.Cells[i,20].value="Không tìm thấy";
                   continue;
               }
               //Manv TCT
               var tmp = (from y in ax
                      where y["ID cũ do đơn vị cấp"].ToString() == manv.Field<string>("manv")
                      select y).FirstOrDefault();
               
               if (tmp == null)
               {
                   veid.Cells[i, 20].value = "Không ánh xạ được";
                   continue;
               }
               var ns=dsnv.Where(z=>z["Mã nhân viên cũ"]==manv.Field<string>("manv")).FirstOrDefault();
               if (ns == null)
               {
                   veid.Cells[i, 20].value = "Đã nghỉ";
                   continue;
               }
               xlsheet.Cells[ive, 1].value = ive - 2;
               xlsheet.Cells[ive, 3].value = tmp["ID-TCT"].Value;
               xlsheet.Cells[ive, 5].value = ns["Họ & tên"];
               xlsheet.Cells[ive, 6].value = "'"+ns["Ngày sinh"];
               xlsheet.Cells[ive, 7].value = veid.Cells[i,15].value; //Quan hệ
               xlsheet.Cells[ive, 8].value = veid.Cells[i, 12].value; //Tên người thân
               
               var thansinh = veid.Cells[i, 13].value;
               if (thansinh != null)
               {
                   hoten = thansinh.ToString();
                   if (hoten.Length > 10)
                       hoten = hoten.ToString().Substring(0, 10);
                   xlsheet.Cells[ive, 9].value = "'" + hoten; //Ngày sinh người thân
               }
               
               xlsheet.Cells[ive, 11].value = veid.Cells[i, 16].value; //CMND               
               ive++;    
           }

            fdsnv.Dispose();
            fax.Dispose();
            MessageBox.Show("Complate!");
        }

        private void congTacToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Danh sách nhân sự
            var fdsnv = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsnv = fdsnv.Worksheet("HSNV").ToList();
            HREntities db = new HREntities();
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["QL_Cong tac"];
            xlApp.Visible = true;
            int i = 9;
            string manv;
            foreach (var nv in dsnv)
            {
                manv = nv["Mã nhân viên cũ"].Value.ToString().Trim();
                var ns = db.HoSoGocs.Where(x => x.mans.Trim() == manv).FirstOrDefault(); //Xác định nhân sự
                var lghd = db.luonghds.Where(x => x.id_ns == ns.id).FirstOrDefault();
                if (lghd != null)
                {
                    xlsheet.Cells[i, 1].value = i - 8;
                    xlsheet.Cells[i, 2].value = nv["Mã nhân viên TCT"];
                    xlsheet.Cells[i, 3].value = nv["Họ & tên"];
                    xlsheet.Cells[i, 4].value = "'"+nv["Ngày sinh"];
                    xlsheet.Cells[i, 5].value = "Đoàn Tiếp viên";
                    xlsheet.Cells[i, 6].value = nv["Phòng Ban Cấp 2"];
                    xlsheet.Cells[i, 7].value = nv["Phòng Ban Cấp 3"];
                    xlsheet.Cells[i, 8].value = "'"+nv["Phòng Ban Cấp 4"];
                    xlsheet.Cells[i, 9].value = nv["Nhóm chức danh"];
                    xlsheet.Cells[i, 10].value = nv["Chức Danh"];
                
                    var luong = db.mucluongs.Where(x => x.id == lghd.luong_bac).FirstOrDefault();
                    var blg = luong.bacluong;
                    if (blg != null)
                    {
                        xlsheet.Cells[i, 11].value = "MS"+blg.Substring(5,2);
                        xlsheet.Cells[i, 12].value = "";
                        if (blg.Substring(blg.Length - 4, 1)!="O")
                            xlsheet.Cells[i, 13].value = "Nhóm " + blg.Substring(blg.Length - 4, 1);
                        xlsheet.Cells[i, 14].value = blg.Substring(blg.Length-3,1);
                        xlsheet.Cells[i, 15].value = luong.muc_luong;
                        if (lghd.luong_ngay != null)
                            xlsheet.Cells[i, 22].value = "'" + lghd.luong_ngay.ToString().Substring(0, 10);

                        
                    }
                 i++;    
                }                
            }
            db.Dispose();
            fdsnv.Dispose();
            MessageBox.Show("Complete!");
        }

        private void dangToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["QT Cong tac Dang"];
            xlApp.Visible = true;

            var fdsnv = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsnv = fdsnv.Worksheet("HSNV").ToList();
            
            var dsdv = (from dv in dsnv
                        where dv["Đảng viên"] == "Có"
                        select dv).ToList();
            int i = 3;
            foreach (var nv in dsdv)
            {
                xlsheet.Cells[i, 1].value = i - 2;
                xlsheet.Cells[i, 2].value = nv["Mã nhân viên TCT"];
                xlsheet.Cells[i, 3].value = nv["Họ & tên"];
                xlsheet.Cells[i, 4].value = "Đảng bộ Đoàn tiếp viên";
                xlsheet.Cells[i, 5].value = nv["Chức vụ đảng 1"];
                i++;
            }

            fdsnv.Dispose();
            MessageBox.Show("Complete!");
        }

        private void matTrenRedAntToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fdsnv = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsnv = fdsnv.Worksheet("HSNV").ToList();
            
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Add(); ;
            Excel.Worksheet xlsheet = xlmau.Sheets["Sheet1"];
            xlApp.Visible = true;
            
            xlsheet.Cells[1,1].value="STT";
            xlsheet.Cells[1,2].value="Manv";
            xlsheet.Cells[1,3].value="Họ đệm";
            xlsheet.Cells[1,4].value="Tên";
            xlsheet.Cells[1,5].value="Bộ phận";

            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;
            string manv;
            int i = 2;
            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");
            doc = view.GetFirstDocument();
            while (doc != null)
            {
                manv=doc.GetItemValue("MSNV")[0];
                var ns = (from x in dsnv
                          where x["Mã nhân viên cũ"] == manv
                          select x).FirstOrDefault();
                if (ns == null)
                {
                    xlsheet.Cells[i, 1].value = i - 1;
                    xlsheet.Cells[i, 2].value = "'"+manv;
                    xlsheet.Cells[i, 3].value = doc.GetItemValue("Hodem")[0] ;
                    xlsheet.Cells[i, 4].value = doc.GetItemValue("Ten")[0];
                    xlsheet.Cells[i, 5].value = doc.GetItemValue("Bophan")[0];
                    i++;
                }
                doc = view.GetNextDocument(doc);
            }

            fdsnv.Dispose();
            MessageBox.Show("Complete!");
        }

        private void khenThuongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["KhenThuong"];
            xlApp.Visible = true;
            int i = 5;
            HREntities db = new HREntities();
            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();
            foreach (var ns in dsns)
            {
                var manv=ns["Mã nhân viên cũ"];
                var ant=db.HoSoGocs.Where(x=>x.mans.Trim()==manv).FirstOrDefault();
                if (ant!=null){
                    var dskt = (from x in db.khenkluats
                                where x.id_ns == ant.id && x.kyluat == false && x.ktkl_hinhthuc!=null && x.ktkl_ngayqd!=null &&x.ktkl_hinhthuc>0
                                select x).ToList();
                    if (dskt.Any())
                    {
                        foreach (var kt in dskt)
                        {
                            var hinhthuc=db.danhmucs.Where(y => y.id == kt.ktkl_hinhthuc).FirstOrDefault();
                            xlsheet.Cells[i,1].value=i-4;
                            xlsheet.Cells[i, 2].value = ns["Mã nhân viên TCT"];
                            xlsheet.Cells[i, 3].value = ns["Họ & tên"];
                            xlsheet.Cells[i, 8].value = "'" + ns["Ngày sinh"];
                            xlsheet.Cells[i, 9].value = ns["Nhóm chức danh"];
                            xlsheet.Cells[i, 10].value = ns["Chức Danh"];
                            xlsheet.Cells[i, 14].value = "'"+kt.ktkl_ngayqd.ToString().Substring(0,10);
                            xlsheet.Cells[i, 20].value = hinhthuc.TenDanhMuc;
                            xlsheet.Cells[i, 20].font.color = 255;
                            xlsheet.Cells[i, 22].value = kt.ktkl_ndung;
                            i++;
                        }
                    }
                }                
            }
            lstns.Dispose();
            db.Dispose();
            MessageBox.Show("Complete");
        }

        private void kyLuatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["QuaTrinh_KyLuat"];
            xlApp.Visible = true;
            int i = 4;
            HREntities db = new HREntities();
            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();
            foreach (var ns in dsns)
            {
                var manv = ns["Mã nhân viên cũ"];
                var ant = db.HoSoGocs.Where(x => x.mans.Trim() == manv).FirstOrDefault();
                if (ant != null)
                {
                    var dskt = (from x in db.khenkluats
                                where x.id_ns == ant.id && x.kyluat == true && x.ktkl_hinhthuc != null && x.ktkl_ngayqd != null && x.ktkl_hinhthuc!=3639 && x.ktkl_hinhthuc!=3677 && x.ktkl_hinhthuc!=4020
                                select x).ToList();
                    if (dskt.Any())
                    {
                        foreach (var kt in dskt)
                        {
                            var hinhthuc = db.danhmucs.Where(y => y.id == kt.ktkl_hinhthuc).FirstOrDefault();
                            xlsheet.Cells[i, 1].value = i - 3;
                            xlsheet.Cells[i, 2].value = ns["Mã nhân viên TCT"];
                            xlsheet.Cells[i, 3].value = ns["Họ & tên"];
                            xlsheet.Cells[i, 6].value = "'" + ns["Ngày sinh"];
                            xlsheet.Cells[i, 13].value = "'" + kt.ktkl_ngayqd.ToString().Substring(0, 10);
                            xlsheet.Cells[i, 14].value = "'" + kt.ktkl_ngayqd.ToString().Substring(0, 10);
                            xlsheet.Cells[i, 16].value = hinhthuc.TenDanhMuc;
                            xlsheet.Cells[i, 16].font.color = 255;
                            xlsheet.Cells[i, 18].value = kt.ktkl_ndung;
                            i++;
                        }
                    }
                }
            }
            lstns.Dispose();
            db.Dispose();
            MessageBox.Show("Complete");
        }

        private void layThiDua1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["KhenThuong"];
            xlApp.Visible = true;

            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();

            var lsttd = new ExcelQueryFactory(@"F:\HR - TCT\slthidua.xlsx");
            var thidua = lsttd.Worksheet("slthidua").ToList();
            int i = 50783;
            //-4
            foreach (var kq in thidua)
            {
                var manv = kq["manv"].Value.ToString().Trim();
                 var ns = (from x in dsns
                         where x[3].Value.ToString().Trim() == manv
                         select x).FirstOrDefault();

                if (ns != null)
                {
                    xlsheet.Cells[i, 1].value = i - 4;
                    xlsheet.Cells[i, 2].value = ns["Mã nhân viên TCT"];
                    xlsheet.Cells[i, 3].value = ns["Họ & tên"];
                    xlsheet.Cells[i, 8].value = "'" + ns["Ngày sinh"];
                    xlsheet.Cells[i, 9].value = ns["Nhóm chức danh"];
                    xlsheet.Cells[i, 10].value = ns["Chức Danh"];
                    xlsheet.Cells[i, 11].value = kq["soqd"];
                    xlsheet.Cells[i, 14].value = "'"+kq["ngayky"].ToString().Substring(0,10);
                    xlsheet.Cells[i, 20].value = kq["hinhthuc"];
                    xlsheet.Cells[i, 20].font.color = 255;
                    xlsheet.Cells[i, 22].value = "Kết quả thi đua năm " + kq["nam"];
                    i++;
                }
            }           
            lstns.Dispose();
            lsttd.Dispose();
            MessageBox.Show("Complete!");

        }

        private void layThiDua2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["KhenThuong"];
            xlApp.Visible = true;
            int i = 62277;
            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();
            //var lsttd = new ExcelQueryFactory(@"F:\HR - TCT\CAP NHAT danh hieu thi dua 2015-2017\Danh hieu TD 2016 cap nhat HT.xlsx");
            var lsttd = new ExcelQueryFactory(@"F:\HR - TCT\CAP NHAT danh hieu thi dua 2015-2017\Danh hieu TD 2017 cap nhat HT.xlsx");            
            var thidua = lsttd.Worksheet("Cá nhân").ToList();
            foreach (var td in thidua)
            {
                string manv = td["MNV"].Value.ToString().Trim();
                var ns = (from x in dsns
                          where x["Mã nhân viên cũ"].Value.ToString().Trim() == manv
                          select x).FirstOrDefault();
                if (ns != null)
                {
                    xlsheet.Cells[i, 1].value = i - 4;
                    xlsheet.Cells[i, 2].value = ns["Mã nhân viên TCT"];
                    xlsheet.Cells[i, 3].value = ns["Họ & tên"];
                    xlsheet.Cells[i, 8].value = "'" + ns["Ngày sinh"];
                    xlsheet.Cells[i, 9].value = ns["Nhóm chức danh"];
                    xlsheet.Cells[i, 10].value = ns["Chức Danh"];

                    string tmp = td["Số, ngày, cấp ra QĐ"].Value.ToString().Trim();
                    string tmp1;
                    int k;
                    k = tmp.IndexOf(",");
                    tmp1=tmp.Substring(0,k);
                    xlsheet.Cells[i, 11].value = tmp1;
                    if(tmp1.IndexOf("QĐ-TCTHK-TCNL")>0)
                        xlsheet.Cells[i, 14].value = "'28/12/2017";
                    if (tmp1.IndexOf("QĐ-TCTHK-ĐTV") > 0)
                        xlsheet.Cells[i, 14].value = "'16/01/2018";
                    xlsheet.Cells[i, 20].value = td["Danh hiệu"];
                    xlsheet.Cells[i, 20].font.color = 255;
                    xlsheet.Cells[i, 22].value = "Kết quả thi đua năm 2017";
                    i++;
                }

            }

            lsttd.Dispose();
            lstns.Dispose();
            MessageBox.Show("Complete!");
        }

        private void baoHiemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["TT_BHXH"];
            xlApp.Visible = true;
            int i = 3;
            var fbh = new ExcelQueryFactory(@"F:\HR - TCT\BH.xlsx");
            var dsbh = fbh.Worksheet("Sheet1").ToList();
            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();
            foreach (var ns in dsns)
            {
                string manv=ns["Mã nhân viên cũ"].Value.ToString().Trim();
                var bh = (from x in dsbh
                          where x["MÃ NV"].Value.ToString().Trim() == manv
                          select x).FirstOrDefault();

                if (bh != null)
                {
                    xlsheet.Cells[i, 1].value = i - 2;
                    xlsheet.Cells[i, 3].value = ns["Mã nhân viên TCT"];
                    xlsheet.Cells[i, 4].value = ns["Họ & tên"];
                    xlsheet.Cells[i, 8].value = bh["Số sổ BHXH"];
                    xlsheet.Cells[i, 9].value =bh["SỐ THẺ BHYT"];
                    i++;
                }
            }

            lstns.Dispose();
            fbh.Dispose();
            MessageBox.Show("Complete!");
        }

        private void hopDòngLDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["QT Hop dong lao dong"];
            xlApp.Visible = true;
            int i = 3;
            HREntities db = new HREntities();
            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();
            foreach (var ns in dsns)
            {
                var manv = ns["Mã nhân viên cũ"].Value.ToString().Trim();
                var hs=db.HoSoGocs.Where(y=>y.mans.Trim()==manv).FirstOrDefault();
                if (hs==null)
                    continue;

                var hdld = db.Laodongs.Where(x => x.id_ns == hs.id).ToList();
                if (hdld == null)
                    continue;
                string tmp = "";
                bool flag=false;
                foreach (var hd in hdld)
                {

                    if (hd.hd_loai == 387)
                        continue;
                    xlsheet.Cells[i, 1].value = i - 2;
                    xlsheet.Cells[i, 3].value = ns["Mã nhân viên TCT"];
                    xlsheet.Cells[i, 4].value = ns["Họ & tên"];
                    xlsheet.Cells[i, 5].value = "Đoàn tiếp viên";
                    switch (hd.hd_loai)
                    {
                        case 450:
                            tmp = "Hợp đồng không xác định thời hạn";
                            flag = false;
                            break;
                        case 445:
                            tmp = "Hợp đồng đào tạo";
                            flag = true;
                            break;
                        case 446:
                            tmp = "Hợp đồng mùa vụ - vụ việc";
                            flag = false;
                            break;
                        case 447:
                            tmp = "Hợp đồng thử việc";
                            flag = true;
                            break;
                        case 448:
                            tmp = "Hợp đồng xác định thời hạn 1 năm";
                            flag = false;
                            break;
                        case 449:
                            tmp = "Hợp đồng xác định thời hạn 3 năm";
                            flag = false;
                            break;
                        case 3560:
                            tmp = "Hợp đồng không xác định thời hạn (Tuổi 40-45)";
                            flag = true;
                            break;
                        case 3561:
                            tmp = "Hợp đồng xác định thời hạn 6 tháng";
                            flag = true;
                            break;
                        case 3562:
                            tmp = "Hợp đồng xác định thời hạn dưới 6 tháng";
                            flag = true;
                            break;
                        case 3565:
                            tmp = "Hợp đồng ngắn hạn";
                            flag = true;
                            break;
                        case 3566:
                            tmp = "Hợp đồng dài hạn";
                            flag = true;
                            break;
                        case 3567:
                            tmp = "Hợp đồng vô hạn";
                            flag = true;
                            break;
                        default:
                            tmp = hd.hd_loai.ToString();
                            flag = true;
                            break;
                    }
                    xlsheet.Cells[i, 6].value = tmp;
                    if(flag)
                        xlsheet.Cells[i, 6].font.color=255;
                    if(hd.hd_sohd!=null)
                        xlsheet.Cells[i, 7].value = hd.hd_sohd;
                    if(hd.hd_ngayhieuluc!=null)
                        xlsheet.Cells[i, 10].value = "'"+hd.hd_ngayhieuluc.ToString("dd/MM/yyyy");
                    if(hd.hd_ngayhet!=null)
                        xlsheet.Cells[i, 11].value = "'" + hd.hd_ngayhet.ToString("dd/MM/yyyy");                    
                    i++;
                }
            }

            lstns.Dispose();
            db.Dispose();
            MessageBox.Show("Complete!");
        }

        private void daoTaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["QT_Đào_Tạo"];
            xlApp.Visible = true;
            int i = 5;
            HREntities db = new HREntities();
            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var dsns = lstns.Worksheet("HSNV").ToList();
            foreach (var ns in dsns)
            {
                string manv=ns["Mã nhân viên cũ"].Value.ToString().Trim();
                var redant=db.HoSoGocs.Where(x=>x.mans.Trim()==manv).FirstOrDefault();
                if (redant!=null)
                {
                    
                    xlsheet.Cells[i, 1].value = i - 4;
                    xlsheet.Cells[i, 3].value = ns["Mã nhân viên TCT"];
                    xlsheet.Cells[i, 4].value = ns["Họ & tên"];
                    xlsheet.Cells[i, 5].value = "'" + ns["Ngày sinh"];
                    xlsheet.Cells[i, 6].value = "Đoàn tiếp viên";
                    xlsheet.Cells[i, 7].value = ns["Phòng Ban Cấp 2"];
                    xlsheet.Cells[i, 8].value = ns["Nhóm chức danh"];
                    xlsheet.Cells[i, 9].value = ns["Chức Danh"];
                    var dm=db.danhmucs.Where(y=>y.id==redant.hocvantd).FirstOrDefault();
                    if (dm!=null)
                        xlsheet.Cells[i, 12].value = dm.TenDanhMuc;
                    dm=db.danhmucs.Where(y=>y.id==redant.chuyennganh).FirstOrDefault();
                    if (dm!=null)
                        xlsheet.Cells[i, 19].value = dm.TenDanhMuc;
                    else
                    {
                        var cm = db.nhomchuyenmons.Where(z => z.id_ns == redant.id).FirstOrDefault();
                        if (cm != null)
                        {
                            dm = db.danhmucs.Where(k => k.id == cm.nhomchuyenmon1).FirstOrDefault();
                            if(dm!=null)
                                xlsheet.Cells[i, 19].value = dm.TenDanhMuc;
                        }

                    }
                    if(redant.loains==42) 
                        xlsheet.Cells[i, 34].value = "*";
                    i++;
                }
                
            }
            lstns.Dispose();
            db.Dispose();
            MessageBox.Show("Complete!");
        }

        private void daoTaoTVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlmau = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlsheet = xlmau.Sheets["QT_Đào_Tạo"];
            xlApp.Visible = true;
            int i = 88059;

            FileStream fs = new FileStream(@"\\10.100.8.30\FoxApp\HLDT\Data\process.DBF", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable dslop = Table.Open(fs).AsDataTable();

            HREntities db = new HREntities();
            DateTime? dt;
            string tmp;
            var lstlop = new ExcelQueryFactory(@"F:\HR - TCT\dslop.xlsx");
            var tenlop = lstlop.Worksheet("dslop").ToList();

            var lstns = new ExcelQueryFactory(@"F:\HR - TCT\DSNV.xlsx");
            var pns = lstns.Worksheet("HSNV").Where(x => x["Mã nhân viên TCT"] != "").ToList();
            var dsns = (from p in pns 
                       where int.Parse(p["Mã nhân viên TCT"].Value.ToString().Substring(2))>=5344
                       select p).ToList();
            
            foreach (var ns in dsns)
            {
                var manv=ns["Mã nhân viên cũ"].Value.ToString().Trim();
                var hoclop = (from lop in dslop.AsEnumerable()
                              where lop.Field<string>("status") == "OK" && lop.Field<string>("paxcode") == manv
                              select lop).ToList();
                if (hoclop != null)
                {   
                    foreach (var xx in hoclop)
                    {
                        var zz = tenlop.Where(k => k["objcode"] == xx.Field<string>("objcode")).FirstOrDefault();
                        if (zz == null)
                            continue;
                        tmp = ns["Mã nhân viên TCT"].Value.ToString().Trim();
                        if(int.Parse(tmp.Substring(2))<5344)
                            continue;

                        xlsheet.Cells[i, 1].value = i - 4;
                        xlsheet.Cells[i, 3].value = ns["Mã nhân viên TCT"];
                        xlsheet.Cells[i, 4].value = ns["Họ & tên"];
                        xlsheet.Cells[i, 5].value = "'" + ns["Ngày sinh"];
                        xlsheet.Cells[i, 6].value = "Đoàn tiếp viên";
                        xlsheet.Cells[i, 7].value = ns["Phòng Ban Cấp 2"];
                        xlsheet.Cells[i, 8].value = ns["Nhóm chức danh"];
                        xlsheet.Cells[i, 9].value = ns["Chức Danh"];
                        xlsheet.Cells[i, 10].value = "Lĩnh vực  Dịch vụ";
                       
                        dt = xx.Field<DateTime?>("cdatefrom");
                        if (dt!=null)
                            xlsheet.Cells[i, 15].value = "'" + dt.ToString().Substring(0, 10);
                        dt = xx.Field<DateTime?>("cdateto");
                        if(dt!=null)
                            xlsheet.Cells[i, 15].value = "'" + dt.ToString().Substring(0,10);
                        dt=xx.Field<DateTime?>("testdate");
                        if(dt!=null)
                            xlsheet.Cells[i, 25].value = "'" + dt.ToString().Substring(0, 10);
                        dt = xx.Field<DateTime?>("expiredate");
                        if(dt!=null)
                            xlsheet.Cells[i, 26].value = "'" + dt.ToString().Substring(0, 10);

                        
                        xlsheet.Cells[i, 11].value = zz["descript"];
                        i++;
                    }                    
                }
                
            }
            

            lstns.Dispose();
            db.Dispose();
            MessageBox.Show("Complete!");
        }

        private void capNhatTenSoRedAntToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;
            string tennote,tenso;
            int doiso;
            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");
            using (HREntities db = new HREntities())
            {
                foreach (var hs in db.HoSoGocs)
                {
                    if (hs.nghiviec == true)
                        continue;
                    var ld = (from ttld in db.Laodongs
                              where ttld.id_ns == hs.id
                              select ttld).FirstOrDefault();
                    if (ld == null || ld.hd_ngayhlchamduthd == null)
                    {
                        doc = view.GetDocumentByKey(hs.mans.Trim());
                        if (doc == null)
                            continue;
                        tennote = doc.GetItemValue("Ten")[0];
                        //Kiểm tra số trùng tên giữa Note và RedAnt
                        if (tennote.Trim().LastIndexOf(" ") != -1)
                        {
                            tenso = tennote.Substring(tennote.Trim().LastIndexOf(" ") + 1);
                            //Có phải là số trùng tên
                            if (int.TryParse(tenso, out doiso))
                            {
                                if (tenso.Trim() != hs.ns_stt.Trim())
                                    hs.ns_stt = tenso;
                                
                            }                            
                        }                        
                    }                    
                }
                db.SaveChanges();
                MessageBox.Show("Complete!");
            }
        }

        private void createGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void updateHSNVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Excel.Application xlApp = new Excel.Application();            
            xlApp.Visible = true;
            Excel.Workbook xlNL = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlUpdate = xlNL.Sheets["HSNV"];
            string manv;
            var fhsnv = new ExcelQueryFactory(@"F:\HR - TCT\NSNV-NL check.xlsx");
            var hsnv=fhsnv.Worksheet("HSNV").ToList();
            var fax = new ExcelQueryFactory(@"F:\HR - TCT\Ma ID TCT\DTV-ID.xlsx");
            var ax = fax.Worksheet("Sheet1").ToList();

            for (int i = 9; i <= 3189; i++)
            {
                manv = xlUpdate.Cells[i, 3].value;
                //if (manv != "VN04222")
                //    continue;

                var ns = (from x in ax
                          where x["ID-TCT"] == manv
                          select x).FirstOrDefault();

                var nsupdate = (from y in hsnv
                                where y["Mã nhân viên"].Value.ToString().Trim() == ns["ID cũ do đơn vị cấp"].Value.ToString().Trim()
                               select y).FirstOrDefault();
                if (nsupdate != null)
                {
                    if (xlUpdate.Cells[i, 15].value == null)
                    {
                        xlUpdate.Cells[i, 15].value = nsupdate["Chức Danh"];
                        xlUpdate.Cells[i, 15].Interior.Color = 49407;
                    }
                }
            }
            fhsnv.Dispose();
            fax.Dispose();
            MessageBox.Show("Complete!");
        }

        private void updateDaotaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            xlApp.Visible = true;
            Excel.Workbook xlNL = xlApp.Workbooks.Open(@"F:\HR - TCT\nhap lieu don vi ver 04.xlsx");
            Excel.Worksheet xlUpdate = xlNL.Sheets["QT_Đào_Tạo"];
            string manv;
            var fhsdt = new ExcelQueryFactory(@"F:\HR - TCT\Dao tao CBNV mat dat DTV - toan bo - 20180620.xlsx");
            var hsdt = fhsdt.Worksheet("QT_Đào_Tạo").ToList();
           
            for (int i = 5; i <= 119940; i++)
            {
                manv = xlUpdate.Cells[i, 3].value;
                var dt = (from x in hsdt
                         where x["Mã nhân viên"] == manv
                         select x).FirstOrDefault();
                //Cot 35='XX'
                if (dt != null)
                {
                    xlUpdate.Cells[i, 35].value = "XX";
                }
            }
            fhsdt.Dispose();
            MessageBox.Show("Complete!");
        }

        private void getThongtinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            xlApp.Visible = true;
            Excel.Workbook xlwb = xlApp.Workbooks.Add();
            Excel.Worksheet xlns = xlwb.Sheets["Sheet1"];
            xlns.Cells[1, 1].value = "STT";
            xlns.Cells[1, 2].value = "Manv";
            xlns.Cells[1, 3].value = "Họ và tên";
            xlns.Cells[1, 4].value = "Bộ phận";
            xlns.Cells[1, 5].value = "Chức vụ";
            xlns.Cells[1, 6].value = "Chức danh";
            xlns.Cells[1, 7].value = "Thông tin khác";
            int i = 2;
            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;
            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");

            using (HREntities db = new HREntities())
            {
                foreach (var hs in db.HoSoGocs)
                {
                    if (hs.nghiviec == true || hs.loains==42 || hs.loains==3530) 
                        continue;
                    var ld = (from ttld in db.Laodongs
                              where ttld.id_ns == hs.id
                              select ttld).FirstOrDefault();
                    if (ld==null || ld.hd_ngayhlchamduthd == null)
                    {
                        xlns.Cells[i, 1].value=i - 1;
                        xlns.Cells[i, 2].value = "'"+hs.mans.Trim();
                        xlns.Cells[i, 3].value = hs.ns_ho.Trim() + " " + hs.ns_ten.Trim() + " " + hs.ns_stt.Trim();
                        doc = view.GetDocumentByKey(hs.mans.Trim());
                        if (doc != null)
                        {
                            xlns.Cells[i, 4].value = doc.GetItemValue("Bophan")[0];
                            xlns.Cells[i, 5].value = doc.GetItemValue("Chucvu")[0];
                            xlns.Cells[i, 6].value = doc.GetItemValue("ChucvuTV")[0];
                            xlns.Cells[i, 7].value = doc.GetItemValue("loaidoitac")[0];
                        }
                        i++;
                    }
                }//foreach
            }//db
            MessageBox.Show("Complete!");
   
        }

        private void chucDanhChucVuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HREntities db = new HREntities();
            string manv;
            int idns,idcv;
            DateTime? ngayky,ngaykt;
            DateTime mindate = new DateTime(1900, 1, 1);

            Excel.Application xlApp=new Excel.Application();
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet;
            #region Chucdanh
            //xlbook = xlApp.Workbooks.Open(@"F:\HR - TCT\Update-Chucdanh-Chucvu-RedAnt.xlsx");
            //xlApp.Visible = true;
            //xlsheet = xlbook.Worksheets["Chuc danh"];
            //for (int i = 2; i <= 4992; i++)
            //{
            //    manv = xlsheet.Cells[i, 1].value;
            //    var ns = db.HoSoGocs.Where(x => x.mans.Trim() == manv.Trim()).FirstOrDefault();
            //    if (ns == null)
            //    {
            //        xlsheet.Cells[i, 20].value = "Không thấy trong nhân sự";
            //        continue; //Không tìm thấy trong nhân sự bỏ qua
            //    }
                    
            //    ngayky = xlsheet.Cells[i, 6].value;
            //    if (ngayky == null)
            //    {
            //        xlsheet.Cells[i, 20].value = "Không có ngày hiệu lực";
            //        continue; //Không có ngày hiệu lực bỏ qua
            //    }
            //    idns = ns.id;
            //    ngaykt = xlsheet.Cells[i, 7].value;
                
            //    if (ngayky < mindate)
            //        ngayky = mindate;
                
            //    if (ngaykt < mindate)
            //        ngaykt = null;

            //    if (xlsheet.Cells[i, 4].value == null)
            //    {
            //        xlsheet.Cells[i, 20].value = "Không chức danh";
            //        continue; //Không có ngày hiệu lực bỏ qua
            //    }
            //    else
            //        idcv = (int)xlsheet.Cells[i, 4].value;

            //    var chucdanh = (from cd in db.chucdanhs
            //                    where cd.id_ns == idns && cd.chucdanh1 == idcv && cd.chucdanh_ngay==ngayky
            //                    select cd).FirstOrDefault();

            //    if(chucdanh==null) //Chưa có, thêm mới
            //    {
            //        //Kiểm tra trường hợp kg nhập ngày(1900-01-01
            //        var dschucdanh= (from cd in db.chucdanhs
            //                    where cd.id_ns == idns && cd.chucdanh1 == idcv
            //                    select cd).ToList();
            //        if (dschucdanh.Count == 0)
            //        {
            //            chucdanh newcd = new chucdanh();
            //            newcd.chucdanh1 = idcv;
            //            newcd.id_ns = idns;
            //            newcd.chucdanh_ngay = (DateTime)ngayky;
            //            newcd.chucdanh_ngayky = (DateTime)ngayky;
            //            newcd.chucdanh_noilam = true;
            //            if (ngaykt != null)
            //                newcd.chucdanh_ngayhet = (DateTime)ngaykt;

            //            db.chucdanhs.Add(newcd);
            //            xlsheet.Cells[i, 20].value = "Thêm mới";
            //            db.SaveChanges();
            //        }
            //        else
            //        {
            //            var chucdanh1 = (from cd in db.chucdanhs
            //                            where cd.id_ns == idns && cd.chucdanh1 == idcv && cd.chucdanh_ngay == new DateTime(1900,01,01)
            //                            select cd).FirstOrDefault();
            //            if (chucdanh1 != null)
            //            {
            //                chucdanh1.chucdanh_ngay = (DateTime)ngayky;
            //                chucdanh1.chucdanh_ngayky = (DateTime)ngayky;
            //                if (ngaykt != null)
            //                    chucdanh1.chucdanh_ngayhet = (DateTime)ngaykt;
            //                db.SaveChanges();
            //                xlsheet.Cells[i, 20].value = "Cập nhật 1";
            //            }
            //        }
                    
                    
            //    }
            //    else //Có rồi, xem xét cập nhật
            //    {
            //        if (ngaykt != null)
            //        {
            //            chucdanh.chucdanh_ngayhet = (DateTime)ngaykt;
            //            xlsheet.Cells[i, 20].value = "Cập nhật";
            //            db.SaveChanges();
            //        }
            //    }
            //}//end for
            #endregion 
            #region Chucvu
            xlbook = xlApp.Workbooks.Open(@"F:\HR - TCT\Update-Chucdanh-Chucvu-RedAnt.xlsx");
            xlApp.Visible = true;
            xlsheet = xlbook.Worksheets["Chuc vu"];
            for (int i = 2; i <= 131; i++)
            {
                manv = xlsheet.Cells[i, 1].value;
                var ns = db.HoSoGocs.Where(x => x.mans.Trim() == manv.Trim()).FirstOrDefault();
                if (ns == null)
                {
                    xlsheet.Cells[i, 20].value = "Không thấy trong nhân sự";
                    continue; //Không tìm thấy trong nhân sự bỏ qua
                }

                ngayky = xlsheet.Cells[i, 6].value;
                if (ngayky == null)
                {
                    xlsheet.Cells[i, 20].value = "Không có ngày hiệu lực";
                    continue; //Không có ngày hiệu lực bỏ qua
                }
                idns = ns.id;
                ngaykt = xlsheet.Cells[i, 7].value;

                if (ngayky < mindate)
                    ngayky = mindate;

                if (ngaykt < mindate)
                    ngaykt = null;

                if (xlsheet.Cells[i, 4].value == null)
                {
                    xlsheet.Cells[i, 20].value = "Không chức danh";
                    continue; //Không có ngày hiệu lực bỏ qua
                }
                else
                    idcv = (int)xlsheet.Cells[i, 4].value;

                var chucdanh = (from cd in db.chucvus
                                where cd.id_ns == idns && cd.chucvu1 == idcv && cd.chucvu_ngayky == ngayky
                                select cd).FirstOrDefault();

                if (chucdanh == null) //Chưa có, thêm mới
                {
                    //Kiểm tra trường hợp kg nhập ngày(1900-01-01
                    var dschucdanh = (from cd in db.chucvus
                                      where cd.id_ns == idns && cd.chucvu1 == idcv
                                      select cd).ToList();
                    if (dschucdanh.Count == 0)
                    {
                        chucvu newcd = new chucvu();
                        newcd.chucvu1 = idcv;
                        newcd.id_ns = idns;
                        newcd.chucvu_ngay = (DateTime)ngayky;
                        newcd.chucvu_ngayky = (DateTime)ngayky;
                        newcd.chucvu_noibo = true;
                        if (ngaykt != null)
                            newcd.chucvu_ngayhet = (DateTime)ngaykt;

                        db.chucvus.Add(newcd);
                        xlsheet.Cells[i, 20].value = "Thêm mới";
                        db.SaveChanges();
                    }
                    else
                    {
                        var chucdanh1 = (from cd in db.chucvus
                                         where cd.id_ns == idns && cd.chucvu1 == idcv && cd.chucvu_ngay == new DateTime(1900, 01, 01)
                                         select cd).FirstOrDefault();
                        if (chucdanh1 != null)
                        {
                            chucdanh1.chucvu_ngay = (DateTime)ngayky;
                            chucdanh1.chucvu_ngayky = (DateTime)ngayky;
                            if (ngaykt != null)
                                chucdanh1.chucvu_ngayhet = (DateTime)ngaykt;
                            db.SaveChanges();
                            xlsheet.Cells[i, 20].value = "Cập nhật 1";
                        }
                    }


                }
                else //Có rồi, xem xét cập nhật
                {
                    if (ngaykt != null)
                    {
                        chucdanh.chucvu_ngayhet = (DateTime)ngaykt;
                        xlsheet.Cells[i, 20].value = "Cập nhật";
                        db.SaveChanges();
                    }
                }
            }
            #endregion 
            db.Dispose();
            MessageBox.Show("Complete!");
        }

        private void vNAALSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet;
            string filename=@"F:\HR - TCT\TV VNA-ALS.xlsx";
            string _manv = "",doituong;
            NotesSession session = new NotesSession();
            NotesDatabase dbase;
            NotesView view;
            NotesDocument doc;
            session.Initialize("btliem");
            dbase = session.GetDatabase("domino.dev/DTV", "Nhansu\\qlns.nsf");
            view = dbase.GetView("Nhan su\\Theo ma so");
            
            xlbook=xlApp.Workbooks.Open(filename);
            xlsheet = xlbook.Sheets["HSNV"];
            xlApp.Visible = true;
            for (int i = 1; i <= 3261; i++)
            {
                _manv = xlsheet.Cells[i, 4].value;
                doc = view.GetDocumentByKey(_manv);
                if (doc == null)
                    continue;
                doituong = doc.GetItemValue("loaidoitac")[0];
                switch (doituong)
                {
                    case "1":
                        xlsheet.Cells[i, 10].value = "Tiếp viên VNA";
                        break;
                    case "2":
                    case "2a":
                        xlsheet.Cells[i, 10].value = "Nhân viên mặt đất";
                        break;
                    case "3a":
                        xlsheet.Cells[i, 10].value = "Tiếp viên VNA-Trung tâm huấn luyện quản lý";
                        break;
                    case "3b":
                        xlsheet.Cells[i, 10].value = "Tiếp viên VNA-Ban An toàn chất lượng - an ninh quản lý";
                        break;
                    case "3c":
                        xlsheet.Cells[i, 10].value = "Tiếp viên VASCO";
                        break;
                    case "4":
                        xlsheet.Cells[i, 10].value = "Tiếp viên người nước ngoài";
                        break;
                    case "5":
                        xlsheet.Cells[i, 10].value = "Học viên";
                        break;
                    
                    case "7":
                        xlsheet.Cells[i, 10].value = "Tiếp viên Alsimexco";
                        break;
                    case "8":
                        xlsheet.Cells[i, 10].value = "Tiếp viên VNA bay cho K6";
                        break;

                }

            }
            MessageBox.Show("Complete");
        }

       

        private void huyTVPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet;
            HREntities db = new HREntities();
            int macd, newcd;
            string _manv, filename = @"F:\Downloads\207 TV thoi chuc danh TVP tu 01.07.2018_gui thay (1).xlsx";
            xlbook = xlApp.Workbooks.Open(filename);
            xlsheet = xlbook.Sheets["Sheet1"];
            xlApp.Visible = true;
            DateTime ngayhet; // = new DateTime(2018, 6, 30);
            chucdanh ochucdanh, newchucdanh,cdtvp;
            for (int i = 19; i <= 207; i++)
            {
                _manv = xlsheet.Cells[i, 5].value;
                macd = (int)xlsheet.Cells[i, 7].value;
                if (xlsheet.Cells[i, 21].value != null)
                    newcd = (int)xlsheet.Cells[i, 21].value;
                else
                    newcd = 0;
                var hs = db.HoSoGocs.Where(x => x.mans.Trim() == _manv).FirstOrDefault();
                if (hs == null)
                    continue;
                
                ochucdanh = db.chucdanhs.Where(x => x.id_ns == hs.id && (x.chucdanh1 == 3659 || x.chucdanh1==4016)).FirstOrDefault();
                if (ochucdanh != null)
                    ochucdanh.chucdanh_noilam = true;
                //Tìm người là TVP-TVT1 hoặc TVP-TVC, nếu có thì bắt đầu của cái này là kết thúc của TVP, điều chỉnh kết thúc cái này về 30/06/2018
                if (newcd == 0)
                {
                    newchucdanh=db.chucdanhs.Where(x => x.id_ns == hs.id && (x.chucdanh1 == 3661 || x.chucdanh1==4022)).FirstOrDefault();
                    if (newchucdanh == null)
                        xlsheet.Cells[i, 23].value = "OK";
                    else
                    {
                        ngayhet = newchucdanh.chucdanh_ngay;
                        newchucdanh.chucdanh_ngayhet = new DateTime(2018, 06, 30);
                        cdtvp = db.chucdanhs.Where(x => x.id_ns == hs.id && x.chucdanh1 == 3660).FirstOrDefault();
                        if (cdtvp != null)
                        {
                            cdtvp.chucdanh_ngayhet = ngayhet;
                            xlsheet.Cells[i, 21].value =newchucdanh.chucdanh1;
                            xlsheet.Cells[i, 22].value = ngayhet;
                            xlsheet.Cells[i, 21].Interior.Color = 49407;
                            xlsheet.Cells[i, 22].Interior.Color = 49407;
                        }
                    }
                }
                //ochucdanh = db.chucdanhs.Where(x => x.id_ns == hs.id && x.chucdanh1 == macd).FirstOrDefault();
                //if (ochucdanh != null)
                //{
                //    ochucdanh.chucdanh_ngayhet = new DateTime(2018, 06, 30);
                //}
                //else
                //{
                //    macd = 3661;
                //    ochucdanh = db.chucdanhs.Where(x => x.id_ns == hs.id && x.chucdanh1 == macd).FirstOrDefault();
                //    if (ochucdanh != null)
                //    {
                //        ochucdanh.chucdanh_ngayhet = new DateTime(2018, 06, 30);
                //        xlsheet.Cells[i, 21].value = macd;
                //    }
                //    else
                //    {
                //        macd = 4022;
                //        ochucdanh = db.chucdanhs.Where(x => x.id_ns == hs.id && x.chucdanh1 == macd).FirstOrDefault();
                //        if (ochucdanh != null)
                //        {
                //            ochucdanh.chucdanh_ngayhet = new DateTime(2018, 06, 30);
                //            xlsheet.Cells[i, 21].value = macd;
                //        }
                //    }
                //}
                //newchucdanh = new chucdanh();
                //newchucdanh.id_ns = hs.id;
                //newchucdanh.chucdanh1 = newcd;
                //newchucdanh.chucdanh_ngay = new DateTime(2018, 7, 1);
                //newchucdanh.chucdanh_ngayky = new DateTime(2018, 7, 1);
                //db.chucdanhs.Add(newchucdanh);
                db.SaveChanges();
            }

            db.Dispose();
            MessageBox.Show("Complete");
        }

        private void layThongTinNNKhacTOEICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet;
            HREntities db = new HREntities();
            string filename = @"F:\Temp\di_van_cc.xlsx";
            string manv;
            xlbook = xlApp.Workbooks.Open(filename);
            xlsheet = xlbook.Sheets["di_van_cc"];
            xlApp.Visible = true;
            for (int i = 2; i <= 2826; i++)
            {
                if (xlsheet.Cells[i, 17].value > 0)
                    continue;
                manv = xlsheet.Cells[i, 1].value;
                if (manv == "4864")
                    MessageBox.Show(manv);

                var hs = db.HoSoGocs.Where(x => x.mans.Trim() == manv).FirstOrDefault();
                if (hs == null)
                    continue;

                var nnkhac = (from nn in db.ngoaingus
                              where nn.id_ns == hs.id && nn.ngoaingu_loai == 565 && nn.ngoaingu_bangcap!=412 && nn.ngoaingu_bangcap!=669 && nn.ngoaingu_bangcap!=3797 && nn.ngoaingu_bangcap!=3798 && nn.ngoaingu_bangcap!=3796
                              select nn);
                if (nnkhac.Any())
                {
                    var nnk = (from nn1 in nnkhac
                               select nn1).FirstOrDefault();
                    xlsheet.Cells[i, 26].value = nnk.ngoaingu_bangcap;
                    xlsheet.Cells[i, 27].value = nnk.ngoaingu_diemtong ?? 0;

                }
            }
            db.Dispose();
            MessageBox.Show("Complete!");
        }

        private void bHMTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlapp;
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet,xldes;
            xlapp = new Excel.Application();
            xlbook = xlapp.Workbooks.Open(@"F:\Downloads\crew information missing.xlsx");
            xlsheet = xlbook.Sheets["C"];
            //xldes = xlbook.Sheets["C"];
            xlapp.Visible = true;
            string _manv,nametv;
            int n,k;
            DateTime ngay;
            FileStream fs = new FileStream(@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\dm_tvien.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable dstv = Table.Open(fs).AsDataTable();
            //FileStream fs1 = new FileStream(@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\lbaytv2.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //DataTable dscb = Table.Open(fs1).AsDataTable();
            #region Lấy tên tiếp viên
            for (int i = 2; i <= 96; i++)
            {
                _manv = xlsheet.Cells[i, 4].value;
                var tv = (from x in dstv.AsEnumerable()
                          where x.Field<string>("code_tv") == _manv
                          select x).FirstOrDefault();
                if (tv == null)
                    xlsheet.Cells[i, 7].value = "Sai CrewID";
                else
                {
                    nametv = tv.Field<string>("name_tv");
                    n = nametv.LastIndexOf(" ");
                    _manv = nametv.Substring(n + 1);
                    if (int.TryParse(_manv, out k))
                        nametv = nametv.Substring(0, n);
                    xlsheet.Cells[i, 3].value = nametv; ;
                    xlsheet.Cells[i, 5].value = tv.Field<string>("quoctich") ?? "Vietnam";
                    xlsheet.Cells[i, 6].value = tv.Field<string>("pport_no");
                }
            }
            #endregion 
            //#region Bổ sung tiếp viên vào chuyến bay
            //xlsheet = xlbook.Worksheets["A"];
            //k = 2;
            //for (int i = 3; i <= 42; i++)
            //{
            //    _manv = xlsheet.Cells[i, 1].value;
            //    ngay = xlsheet.Cells[i, 2].value;
            //    nametv = "VN" + _manv.Substring(4);
            //    var tobay = (from x in dscb.AsEnumerable()
            //                 where x.Field<string>("fly_no") == _manv && x.Field<DateTime>("start_date") == ngay
            //                 select x).ToList();
            //    if (tobay.Count > 0)
            //    {
            //        xldes.Cells[k, 1].value = _manv;
            //        xldes.Cells[k, 2].value = ngay;
            //        foreach (var tv in tobay)
            //        {
            //            xldes.Cells[k, 4].value = tv.Field<string>("code_tv");
            //            k++;
            //        }
            //    }
            //    else
            //        xlsheet.Cells[i, 3].value = "Không tìm thấy chuyến bay";
            //}
            //#endregion
            fs.Dispose();
            //fs1.Dispose();
            MessageBox.Show("Complete");
        }

        private void bSTTDSCCTHCCSKATBPPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlapp;
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet;
            xlapp = new Excel.Application();
            xlbook = xlapp.Workbooks.Open(@"F:\Downloads\Danh sach 1001 TV_gui thay.xlsx");
            xlsheet = xlbook.Sheets["Đạt VIPA - Toàn đoàn"];
            //xlsheet = xlbook.Sheets["Copy"];
            xlapp.Visible = true;
            string _manv;

            HREntities db = new HREntities();
            SataHRMEntities dbsms = new SataHRMEntities();
            CCSKEntities dbsk = new CCSKEntities();
            //Copy file 
            string filesource, filedes;
            filesource = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\dm_tvien.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\dm_tvien.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            filesource = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\prgkb\giaytobay.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\giaytobay.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            filesource = @"\\10.100.8.30\foxapp\hldt\data\process.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\process.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            filesource = @"\\10.100.8.30\foxapp\hldt\data\cctv0.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\cctv0.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            //Danh sách tiếp viên
            FileStream fstv = new FileStream(@"F:\fox_app\Chuyenco\Data\dm_tvien.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable tv = Table.Open(fstv).AsDataTable();
            //Giấy tờ bay
            FileStream fsgtb = new FileStream(@"F:\fox_app\Chuyenco\Data\giaytobay.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable gtb = Table.Open(fsgtb).AsDataTable();
            ////Tổng hợp giờ bay
            //FileStream fsgb = new FileStream(@"f:\fox_app\chuyenco\dscuoi.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //DataTable gb = Table.Open(fsgb).AsDataTable();
            //Đào tạo
            FileStream fs = new FileStream(@"F:\fox_app\Chuyenco\Data\process.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //FileStream fs = new FileStream(@"f:\temp\process.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable hldt = Table.Open(fs).AsDataTable();
            //Hộ chiếu
            FileStream fs1 = new FileStream(@"F:\fox_app\Chuyenco\Data\cctv0.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable cctv0 = Table.Open(fs1).AsDataTable();

            //List<PView_toeic> diemtoeic;
            //diemtoeic = (from y in db.PView_toeics select y).ToList();

            List<View_Healthcare> dscc;
            dscc = (from cc in dbsk.View_Healthcare select cc).ToList();

            HoSoGoc hs;
            
            View_Healthcare ccsk;
            int i = 1;
            int dongtrong = 0;
            DateTime d1, d2;
            short manv;

            while (dongtrong < 20)
            {
                _manv = xlsheet.Cells[i, 2].value;
                if (_manv == null)
                {
                    dongtrong++;
                    i++;
                    continue;
                }
                if (!Int16.TryParse(_manv, out manv))
                {
                    dongtrong++;
                    i++;
                    continue;
                }
                dongtrong = 0;
                
                hs = db.HoSoGocs.Where(o => o.mans.Trim() == _manv).FirstOrDefault();
                
                //Chứng chỉ sức khỏe

                ccsk = dscc.Where(cc => cc.Code_tv == hs.mans.Trim() && cc.Expired != null).FirstOrDefault();
                if (ccsk != null)
                {
                    xlsheet.Cells[i, 12].Value = "'"+ccsk.Dotkham.ToString();
                    xlsheet.Cells[i, 13].Value = "'" + ccsk.Expired.ToString();
                    if (DateTime.Now > DateTime.Parse(ccsk.Dotkham.ToString()) && DateTime.Now <= DateTime.Parse(ccsk.Expired.ToString()))
                        xlsheet.Cells[i, 11].Value = "Đạt";
                    else 
                        xlsheet.Cells[i, 11].Value = "Không Đạt";    
                }
                //Hộ Chiếu
                var pp = (from v in tv.AsEnumerable()
                          where v.Field<string>("code_tv") == hs.mans.Trim()
                          select new
                          {
                              codetv = v.Field<string>("code_tv"),
                              sohc = v.Field<string>("pport_no"),
                              knb=v.Field<string>("on_plan")
                          }).FirstOrDefault();
                var hc = (from gt in gtb.AsEnumerable()
                          where (gt.Field<string>("loaigt") == "PAPT") && (gt.Field<string>("sogt") == pp.sohc) && (gt.Field<string>("code_tv") == hs.mans.Trim())
                          select new
                          {
                              sohc = gt.Field<string>("sogt"),
                              cap = gt.Field<string>("ngaycap"),
                              hethan = gt.Field<string>("ngayhh")
                          }).FirstOrDefault();

                if (hc != null)
                {
                    xlsheet.Cells[i, 17].Value = hc.sohc;
                    xlsheet.Cells[i, 18].Value = "'"+hc.cap;
                    xlsheet.Cells[i, 19].Value = "'" + hc.hethan;
                    xlsheet.Cells[i, 20].Value = pp.knb;   
                }               
                
                // Chứng chỉ An toàn bay
                var dsatb = (from rec in hldt.AsEnumerable()
                             where rec.Field<string>("objcode") == "REC" && rec.Field<string>("status") == "OK" && rec.Field<string>("Editstat") != "Delete" && rec.Field<string>("Editstat") != "Edit" && rec.Field<string>("paxcode") == hs.mans.Trim()
                                select rec).OrderByDescending(x => x.Field<DateTime>("testdate")).ToList();

                var atb = (from xyz in dsatb.OrderByDescending(x => x.Field<DateTime>("testdate")) select xyz).First();

                d1 = atb.Field<DateTime>("testdate");
                d2 = atb.Field<DateTime>("expiredate");
                xlsheet.Cells[i, 15].Value = "'" + d1.ToString("dd/MM/yyyy");
                xlsheet.Cells[i, 16].Value = "'" + d2.ToString("dd/MM/yyyy");
                if (DateTime.Now> d1 && DateTime.Now<=d2)
                    xlsheet.Cells[i, 14].Value = "Đạt";
                else
                    xlsheet.Cells[i, 14].Value = "Không Đạt";
                i++;
            }
            //Dong fiel excel
            xlbook.Save();
            xlbook.Close();
            xlapp.Quit();
            xlapp = null;
            
            Console.ReadLine();
            db.Dispose();
            dbsms.Dispose();
            dbsk.Dispose();
            fs.Dispose();
            fs1.Dispose();
            //fsgb.Dispose();
            fsgtb.Dispose();
            fstv.Dispose();
            GC.Collect();
            MessageBox.Show("Complete!");
        }

        private void chuyenCoWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog filediag = new OpenFileDialog();
            filediag.Filter = "Microsoft Word|*.docx;*.doc";
            filediag.ShowDialog();
            if (filediag.FileName.Trim() == "")
                return;

            HREntities db = new HREntities();
            SataHRMEntities dbsms = new SataHRMEntities();
            CCSKEntities dbsk = new CCSKEntities();
            string fileword,filesource, filedes,fileimg, f_mau = @"f:\fox_app\chuyenco\data\hoso.docx";
            fileword = @"F:\fox_app\Chuyenco\goc\Danh sach TV phuc vu A1 di Nga - Hungary-Ver1.doc";
            fileword = @"F:\fox_app\Chuyenco\goc\Ha 76 %2c Trang 105.doc";
            fileword = filediag.FileName.Trim();

            bool flag_dubi = true; //Nếu file word không có sanh sách dự bị thì cho flag_dubi=false
            flag_dubi = false;
            int num_tab_dstv, tab_hc;

            //if(flag_dubi){
            //    num_tab_dstv=2;
            //    tab_hc=4;
            //}
            //else {
            //    num_tab_dstv=1;
            //    tab_hc=3;
            //}
            //Copy file             
            filesource = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\dm_tvien.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\dm_tvien.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            filesource = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\prgkb\giaytobay.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\giaytobay.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            filesource = @"\\10.100.8.30\foxapp\hldt\data\process.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\process.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            filesource = @"\\10.100.8.30\foxapp\hldt\data\cctv0.dbf";
            filedes = @"F:\fox_app\Chuyenco\Data\cctv0.dbf";
            System.IO.File.Copy(filesource, filedes, true);

            //Danh sách tiếp viên
            FileStream fstv = new FileStream(@"F:\fox_app\Chuyenco\Data\dm_tvien.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable tv = Table.Open(fstv).AsDataTable();
            //Giấy tờ bay
            FileStream fsgtb = new FileStream(@"F:\fox_app\Chuyenco\Data\giaytobay.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable gtb = Table.Open(fsgtb).AsDataTable();
            //Tổng hợp giờ bay
            FileStream fsgb = new FileStream(@"f:\fox_app\chuyenco\dscuoi.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable gb = Table.Open(fsgb).AsDataTable();
            //Đào tạo
            FileStream fs = new FileStream(@"F:\fox_app\Chuyenco\Data\process.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //FileStream fs = new FileStream(@"f:\temp\process.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable hldt = Table.Open(fs).AsDataTable();
            //Hộ chiếu
            FileStream fs1 = new FileStream(@"F:\fox_app\Chuyenco\Data\cctv0.dbf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataTable cctv0 = Table.Open(fs1).AsDataTable();

            List<PView_toeic> diemtoeic;
            diemtoeic = (from y in db.PView_toeics select y).ToList();

            List<View_Healthcare> dscc;
            dscc = (from cc in dbsk.View_Healthcare select cc).ToList();

            HoSoGoc hs;
            danhmuc dm;
            chucvu cv;
            smsAddressBook contact;
            View_Healthcare ccsk;
            
            Word.Application wapp;
            Word.Document doc,oDoc;
            Word.Tables tables;
            Word.Table tab,tabimg;
            Word.Cell cell;
            wapp = new Word.Application();

            doc = wapp.Documents.Open(fileword);
            //var test =doc.Content.Find("Tiếp viên dự bị");
            //if (doc.Content.Text.IndexOf("Tiếp viên dự bị") > 0)
            //{
            //    num_tab_dstv = 2;
            //    tab_hc = 4;
            //}
            //else
            //{
            //    num_tab_dstv = 1;
            //    tab_hc = 3;
            //}
            wapp.Visible = true;
            tables = doc.Tables;
            tab = tables[2];
            
            if (tab.Cell(2, 3).Range.Text.Contains("DUYỆT CỦA PTGĐ DỊCH VỤ")) //Khong co danh sách tiếp viên dự bị
            {
                num_tab_dstv = 1;
                tab_hc = 3;
            }
            else
            {
                num_tab_dstv = 2;
                tab_hc = 4;
            }
            string _manv,f_hs,loaitau="787";
            for (int k=1; k<=num_tab_dstv;k++)
            {
                tab= tables[k];
                //Xác định loại tàu
                if (k == 1)
                {
                    loaitau=tab.Cell(1,9).Range.Text;
                }
                for (int i = 2; i <= tab.Rows.Count; i++)
                {
                    cell = tab.Cell(i, 3);
                    _manv = cell.Range.Text;
                    if (_manv.Length < 4)
                        continue;
                    _manv = _manv.Trim().Substring(0, 4);
#region xu_ly_tung_nguoi

                    //oApp.Visible = true;
                    hs = db.HoSoGocs.Where(o => o.mans.Trim() == _manv).FirstOrDefault();
                    f_hs = @"f:\fox_app\chuyenco\hoso\" + hs.Tenkd.Trim() + ".docx";
                    fileimg = @"\\10.105.2.243\TaiLieu\Photo\HinhTheTV\profiles\"+hs.mans.Trim()+".jpg";
                    File.Copy(f_mau, f_hs, true);
                    oDoc = wapp.Documents.Open(f_hs);

                    if (File.Exists(fileimg))
                    {
                        tabimg = oDoc.Tables[1];
                        Word.InlineShape shape;
                        shape=tabimg.Cell(1, 1).Range.InlineShapes.AddPicture(fileimg);
                        shape.Height = 172; // 6.05f;
                        shape.Width = 119; // 4.2f;
                        //tab.Cell(1,1).Range.Paragraphs[1].AppendPicture(Image.FromFile(fileimg))
                    }
                    
                    ((Word.FormField)oDoc.FormFields.get_Item("txtHovaTen")).Result = hs.ns_ho.Trim() + " " + hs.ns_ten.Trim();
                    oDoc.FormFields.get_Item("txtstt").Result = hs.ns_stt;
                    oDoc.FormFields.get_Item("txtNgaysinh").Result = hs.ngaysinh.ToString("dd/MM/yyyy");
                    dm = db.danhmucs.Where(m => m.id == hs.quequan_tinhtp).FirstOrDefault();
                    if (dm.TenDanhMuc.Trim() != "Không rõ")
                        oDoc.FormFields.get_Item("txtQuequan").Result = hs.quequan_dc.Trim() + ", " + dm.TenDanhMuc.Trim();
                    else
                        oDoc.FormFields.get_Item("txtQuequan").Result = hs.quequan_dc.Trim();
                    dm = db.danhmucs.Where(m => m.id == hs.noio_tinhtp).FirstOrDefault();
                    if (dm.TenDanhMuc.Trim() != "Không rõ")
                        oDoc.FormFields.get_Item("TxtDiachi").Result = hs.noio_dc + ", " + dm.TenDanhMuc.Trim();
                    else
                        oDoc.FormFields.get_Item("TxtDiachi").Result = hs.noio_dc;

                    contact = dbsms.smsAddressBooks.Where(obj => obj.ContactCode.Trim() == _manv).FirstOrDefault();
                    oDoc.FormFields.get_Item("txtDienthoai").Result = contact.MobilePhone;

                    cv = db.chucvus.Where(v => v.id_ns == hs.id).FirstOrDefault();
                    dm = db.danhmucs.Where(m => m.id == cv.chucvu1).FirstOrDefault();
                    oDoc.FormFields.get_Item("txtChucvucq").Result = dm.TenDanhMuc;


                    if (hs.dang_ngaykn != null)
                    {
                        dm = db.danhmucs.Where(m => m.id == hs.dang_chucvu).FirstOrDefault();
                        if (dm != null)
                        {
                            oDoc.FormFields.get_Item("txtChucvuDang").Result = dm.TenDanhMuc.Trim();
                        }
                        else
                        {
                            oDoc.FormFields.get_Item("txtChucvuDang").Result = "";

                        }
                        oDoc.FormFields.get_Item("txtNgayvaodang").Result = hs.dang_ngaykn.ToString();
                    }

                    dm = db.danhmucs.Where(m => m.id == hs.bophanlamviec).FirstOrDefault();
                    oDoc.FormFields.get_Item("txtBophan").Result = dm.TenDanhMuc;

                    oDoc.FormFields.get_Item("txtNgayvn").Result = hs.bienche_tct.ToString();

                    dm = db.danhmucs.Where(m => m.id == hs.hocvantd).FirstOrDefault();
                    if (dm.TenDanhMuc.Trim() != "Không rõ")
                        oDoc.FormFields.get_Item("txtTrinhdo").Result = dm.TenDanhMuc.Trim();

                    DateTime dt1 = DateTime.Now;
                    DateTime dt2 = dt1.AddMonths(-12);

                    var ktkl = from kk in db.khenkluats
                               where (kk.kyluat == false) && (kk.ktkl_ngayqd <= dt1) && (kk.ktkl_ngayqd >= dt2) && (kk.id_ns == hs.id) && (kk.ktkl_hinhthuc != null)
                               select kk;
                    int solan = 0, sl, nht;
                    string kq = "", kq1, kq2, ht = "";
                    foreach (khenkluat kl in ktkl)
                    {
                        solan++;
                        nht = (int)kl.ktkl_hinhthuc;
                        if (nht != 3677 && nht != 3679 && nht != 3687 && nht != 4020)
                        {
                            ht = kl.ktkl_hinhthuc.ToString().Trim();
                            if (kq.Contains(ht))
                            {
                                kq1 = kq.Substring(0, kq.IndexOf(ht) + ht.Length + 1);
                                kq2 = kq.Substring(kq.IndexOf(ht) + ht.Length + 1);
                                sl = Int16.Parse(kq2.Substring(0, kq2.IndexOf(';')));
                                kq = kq1 + (sl + 1).ToString().Trim() + ";" + kq2.Substring(kq2.IndexOf(';') + 1);
                            }
                            else
                            {
                                if (kq == "")
                                {
                                    kq = ht + ":1;";
                                }
                                else
                                {
                                    kq = kq.Trim() + ht + ":1;";
                                }

                            }
                        }

                    }//foreach
                    if (solan > 0)
                    {
                        kq1 = solan.ToString().Trim() + " (";
                        kq2 = "";
                        while (kq != "")
                        {
                            nht = kq.IndexOf(':');
                            ht = kq.Substring(0, nht); //hình thức
                            kq = kq.Substring(nht + 1);
                            nht = kq.IndexOf(';');
                            kq2 = kq.Substring(0, nht); //số lượng
                            kq = kq.Substring(nht + 1);
                            solan = Int16.Parse(ht);
                            dm = db.danhmucs.Where(m => m.id == solan).FirstOrDefault();
                            if (kq1.EndsWith("("))
                            {
                                kq1 = kq1.Trim() + kq2 + " " + dm.TenDanhMuc;
                            }
                            else
                            {
                                kq1 = kq1.Trim() + "+" + kq2 + " " + dm.TenDanhMuc;
                            }

                        }
                        kq1 = kq1.Trim() + ")";
                        oDoc.FormFields.get_Item("txtKT").Result = kq1;
                    }
                    else
                    {
                        oDoc.FormFields.get_Item("txtKT").Result = "";
                    }
                    ///Phan ky luat
                    ktkl = from kk in db.khenkluats
                           where (kk.kyluat == true) && (kk.ktkl_ngayqd <= dt1) && (kk.ktkl_ngayqd >= dt2) && (kk.id_ns == hs.id) && (kk.ktkl_hinhthuc != null)
                           select kk;
                    solan = 0;
                    foreach (khenkluat kl in ktkl)
                    {
                       
                        nht = (int)kl.ktkl_hinhthuc;
                        if (nht != 3677 && nht != 3679 && nht != 3687 && nht != 4020)
                        {
                            solan++;
                            ht = kl.ktkl_hinhthuc.ToString().Trim();
                            if (kq.Contains(ht))
                            {
                                kq1 = kq.Substring(0, kq.IndexOf(ht) + ht.Length + 1);
                                kq2 = kq.Substring(kq.IndexOf(ht) + ht.Length + 1);
                                sl = Int16.Parse(kq2.Substring(0, kq2.IndexOf(';')));
                                kq = kq1 + (sl + 1).ToString().Trim() + ";" + kq2.Substring(kq2.IndexOf(';') + 1);
                            }
                            else
                            {
                                if (kq == "")
                                {
                                    kq = ht + ":1;";
                                }
                                else
                                {
                                    kq = kq.Trim() + ht + ":1;";
                                }

                            }
                        }

                    }//foreach
                    if (solan > 0)
                    {
                        kq1 = solan.ToString().Trim() + " (";
                        kq2 = "";
                        while (kq != "")
                        {
                            nht = kq.IndexOf(':');
                            ht = kq.Substring(0, nht); //hình thức
                            kq = kq.Substring(nht + 1);
                            nht = kq.IndexOf(';');
                            kq2 = kq.Substring(0, nht); //số lượng
                            kq = kq.Substring(nht + 1);
                            solan = Int16.Parse(ht);
                            dm = db.danhmucs.Where(m => m.id == solan).FirstOrDefault();
                            if (kq1.EndsWith("("))
                            {
                                kq1 = kq1.Trim() + kq2 + " " + dm.TenDanhMuc;
                            }
                            else
                            {
                                kq1 = kq1.Trim() + "+" + kq2 + " " + dm.TenDanhMuc;
                            }

                        }
                        kq1 = kq1.Trim() + ")";
                        oDoc.FormFields.get_Item("txtKL").Result = kq1;
                    }
                    else
                    {
                        oDoc.FormFields.get_Item("txtKL").Result = "Không có";
                    }
                    //Chứng chỉ sức khỏe


                    ccsk = dscc.Where(cc => cc.Code_tv == hs.mans.Trim() && cc.Expired != null).FirstOrDefault();
                    if (ccsk != null)
                    {
                        oDoc.FormFields.get_Item("txtLoaiSK").Result = "Nhóm 2";
                        oDoc.FormFields.get_Item("txtCCSKCap").Result = ccsk.Dotkham.ToString();
                        oDoc.FormFields.get_Item("txtCCSKHH").Result = ccsk.Expired.ToString();
                    }
                    //Hộ Chiếu
                    var pp = (from v in tv.AsEnumerable()
                              where v.Field<string>("code_tv") == hs.mans.Trim()
                              select new
                              {
                                  codetv = v.Field<string>("code_tv"),
                                  sohc = v.Field<string>("pport_no")
                              }).FirstOrDefault();
                    var hc = (from gt in gtb.AsEnumerable()
                              where (gt.Field<string>("loaigt") == "PAPT") && (gt.Field<string>("sogt") == pp.sohc) && (gt.Field<string>("code_tv") == hs.mans.Trim())
                              select new
                              {
                                  sohc = gt.Field<string>("sogt"),
                                  cap = gt.Field<string>("ngaycap"),
                                  hethan = gt.Field<string>("ngayhh")
                              }).FirstOrDefault();

                    oDoc.FormFields.get_Item("txtHochieu").Result = hc.sohc;
                    oDoc.FormFields.get_Item("txtHCCap").Result = hc.cap;
                    oDoc.FormFields.get_Item("txtHCHH").Result = hc.hethan;
                    // Chứng chỉ An toàn bay

                    var atb = (from rec in hldt.AsEnumerable()
                               where rec.Field<string>("objcode") == "REC" && rec.Field<string>("status") == "OK" && rec.Field<string>("Editstat") != "Delete" && rec.Field<string>("paxcode") == hs.mans.Trim()
                               select rec).OrderByDescending(x => x.Field<DateTime>("testdate")).First();

                    oDoc.FormFields.get_Item("txtDtCap").Result = atb.Field<DateTime>("testdate").ToString("dd/MM/yyyy");
                    oDoc.FormFields.get_Item("txtDtHH").Result = atb.Field<DateTime>("expiredate").ToString("dd/MM/yyyy");
                    //Phần ghi lên file gốc                    
                    var gbtv = (from pgb in gb.AsEnumerable()
                                where pgb.Field<string>("manv") == hs.mans.Trim()
                                select new
                                {
                                    a321 = pgb.Field<decimal>("f_321") / 60,
                                    a350 = pgb.Field<decimal>("f_350") / 60,
                                    b787 = pgb.Field<decimal>("f_787") / 60,
                                    tonggb = pgb.Field<decimal>("tong") / 60


                                }).FirstOrDefault();
                    tab.Cell(i, 8).Range.Text = gbtv.tonggb.ToString("#,##0");
                    if (loaitau.Contains("787"))
                        tab.Cell(i, 9).Range.Text = gbtv.b787.ToString("#,##0");
                    else 
                        if(loaitau.Contains("350"))
                            tab.Cell(i, 9).Range.Text = gbtv.a350.ToString("#,##0");
                        else 
                            tab.Cell(i, 9).Range.Text = gbtv.a321.ToString("#,##0");

                    var toeic = (from av in diemtoeic
                                 where av.manv == hs.mans.Trim()
                                 select av
                                 ).FirstOrDefault();

                    tab.Cell(i, 10).Range.Text = toeic.diem.ToString();
                    oDoc.Save();
                    oDoc.Close();
#endregion
                    
                }
            }
            tab = doc.Tables[tab_hc];
            for (int i = 2; i <= tab.Rows.Count; i++)
            {
                cell = tab.Cell(i, 3);
                _manv = cell.Range.Text;
                if (_manv.Length < 4)
                    continue;
                _manv = _manv.Trim().Substring(0, 4);
                hs = db.HoSoGocs.Where(o => o.mans.Trim() == _manv).FirstOrDefault();
                var pp = (from v in tv.AsEnumerable()
                          where v.Field<string>("code_tv") == hs.mans.Trim()
                          select new
                          {
                              codetv = v.Field<string>("code_tv"),
                              sohc = v.Field<string>("pport_no")
                          }).FirstOrDefault();
                var hc = (from gt in gtb.AsEnumerable()
                          where (gt.Field<string>("loaigt") == "PAPT") && (gt.Field<string>("sogt") == pp.sohc) && (gt.Field<string>("code_tv") == hs.mans.Trim())
                          select new
                          {
                              sohc = gt.Field<string>("sogt"),
                              cap = gt.Field<string>("ngaycap"),
                              hethan = gt.Field<string>("ngayhh")
                          }).FirstOrDefault();

                tab.Cell(i, 5).Range.Text = hs.ngaysinh.ToString("dd/MM/yyyy");
                tab.Cell(i, 6).Range.Text = hc.sohc;
                tab.Cell(i, 7).Range.Text = hc.cap;
                tab.Cell(i, 8).Range.Text = hc.hethan;
                
                try
                {
                    var hccv = (from pcv in cctv0.AsEnumerable()
                                where pcv.Field<string>("manv") == hs.mans.Trim()
                                select new
                                {
                                    socv = pcv.Field<string>("pportno"),
                                    cvcap = pcv.Field<DateTime>("isspport"),
                                    cvhh = pcv.Field<DateTime>("exppport")
                                }).FirstOrDefault();

                    tab.Cell(i, 9).Range.Text = hccv.socv;
                    tab.Cell(i, 10).Range.Text = hccv.cvcap.ToString("dd/MM/yyyy");
                    tab.Cell(i, 11).Range.Text = hccv.cvhh.ToString("dd/MM/yyyy");
                }
                catch (Exception ex)
                {

                }
            }
            doc.Save();
            doc.Close();
            wapp.Quit();
            db.Dispose();
            dbsms.Dispose();
            dbsk.Dispose();
            fs.Dispose();
            fs1.Dispose();
            fsgb.Dispose();
            fsgtb.Dispose();
            fstv.Dispose();
            GC.Collect();
            MessageBox.Show("Complete!");
        }

        private void bSInfoDCBCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application xlapp = new Excel.Application();
            Excel.Workbook xlbook;
            Excel.Worksheet xlsheet;
            xlbook = xlapp.Workbooks.Add();
            xlsheet = xlbook.Sheets[1];
            xlsheet.Cells[1, 1].value = "STT";
            xlsheet.Cells[1, 2].value = "Manv";
            xlsheet.Cells[1, 3].value = "Họ và Tên";
            xlsheet.Cells[1, 4].value = "Địa chỉ";
            xlsheet.Cells[1, 5].value = "Địa chỉ tỉnh/thành";
            xlsheet.Cells[1, 6].value = "Quê quán";
            xlsheet.Cells[1, 7].value = "Quê tỉnh/thành";
            xlsheet.Cells[1, 8].value = "Trình độ học vấn";
            int i = 1;
            bool flag;
            using (HREntities db = new HREntities())
            {
                var ds = db.HoSoGocs.Where(x => x.nghiviec == false).ToList();
                foreach (var nv in ds)
                {
                    flag = false;
                    var dctp = db.danhmucs.Where(z => z.id == nv.ttru_tinhtp).FirstOrDefault();
                    var qqtp = db.danhmucs.Where(y => y.id == nv.quequan_tinhtp).FirstOrDefault();
                    var hv = db.danhmucs.Where(k => k.id == nv.hocvantd).FirstOrDefault();
                    if (dctp.TenDanhMuc.Trim() == "Không rõ" || qqtp.TenDanhMuc.Trim() == "Không rõ" || hv.TenDanhMuc.Trim() == "Không rõ")                    
                    {
                        if (!flag)
                        {
                            i++;
                            xlsheet.Cells[i, 1].value = i - 1;
                            xlsheet.Cells[i, 2].value = "'"+nv.mans.Trim();
                            xlsheet.Cells[i, 3].value = nv.ns_ho.Trim()+" "+nv.ns_ten.Trim()+" "+nv.ns_stt.Trim();                            
                        }
                        xlsheet.Cells[i, 4].value = nv.ttru_dc;
                        xlsheet.Cells[i, 5].value = dctp.TenDanhMuc.Trim();
                        xlsheet.Cells[i, 6].value = nv.quequan_dc;
                        xlsheet.Cells[i, 7].value = qqtp.TenDanhMuc.Trim();
                        xlsheet.Cells[i, 8].value = hv.TenDanhMuc.Trim();
                        
                    }
                   
                }
            }
            xlapp.Visible = true;
            MessageBox.Show("Complete!");
        }

        private void testDEVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XtraForm1 frm = new XtraForm1();
            frm.Show(); 
        }

        private void getCrewFLightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CrewActivityRoster cr = AVES.CrewActivityRoster
            //CrewPlanServiceClient client = new CrewPlanServiceClient();
            //CrewActivityRoster cr = client.GetCrewOnFlight("VN54", new DateTime(2018, 7, 7),"HAN", AVES.ProfessionCode.C );
            MessageBox.Show("Test");
        }

        private void getToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xfrxgetDeposit frxdutytfree = new xfrxgetDeposit();
            frxdutytfree.Show();
        }

        private void xétHDLDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmXetHDLD frmhdld = new frmXetHDLD();
            frmhdld.Show();
        }

        private void khenThưởngKỷLuậtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGetKTKL frmktkl = new frmGetKTKL();
            frmktkl.Show();
        }

        private void tiếngAnhTrinhĐộToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongke frmtk = new frmThongke();
            frmtk.Show();
        }

        private void vNCrexHuyConTVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVNCrewFlightDelete frmFltDel = new frmVNCrewFlightDelete();
            frmFltDel.Show();
        }

        private void vNCrewDDKTKhongTVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVNCrewNoCrew frmcheck = new frmVNCrewNoCrew();
            frmcheck.Show();
        }
       
    }
}
