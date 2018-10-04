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
using Cabincrew.Model.ERMS;
using Cabincrew.Model.DDKT;
using System.IO;
using DevExpress.Spreadsheet;

namespace Cabincrew.Forms
{
    public partial class frmVNCrewFlightDelete : Cabincrew.Forms.shareform
    {
        public frmVNCrewFlightDelete()
        {
            InitializeComponent();
        }

        private void frmVNCrewFlightDelete_Load(object sender, EventArgs e)
        {
            base.sbtnLoadExcel.Visible = false;
            base.textEdit1.Visible = false;
            this.Text = "Kiểm tra các chuyến bay bị Hủy trên VNCrew";
        }

        private void sbtnCheck_Click(object sender, EventArgs e)
        {
            List<CR_FlightInfo> flightdel = new List<CR_FlightInfo>();
            string f_lbaytv2 = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\lbaytv2.dbf";
            string f_des = @"c:\temp\lbaytv2.dbf";
            DateTime ngaymin = new DateTime(2018, 09, 01);
            DateTime ngaymax = new DateTime(2018, 09, 30);
            List<lbaytv2> lbtv = new List<lbaytv2>();
            try {
                using (ERMSEntities1 db = new ERMSEntities1())
                {
                    flightdel = db.CR_FlightInfo.Where(x => x.IsDeleted == true && x.Date>=ngaymin && x.Date<=ngaymax).ToList();
                }
                File.Copy(f_lbaytv2, f_des, true);
                FileStream flbtv2 = new FileStream(f_des, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                NDbfReader.Table table = NDbfReader.Table.Open(flbtv2);
                NDbfReader.Reader readder = table.OpenReader();
                while (readder.Read())
                {
                    
                    string codenv = readder.GetString("CODE_TV");
                    DateTime? sdate = readder.GetDateTime("START_DATE");
                    DateTime? edate = readder.GetDateTime("END_DATE");
                    string loai = readder.GetString("LOAI");
                    string flyno = readder.GetString("FLY_NO");
                    string cfg = readder.GetString("CFG");
                    string from = readder.GetString("FROM_PLACE");
                    string to = readder.GetString("END_PLACE");

                    if (sdate == null || edate == null)
                        continue;                    
                    if (ngaymin > edate || ngaymax < sdate)
                        continue;
                    if (codenv == null) //Không có Code_tv ==> Bỏ qua
                        continue;
                    if (loai != "FLY" || flyno==null || flyno.Substring(0,1)=="R") //Không phải bay hoặc dự bị bỏ qua
                        continue;

                    var cb = flightdel.Where(x => x.Date == sdate && x.FlightNo == flyno && x.Routing == from + "-" + to).FirstOrDefault();
                    if (cb!=null)
                    {
                        lbaytv2 item = new lbaytv2();
                        item.flightID = cb.FlightID;
                        item.code_tv = codenv;
                        item.start_date = (DateTime)sdate;
                        item.end_date = (DateTime)edate;
                        item.start_time = readder.GetString("START_TIME");
                        item.end_time = readder.GetString("END_TIME");
                        item.fly_no = flyno;
                        item.from_place = from;
                        item.end_place = to;                        
                        item.cfg = cfg;
                        item.loai = loai;
                        item.note = readder.GetString("NOTE");
                        lbtv.Add(item);
                    } //if                  
                }//while
                flbtv2.Dispose();
                if (flightdel.Count > 0) //Ghi kết quả
                {
                    Worksheet spreadst = base.spreadsheetControl1.ActiveWorksheet;
                    spreadst.Cells[0, 0].Value = "STT";
                    spreadst.Cells[0, 1].Value = "Date";
                    spreadst.Cells[0, 2].Value = "Flyno";
                    spreadst.Cells[0, 3].Value = "Routing";
                    spreadst.Cells[0, 4].Value = "Codetv";
                    int stt=1,dong=1;
                    foreach (var cb in flightdel)
                    {
                        spreadst.Cells[dong, 0].Value = stt;
                        spreadst.Cells[dong, 1].Value = cb.Date;
                        spreadst.Cells[dong, 2].Value = cb.FlightNo;
                        spreadst.Cells[dong, 3].Value = cb.Routing;
                        var totv = lbtv.Where(x => x.flightID == cb.FlightID).ToList();
                        if (totv.Count>0)
                        {
                            foreach (var tv in totv)
                            {
                                spreadst.Cells[dong, 4].Value = "'" + tv.code_tv;
                                dong++;
                            }
                        }
                        else
                            dong++;
                        stt++;
                    }
                }
                MessageBox.Show("Complete!");
            }
            catch (Exception ex) { }
        }
    }
    class flight
    {
        public int flightID { get; set; }
        public DateTime flydate { get; set; }
        public string flightno { get; set; }
        public string sector { get; set; }
    }
    //class lbaytv2
    //{
    //    public int flightID { get; set; }
    //    public string manv { get; set; }
    //    public string flyno { get; set; }
    //    public string sector { get; set; }
    //    public DateTime start_date { get; set; }
    //    public string start_time { get; set; }
    //    public DateTime end_date { get; set; }
    //    public string end_time { get; set; }
    //    public string cfg { get; set; }
    //    public string loai { get; set; }
    //    public string note { get; set; }
    //}
}