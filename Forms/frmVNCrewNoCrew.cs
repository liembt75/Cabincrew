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

namespace Cabincrew.Forms
{
    public partial class frmVNCrewNoCrew : Cabincrew.Forms.shareform
    {
        public frmVNCrewNoCrew()
        {
            InitializeComponent();
        }

        private void frmVNCrewNoCrew_Load(object sender, EventArgs e)
        {

        }

        private void sbtnCollectInfo_Click(object sender, EventArgs e)
        {
            string f_lbaytv2 = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\lbaytv2.dbf";
            string f_des = @"c:\temp\lbaytv2.dbf";
            List<CR_FlightInfo> dscb = new List<CR_FlightInfo>();
            List<CR_FlightInfo> kqcb = new List<CR_FlightInfo>();
            List<lbaytv2> ctlb = new List<lbaytv2>();
            bool flag;
            DateTime ngaymin = new DateTime(2018, 09, 01);
            DateTime ngaymax = new DateTime(2018, 09, 30);
            FileStream flbtv2=null;
            try {
                using (ERMSEntities1 db = new ERMSEntities1())
                {
                    dscb = db.CR_FlightInfo.Where(x => x.Date >= ngaymin && x.Date <= ngaymax && x.IsDeleted == true).ToList();                   
                }
                //File.Copy(f_lbaytv2, f_des, true);
                flbtv2 = new FileStream(f_des, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                   
                    lbaytv2 item = new lbaytv2();
                    item.code_fly = readder.GetString("CODE_FLY");
                    item.code_tv = codenv;
                    item.from_place = from;
                    item.end_place = to;
                    item.loai = loai;
                    item.cfg = cfg;
                    item.fly_no = flyno;
                    item.start_date = (DateTime)sdate;
                    item.end_date = (DateTime)edate;
                    item.start_time = readder.GetString("START_TIME");
                    item.end_time = readder.GetString("END_TIME");
                    item.job = readder.GetString("JOB");
                    item.note = readder.GetString("NOTE");
                    item.type_apl = readder.GetString("TYPE_APL");
                    item.acf = readder.GetString("ACF");
                    item.status = readder.GetString("STATUS");
                    ctlb.Add(item);
                }//While
                MessageBox.Show(ctlb.Count.ToString());
            }//Try
            catch (Exception ex) {
            }
            finally
            {
                flbtv2.Dispose();
            }
        }
    }
}