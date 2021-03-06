﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API_DbAccess
{
    public partial class PII_DBContext : IdentityDbContext<User>
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
            if (!optionsBuilder.IsConfigured)
            {
                /*IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DressDatabase"));*/
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Dress>().HasKey(d => d.Id);
            builder.Entity<DressOrder>().HasKey(d => d.Id);
            builder.Entity<OrderLine>().HasKey(o => o.Id);
            builder.Entity<Favorites>().HasKey(f => f.Id);
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<SentencesOfTheDay>().HasKey(s => s.Id);
            base.OnModelCreating(builder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
