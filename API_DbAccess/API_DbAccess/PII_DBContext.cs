using System;
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

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Dress> Dress { get; set; }
        public virtual DbSet<DressOrder> DressOrder { get; set; }
        public virtual DbSet<Favorites> Favorites { get; set; }
        public virtual DbSet<OrderLine> OrderLine { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<SentencesOfTheDay> SentencesOfTheDay { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost; Database=PII_DB; User Id=SA; Password=24Naruto24;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__customer__AB6E6164BB83282C")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ__customer__A1936A6BB530937C")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerAddress)
                    .HasColumnName("customer_address")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPassword)
                    .IsRequired()
                    .HasColumnName("customer_password")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FidelityPoints).HasColumnName("fidelity_points");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Dress>(entity =>
            {
                entity.ToTable("dress");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Availible).HasColumnName("availible");

                entity.Property(e => e.DateBeginAvailable)
                    .HasColumnName("date_begin_available")
                    .HasColumnType("date");

                entity.Property(e => e.DateEndAvailable)
                    .HasColumnName("date_end_available")
                    .HasColumnType("date");

                entity.Property(e => e.Describe)
                    .IsRequired()
                    .HasColumnName("describe")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DressName)
                    .IsRequired()
                    .HasColumnName("dress_name")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PartnersId).HasColumnName("partners_id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Partners)
                    .WithMany(p => p.Dress)
                    .HasForeignKey(d => d.PartnersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__dress__partners___412EB0B6");
            });

            modelBuilder.Entity<DressOrder>(entity =>
            {
                entity.ToTable("dress_order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BillingAddress)
                    .HasColumnName("billing_address")
                    .HasColumnType("date");

                entity.Property(e => e.BillingDate)
                    .HasColumnName("billing_date")
                    .HasColumnType("date");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DeliveryAddress)
                    .HasColumnName("delivery_address")
                    .HasColumnType("date");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnName("delivery_date")
                    .HasColumnType("date");

                entity.Property(e => e.IsValid).HasColumnName("isValid");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DressOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__dress_ord__custo__440B1D61");
            });

            modelBuilder.Entity<Favorites>(entity =>
            {
                entity.ToTable("favorites");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DressId).HasColumnName("dress_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__favorites__custo__4AB81AF0");

                entity.HasOne(d => d.Dress)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.DressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__favorites__dress__4BAC3F29");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.ToTable("order_line");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DateBeginLocation)
                    .HasColumnName("date_begin_location")
                    .HasColumnType("date");

                entity.Property(e => e.DateEndLocation)
                    .HasColumnName("date_end_location")
                    .HasColumnType("date");

                entity.Property(e => e.DressId).HasColumnName("dress_id");

                entity.Property(e => e.DressOrderId).HasColumnName("dress_order_id");

                entity.Property(e => e.FinalPrice)
                    .HasColumnName("final_price")
                    .HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Dress)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.DressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__order_lin__dress__47DBAE45");

                entity.HasOne(d => d.DressOrder)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.DressOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__order_lin__dress__46E78A0C");
            });

            modelBuilder.Entity<Partners>(entity =>
            {
                entity.ToTable("partners");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__partners__AB6E616497AC2144")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ__partners__A1936A6B6BED9215")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PartnerAddress)
                    .HasColumnName("partner_address")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SentencesOfTheDay>(entity =>
            {
                entity.ToTable("sentences_of_the_day");

                entity.HasIndex(e => e.Sentence)
                    .HasName("UQ__sentence__80874A3045983A0B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Sentence)
                    .IsRequired()
                    .HasColumnName("sentence")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
