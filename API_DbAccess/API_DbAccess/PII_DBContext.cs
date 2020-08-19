using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
    
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Dress>().Property(p => p.RowVersion).IsConcurrencyToken();
            builder.Entity<User>().Property(p => p.RowVersion).IsConcurrencyToken();
            builder.Entity<DressOrder>().Property(p => p.RowVersion).IsConcurrencyToken();
            base.OnModelCreating(builder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
