﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_DbAccess
{
    public partial class PII_DBContext : DbContext
    {
        public PII_DBContext()
        {
        }

        public PII_DBContext(DbContextOptions<PII_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dress> Dress { get; set; }
        public virtual DbSet<DressOrder> DressOrder { get; set; }
        public virtual DbSet<Favorites> Favorites { get; set; }
        public virtual DbSet<OrderLine> OrderLine { get; set; }
        public virtual DbSet<SentencesOfTheDay> SentencesOfTheDay { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost,1433; Database=PII_DB_IG; User Id=SA; Password=24Naruto24;");
            }*/
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
