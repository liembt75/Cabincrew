﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HREntities : DbContext
    {
        public HREntities()
            : base("name=HREntities")
        {
        }
    
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    throw new UnintentionalCodeFirstException();
        //}
    
        public DbSet<chucdanh> chucdanhs { get; set; }
        public DbSet<chucvu> chucvus { get; set; }
        public DbSet<danhmuc> danhmucs { get; set; }
        public DbSet<HoSoGoc> HoSoGocs { get; set; }
        public DbSet<khenkluat> khenkluats { get; set; }
        public DbSet<PView_toeic> PView_toeics { get; set; }
        public DbSet<Laodong> Laodongs { get; set; }
        public DbSet<ngoaingu> ngoaingus { get; set; }
        public DbSet<quanlyhcnn> quanlyhcnns { get; set; }
        public DbSet<mucluongtl> mucluongtls { get; set; } 
        
        public DbSet<luonghd> luonghds { get; set; }

        public DbSet<mucluong> mucluongs { get; set; }
        public DbSet<nhomchuyenmon> nhomchuyenmons { get; set; }
    }
}
