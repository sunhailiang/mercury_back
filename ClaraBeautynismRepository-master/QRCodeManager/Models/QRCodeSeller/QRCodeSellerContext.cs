using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QRCodeManager.Models.QRCodeSeller
{
    public partial class QRCodeSellerContext : DbContext
    {
        //public QRCodeSellerContext()
        //{
        //}

        public QRCodeSellerContext(DbContextOptions<QRCodeSellerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<QrcodeSellerList> QrcodeSellerList { get; set; }
        public virtual DbSet<SellerUserMapping> SellerUserMapping { get; set; }
        public virtual DbSet<Commission> Commission { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=clarabeautynism.com,61433;Initial Catalog=QRCodeSeller; User ID = QRCodeSeller; Password = M$4d!n0urbR2vIIgibzk7%%A7udaXrdW");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<QrcodeSellerList>(entity =>
            {
                entity.HasKey(e => e.SellerId);

                entity.ToTable("QRCodeSellerList");

                entity.Property(e => e.SellerId)
                    .HasColumnName("SellerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.IdentityCardNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SellerUserMapping>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PK__SellerUs__FBDF78C96D2AC288");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.Property(e => e.CostumerId).HasColumnName("CostumerID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
        }
    }
}
