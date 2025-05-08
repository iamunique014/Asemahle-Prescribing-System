using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Models;

namespace PrescribingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ActiveIngredients> ActiveIngredients { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<DorsageForm> DorsageForm { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerAllergy> CustomerAllergy { get; set; }
        public DbSet<Pharmacy> Pharmacy { get; set; }
        public DbSet<Pharmacist> Pharmacist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationship explicitly (optional if conventions are used)
            modelBuilder.Entity<CustomerAllergy>()
                .HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAllergies)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerAllergy>()
                .HasOne(ca => ca.ActiveIngredients)
                .WithMany(ai => ai.CustomerAllergies)
                .HasForeignKey(ca => ca.ActiveIngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
