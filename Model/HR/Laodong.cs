using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.HR
{
 [Table("qlttlaodong")]
    public class Laodong
    {
        public int id { get; set; }
        public int id_ns { get; set; }
        public int hd_loai { get; set; }
        public string hd_sohd { get; set; }
        public DateTime hd_ngaykyhd { get; set; }
        public DateTime hd_ngayhieuluc { get; set; }
        public DateTime hd_ngayhet { get; set; }
        public Nullable<DateTime> hd_ngaychamdut { get; set; }
        public Nullable<DateTime> hd_ngayhlchamduthd { get; set; }
    }
}
