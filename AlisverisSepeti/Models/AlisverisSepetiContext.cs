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
        public virtual DbSet<Dovizler> Dovizlers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=190598;database=AlisverisSepeti");
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
                    .HasMaxLength(3);

                entity.Property(e => e.Sembol).HasMaxLength(0);

                entity.Property(e => e.Tarih).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
