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

namespace Cabincrew.Forms
{
    public partial class shareform : DevExpress.XtraEditors.XtraForm
    {
        
        public shareform()
        {
            InitializeComponent();
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

        private void shareform_Load(object sender, EventArgs e)
        {
            spreadsheetControl1.Width = this.Width-30;
            spreadsheetControl1.Height = this.Height - 100;
            textEdit1.Width = this.Width - sbtnLoadExcel.Width - 30;
        }

        private void shareform_SizeChanged(object sender, EventArgs e)
        {
            textEdit1.Width = this.Width - sbtnLoadExcel.Width - 30;
            spreadsheetControl1.Width = this.Width-30;
            if (this.Height > 100)
                spreadsheetControl1.Height = this.Height - 100;
            else
                spreadsheetControl1.Height = this.Height;
        }

        //private void simpleButton1_Click(object sender, EventArgs e)
        //{

        //}
        //Tìm cột theo title
        public int get_col(string title)
        {
            int kq_col=0;
            SearchOptions option = new SearchOptions();
            option.SearchBy = SearchBy.Columns;
            option.SearchIn = SearchIn.Values;
            option.MatchEntireCellContents = true;
            option.MatchCase = false;
            IEnumerable<Cell> searchResult;
            Worksheet spreadst = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
            searchResult = spreadst.Search(title, option);
            if (searchResult.Count() == 0)
            {
                MessageBox.Show("Không tìm thấy cột Mã nhân sự");
                return -1;
            }
            kq_col = searchResult.First().LeftColumnIndex;
            return kq_col;
        }
        public int get_row(string title)
        {
            int kq_row = 0;
            SearchOptions option = new SearchOptions();
            option.SearchBy = SearchBy.Columns;
            option.SearchIn = SearchIn.Values;
            option.MatchEntireCellContents = true;
            option.MatchCase = false;
            IEnumerable<Cell> searchResult;
            Worksheet spreadst = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
            searchResult = spreadst.Search(title, option);
            if (searchResult.Count() == 0)
            {
                MessageBox.Show("Không tìm thấy cột Mã nhân sự");
                return -1;
            }
            kq_row = searchResult.First().TopRowIndex;
            return kq_row;
        }
        public int get_lastcol(string title)
        {
            int kq_col = -1;
            int col_title = get_col(title);
            int row_title=get_row(title);
            int i=col_title+1;
            Worksheet spreadst = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
            while (true)
            {
                if (spreadst.Cells[row_title, i].Value.IsEmpty && spreadst.Cells[row_title, i + 1].Value.IsEmpty && spreadst.Cells[row_title, i + 2].Value.IsEmpty && spreadst.Cells[row_title, i + 3].Value.IsEmpty && spreadst.Cells[row_title, i + 4].Value.IsEmpty)
                {
                    kq_col = i;
                    break;
                }
                i++;
            }
            return kq_col;
        }
        public void set_value(int row, int col, object value)
        {
            Worksheet spreadst = spreadsheetControl1.Document.Worksheets.ActiveWorksheet;
            spreadst.Cells[row, col].Value = value.ToString();
        }
    }
}