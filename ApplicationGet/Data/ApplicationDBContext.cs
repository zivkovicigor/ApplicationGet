using ApplicationGet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ApplicationGet.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Flight>()
                .HasOne(e => e.ToCity)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // <--


            modelBuilder.Entity<Flight>()
                .HasOne(e => e.FromCity)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // <--

            modelBuilder.Entity<Reservation>()
                .HasKey(a => new { a.UserId, a.Id });

            modelBuilder.Entity<Reservation>()
                .HasOne(e => e.Flight)
                .WithMany();

            base.OnModelCreating(modelBuilder);
        }
    }
}
