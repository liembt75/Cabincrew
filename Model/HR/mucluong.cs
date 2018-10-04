using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.HR
{
    [Table("mucluong")]
    public partial class mucluong
    {
        public int id { get; set; }
        [Column("mucluong")]
        public int muc_luong { get; set; }
        public string bacluong { get; set; }
    }
}
