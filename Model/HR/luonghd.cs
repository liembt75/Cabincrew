﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.HR
{
    [Table("Luonghd")]
    public partial class luonghd
    {
        public int id { get; set; }
        public int id_ns { get; set; }
        public string luong_ma { get; set; }
        public int luong_bac { get; set; }
        public int luong_muc { get; set; }
        public int luong_kieu { get; set; }
        public DateTime luong_ngay { get; set; }
    }
}
