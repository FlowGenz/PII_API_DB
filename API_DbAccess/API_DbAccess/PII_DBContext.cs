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
                optionsBuilder.UseSqlServer("Server=localhost,1433; Database=PII_DB; User Id=SA; Password=24Naruto24;");
            }*/
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dress>(entity =>
            {
                entity.ToTable("dress");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Available).HasColumnName("available");

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

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(6, 2)");

                entity.Property(e => e.UrlImage)
                    .IsRequired()
                    .HasColumnName("urlImage")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Dress)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__dress__user_id__3E52440B");
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

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasColumnName("delivery_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate)
                    .HasColumnName("delivery_date")
                    .HasColumnType("date");

                entity.Property(e => e.IsValid).HasColumnName("isValid");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DressOrder)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__dress_ord__user___412EB0B6");
            });

            modelBuilder.Entity<Favorites>(entity =>
            {
                entity.ToTable("favorites");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DressId).HasColumnName("dress_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Dress)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.DressId)
                    .HasConstraintName("FK__favorites__dress__48CFD27E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__favorites__user___47DBAE45");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.ToTable("order_line");

                entity.Property(e => e.Id).HasColumnName("id");

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
                    .HasColumnType("decimal(6, 2)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Dress)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.DressId)
                    .HasConstraintName("FK__order_lin__dress__44FF419A");

                entity.HasOne(d => d.DressOrder)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.DressOrderId)
                    .HasConstraintName("FK__order_lin__dress__440B1D61");
            });

            modelBuilder.Entity<SentencesOfTheDay>(entity =>
            {
                entity.ToTable("sentences_of_the_day");

                entity.HasIndex(e => e.Sentence)
                    .HasName("UQ__sentence__80874A30D8E32EC5")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Sentence)
                    .IsRequired()
                    .HasColumnName("sentence")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("_user");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ___user__AB6E6164728C1673")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ___user__A1936A6B2A9E684A")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("UQ___user__F3DBC572D5E557DA")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(30)
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

                entity.Property(e => e.LoyaltyPoints).HasColumnName("loyalty_points");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.UserAddress)
                    .HasColumnName("user_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("user_password")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
