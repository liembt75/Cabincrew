using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.HR
{
    [Table("nhomchuyenmon")]
    public partial class nhomchuyenmon
    {
        public int id { get; set; }
        public int id_ns { get; set; }
        public int chuyenmon { get; set; }
        [Column("nhomchuyenmon")]
        public int nhomchuyenmon1 { get; set; }
    }
}
