using Microsoft.EntityFrameworkCore;
using Sprinters.SharedTypes.BeheerService.Entities;

namespace Sprinters.BestellingBeheer.DAL
{
    public class BeheerContext : DbContext
    {
        public BeheerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Artikel> Artikelen { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Klant> Klanten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artikel>(artikel =>
            {
                artikel.Property(k => k.Artikelnummer)
                    .ValueGeneratedNever()
                    .IsRequired();
            });

            modelBuilder.Entity<Klant>(klant =>
            {
                klant.Property(k => k.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                klant.Property(k => k.Voornaam)
                    .HasMaxLength(255);

                klant.Property(k => k.Achternaam)
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<BestellingItem>()
                .HasOne(a => a.Artikel);
        }
    }
}