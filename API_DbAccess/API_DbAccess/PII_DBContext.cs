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
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost,1433; Database=PII_DB; User Id=SA; Password=24Naruto24;");
            }*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__customer__AB6E61641FD9C4A8")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ__customer__A1936A6B4E857A98")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerAddress)
                    .HasColumnName("customer_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FidelityPoints).HasColumnName("fidelity_points");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.UsernameUser)
                    .IsRequired()
                    .HasColumnName("username_user")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsernameUserNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.UsernameUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__customer__userna__4222D4EF");
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
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DressName)
                    .IsRequired()
                    .HasColumnName("dress_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PartnersId).HasColumnName("partners_id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UrlImage)
                    .IsRequired()
                    .HasColumnName("urlImage")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Partners)
                    .WithMany(p => p.Dress)
                    .HasForeignKey(d => d.PartnersId)
                    .HasConstraintName("FK__dress__partners___44FF419A");
            });

            modelBuilder.Entity<DressOrder>(entity =>
            {
                entity.ToTable("dress_order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BillingAddress)
                    .IsRequired()
                    .HasColumnName("billing_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillingDate)
                    .HasColumnName("billing_date")
                    .HasColumnType("date");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasColumnName("delivery_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate)
                    .HasColumnName("delivery_date")
                    .HasColumnType("date");

                entity.Property(e => e.IsValid).HasColumnName("isValid");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DressOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__dress_ord__custo__47DBAE45");
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
                    .HasConstraintName("FK__favorites__custo__4E88ABD4");

                entity.HasOne(d => d.Dress)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.DressId)
                    .HasConstraintName("FK__favorites__dress__4F7CD00D");
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
                    .HasConstraintName("FK__order_lin__dress__4BAC3F29");

                entity.HasOne(d => d.DressOrder)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.DressOrderId)
                    .HasConstraintName("FK__order_lin__dress__4AB81AF0");
            });

            modelBuilder.Entity<Partners>(entity =>
            {
                entity.ToTable("partners");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__partners__AB6E6164D1A14C54")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ__partners__A1936A6BE616B6E5")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PartnerAddress)
                    .HasColumnName("partner_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.UsernameUser)
                    .IsRequired()
                    .HasColumnName("username_user")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsernameUserNavigation)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.UsernameUser)
                    .HasConstraintName("FK__partners__userna__3D5E1FD2");
            });

            modelBuilder.Entity<SentencesOfTheDay>(entity =>
            {
                entity.ToTable("sentences_of_the_day");

                entity.HasIndex(e => e.Sentence)
                    .HasName("UQ__sentence__80874A30C00368D3")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Sentence)
                    .IsRequired()
                    .HasColumnName("sentence")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__users__F3DBC5738A51835F");

                entity.ToTable("users");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Privilege)
                    .IsRequired()
                    .HasColumnName("privilege")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("user_password")
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
