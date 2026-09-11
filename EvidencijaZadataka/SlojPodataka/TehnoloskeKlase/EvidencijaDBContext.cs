using Microsoft.EntityFrameworkCore;
using SlojPodataka.KlasePodataka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.TehnoloskeKlase
{
    public class EvidencijaDbContext : DbContext
    {
        public EvidencijaDbContext(DbContextOptions<EvidencijaDbContext> options)
            : base(options) { }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<KljucnaTackaRazvoja> KljucneTacke { get; set; }
        public DbSet<ClanTima> Clanovi { get; set; }
        public DbSet<Zadatak> Zadaci { get; set; }
        public DbSet<StavkaZadatka> StavkeZadataka { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Zadatak>()
                .HasOne(z => z.KljucnaTackaRazvoja)
                .WithMany(k => k.Zadaci)
                .HasForeignKey(z => z.KljucnaTackaRazvojaID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zadatak>()
                .HasOne(z => z.ClanTima)
                .WithMany(c => c.Zadaci)
                .HasForeignKey(z => z.ClanTimaID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StavkaZadatka>()
                .HasOne(s => s.Zadatak)
                .WithMany(z => z.Stavke)
                .HasForeignKey(s => s.ZadatakID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Zadatak>()
                .HasIndex(z => z.Sifra)
                .IsUnique();
        }
    }
}