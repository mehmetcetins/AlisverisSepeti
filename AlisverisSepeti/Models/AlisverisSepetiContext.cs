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
        public virtual DbSet<Ozellikdegerleri> Ozellikdegerleris { get; set; }
        public virtual DbSet<OzellikdegerleriDil> OzellikdegerleriDils { get; set; }
        public virtual DbSet<Ozellikgrup> Ozellikgrups { get; set; }
        public virtual DbSet<OzellikgrupDil> OzellikgrupDils { get; set; }
        public virtual DbSet<Ozellikler> Ozelliklers { get; set; }
        public virtual DbSet<OzelliklerDil> OzelliklerDils { get; set; }
        public virtual DbSet<Ozelliktipleri> Ozelliktipleris { get; set; }
        public virtual DbSet<Poslar> Poslars { get; set; }
        public virtual DbSet<Stokdurum> Stokdurums { get; set; }
        public virtual DbSet<StokdurumDil> StokdurumDils { get; set; }
        public virtual DbSet<Urundosyalar> Urundosyalars { get; set; }
        public virtual DbSet<Urunkategoriler> Urunkategorilers { get; set; }
        public virtual DbSet<UrunkategorilerDil> UrunkategorilerDils { get; set; }
        public virtual DbSet<Urunler> Urunlers { get; set; }
        public virtual DbSet<UrunlerDil> UrunlerDils { get; set; }
        public virtual DbSet<Urunopsiyonlar> Urunopsiyonlars { get; set; }
        public virtual DbSet<Urunozellikleri> Urunozellikleris { get; set; }
        public virtual DbSet<Urunsekilleri> Urunsekilleris { get; set; }
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

            modelBuilder.Entity<Ozellikdegerleri>(entity =>
            {
                entity.HasKey(e => e.OzellikDegerId)
                    .HasName("PRIMARY");

                entity.ToTable("ozellikdegerleri");

                entity.HasIndex(e => e.OzellikId, "lnk_ozellikler_ozellikdegerleri");

                entity.Property(e => e.OzellikDegerId).HasColumnName("OzellikDegerID");

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(30);

                entity.Property(e => e.OzellikId).HasColumnName("OzellikID");

                entity.HasOne(d => d.Ozellik)
                    .WithMany(p => p.Ozellikdegerleris)
                    .HasForeignKey(d => d.OzellikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozellikler_ozellikdegerleri");
            });

            modelBuilder.Entity<OzellikdegerleriDil>(entity =>
            {
                entity.HasKey(e => e.OzellikDegerDilId)
                    .HasName("PRIMARY");

                entity.ToTable("ozellikdegerleri_dil");

                entity.HasIndex(e => e.DilId, "lnk_diller_ozellikdegerleri_dil");

                entity.HasIndex(e => e.OzellikDegerId, "lnk_ozellikdegerleri_ozellikdegerleri_dil");

                entity.Property(e => e.OzellikDegerDilId).HasColumnName("OzellikDegerDilID");

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.OzellikDeger)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.OzellikDegerId).HasColumnName("OzellikDegerID");

                entity.HasOne(d => d.Dil)
                    .WithMany(p => p.OzellikdegerleriDils)
                    .HasForeignKey(d => d.DilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_diller_ozellikdegerleri_dil");

                entity.HasOne(d => d.OzellikDegerNavigation)
                    .WithMany(p => p.OzellikdegerleriDils)
                    .HasForeignKey(d => d.OzellikDegerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozellikdegerleri_ozellikdegerleri_dil");
            });

            modelBuilder.Entity<Ozellikgrup>(entity =>
            {
                entity.ToTable("ozellikgrup");

                entity.HasIndex(e => e.EkleyenId, "lnk_users_ozellikgrup");

                entity.HasIndex(e => e.GuncelleyenId, "lnk_users_ozellikgrup_2");

                entity.Property(e => e.OzellikGrupId).HasColumnName("OzellikGrupID");

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Ekleyen)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.EkleyenId).HasColumnName("EkleyenID");

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(30);

                entity.Property(e => e.Guncelleyen).HasMaxLength(30);

                entity.Property(e => e.GuncelleyenId).HasColumnName("GuncelleyenID");

                entity.HasOne(d => d.EkleyenNavigation)
                    .WithMany(p => p.OzellikgrupEkleyenNavigations)
                    .HasForeignKey(d => d.EkleyenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_users_ozellikgrup");

                entity.HasOne(d => d.GuncelleyenNavigation)
                    .WithMany(p => p.OzellikgrupGuncelleyenNavigations)
                    .HasForeignKey(d => d.GuncelleyenId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lnk_users_ozellikgrup_2");
            });

            modelBuilder.Entity<OzellikgrupDil>(entity =>
            {
                entity.ToTable("ozellikgrup_dil");

                entity.HasIndex(e => e.DilId, "lnk_diller_ozellikgrup_dil");

                entity.HasIndex(e => e.OzellikGrupId, "lnk_ozellikgrup_ozellikgrup_dil");

                entity.Property(e => e.OzellikGrupDilId).HasColumnName("OzellikGrupDilID");

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.OzellikGrupAciklama).HasColumnType("tinytext");

                entity.Property(e => e.OzellikGrupAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OzellikGrupId).HasColumnName("OzellikGrupID");

                entity.HasOne(d => d.Dil)
                    .WithMany(p => p.OzellikgrupDils)
                    .HasForeignKey(d => d.DilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_diller_ozellikgrup_dil");

                entity.HasOne(d => d.OzellikGrup)
                    .WithMany(p => p.OzellikgrupDils)
                    .HasForeignKey(d => d.OzellikGrupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozellikgrup_ozellikgrup_dil");
            });

            modelBuilder.Entity<Ozellikler>(entity =>
            {
                entity.HasKey(e => e.OzellikId)
                    .HasName("PRIMARY");

                entity.ToTable("ozellikler");

                entity.HasIndex(e => e.OzellikGrupId, "lnk_ozellikgrup_ozellikler");

                entity.HasIndex(e => e.OzellikTipiId, "lnk_ozelliktipleri_ozellikler");

                entity.Property(e => e.OzellikId).HasColumnName("OzellikID");

                entity.Property(e => e.Birim).HasMaxLength(10);

                entity.Property(e => e.DegiskenTipi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(30);

                entity.Property(e => e.OzellikGrupId).HasColumnName("OzellikGrupID");

                entity.Property(e => e.OzellikTipi)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.OzellikTipiId).HasColumnName("OzellikTipiID");

                entity.HasOne(d => d.OzellikGrup)
                    .WithMany(p => p.Ozelliklers)
                    .HasForeignKey(d => d.OzellikGrupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozellikgrup_ozellikler");

                entity.HasOne(d => d.OzellikTipiNavigation)
                    .WithMany(p => p.Ozelliklers)
                    .HasForeignKey(d => d.OzellikTipiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozelliktipleri_ozellikler");
            });

            modelBuilder.Entity<OzelliklerDil>(entity =>
            {
                entity.HasKey(e => e.OzellikDilId)
                    .HasName("PRIMARY");

                entity.ToTable("ozellikler_dil");

                entity.HasIndex(e => e.DilId, "lnk_diller_ozellikler_dil");

                entity.HasIndex(e => e.OzellikId, "lnk_ozellikler_ozellikler_dil");

                entity.Property(e => e.OzellikDilId).HasColumnName("OzellikDilID");

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.OzellikAciklama).HasColumnType("tinytext");

                entity.Property(e => e.OzellikAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OzellikId).HasColumnName("OzellikID");

                entity.HasOne(d => d.Dil)
                    .WithMany(p => p.OzelliklerDils)
                    .HasForeignKey(d => d.DilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_diller_ozellikler_dil");

                entity.HasOne(d => d.Ozellik)
                    .WithMany(p => p.OzelliklerDils)
                    .HasForeignKey(d => d.OzellikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozellikler_ozellikler_dil");
            });

            modelBuilder.Entity<Ozelliktipleri>(entity =>
            {
                entity.HasKey(e => e.OzellikTipiId)
                    .HasName("PRIMARY");

                entity.ToTable("ozelliktipleri");

                entity.HasIndex(e => e.DegiskenTipi, "lnk_degiskentipleri_ozelliktipleri");

                entity.Property(e => e.OzellikTipiId).HasColumnName("OzellikTipiID");

                entity.Property(e => e.Durum).HasMaxLength(5);

                entity.Property(e => e.OzellikTipi).HasMaxLength(15);

                entity.Property(e => e.Tanim).HasMaxLength(21);

                entity.HasOne(d => d.DegiskenTipiNavigation)
                    .WithMany(p => p.Ozelliktipleris)
                    .HasForeignKey(d => d.DegiskenTipi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_degiskentipleri_ozelliktipleri");
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

            modelBuilder.Entity<StokdurumDil>(entity =>
            {
                entity.ToTable("stokdurum_dil");

                entity.HasIndex(e => e.DilId, "lnk_diller_stokdurum_dil");

                entity.HasIndex(e => e.StokDurumId, "lnk_stokdurum_stokdurum_dil");

                entity.Property(e => e.StokDurumDilId).HasColumnName("StokDurumDilID");

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.StokDurum)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.StokDurumAciklama).HasColumnType("tinytext");

                entity.Property(e => e.StokDurumId).HasColumnName("StokDurumID");

                entity.HasOne(d => d.Dil)
                    .WithMany(p => p.StokdurumDils)
                    .HasForeignKey(d => d.DilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_diller_stokdurum_dil");

                entity.HasOne(d => d.StokDurumNavigation)
                    .WithMany(p => p.StokdurumDils)
                    .HasForeignKey(d => d.StokDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_stokdurum_stokdurum_dil");
            });

            modelBuilder.Entity<Urundosyalar>(entity =>
            {
                entity.HasKey(e => e.UrunDosyaId)
                    .HasName("PRIMARY");

                entity.ToTable("urundosyalar");

                entity.HasIndex(e => e.UrunId, "lnk_urunler_urundosyalar");

                entity.Property(e => e.UrunDosyaId).HasColumnName("UrunDosyaID");

                entity.Property(e => e.DosyaBaslik).HasMaxLength(100);

                entity.Property(e => e.DosyaBilgi).HasMaxLength(200);

                entity.Property(e => e.DosyaTipi)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.UrunId).HasColumnName("UrunID");

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.Urundosyalars)
                    .HasForeignKey(d => d.UrunId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_urunler_urundosyalar");
            });

            modelBuilder.Entity<Urunkategoriler>(entity =>
            {
                entity.HasKey(e => e.KategoriId)
                    .HasName("PRIMARY");

                entity.ToTable("urunkategoriler");

                entity.HasIndex(e => e.PkategoriId, "lnk_urunkategoriler_urunkategoriler");

                entity.HasIndex(e => e.EkleyenId, "lnk_users_urunkategoriler");

                entity.HasIndex(e => e.GuncelleyenId, "lnk_users_urunkategoriler_2");

                entity.Property(e => e.KategoriId).HasColumnName("KategoriID");

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Ekleyen).HasMaxLength(50);

                entity.Property(e => e.EkleyenId).HasColumnName("EkleyenID");

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(30);

                entity.Property(e => e.Guncelleyen).HasMaxLength(50);

                entity.Property(e => e.GuncelleyenId).HasColumnName("GuncelleyenID");

                entity.Property(e => e.KategoriAdi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.KategoriLogo).HasMaxLength(50);

                entity.Property(e => e.PkategoriAdi)
                    .HasMaxLength(30)
                    .HasColumnName("PKategoriAdi");

                entity.Property(e => e.PkategoriId).HasColumnName("PKategoriID");

                entity.HasOne(d => d.EkleyenNavigation)
                    .WithMany(p => p.UrunkategorilerEkleyenNavigations)
                    .HasForeignKey(d => d.EkleyenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_users_urunkategoriler");

                entity.HasOne(d => d.GuncelleyenNavigation)
                    .WithMany(p => p.UrunkategorilerGuncelleyenNavigations)
                    .HasForeignKey(d => d.GuncelleyenId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lnk_users_urunkategoriler_2");

                entity.HasOne(d => d.Pkategori)
                    .WithMany(p => p.InversePkategori)
                    .HasForeignKey(d => d.PkategoriId)
                    .HasConstraintName("lnk_urunkategoriler_urunkategoriler");
            });

            modelBuilder.Entity<UrunkategorilerDil>(entity =>
            {
                entity.HasKey(e => e.KategoriDilId)
                    .HasName("PRIMARY");

                entity.ToTable("urunkategoriler_dil");

                entity.HasIndex(e => e.DilId, "lnk_diller_urunkategoriler_dil");

                entity.HasIndex(e => e.KategoriId, "lnk_urunkategoriler_urunkategoriler_dil");

                entity.Property(e => e.KategoriDilId).HasColumnName("KategoriDilID");

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.KategoriAciklama).HasColumnType("tinytext");

                entity.Property(e => e.KategoriAdi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.KategoriId).HasColumnName("KategoriID");

                entity.Property(e => e.PageDescription).HasColumnType("tinytext");

                entity.Property(e => e.PageKeywords).HasColumnType("tinytext");

                entity.Property(e => e.PageLabels).HasColumnType("tinytext");

                entity.Property(e => e.PageTitle).HasMaxLength(50);

                entity.HasOne(d => d.Dil)
                    .WithMany(p => p.UrunkategorilerDils)
                    .HasForeignKey(d => d.DilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_diller_urunkategoriler_dil");

                entity.HasOne(d => d.Kategori)
                    .WithMany(p => p.UrunkategorilerDils)
                    .HasForeignKey(d => d.KategoriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_urunkategoriler_urunkategoriler_dil");
            });

            modelBuilder.Entity<Urunler>(entity =>
            {
                entity.HasKey(e => e.UrunId)
                    .HasName("PRIMARY");

                entity.ToTable("urunler");

                entity.HasIndex(e => e.MarkaId, "lnk_markalar_urunler");

                entity.HasIndex(e => e.StokDurumId, "lnk_stokdurum_urunler");

                entity.HasIndex(e => e.UrunTipi, "lnk_uruntipleri_urunler");

                entity.HasIndex(e => e.EkleyenId, "lnk_users_urunler");

                entity.HasIndex(e => e.GuncelleyenId, "lnk_users_urunler_2");

                entity.Property(e => e.UrunId).HasColumnName("UrunID");

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ekleyen)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.EkleyenId).HasColumnName("EkleyenID");

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(50);

                entity.Property(e => e.Guncelleyen).HasMaxLength(30);

                entity.Property(e => e.GuncelleyenId).HasColumnName("GuncelleyenID");

                entity.Property(e => e.MarkaId).HasColumnName("MarkaID");

                entity.Property(e => e.StokDurumId).HasColumnName("StokDurumID");

                entity.Property(e => e.UrunBarkodu).HasMaxLength(7);

                entity.Property(e => e.UrunGosterimTarihiBas).HasMaxLength(50);

                entity.Property(e => e.UrunGosterimTarihiBit).HasMaxLength(50);

                entity.Property(e => e.UrunKodu).HasMaxLength(7);

                entity.Property(e => e.UrunMuhasebeKodu).HasMaxLength(7);

                entity.HasOne(d => d.EkleyenNavigation)
                    .WithMany(p => p.UrunlerEkleyenNavigations)
                    .HasForeignKey(d => d.EkleyenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_users_urunler");

                entity.HasOne(d => d.GuncelleyenNavigation)
                    .WithMany(p => p.UrunlerGuncelleyenNavigations)
                    .HasForeignKey(d => d.GuncelleyenId)
                    .HasConstraintName("lnk_users_urunler_2");

                entity.HasOne(d => d.Marka)
                    .WithMany(p => p.Urunlers)
                    .HasForeignKey(d => d.MarkaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_markalar_urunler");

                entity.HasOne(d => d.StokDurum)
                    .WithMany(p => p.Urunlers)
                    .HasForeignKey(d => d.StokDurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_stokdurum_urunler");

                entity.HasOne(d => d.UrunTipiNavigation)
                    .WithMany(p => p.Urunlers)
                    .HasForeignKey(d => d.UrunTipi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_uruntipleri_urunler");
            });

            modelBuilder.Entity<UrunlerDil>(entity =>
            {
                entity.HasKey(e => e.UrunDilId)
                    .HasName("PRIMARY");

                entity.ToTable("urunler_dil");

                entity.HasIndex(e => e.DilId, "lnk_diller_urunler_dil");

                entity.HasIndex(e => e.UrunId, "lnk_urunler_urunler_dil");

                entity.Property(e => e.UrunDilId).HasColumnName("UrunDilID");

                entity.Property(e => e.DilId).HasColumnName("DilID");

                entity.Property(e => e.PageDescription).HasColumnType("tinytext");

                entity.Property(e => e.PageKeywords).HasColumnType("tinytext");

                entity.Property(e => e.PageLabels).HasColumnType("tinytext");

                entity.Property(e => e.PageTitle).HasMaxLength(50);

                entity.Property(e => e.UrunAdi)
                    .IsRequired()
                    .HasMaxLength(41);

                entity.Property(e => e.UrunBilgisi).HasColumnType("tinytext");

                entity.Property(e => e.UrunBilgisiText).HasColumnType("longtext");

                entity.Property(e => e.UrunId).HasColumnName("UrunID");

                entity.Property(e => e.UrunKisaAciklama).HasColumnType("mediumtext");

                entity.HasOne(d => d.Dil)
                    .WithMany(p => p.UrunlerDils)
                    .HasForeignKey(d => d.DilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_diller_urunler_dil");

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.UrunlerDils)
                    .HasForeignKey(d => d.UrunId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_urunler_urunler_dil");
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

            modelBuilder.Entity<Urunozellikleri>(entity =>
            {
                entity.HasKey(e => e.UrunOzellikId)
                    .HasName("PRIMARY");

                entity.ToTable("urunozellikleri");

                entity.HasIndex(e => e.OzellikId, "lnk_ozellikler_urunozellikleri");

                entity.HasIndex(e => e.UrunId, "lnk_urunler_urunozellikleri");

                entity.Property(e => e.UrunOzellikId).HasColumnName("UrunOzellikID");

                entity.Property(e => e.Deger)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.DegiskenTipi).HasMaxLength(20);

                entity.Property(e => e.OzellikAciklama).HasColumnType("tinytext");

                entity.Property(e => e.OzellikAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OzellikId).HasColumnName("OzellikID");

                entity.Property(e => e.OzellikTipi).HasMaxLength(20);

                entity.Property(e => e.UrunId).HasColumnName("UrunID");

                entity.HasOne(d => d.Ozellik)
                    .WithMany(p => p.Urunozellikleris)
                    .HasForeignKey(d => d.OzellikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_ozellikler_urunozellikleri");

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.Urunozellikleris)
                    .HasForeignKey(d => d.UrunId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_urunler_urunozellikleri");
            });

            modelBuilder.Entity<Urunsekilleri>(entity =>
            {
                entity.HasKey(e => e.UrunSekilId)
                    .HasName("PRIMARY");

                entity.ToTable("urunsekilleri");

                entity.HasIndex(e => e.EkleyenId, "lnk_users_urunsekilleri");

                entity.HasIndex(e => e.GuncelleyenId, "lnk_users_urunsekilleri_2");

                entity.HasIndex(e => e.SilenId, "lnk_users_urunsekilleri_3");

                entity.Property(e => e.EklenmeTarihi)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Ekleyen)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EkleyenId).HasColumnName("EkleyenID");

                entity.Property(e => e.GuncellenmeTarihi).HasMaxLength(30);

                entity.Property(e => e.Guncelleyen).HasMaxLength(20);

                entity.Property(e => e.GuncelleyenId).HasColumnName("GuncelleyenID");

                entity.Property(e => e.Silen).HasMaxLength(20);

                entity.Property(e => e.SilenId).HasColumnName("SilenID");

                entity.Property(e => e.UrunSekilAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.EkleyenNavigation)
                    .WithMany(p => p.UrunsekilleriEkleyenNavigations)
                    .HasForeignKey(d => d.EkleyenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lnk_users_urunsekilleri");

                entity.HasOne(d => d.GuncelleyenNavigation)
                    .WithMany(p => p.UrunsekilleriGuncelleyenNavigations)
                    .HasForeignKey(d => d.GuncelleyenId)
                    .HasConstraintName("lnk_users_urunsekilleri_2");

                entity.HasOne(d => d.SilenNavigation)
                    .WithMany(p => p.UrunsekilleriSilenNavigations)
                    .HasForeignKey(d => d.SilenId)
                    .HasConstraintName("lnk_users_urunsekilleri_3");
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
