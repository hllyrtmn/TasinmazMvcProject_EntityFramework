﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TasinmazMvc.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TasinmazDbEntities : DbContext
    {
        public TasinmazDbEntities()
            : base("name=TasinmazDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BILGI> BILGI { get; set; }
        public virtual DbSet<IL> IL { get; set; }
        public virtual DbSet<ILCE> ILCE { get; set; }
        public virtual DbSet<KULLANICI> KULLANICI { get; set; }
        public virtual DbSet<LOG> LOG { get; set; }
        public virtual DbSet<MAHALLE> MAHALLE { get; set; }
        public virtual DbSet<SEMT> SEMT { get; set; }
    }
}
