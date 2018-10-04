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
using DevExpress.Export.Xl;
using Cabincrew.Model;
using Cabincrew.Model.HR;
using Cabincrew.Model.ERMS;
using System.IO;
using DevExpress.XtraSpreadsheet;


namespace Cabincrew
{
    public partial class xfrxgetDeposit : DevExpress.XtraEditors.XtraForm
    {
        
        public xfrxgetDeposit()
        {
            InitializeComponent();
        }

        private void sbtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Microsoft Excel|*.xlsx;*.xls";
            file.ShowDialog();
            if (file.FileName.Trim() != "")
            {
                lblFilename.Text = file.FileName.Trim();
                spreadsheetControl1.LoadDocument(file.FileName);
                spreadsheetControl1.SelectedCell = spreadsheetControl1.ActiveWorksheet.Cells[0, 0];
                sbtnGetInfo.Enabled = true;
                simpleButton1.Enabled = true; 
            }
        }

        private void sbtnGetInfo_Click(object sender, EventArgs e)
        {
            IList<DevExpress.Spreadsheet.Range> selectedRange = spreadsheetControl1.GetSelectedRanges();
            
            DevExpress.Spreadsheet.Range curentRange = selectedRange[0];
            if (!(curentRange.TopRowIndex == curentRange.BottomRowIndex && curentRange.LeftColumnIndex == curentRange.RightColumnIndex))
            {
                //MessageBox.Show(curentRange.TopRowIndex.ToString());
                SearchOptions option = new SearchOptions();
                option.SearchBy = SearchBy.Columns;
                option.SearchIn = SearchIn.Values;
                option.MatchEntireCellContents = true;
                option.MatchCase = false;
                IEnumerable<Cell> searchResult;
                #region Tìm các cột 
                // Tìm cột Crew ID
                searchResult = curentRange.Search("CREW ID", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("CREWID", option);
                    if (searchResult.Count() == 0) { 
                        MessageBox.Show("Không tìm thấy cột Crew ID trong vùng dữ liệu được chọn");
                        return;
                    }
                }
                int colcrewid = searchResult.First().LeftColumnIndex;
                int colflightid = colcrewid + 1;
                int colnote = colcrewid + 2;
                // Dòng dữ liệu đầu tiên
                int firstRow = searchResult.First().TopRowIndex + 1;
                curentRange.Worksheet.Cells[firstRow-1,colflightid].Value="FLight ID";
                curentRange.Worksheet.Cells[firstRow - 1, colnote].Value = "Note";
                curentRange.Worksheet.Cells[firstRow - 1, colnote+1].Value = "Database";
                int colds, cols;
                colds = colnote +2;
                cols = colds + 2;
                //curentRange.Worksheet.Cells[firstRow - 1, colds].Value = "DS ID";
                //curentRange.Worksheet.Cells[firstRow - 1, colds+1].Value = "DS Name";
                //curentRange.Worksheet.Cells[firstRow - 1, cols].Value = "S ID";
                //curentRange.Worksheet.Cells[firstRow - 1, cols + 1].Value = "S Name";
                // Tìm cột Flight Date
                searchResult = curentRange.Search("Flight Date", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("FlightDate", option);
                    if (searchResult.Count() == 0)
                    {
                        MessageBox.Show("Không tìm thấy cột Flight Date trong vùng dữ liệu được chọn");
                        return;
                    }                    
                }
                int colfltdate = searchResult.First().LeftColumnIndex;

                // Tìm cột Route
                searchResult = curentRange.Search("Route", option);
                if (searchResult.Count() == 0)
                {
                    MessageBox.Show("Không tìm thấy cột Route trong vùng dữ liệu được chọn");
                    return;
                }
                int colroute = searchResult.First().LeftColumnIndex;

                // Tìm cột FlightNo Flight No
                searchResult = curentRange.Search("FlightNo.", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("Flight No.", option);
                    if (searchResult.Count() == 0)
                    {
                        MessageBox.Show("Không tìm thấy cột FlightNo trong vùng dữ liệu được chọn");
                        return;
                    }                    
                }
                int colfltno = searchResult.First().LeftColumnIndex;

                // Tìm cột CREW
                searchResult = curentRange.Search("CREW", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("CREW NAME", option);
                    if (searchResult.Count() == 0)
                    {
                        MessageBox.Show("Không tìm thấy cột CREW trong vùng dữ liệu được chọn");
                        return;
                    }
                }
                int colcrew = searchResult.First().LeftColumnIndex;

                // Tìm cột KPT Revenue
                
                searchResult = curentRange.Search("KPT \nRevenue", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("KPT\n Revenue", option);
                    if (searchResult.Count() == 0)
                    {
                        searchResult = curentRange.Search("KPT\nRevenue", option);
                        if (searchResult.Count() == 0)
                        {
                            MessageBox.Show("Không tìm thấy cột KPT Revenue trong vùng dữ liệu được chọn");
                            return;
                        }
                    }
                }
                int colrevenue = searchResult.First().LeftColumnIndex;

                // Tìm cột Discrepancy
                searchResult = curentRange.Search("Discrepancy", option);
                if (searchResult.Count() == 0)
                {
                    MessageBox.Show("Không tìm thấy cột Discrepancy trong vùng dữ liệu được chọn");
                    return;
                }
                int coldiscrepancy = searchResult.First().LeftColumnIndex;

                // Tìm cột Cashier Collected
                //string test = curentRange.Worksheet.Cells[1, 4].Value.ToString();
                searchResult = curentRange.Search("Cashier\n Collected", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("Cashier \nCollected", option);
                    if (searchResult.Count() == 0)
                    {
                        searchResult = curentRange.Search("Cashier\nCollected", option);
                        if (searchResult.Count() == 0)
                        {
                            MessageBox.Show("Không tìm thấy cột Cashier Collected trong vùng dữ liệu được chọn");
                            return;
                        }
                    }
                }
                int colcashier = searchResult.First().LeftColumnIndex;

                // Tìm cột Card Settlement
                searchResult = curentRange.Search("Card \nSettlement", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("Card\n Settlement", option);
                    if (searchResult.Count() == 0)
                    {
                        searchResult = curentRange.Search("Card\nSettlement", option);
                        if (searchResult.Count() == 0)
                        {
                            MessageBox.Show("Không tìm thấy cột Card Settlement trong vùng dữ liệu được chọn");
                            return;
                        }
                    }
                }
                int colcard = searchResult.First().LeftColumnIndex;

                // Tìm cột REMARK
                searchResult = curentRange.Search("REMARK", option);
                if (searchResult.Count() == 0)
                {
                    searchResult = curentRange.Search("REMARKS", option);
                    if (searchResult.Count() == 0)
                    {
                        MessageBox.Show("Không tìm thấy cột REMARK trong vùng dữ liệu được chọn");
                        return;
                    }                    
                }
                int colremark = searchResult.First().LeftColumnIndex;
                #endregion
                // Dòng và cột cuối của Range
                int lastcol = curentRange.RightColumnIndex;
                int lastrow = curentRange.BottomRowIndex;
                if (lastrow < firstRow)
                {
                    MessageBox.Show("Vùng chọn không có dữ liệu");
                    return;
                }
                HREntities hr = new HREntities();
                ERMSEntities1 cb = new ERMSEntities1();
                
                //string filesource = @"\\10.100.8.108\phanbay\doantv\ddtvvfp6\solieu\lbaytv2.dbf";
                string manv,hoten,tentv,route,flyno,remark,nametv="";
                double Discrepancy, KPTRevenue, CashierCollected, CardSettlement;
                DateTime flydate;
                int first_space,second_space,tt_ten,id_cb=-1;
                List<int> lstflightID = new List<int>();
                bool flag_add;
                try {
                    for (int i = firstRow; i <= lastrow; i++)
                    {
                        flag_add = false;
                        var codetv=curentRange.Worksheet.Cells[i,colcrewid].Value;
                        if (!codetv.IsEmpty)
                        {
                            manv = codetv.ToString().PadLeft(4,'0');
                            tentv = curentRange.Worksheet.Cells[i, colcrew].Value.ToString();
                        }
                        #region Tìm Crewid
                        else //Tìm Manv trong nhân sự dựa theo tên
                        {
                            tentv = curentRange.Worksheet.Cells[i, colcrew].Value.ToString().Trim();
                            manv = "";
                            //Có khoảng trắng có thể là tên có số hoặc cả họ tên
                            if (tentv.Contains(" "))
                            {
                                first_space = tentv.IndexOf(" ");
                                second_space = tentv.IndexOf(" ", first_space + 1);
                                if (second_space < 0) // Chỉ một khoản trằn có thể là tên và số trùng tên. Tìm trong Nhân sự theo hướng đó.
                                {
                                    var ns=hr.HoSoGocs.Where(x => x.Tenkd.Trim().ToUpper() == tentv.Trim().ToUpper()).FirstOrDefault();
                                    if (ns!=null)
                                    {
                                        curentRange.Worksheet.Cells[i, colcrewid].Value = ns.mans.Trim();
                                        manv = ns.mans.Trim();
                                        curentRange.Worksheet.Cells[i, colnote].Value = "KPT không có CrewID";
                                    }                                        
                                    else
                                        curentRange.Worksheet.Cells[i, colnote].Value = "KPT không có CrewID";
                                }
                                else //Có khoảng trắng thứ hai ==> Có phần họ và tên đệm
                                {
                                    string sotrung;
                                    sotrung = tentv.Substring(first_space + 1, second_space - first_space-1);
                                    //Kiểm tra xem phải số trùng tên không
                                    if (Int32.TryParse(sotrung,out tt_ten)) {
                                        hoten = tentv.Substring(second_space + 1).Trim() + " " + tentv.Substring(0, second_space);
                                    }
                                    else
                                    {
                                        hoten = tentv.Substring(first_space + 1).Trim() + " " + tentv.Substring(0, first_space);
                                    }
                                    var ns = hr.HoSoGocs.Where(x => x.tenkodau.Trim().ToUpper() == hoten.ToUpper()).FirstOrDefault();
                                    if (ns != null)
                                    {
                                        curentRange.Worksheet.Cells[i, colcrewid].Value = ns.mans.Trim();
                                        manv = ns.mans.Trim();
                                        curentRange.Worksheet.Cells[i, colnote].Value = "KPT không có CrewID";
                                    }                                        
                                    else
                                        curentRange.Worksheet.Cells[i, colnote].Value = "Không tìm được CrewID";
                                }
    
                            }
                            // Nhập mỗi tên không có số trùng tên
                            else
                            {
                                curentRange.Worksheet.Cells[i, colnote].Value = "Không thể tìm được Crew ID";
                            }
                        }
                        #endregion 
                        #region Tìm Chuyến Bay
                        if(curentRange.Worksheet.Cells[i, colfltdate].Value.IsDateTime);
                            flydate =Convert.ToDateTime(curentRange.Worksheet.Cells[i, colfltdate].Value.ToString());
                        route = curentRange.Worksheet.Cells[i, colroute].Value.ToString();
                        flyno = "VN" + curentRange.Worksheet.Cells[i, colfltno].Value.ToString();
                        
                        var flight = cb.CR_FlightInfo.Where(x => x.Routing == route && x.FlightNo==flyno && x.Date ==flydate.Date).FirstOrDefault();
                        if (flight != null)
                            {
                            curentRange.Worksheet.Cells[i, colflightid].Value = flight.FlightID;
                            id_cb=flight.FlightID;
                            var fltid = lstflightID.Where(x => x == id_cb).FirstOrDefault();
                            if (fltid >0)
                                flag_add = true;
                            }
                            //Tìm DS và S
                            var totv=cb.CR_Flight_Crew.Where(x=>x.FlightID==id_cb && x.IsDeleted==false && x.Dutyfree!=null && x.Dutyfree!="").OrderBy(z=>z.Dutyfree).ToList();
                            if(totv!=null){
                                int k = 0;
                                foreach(var tv in totv){
                                    var hs=hr.HoSoGocs.Where(x=>x.mans.Trim()==tv.CrewID).FirstOrDefault();
                                    if (hs.mans.Trim() == manv)
                                        curentRange.Worksheet.Cells[i, colcrewid].Font.Color = System.Drawing.Color.Cyan;
                                    if(hs!=null)
                                        nametv=hs.tenkodau.Trim().ToUpper();
                                    curentRange.Worksheet.Cells[i, colds + k].Value = tv.CrewID;
                                    curentRange.Worksheet.Cells[i, colds + k+1].Value = nametv;
                                    curentRange.Worksheet.Cells[i, colds + k+2].Value = tv.Dutyfree;
                                    k = k + 3;
                                    //if(tv.Dutyfree=="S"){                                    
                                    //     curentRange.Worksheet.Cells[i,cols].Value=tv.CrewID;
                                    //     curentRange.Worksheet.Cells[i,cols+1].Value=nametv; 
                                    //}
                                    //else 
                                    //    if(tv.Dutyfree=="DS" || tv.Dutyfree=="D"){
                                    //        curentRange.Worksheet.Cells[i,colds].Value=tv.CrewID;
                                    //        curentRange.Worksheet.Cells[i,colds+1].Value=nametv;
                                    //    }
                                }
                            }
                        else
                        {
                            curentRange.Worksheet.Cells[i, colnote].Value = "Không tìm được chuyến bay tương ứng";
                            id_cb=-1;
                        }
                        
                        #endregion 
                        #region Lấy thông tin khác
                        if (!curentRange.Worksheet.Cells[i, coldiscrepancy].Value.IsEmpty)
                            Discrepancy = Convert.ToDouble(curentRange.Worksheet.Cells[i, coldiscrepancy].Value.ToString());
                        else
                            Discrepancy = 0.0;
                        if (!curentRange.Worksheet.Cells[i, colrevenue].Value.IsEmpty)
                            KPTRevenue = Convert.ToDouble(curentRange.Worksheet.Cells[i, colrevenue].Value.ToString());
                        else
                            KPTRevenue = 0.0;
                        if (!curentRange.Worksheet.Cells[i, colcashier].Value.IsEmpty)
                            CashierCollected = Convert.ToDouble(curentRange.Worksheet.Cells[i, colcashier].Value.ToString());
                        else
                            CashierCollected = 0.0;
                        if (!curentRange.Worksheet.Cells[i, colcard].Value.IsEmpty)
                            CardSettlement = Convert.ToDouble(curentRange.Worksheet.Cells[i, colcard].Value.ToString());
                        else
                            CardSettlement = 0.0;
                        if (curentRange.Worksheet.Cells[i, colremark].Value.IsEmpty)
                            remark = "";
                        else 
                            remark = curentRange.Worksheet.Cells[i, colremark].Value.ToString(); // ?? curentRange.Worksheet.Cells[i, colremark].Value.ToString(),"";
                        #endregion
                        #region Ghi vào CSDL
                        if (id_cb > 0)
                        {
                            var dt = cb.CR_Flight_Dutyfree.Where(x => x.FlightID == id_cb).FirstOrDefault();
                            if (dt != null) // Có rồi ==> Cập nhật Total và KPTinfo
                            {
                                if(!flag_add)
                                    if (Discrepancy > 0)
                                        dt.Total = KPTRevenue;
                                    else
                                        dt.Total = CashierCollected + CardSettlement;
                                else
                                    if (Discrepancy > 0)
                                        dt.Total = dt.Total+KPTRevenue;
                                    else
                                        dt.Total = dt.Total+CashierCollected + CardSettlement;

                                dt.KPTinfo = "Flyno=" + flyno.Trim() + " Date=" + flydate.Date.ToString("dd/MM/yyyy") + " Route=" + route + " Discrepancy=" + Discrepancy.ToString() + " KPT Revenue=" + KPTRevenue.ToString() + " Cashier Collected=" + CashierCollected.ToString() + " Card Settlement " + CardSettlement.ToString() + " Crewid=" + manv + " Crewname=" + tentv + " Remark=" + remark;                                   
                                dt.Modified = DateTime.Now;
                                dt.Modifier = "liembt";
                                dt.Modifierid = "1067";
                                cb.SaveChanges();
                                //curentRange.Worksheet.Cells[i, colnote + 1].Value = "Update";                                
                            }
                            else //Chưa có ==> Add vào
                            {
                                CR_Flight_Dutyfree newdt = new CR_Flight_Dutyfree();

                                newdt.FlightID = id_cb;
                                
                                if (Discrepancy > 0)
                                    newdt.Total = KPTRevenue;
                                else
                                    newdt.Total = CashierCollected + CardSettlement;
                               
                                newdt.KPTinfo = "Flyno=" + flyno.Trim() + " Date=" + flydate.Date.ToString("dd/MM/yyyy") + " Route=" + route + " Discrepancy=" + Discrepancy.ToString() + " KPT Revenue=" + KPTRevenue.ToString() + " Cashier Collected=" + CashierCollected.ToString() + " Card Settlement " + CardSettlement.ToString() + " Crewid=" + manv + " Crewname=" + tentv + " Remark=" + remark;
                                newdt.Created = DateTime.Now;
                                newdt.Creator = "liembt";
                                newdt.Creatorid = "1067";

                                cb.CR_Flight_Dutyfree.Add(newdt);
                                
                                cb.SaveChanges();
                                //curentRange.Worksheet.Cells[i, colnote + 1].Value = "Insert";                                
                            }
                           
                        }
                        else
                            curentRange.Worksheet.Cells[i, colnote + 1].Value = "";
                        
                        #endregion 
                        
                    }
                    #region Export Excel Kết quả xử lý
                    string filename;
                    filename = lblFilename.Text.Substring(0, lblFilename.Text.IndexOf(".")) + "_kqxl.xlsx";
                    IWorkbook workbook = spreadsheetControl1.Document;
                    using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                    {
                        workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                    }
                    #endregion
                }
                //catch(Exception ex){
                //    //MessageBox.Show(ex.Message);
                //}        
                finally{
                    hr.Dispose();
                    cb.Dispose();
                }                

            }
            else
                MessageBox.Show("Vui lòng chọn vùng dữ liệu");

            MessageBox.Show("Complete!");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           Worksheet sale = spreadsheetControl1.Document.Worksheets[1];
           Cell odau, ocuoi;
           SearchOptions option = new SearchOptions();
           option.SearchBy = SearchBy.Columns;
           option.SearchIn = SearchIn.Values;
           option.MatchEntireCellContents = true;
           option.MatchCase = false;
           IEnumerable<Cell> searchResult;
           string file_ct = lblFilename.Text.Substring(0, lblFilename.Text.LastIndexOf(@"\")) + @"\BC_CT.xlsx";
           string file_th = lblFilename.Text.Substring(0, lblFilename.Text.LastIndexOf(@"\")) + @"\BC_TH.xlsx";
           
           //Chi tiết
           SpreadsheetControl spreadct = new SpreadsheetControl();
           spreadct.Document.LoadDocument(file_ct);
           Worksheet bcct = spreadct.Document.Worksheets[0];
           //Tổng  hợp
           SpreadsheetControl spreadth = new SpreadsheetControl();
           spreadth.Document.LoadDocument(file_th);
           Worksheet bcth = spreadth.Document.Worksheets[0];
           CR_Flight_Crew tvt;

           int rowct = 5,rowth=8;
           int songuoi,stt,stt_th,rate;
           stt = 1;
           stt_th = 1;
           rate = 235000;

#region Tìm các cột
           // Tìm cột FlightID 
           searchResult = sale.Search("FLight ID", option);
           if (searchResult.Count() == 0)
           {
               MessageBox.Show("Không tìm thấy cột Flight ID trong vùng dữ liệu được chọn");
               return;               
           }
           int colfltid = searchResult.First().LeftColumnIndex; 
           // Dòng dữ liệu đầu tiên
           int firstRow = searchResult.First().TopRowIndex + 1;
           // Tìm cột KPT Revenue

           searchResult = sale.Search("KPT \nRevenue", option);
           if (searchResult.Count() == 0)
           {
               searchResult = sale.Search("KPT\n Revenue", option);
               if (searchResult.Count() == 0)
               {
                   searchResult = sale.Search("KPT\nRevenue", option);
                   if (searchResult.Count() == 0)
                   {
                       MessageBox.Show("Không tìm thấy cột KPT Revenue trong vùng dữ liệu được chọn");
                       return;
                   }
               }
           }
           int colrevenue = searchResult.First().LeftColumnIndex;

           // Tìm cột Discrepancy
           searchResult = sale.Search("Discrepancy", option);
           if (searchResult.Count() == 0)
           {
               MessageBox.Show("Không tìm thấy cột Discrepancy trong vùng dữ liệu được chọn");
               return;
           }
           int coldiscrepancy = searchResult.First().LeftColumnIndex;

           // Tìm cột Cashier Collected
           searchResult = sale.Search("Cashier\n Collected", option);
           if (searchResult.Count() == 0)
           {
               searchResult = sale.Search("Cashier \nCollected", option);
               if (searchResult.Count() == 0)
               {
                   searchResult = sale.Search("Cashier\nCollected", option);
                   if (searchResult.Count() == 0)
                   {
                       MessageBox.Show("Không tìm thấy cột Cashier Collected trong vùng dữ liệu được chọn");
                       return;
                   }
               }
           }
           int colcashier = searchResult.First().LeftColumnIndex;

           // Tìm cột Card Settlement
           searchResult = sale.Search("Card \nSettlement", option);
           if (searchResult.Count() == 0)
           {
               searchResult = sale.Search("Card\n Settlement", option);
               if (searchResult.Count() == 0)
               {
                   searchResult = sale.Search("Card\nSettlement", option);
                   if (searchResult.Count() == 0)
                   {
                       MessageBox.Show("Không tìm thấy cột Card Settlement trong vùng dữ liệu được chọn");
                       return;
                   }
               }
           }
           int colcard = searchResult.First().LeftColumnIndex;

           // Tìm cột Manv DS
           searchResult = sale.Search("MNV_DS", option);
           if (searchResult.Count() == 0)
           {
               MessageBox.Show("Không tìm thấy cột MNV_DS trong vùng dữ liệu được chọn");
               return;               
           }
           int colds = searchResult.First().LeftColumnIndex;
           // Tìm cột Manv S
           searchResult = sale.Search("MNV_S", option);
           if (searchResult.Count() == 0)
           {
               MessageBox.Show("Không tìm thấy cột MNV_S trong vùng dữ liệu được chọn");
               return;
               
           }
           int cols = searchResult.First().LeftColumnIndex;
#endregion
           
           HREntities hr = new HREntities();
           ERMSEntities1 cb = new ERMSEntities1();
           SalaryEntities sal = new SalaryEntities();
           int flid, i;
           string manvds,manvs,thang,mbase;
           double kptrevenue, cashiercollected, cardsettlement, discrepancy,doanhthu,hoahong;
           i = firstRow;
           HoSoGoc hs;
           Crew_dutyfree crewdt;
           bool flag20,tvtban=false;
           thang=lblFilename.Text.Substring(0,lblFilename.Text.IndexOf("_kqxl"));
           mbase = thang.Substring(0, thang.LastIndexOf(" "));
           thang = thang.Substring(thang.LastIndexOf(" ") + 1) + "-2018";
           mbase = mbase.Substring(mbase.LastIndexOf(" ")+1);
           try { 
               while (true){
                   if (sale.Cells[i, colfltid].Value.IsEmpty) // Không có Flight ID ==> coi như hết. Nghỉ
                       break;

                   flid = Convert.ToInt32(sale.Cells[i,colfltid].Value.ToString());                   
                   manvds = sale.Cells[i, colds].Value.ToString().PadLeft(4, '0');
                   if (sale.Cells[i, cols].Value.IsEmpty)
                       manvs = "";
                   else 
                       manvs = sale.Cells[i, cols].Value.ToString().PadLeft(4, '0');
                   
                   if (!sale.Cells[i, coldiscrepancy].Value.IsEmpty)
                        discrepancy = Convert.ToDouble(sale.Cells[i, coldiscrepancy].Value.ToString());
                   else
                        discrepancy = 0.0;
                   
                   if (!sale.Cells[i, colrevenue].Value.IsEmpty)
                        kptrevenue = Convert.ToDouble(sale.Cells[i, colrevenue].Value.ToString());
                   else
                        kptrevenue = 0.0;

                   if (!sale.Cells[i, colcashier].Value.IsEmpty)
                        cashiercollected = Convert.ToDouble(sale.Cells[i, colcashier].Value.ToString());
                   else
                        cashiercollected = 0.0;

                   if (!sale.Cells[i, colcard].Value.IsEmpty)
                       cardsettlement = Convert.ToDouble(sale.Cells[i, colcard].Value.ToString());
                   else
                       cardsettlement = 0.0;
                   //Doanh thu
                   doanhthu = kptrevenue;
                   // Tìm thông tin chuyến bay và danh sách tổ tiếp viên
                   var sector = cb.CR_FlightInfo.Where(x => x.FlightID == flid).FirstOrDefault();

                   bcct.Cells[rowct, 0].Value = stt;
                   bcct.Cells[rowct, 1].Value = sector.Date.ToString("dd/MM/yyyy");
                   bcct.Cells[rowct, 2].Value = sector.Routing;
                   bcct.Cells[rowct, 3].Value = sector.FlightNo;
                   bcct.Cells[rowct, 4].Value = doanhthu;
                   bcct.Cells[rowct, 5].Value = 0.08*doanhthu;
                   stt++;
                   //Ghi lên file tổng hợp
                   bcth.Cells[rowth, 0].Value = stt_th;
                   bcth.Cells[rowth, 1].Value = sector.Date.ToString("dd/MM/yyyy");
                   bcth.Cells[rowth, 2].Value = sector.FlightNo;
                   bcth.Cells[rowth, 3].Value = sector.Routing;
                   bcth.Cells[rowth, 4].Value = doanhthu;
                   bcth.Cells[rowth, 5].Value = 0.08 * doanhthu;
                   bcth.Cells[rowth, 7].Value = 0.02 * doanhthu;
                   bcth.Cells[rowth, 9].Value = 0.1 * doanhthu;
                   stt_th++;
                   rowth++;
                   tvtban = false;
                   //TVT CB

                   if(flid==286956)
                        tvt = cb.CR_Flight_Crew.Where(x => x.FlightID == flid  && x.ca=="1" && x.Job=="P").FirstOrDefault();
                   else 
                        tvt = cb.CR_Flight_Crew.Where(x => x.FlightID == flid && x.IsDeleted == false && x.ca=="1" && x.Job=="P").FirstOrDefault();

                   //TV DS
                   hs = hr.HoSoGocs.Where(x => x.mans.Trim() == manvds).FirstOrDefault();                   
                   if(hs!=null){
                       
                       bcct.Cells[rowct, 6].Value =hs.tenkodau.ToUpper();
                       bcct.Cells[rowct, 7].Value = "'"+manvds;                       
                       if (tvt.CrewID == hs.mans.Trim())
                       {
                           tvtban = true;
                           bcct.Cells[rowct, 8].Value = "DS/P";
                           bcct.Cells[rowct, 9].Value = 50;
                           bcct.Cells[rowct, 10].Value = 0.5 * 0.08 * doanhthu;
                           hoahong = 0.5 * 0.08 * doanhthu;
                           if (manvs == "")
                           {
                               bcct.Cells[rowct, 9].Value = 80;
                               bcct.Cells[rowct, 10].Value = 0.8 * 0.08 * doanhthu;
                               hoahong = 0.8 * 0.08 * doanhthu;
                           }
                               
                       }
                       else
                       {
                           bcct.Cells[rowct, 8].Value = "DS";
                           bcct.Cells[rowct, 9].Value = 40;
                           bcct.Cells[rowct, 10].Value = 0.4 * 0.08 * doanhthu;
                           hoahong = 0.4 * 0.08 * doanhthu;
                           if (manvs == "")
                           {
                               bcct.Cells[rowct, 9].Value = 70;
                               bcct.Cells[rowct, 10].Value = 0.7 * 0.08 * doanhthu;
                               hoahong = 0.7 * 0.08 * doanhthu;
                           }
                       }                       
                       
                       var crewdtds = sal.Crew_dutyfree.Where(x => x.crewid == manvds && x.flightid == flid && x.month == thang).FirstOrDefault();
                       if (crewdtds == null)
                       {
                           Crew_dutyfree newdt = new Crew_dutyfree();
                           newdt.flightid = flid;
                           newdt.crewid = manvds;
                           newdt.month = thang;
                           newdt.commission = hoahong;
                           newdt.rate = rate;
                           newdt.mainbase = mbase;
                           newdt.note = "Insert*" + rowct.ToString();
                           sal.Crew_dutyfree.Add(newdt);
                       }
                       else
                       {
                           crewdtds.note = crewdtds.note.Trim() + "Update*" + rowct.ToString(); ;
                           crewdtds.commission = hoahong;
                       }
                           
                       sal.SaveChanges();
                       rowct++;
                   }
                   //TV S
                   if (manvs != "")
                   {
                       hs = hr.HoSoGocs.Where(x => x.mans.Trim() == manvs).FirstOrDefault();
                       if (hs != null)
                       {
                           bcct.Cells[rowct, 6].Value = hs.tenkodau.ToUpper();
                           bcct.Cells[rowct, 7].Value = "'" + manvs;
                           if (tvt.CrewID == hs.mans.Trim())
                           {
                               tvtban = true;
                               bcct.Cells[rowct, 8].Value = "S/P";
                               bcct.Cells[rowct, 9].Value = 40;
                               bcct.Cells[rowct, 10].Value = 0.4 * 0.08 * doanhthu;
                               hoahong = 0.4 * 0.08 * doanhthu;
                           }
                           else
                           {
                               bcct.Cells[rowct, 8].Value = "S";
                               bcct.Cells[rowct, 9].Value = 30;
                               bcct.Cells[rowct, 10].Value = 0.3 * 0.08 * doanhthu;
                               hoahong = 0.3 * 0.08 * doanhthu;
                           }
                           var crewdts = sal.Crew_dutyfree.Where(x => x.crewid == manvs && x.flightid == flid && x.month == thang).FirstOrDefault();
                           if (crewdts == null)
                           {
                               Crew_dutyfree newdt = new Crew_dutyfree();
                               newdt.flightid = flid;
                               newdt.crewid = manvs;
                               newdt.month = thang;
                               newdt.rate = rate;
                               newdt.commission = hoahong;
                               newdt.mainbase = mbase;
                               newdt.note = "Insert*" + rowct.ToString();
                               sal.Crew_dutyfree.Add(newdt);
                           }
                           else
                           {
                               crewdts.note = crewdts.note + "Update*" + rowct.ToString();
                               crewdts.commission = hoahong;
                           }
                               
                           sal.SaveChanges();
                           rowct++;
                       }
                   }
                   //TVT
                   if (!tvtban)
                   {
                       hs = hr.HoSoGocs.Where(x => x.mans.Trim() == tvt.CrewID).FirstOrDefault();
                       bcct.Cells[rowct, 6].Value = hs.tenkodau.ToUpper();
                       bcct.Cells[rowct, 7].Value = "'" + tvt.CrewID;
                       bcct.Cells[rowct, 8].Value = "P";
                       bcct.Cells[rowct, 9].Value = 10;
                       bcct.Cells[rowct, 10].Value = 0.1 * 0.08 * doanhthu;
                       hoahong = 0.1 * 0.08 * doanhthu;
                       crewdt = sal.Crew_dutyfree.Where(x => x.crewid == tvt.CrewID && x.flightid == flid && x.month == thang).FirstOrDefault();
                       if (crewdt == null)
                       {
                           Crew_dutyfree newdt = new Crew_dutyfree();
                           newdt.flightid = flid;
                           newdt.crewid = tvt.CrewID;
                           newdt.month = thang;
                           newdt.rate = rate;
                           newdt.commission = hoahong;
                           newdt.mainbase = mbase;
                           newdt.note = "Insert*" + rowct.ToString();
                           sal.Crew_dutyfree.Add(newdt);
                       }
                       else
                       {
                           crewdt.note = crewdt.note + "Update*" + rowct.ToString();
                           crewdt.commission = hoahong;
                       }                           
                       sal.SaveChanges();
                       rowct++;
                   }
                   //Danh sách tiếp viên còn lại
                   List<CR_Flight_Crew> dstv;
                   if(flid==286956)
                       dstv = cb.CR_Flight_Crew.Where(x => x.FlightID == flid && x.CrewID!=tvt.CrewID && x.CrewID!=manvds && x.CrewID!=manvs && x.CrewID!="1265" && x.CrewID!="3583" && x.CrewID!="4247" && x.CrewID!="4623" && x.CrewID!="5445" && x.CrewID!="5482").ToList(); 
                   else 
                       dstv = cb.CR_Flight_Crew.Where(x => x.FlightID == flid && x.IsDeleted == false && x.CrewID!=tvt.CrewID && x.CrewID!=manvds && x.CrewID!=manvs).ToList();
                   flag20 = false;
                   songuoi = dstv.Count;
                   foreach (var tv in dstv)
                   {
                       hs = hr.HoSoGocs.Where(x => x.mans.Trim() == tv.CrewID).FirstOrDefault();
                       bcct.Cells[rowct, 6].Value = hs.tenkodau.ToUpper();
                       bcct.Cells[rowct, 7].Value = "'" + tv.CrewID;
                       bcct.Cells[rowct, 8].Value = "CA";
                       if(!flag20)
                       {
                           bcct.Cells[rowct, 9].Value = 20;
                           flag20 = true;
                           odau = bcct.Cells[rowct, 9];
                       }
                       bcct.Cells[rowct, 10].Value = 0.2*0.08*doanhthu/songuoi;
                       ocuoi = bcct.Cells[rowct, 9];
                       hoahong = 0.2 * 0.08 * doanhthu / songuoi;
                       var crewdtv = sal.Crew_dutyfree.Where(x => x.crewid == tv.CrewID && x.flightid == flid && x.month == thang).FirstOrDefault();
                       if (crewdtv == null)
                       {
                           Crew_dutyfree newdt = new Crew_dutyfree();
                           newdt.flightid = flid;
                           newdt.crewid = tv.CrewID;
                           newdt.month = thang;
                           newdt.rate = rate;
                           newdt.commission = hoahong;
                           newdt.mainbase = mbase;
                           newdt.note = "Insert*" + rowct.ToString();
                           sal.Crew_dutyfree.Add(newdt);
                       }
                       else
                       {
                           crewdtv.note = crewdtv.note + "Update*" + rowct.ToString();
                           crewdtv.commission = hoahong;
                       }                          
                       sal.SaveChanges();
                       rowct++;
                   }
                   //bcct.Range[odau, ocuoi].Merge();
                   i++;
                };
               //Lưu Kết quả CT
               string filekq;
               filekq = lblFilename.Text.Substring(0, lblFilename.Text.IndexOf(".")) + "-BC_CT.xlsx";
               IWorkbook workbook = spreadct.Document;
               using (FileStream stream = new FileStream(filekq, FileMode.Create, FileAccess.ReadWrite))
               {
                   workbook.SaveDocument(stream, DocumentFormat.Xlsx);
               }
               //Lưu Kết quả TH
               string filekqth;
               filekqth = lblFilename.Text.Substring(0, lblFilename.Text.IndexOf(".")) + "-BC_TH.xlsx";
               IWorkbook workbookth = spreadth.Document;
               using (FileStream streamth = new FileStream(filekqth, FileMode.Create, FileAccess.ReadWrite))
               {
                   workbookth.SaveDocument(streamth, DocumentFormat.Xlsx);
               }
           }
           //catch (Exception ex)
           //{

           //}
           finally
           {
               hr.Dispose();
               cb.Dispose();
           }
          
           MessageBox.Show("Complete");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

              
    }
}