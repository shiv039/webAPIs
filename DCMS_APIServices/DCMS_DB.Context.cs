﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCMS_APIServices
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ITFC_DCMSEntities : DbContext
    {
        public ITFC_DCMSEntities()
            : base("name=ITFC_DCMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ApproverStack> ApproverStacks { get; set; }
        public virtual DbSet<AuditTrial> AuditTrials { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }
}
