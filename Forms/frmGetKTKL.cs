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
using Cabincrew.Forms;
using DevExpress.XtraSpreadsheet;
using Cabincrew.Model.HR;

namespace Cabincrew.Forms
{
    public partial class frmGetKTKL : shareform
    {
        public frmGetKTKL()
        {
            InitializeComponent();
        }

        private void frmGetKTKL_Load(object sender, EventArgs e)
        {
            this.Text = "Tổng hợp khen thưởng - Kỷ luật";            
            edtTungay.EditValue = DateTime.Now.AddMonths(-1).AddDays(1);
            edtToingay.EditValue = DateTime.Now;
        }

        private void sbtnCollectInfo_Click(object sender, EventArgs e)
        {
            DateTime tungay = (DateTime)edtTungay.EditValue;// Convert.ToDateTime(txtTungay.Text);
            DateTime Toingay = (DateTime)edtToingay.EditValue; // Convert.ToDateTime(txtToingay.Text);
            int so;
            int colmanv = base.get_col("code_tv");
            int lastcol = base.get_lastcol("code_tv");
            int firstrow=base.get_row("code_tv");
            int dong,col_num,dongtrong = 0;
            col_num = lastcol;
            dong = firstrow;
            Worksheet spreadst =base.spreadsheetControl1.ActiveWorksheet;
            List<danhsachns> danhsachtv = new List<danhsachns>();
            List<khenkluat> dsktkl = new List<khenkluat>();
            List<dmktkl> dmhinhthuc = new List<dmktkl>();
            string codetv;
            try {
                using (HREntities hr = new HREntities())
                {
                    while (true)
                    {
                        if (dongtrong > 10)
                            break;
                        if (spreadst.Cells[dong, colmanv].Value.IsEmpty)
                        {
                            dongtrong++;
                            dong++;
                            continue;
                        }
                        codetv = spreadst.Cells[dong, colmanv].Value.ToString();
                        if (!Int32.TryParse(codetv, out so))
                        {
                            dongtrong++;
                            dong++;
                            continue;
                        }
                        dongtrong = 0;
                        var ns = hr.HoSoGocs.Where(x => x.mans.Trim() == codetv).FirstOrDefault();
                        if (ns != null)
                        {
                            var motdong = new danhsachns();
                            motdong.manv = codetv;
                            motdong.dong = dong;
                            motdong.id_ns = ns.id;
                            danhsachtv.Add(motdong);
                            var ktkl = hr.khenkluats.Where(x => x.id_ns == ns.id && x.ktkl_ngayqd >= tungay && x.ktkl_ngayqd <= Toingay && x.ktkl_hinhthuc != 3677 && x.ktkl_hinhthuc != 3679 && x.ktkl_hinhthuc != 3687 && x.ktkl_hinhthuc != 4020 && x.ktkl_hinhthuc != null).ToList();
                            if (ktkl != null)
                                dsktkl.AddRange(ktkl);                                
                        }//if ns!=null
                        dong++;
                    }//while
                    // Hình thức <--> Cột
                    var dsht = dsktkl
                                .GroupBy(x => new { x.ktkl_hinhthuc, x.ktkl_loai })
                                .Select(y => new
                                {
                                    loai = y.Key.ktkl_loai,
                                    hinhthuc = y.Key.ktkl_hinhthuc
                                })
                                .OrderBy(it => it.loai)
                                .ThenBy(it=>it.hinhthuc);

                    foreach (var ht in dsht)
                    {
                        var tenht = hr.danhmucs.Where(x => x.id == ht.hinhthuc).FirstOrDefault();
                        var hinhthuc = new dmktkl();
                        hinhthuc.cot = col_num;
                        hinhthuc.hinhthuc = (int)ht.hinhthuc;
                        hinhthuc.ten = tenht.TenDanhMuc;
                        spreadst.Cells[firstrow, col_num].Value = tenht.TenDanhMuc;
                        col_num++;
                        dmhinhthuc.Add(hinhthuc);
                    }
                }//using
            }//try
            catch (Exception ex) { }
            if (dsktkl.Count > 0) //Có khen thưởng kỷ luật thì ghi kết quả
            {
                
                //Đếm số ktkl từng người
                var kqktkl =    dsktkl
                                .GroupBy(x=> new{x.id_ns,x.ktkl_hinhthuc})
                                .Select(y=> new{
                                    id_ns=y.Key.id_ns,
                                    hinhthuc=y.Key.ktkl_hinhthuc,
                                    solan=y.Count()
                                }).ToList();
               
                //dmhinhthuc.Select(x => { x.cot = col_num; col_num++; return x;}).ToList();
                //foreach(var dm in dmhinhthuc){
                //    spreadst.Cells[firstrow,dm.cot].Value=dm.ten;
                //}
                    
                foreach (var tv in danhsachtv)
                {
                    var tvktkl = kqktkl.Where(x => x.id_ns == tv.id_ns).ToList();
                    foreach (var kt in tvktkl)
                    {
                        var zz=dmhinhthuc.Where(y=>y.hinhthuc==kt.hinhthuc).FirstOrDefault();
                        spreadst.Cells[tv.dong,zz.cot].Value=kt.solan;
                    }
                }
            }//if 
            MessageBox.Show("Complete!");
        }

        private void txtTungay_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void edtTungay_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
    struct danhsachns
    {
        public int id_ns { get; set; }
        public string manv { get; set; }
        public int dong { get; set; }
    }
    class dmktkl
    {
        public int hinhthuc { get; set; }
        public string ten { get; set; }
        public int cot { get; set; }
    }
}