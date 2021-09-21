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
        public virtual DbSet<Dilbolgeler> Dilbolgelers { get; set; }
        public virtual DbSet<Diller> Dillers { get; set; }
        public virtual DbSet<Dovizkurlari> Dovizkurlaris { get; set; }
        public virtual DbSet<Dovizler> Dovizlers { get; set; }
        public virtual DbSet<Gonderimsekilleri> Gonderimsekilleris { get; set; }
        public virtual DbSet<Havalebankalari> Havalebankalaris { get; set; }
        public virtual DbSet<Kargolar> Kargolars { get; set; }
        public virtual DbSet<Karttaksitleri> Karttaksitleris { get; set; }
        public virtual DbSet<Kredikartlari> Kredikartlaris { get; set; }
        public virtual DbSet<Markalar> Markalars { get; set; }
        public virtual DbSet<Poslar> Poslars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;port=3306;uid=root;pwd=190598;database=AlisverisSepeti;");
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

            modelBuilder.Entity<Dilbolgeler>(entity =>
            {
                entity.HasKey(e => e.BolgeId)
                    .HasName("PRIMARY");

                entity.ToTable("dilbolgeler");

                entity.Property(e => e.BolgeId).HasColumnName("BolgeID");

                entity.Property(e => e.BolgeDilAdi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DilKodu)
                    .IsRequired()
                    .HasMaxLength(10);
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

            modelBuilder.Entity<Dovizkurlari>(entity =>
            {
                entity.HasKey(e => e.DovizKurId)
                    .HasName("PRIMARY");

                entity.ToTable("dovizkurlari");

                entity.HasIndex(e => e.DovizId, "unique_DovizID")
                    .IsUnique();

                entity.Property(e => e.DovizKurId).HasColumnName("DovizKurID");

                entity.Property(e => e.DovizId).HasColumnName("DovizID");

                entity.Property(e => e.DovizKodu)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Tarih)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.Doviz)
                    .WithOne(p => p.Dovizkurlari)
                    .HasForeignKey<Dovizkurlari>(d => d.DovizId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lnk_dovizkurlari_dovizler");
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

            modelBuilder.Entity<Gonderimsekilleri>(entity =>
            {
                entity.HasKey(e => e.GonderimId)
                    .HasName("PRIMARY");

                entity.ToTable("gonderimsekilleri");

                entity.Property(e => e.DizilisSira).HasDefaultValueSql("'-1'");

                entity.Property(e => e.GonderimSekli)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.MinTutar).HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Havalebankalari>(entity =>
            {
                entity.HasKey(e => e.HavaleBankaId)
                    .HasName("PRIMARY");

                entity.ToTable("havalebankalari");

                entity.HasIndex(e => e.HesapNo, "HesapNo_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Iban, "IBAN_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.SubeKodu, "SubeKodu_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.HavaleBankaId).HasColumnName("HavaleBankaID");

                entity.Property(e => e.BankaAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.BankaLogo).HasMaxLength(50);

                entity.Property(e => e.DovizKodu).HasMaxLength(10);

                entity.Property(e => e.HesapNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("IBAN");

                entity.Property(e => e.SubeAdi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubeKodu)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Ydaktifmi).HasColumnName("YDAktifmi");

                entity.Property(e => e.Yiaktifmi).HasColumnName("YIAktifmi");
            });

            modelBuilder.Entity<Kargolar>(entity =>
            {
                entity.HasKey(e => e.KargoId)
                    .HasName("PRIMARY");

                entity.ToTable("kargolar");

                entity.Property(e => e.KargoId).HasColumnName("KargoID");

                entity.Property(e => e.KargoAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.KargoBedeliDovizKodu).HasMaxLength(10);

                entity.Property(e => e.KargoLogo).HasMaxLength(100);

                entity.Property(e => e.UcretsizKargoBedeliDovizKodu).HasMaxLength(10);

                entity.Property(e => e.YdgonderimVarmi).HasColumnName("YDGonderimVarmi");

                entity.Property(e => e.YigonderimVarmi).HasColumnName("YIGonderimVarmi");
            });

            modelBuilder.Entity<Karttaksitleri>(entity =>
            {
                entity.HasKey(e => e.TaksitId)
                    .HasName("PRIMARY");

                entity.ToTable("karttaksitleri");

                entity.HasIndex(e => e.KartId, "lnk_kredikartlari_karttaksitleri");

                entity.Property(e => e.TaksitId).HasColumnName("TaksitID");

                entity.Property(e => e.Durum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.KartId).HasColumnName("KartID");

                entity.Property(e => e.TaksitAciklama).HasMaxLength(200);

                entity.Property(e => e.TaksitOran).HasDefaultValueSql("'1'");

                entity.Property(e => e.TaksitSayisi)
                    .HasColumnType("tinyint")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Kart)
                    .WithMany(p => p.Karttaksitleris)
                    .HasForeignKey(d => d.KartId)
                    .HasConstraintName("lnk_kredikartlari_karttaksitleri");
            });

            modelBuilder.Entity<Kredikartlari>(entity =>
            {
                entity.HasKey(e => e.KartId)
                    .HasName("PRIMARY");

                entity.ToTable("kredikartlari");

                entity.HasIndex(e => e.PosId, "lnk_poslar_kredikartlari");

                entity.Property(e => e.KartId).HasColumnName("KartID");

                entity.Property(e => e.DizilisSira).HasDefaultValueSql("'0'");

                entity.Property(e => e.GecerliKartlar).HasMaxLength(100);

                entity.Property(e => e.KartAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.KartLogo).HasMaxLength(100);

                entity.Property(e => e.PosBankaAdi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.Ydaktifmi).HasColumnName("YDAktifmi");

                entity.Property(e => e.Yiaktifmi).HasColumnName("YIAktifmi");

                entity.HasOne(d => d.Pos)
                    .WithMany(p => p.Kredikartlaris)
                    .HasForeignKey(d => d.PosId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lnk_poslar_kredikartlari");
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

            modelBuilder.Entity<Poslar>(entity =>
            {
                entity.HasKey(e => e.PosId)
                    .HasName("PRIMARY");

                entity.ToTable("poslar");

                entity.Property(e => e.PosId).HasColumnName("PosID");

                entity.Property(e => e.ApiPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ApiUser)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DizilisSira).HasDefaultValueSql("'-1'");

                entity.Property(e => e.GecerliKartlar).HasMaxLength(100);

                entity.Property(e => e.PosBankaAdi)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
