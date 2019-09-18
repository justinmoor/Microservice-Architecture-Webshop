using Microsoft.EntityFrameworkCore;
using Sprinters.Webshop.BFF.Entities;

namespace Sprinters.Webshop.BFF.DAL
{
    public class WebshopContext : DbContext
    {
        public WebshopContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Artikel> Artikelen { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artikel>(artikel =>
            {
                artikel.Property(k => k.Artikelnummer)
                    .ValueGeneratedNever()
                    .IsRequired();
                artikel.Ignore(a => a.PrijsWithBtw);
            });

            modelBuilder.Entity<Bestelling>(b =>
            {
                b.Property(bs => bs.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                b.Property(bs => bs.AdresRegel1)
                    .HasMaxLength(255);

                b.Property(bs => bs.AdresRegel2)
                    .HasMaxLength(255);

                b.Property(bs => bs.Plaats)
                    .HasMaxLength(255);

                b.Property(bs => bs.Postcode)
                    .HasMaxLength(7);
            });

            modelBuilder.Entity<Klant>(b =>
            {
                b.Property(k => k.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                b.Property(k => k.Voornaam)
                    .HasMaxLength(255);

                b.Property(k => k.Achternaam)
                    .HasMaxLength(255);

                b.Property(k => k.Plaats)
                    .HasMaxLength(255);

                b.Property(k => k.AdresRegel)
                    .HasMaxLength(255);

                b.Property(k => k.Telefoonnummer)
                    .HasMaxLength(20);

                b.Property(k => k.Postcode)
                    .HasMaxLength(7);

                b.Property(k => k.Email).IsRequired(false);
            });

            modelBuilder.Entity<BestellingItem>(bi =>
            {
                bi.Property(k => k.Id)
                    .ValueGeneratedNever()
                    .IsRequired();
            });
        }
    }
}