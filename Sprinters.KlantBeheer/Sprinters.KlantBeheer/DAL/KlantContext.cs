using Microsoft.EntityFrameworkCore;
using Sprinters.SharedTypes.KlantService.Entities;

namespace Sprinters.KlantBeheer.DAL
{
    public class KlantContext : DbContext
    {
        public KlantContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Klant> Klanten { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Klant>(klant =>
            {
                klant.Property(k => k.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                klant.Property(k => k.Voornaam)
                    .HasMaxLength(255);

                klant.Property(k => k.Achternaam)
                    .HasMaxLength(255);

                klant.Property(k => k.Plaats)
                    .HasMaxLength(255);

                klant.Property(k => k.AdresRegel)
                    .HasMaxLength(255);

                klant.Property(k => k.Telefoonnummer)
                    .HasMaxLength(20);

                klant.Property(k => k.Postcode)
                    .HasMaxLength(7);

            });
        }
    }
}