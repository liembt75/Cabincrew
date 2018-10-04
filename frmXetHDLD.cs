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
using DevExpress.Spreadsheet;
using Cabincrew.Model.HR;
using System.IO;
using NDbfReader;
using Cabincrew.Utils;
namespace Cabincrew
{
    public partial class frmXetHDLD : DevExpress.XtraEditors.XtraForm
    {
        public frmXetHDLD()
        {
            InitializeComponent();
        }

        private void frmXetHDLD_Load(object sender, EventArgs e)
        {
            spreadsheetControl1.Width = this.Width;
            spreadsheetControl1.Height = this.Height - 100;
            textEdit1.Width = this.Width - sbtnLoadExcel.Width-30;
        }

        private void frmXetHDLD_SizeChanged(object sender, EventArgs e)
        {
            textEdit1.Width = this.Width - sbtnLoadExcel.Width - 30;
            spreadsheetControl1.Width = this.Width;
            if (this.Height > 100)
                spreadsheetControl1.Height = this.Height - 100;
            else
                spreadsheetControl1.Height = this.Height;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<hopdong> dshd = new List<hopdong>();
            Worksheet hhhd = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
            SearchOptions option = new SearchOptions();
            option.SearchBy = SearchBy.Columns;
            option.SearchIn = SearchIn.Values;
            option.MatchEntireCellContents = true;
            option.MatchCase = false;
            IEnumerable<Cell> searchResult;
            //Tìm Mã nhân viên
            searchResult = hhhd.Search("Mã nhân sự", option);
            if (searchResult.Count() == 0)
            {
                MessageBox.Show("Không tìm thấy cột Mã nhân sự");
                return;
            }
            int colmanv = searchResult.First().LeftColumnIndex;
            int dongmanv = searchResult.First().TopRowIndex;
            //Tìm Ngày hiệu lực HĐ
            searchResult = hhhd.Search("Ngày hiệu lực HĐ", option);
            if (searchResult.Count() == 0)
            {
                MessageBox.Show("Không tìm thấy cột Ngày hiệu lực HĐ");
                return;
            }
            int colngayky = searchResult.First().LeftColumnIndex;
            //Tìm Ngày hết hiệu lực HĐ
            searchResult = hhhd.Search("Ngày hết hiệu lực HĐ", option);
            if (searchResult.Count() == 0)
            {
                MessageBox.Show("Không tìm thấy cột Ngày hết hiệu lực HĐ");
                return;
            }
            int colngayhh = searchResult.First().LeftColumnIndex;
            //Tìm cột cuối cùng có giá trị. Nếu sau 5 cột liên tiếp không có thì coi là cột cuối
            int k = colngayhh + 1;            
            while (true){
                if(hhhd.Cells[dongmanv, k].Value.IsEmpty && hhhd.Cells[dongmanv, k+1].Value.IsEmpty && hhhd.Cells[dongmanv, k+2].Value.IsEmpty && hhhd.Cells[dongmanv, k+3].Value.IsEmpty && hhhd.Cells[dongmanv, k+4].Value.IsEmpty)
                    break;
                k++;
            }
            //Các cột mới
            int colToeic = k,colKhenthuong=k+2,colKyluat=k+3;
            hhhd.Cells[dongmanv, colToeic].Value = "Ngoại ngữ";
            hhhd.Cells[dongmanv, colKhenthuong].Value = "Khen thưởng";
            hhhd.Cells[dongmanv, colKyluat].Value = "Kỷ luật";
            //Lấy danh sách người hết hạn
#region Lấy danh sách hết hạn
            int i, blank,maso;
            string codetv;
            DateTime ky,hethan;
            i = 0;
            blank = 0;
            while (true)
            {
                if (blank > 10)
                    break;

                //Không có Manv coi như dòng trống
                if (hhhd.Cells[i, colmanv].Value.IsEmpty)
                {
                    i++;
                    blank++;
                    continue;
                }
                //Manv không phải là số coi như dòng tróng
                if (!hhhd.Cells[i, colmanv].Value.IsNumeric && !hhhd.Cells[i,colmanv].Value.IsText)
                {
                    i++;
                    blank++;
                    continue;
                }
                //Manv không phải dạng số bỏ qua
                if (!Int32.TryParse(hhhd.Cells[i, colmanv].Value.ToString(),out maso))
                {
                    i++;
                    blank++;
                    continue;
                }
                blank = 0;
                codetv = hhhd.Cells[i, colmanv].Value.ToString();
                ky=Convert.ToDateTime(hhhd.Cells[i,colngayky].Value.ToString());
                hethan=Convert.ToDateTime(hhhd.Cells[i,colngayhh].Value.ToString());
                hopdong onetv = new hopdong();
                onetv.manv = codetv;
                onetv.ngayky = ky;
                onetv.ngayhh = hethan;
                onetv.dong = i;
                dshd.Add(onetv);
                i++;
            }
#endregion 
            #region Lấy lịch khác bay
            List<lichbaycn> lbtv = new List<lichbaycn>();
            List<lichkhacbay> dscode = new List<lichkhacbay>();
            DateTime ngaymin, ngaymax;
            int socot = colKyluat + 1;
            ngaymax = dshd.Max(x => x.ngayhh);
            ngaymin = dshd.Min(x => x.ngayky);
            string f_lbaytv2=@"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\lbaytv2.dbf";
            string f_des=@"c:\temp\lbaytv2.dbf";
            string f_dmctmd = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\tudien\dm_ctmd.dbf";
            string f_dm = @"c:\temp\dm_ctmd.dbf";
            File.Copy(f_lbaytv2, f_des, true);
            File.Copy(f_dmctmd, f_dm, true);
            FileStream flbtv2 = new FileStream(f_des, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream fdm = new FileStream(f_dm,FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            NDbfReader.Table tbldm = NDbfReader.Table.Open(fdm);
            NDbfReader.Reader rddm = tbldm.OpenReader(Encoding.GetEncoding(1252));
            
            while (rddm.Read())
            {
                if (rddm.GetString("NHOMTK")=="NGHI")
                {
                    lichkhacbay kb = new lichkhacbay();
                    kb.loai =rddm.GetString("LOAI");
                    kb.ten = Utils.Utils.TCVN3ToUnicode(rddm.GetString("TEN_LOAI"));
                    dscode.Add(kb);                    
                }
            }           
            NDbfReader.Table table = NDbfReader.Table.Open(flbtv2);
            NDbfReader.Reader readder = table.OpenReader();
            while (readder.Read())
            {
                string codenv = readder.GetString("CODE_TV");
                DateTime? sdate = readder.GetDateTime("START_DATE");
                DateTime? edate = readder.GetDateTime("END_DATE");
                string loai = readder.GetString("LOAI");                
                if (codenv == null) //Không có Code_tv ==> Bỏ qua
                    continue;
                if (!dshd.Any(x => x.manv == codenv)) //Không thuộc danh sách ==> bỏ qua
                    continue;
                
                if (sdate == null || edate == null || edate < ngaymin || sdate > ngaymax) //
                    continue;
                if (loai == null || loai == "FLY" || loai=="NKTT") //Trống field Loai hoặc Loai là FLY, NKTT (không có trong DM_CTMD) ==> bỏ qua
                    continue;
                //Loai không phải nghỉ ==> Bỏ luôn
                var tendm = dscode.Where(x => x.loai==loai).FirstOrDefault();
                if(tendm==null)
                    continue;
                
                lichbaycn item = new lichbaycn();
                item.manv = codenv;
                item.start_date = (DateTime)sdate;
                item.end_date = (DateTime)edate;
                item.loai = loai;
                item.note = readder.GetString("NOTE");
                lbtv.Add(item);
                var codekb = dscode.Where(x => x.loai == loai).FirstOrDefault();
                if (codekb != null && codekb.cot==0)
                {
                    codekb.cot = socot;
                    hhhd.Cells[dongmanv, socot].Value = codekb.ten;
                    socot++;
                }
            }
            flbtv2.Dispose();
            fdm.Dispose();            
            #endregion 
            #region Lấy thông tin khen thưởng kỷ luật
            string kq="";
            using (HREntities hr = new HREntities())
            {
                foreach (var tv in dshd)
                {
                    
                    var ns = hr.HoSoGocs.Where(x => x.mans.Trim() == tv.manv).First();
                    //TOEIC
                    var toeic = hr.ngoaingus.Where(x => x.id_ns == ns.id && x.ngoaingu_loai==565 && (x.ngoaingu_bangcap==669 || x.ngoaingu_bangcap==670 || x.ngoaingu_bangcap==671)).OrderByDescending(y=>y.ngoaingu_ngaycap).FirstOrDefault();
                    if(toeic!=null)
                    { 
                        hhhd.Cells[tv.dong, colToeic].Value = toeic.ngoaingu_diemtong;
                        if(toeic.ngoaingu_bangcap==669)
                            hhhd.Cells[tv.dong, colToeic+1].Value = "TOEIC";
                        else 
                            if(toeic.ngoaingu_bangcap==670)
                                hhhd.Cells[tv.dong, colToeic + 1].Value = "TOEFL";
                            else
                                hhhd.Cells[tv.dong, colToeic + 1].Value = "IELTS";

                    }
                    //Khen thưởng
                    var khenthuongs = hr.khenkluats.Where(x => x.id_ns == ns.id && x.kyluat == false && x.ktkl_ngayqd>=tv.ngayky && x.ktkl_ngayqd<=tv.ngayhh).ToList();
                    kq = "";
                    var dskt = khenthuongs
                                .GroupBy(kt => kt.ktkl_hinhthuc)
                                .Select(kt => new
                                {
                                    hinhthuc = kt.Key,
                                    solan = kt.Count()
                                });
                    foreach (var kt in dskt)
                    {
                        kq += hr.danhmucs.Where(x => x.id == kt.hinhthuc).FirstOrDefault().TenDanhMuc.Trim() + ":" + kt.solan.ToString() + ";";
                    }
                    hhhd.Cells[tv.dong, colKhenthuong].Value = kq;
                    //Kỷ luật
                    var kyluats = hr.khenkluats.Where(x => x.id_ns == ns.id && x.kyluat == true && x.ktkl_hinhthuc!=3677 && x.ktkl_hinhthuc!=3679 && x.ktkl_hinhthuc!=3687 && x.ktkl_hinhthuc!=4020 && x.ktkl_hinhthuc!=null && x.ktkl_ngayqd >= tv.ngayky && x.ktkl_ngayqd <= tv.ngayhh).ToList();
                    kq = "";
                    var dskl = kyluats
                                .GroupBy(kt => kt.ktkl_hinhthuc)
                                .Select(kt => new
                                {
                                    hinhthuc = kt.Key,
                                    solan = kt.Count()
                                });
                    foreach (var kl in dskl)
                    {
                        kq += hr.danhmucs.Where(x => x.id == kl.hinhthuc).FirstOrDefault().TenDanhMuc.Trim() + ":" + kl.solan.ToString() + ";";
                    }
                    hhhd.Cells[tv.dong, colKyluat].Value = kq;
                    //Lịch khác bay
                    //Tính số ngày nghỉ từng loại. 
                    DateTime tungay;
                    var lbcn = lbtv.Where(x => x.manv == tv.manv).ToList();
                    List<ctlkb> lbct = new List<ctlkb>();
                    foreach (var lich in lbcn)
                    {
                        tungay = lich.start_date;
                        while (tungay <= lich.end_date)
                        {
                            if (!lbct.Any(x => x.ngay == tungay) && tungay>=tv.ngayky && tungay<=tv.ngayhh)
                            {
                                ctlkb newlct = new ctlkb();
                                newlct.ngay = tungay;
                                newlct.loai = lich.loai;
                                lbct.Add(newlct);
                            }
                            tungay = tungay.AddDays(1);
                        }
                    }
                    //Tổng hợp các loại 
                    var kqcuoi = lbct
                               .GroupBy(x => x.loai)
                               .Select(x => new
                               {
                                   loai = x.Key,
                                   songay = x.Count()
                               });
                    //Ghi kết quả
                    foreach (var xkq in kqcuoi)
                    {
                        var xcot = dscode.Where(x => x.loai == xkq.loai).FirstOrDefault();
                        hhhd.Cells[tv.dong, xcot.cot].Value = xkq.songay;
                    }
                    
                    
                }
            }
#endregion 
            
            MessageBox.Show("Complete!");
        }

        private void sbtnLoadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "Microsoft Excel|*.xlsx;*.xls";
            diag.ShowDialog();
            string filename = diag.FileName;
            if (filename != "")
            {
                this.textEdit1.Text = filename;
                spreadsheetControl1.Document.LoadDocument(filename);
            }
                
        }    

       
    }
    class hopdong
    {
        public string manv { get; set; }
        public DateTime ngayky { get; set; }
        public DateTime ngayhh { get; set; }
        public int dong { get; set; }
    }
    class lichbaycn
    {
        public string manv { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string loai { get; set; }
        public string note { get; set; }
    }
    class lichkhacbay
    {
        public string loai { get; set; }
        public string ten { get; set; }
        public int cot { get; set; }
    }
    struct ctlkb
    {
        public string loai { get; set; }
        public DateTime ngay { get; set; }
        public string status { get; set; }
    }
}