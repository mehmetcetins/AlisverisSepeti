﻿using System;
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
        public virtual DbSet<Degiskentipleri> Degiskentipleris { get; set; }
        public virtual DbSet<Dilbolgeler> Dilbolgelers { get; set; }
        public virtual DbSet<Diller> Dillers { get; set; }
        public virtual DbSet<Dovizkurlari> Dovizkurlaris { get; set; }
        public virtual DbSet<Dovizler> Dovizlers { get; set; }
        public virtual DbSet<Gonderimsekilleri> Gonderimsekilleris { get; set; }
        public virtual DbSet<Havalebankalari> Havalebankalaris { get; set; }
        public virtual DbSet<Kargolar> Kargolars { get; set; }
        public virtual DbSet<Karttaksitleri> Karttaksitleris { get; set; }
        public virtual DbSet<Kredikartlari> Kredikartlaris { get; set; }
        public virtual DbSet<Lang> Langs { get; set; }
        public virtual DbSet<Markalar> Markalars { get; set; }
        public virtual DbSet<Odemesecenekleri> Odemesecenekleris { get; set; }
        public virtual DbSet<Opsiyontipleri> Opsiyontipleris { get; set; }
        public virtual DbSet<Poslar> Poslars { get; set; }
        public virtual DbSet<Stokdurum> Stokdurums { get; set; }
        public virtual DbSet<Urunopsiyonlar> Urunopsiyonlars { get; set; }
        public virtual DbSet<Uruntipleri> Uruntipleris { get; set; }
        public virtual DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<Degiskentipleri>(entity =>
            {
                entity.ToTable("degiskentipleri");

                entity.Property(e => e.DegiskenAdi)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("degiskenAdi");
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

            modelBuilder.Entity<Lang>(entity =>
            {
                entity.ToTable("lang");

                entity.Property(e => e.Aktifmi).HasColumnName("aktifmi");

                entity.Property(e => e.Title2)
                    .IsRequired()
                    .HasMaxLength(30);
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

            modelBuilder.Entity<Odemesecenekleri>(entity =>
            {
                entity.ToTable("odemesecenekleri");

                entity.Property(e => e.DizilisSira).HasDefaultValueSql("'0'");

                entity.Property(e => e.OdemeSekli)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Opsiyontipleri>(entity =>
            {
                entity.ToTable("opsiyontipleri");

                entity.Property(e => e.Ismi)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ismi");
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

            modelBuilder.Entity<Stokdurum>(entity =>
            {
                entity.ToTable("stokdurum");

                entity.HasIndex(e => e.EkleyenId, "lnk_users_stokdurum");

                entity.HasIndex(e => e.GuncelleyenId, "lnk_users_stokdurum_2");

                entity.Property(e => e.StokDurumId).HasColumnName("StokDurumID");

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(19);

                entity.Property(e => e.Ekleyen)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.EkleyenId).HasColumnName("EkleyenID");

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(19);

                entity.Property(e => e.Guncelleyen).HasMaxLength(30);

                entity.Property(e => e.GuncelleyenId).HasColumnName("GuncelleyenID");

                entity.Property(e => e.StokDurumKod)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.StokDurumResim).HasMaxLength(50);

                entity.HasOne(d => d.EkleyenNavigation)
                    .WithMany(p => p.StokdurumEkleyenNavigations)
                    .HasForeignKey(d => d.EkleyenId)
                    .HasConstraintName("lnk_users_stokdurum");

                entity.HasOne(d => d.GuncelleyenNavigation)
                    .WithMany(p => p.StokdurumGuncelleyenNavigations)
                    .HasForeignKey(d => d.GuncelleyenId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lnk_users_stokdurum_2");
            });

            modelBuilder.Entity<Urunopsiyonlar>(entity =>
            {
                entity.ToTable("urunopsiyonlar");

                entity.HasIndex(e => e.DegiskenId, "lnk_degiskentipleri_urunopsiyonlar");

                entity.HasIndex(e => e.OpsiyonTipi, "lnk_opsiyontipleri_urunopsiyonlar");

                entity.Property(e => e.OpsiyonAdi).HasMaxLength(20);

                entity.HasOne(d => d.Degisken)
                    .WithMany(p => p.Urunopsiyonlars)
                    .HasForeignKey(d => d.DegiskenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_degiskentipleri_urunopsiyonlar");

                entity.HasOne(d => d.OpsiyonTipiNavigation)
                    .WithMany(p => p.Urunopsiyonlars)
                    .HasForeignKey(d => d.OpsiyonTipi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_opsiyontipleri_urunopsiyonlar");
            });

            modelBuilder.Entity<Uruntipleri>(entity =>
            {
                entity.HasKey(e => e.UrunTipiId)
                    .HasName("PRIMARY");

                entity.ToTable("uruntipleri");

                entity.Property(e => e.UrunTipiId).HasColumnName("UrunTipiID");

                entity.Property(e => e.UrunTipi)
                    .IsRequired()
                    .HasMaxLength(8);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Durum).HasMaxLength(50);

                entity.Property(e => e.DurumTxt).HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.KullaniciIsim)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.KullaniciTipi)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(16);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
