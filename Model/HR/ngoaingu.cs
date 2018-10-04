using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.HR
{
    [Table("ngoaingu")]
    public partial class ngoaingu
    {
        public int id { get; set; }
        public int id_ns { get; set; }
        public int ngoaingu_loai { get; set; }
        public int ngoaingu_bangcap { get; set; }
        public DateTime ngoaingu_ngaycap { get; set; }
        public Nullable<double> ngoaingu_diemtong { get; set; }
    }
}
