//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cabincrew.Model.HR
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("chucdanh")]
    public partial class chucdanh
    {
        public int id { get; set; }
        public int id_ns { get; set; }
        [Column("chucdanh")]
        public int chucdanh1 { get; set; }
        public bool chucdanh_noilam { get; set; }
        public System.DateTime chucdanh_ngay { get; set; }
        public Nullable<System.DateTime> chucdanh_ngayhet { get; set; }
        public string chucdang_soqd { get; set; }
        public Nullable<System.DateTime> chucdanh_ngayky { get; set; }
    
        
    }
}
