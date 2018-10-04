using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.HR
{
    [Table("quanlyhcnn")]
    public partial class quanlyhcnn
    {
        public int id { get; set; }
        public int id_ns { get; set; }
        public int qlhcnn { get; set; }
        //public DateTime ngaycap { get; set; }
    }
}
