using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AlisverisSepeti.Models
{
    public partial class AlisverisSepetiContext : DbContext
    {
        public AlisverisSepetiContext()
        {
        }

        public AlisverisSepetiContext(DbContextOptions<AlisverisSepetiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Birimler> Birimlers { get; set; }
        public virtual DbSet<Diller> Dillers { get; set; }
        public virtual DbSet<Dovizler> Dovizlers { get; set; }
        public virtual DbSet<Markalar> Markalars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;port=3306;uid=root;pwd=190598;database=AlisverisSepeti");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Birimler>(entity =>
            {
                entity.HasKey(e => e.BirimId)
                    .HasName("PRIMARY");

                entity.ToTable("birimler");

                entity.Property(e => e.BirimId).HasColumnName("BirimID");

                entity.Property(e => e.Birim)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(50);
            });

            modelBuilder.Entity<Diller>(entity =>
            {
                entity.HasKey(e => e.DilId)
                    .HasName("PRIMARY");

                entity.ToTable("diller");

                entity.HasIndex(e => e.BolgeDilAdi, "BolgeDilAdi_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.BolgeDilAdi)
                    .IsRequired()
                    .HasMaxLength(23);

                entity.Property(e => e.DilAdi)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.DilKodu)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.DilLogo).HasMaxLength(100);

                entity.Property(e => e.DovizKodu).HasMaxLength(3);
            });

            modelBuilder.Entity<Dovizler>(entity =>
            {
                entity.HasKey(e => e.DovizId)
                    .HasName("PRIMARY");

                entity.ToTable("dovizler");

                entity.Property(e => e.DovizId).HasColumnName("DovizID");

                entity.Property(e => e.Aktifmi).HasColumnType("tinyint");

                entity.Property(e => e.DovizAdi)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.DovizAdiGlobal)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.DovizKodu)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Kur)
                    .IsRequired()
                    .HasMaxLength(4);

                entity.Property(e => e.Sembol).HasMaxLength(5);

                entity.Property(e => e.Tarih).HasMaxLength(50);
            });

            modelBuilder.Entity<Markalar>(entity =>
            {
                entity.HasKey(e => e.MarkaId)
                    .HasName("PRIMARY");

                entity.ToTable("markalar");

                entity.Property(e => e.MarkaId).HasColumnName("MarkaID");

                entity.Property(e => e.MarkaAdi)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.MarkaBanner).HasMaxLength(50);

                entity.Property(e => e.MarkaHakkinda).HasMaxLength(300);

                entity.Property(e => e.MarkaLogo).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
