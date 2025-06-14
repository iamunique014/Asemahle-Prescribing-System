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
        public DbSet<MedicationActiveIngredient> MedicationActiveIngredient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<DorsageForm> DorsageForm { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerAllergy> CustomerAllergy { get; set; }
        public DbSet<Pharmacy> Pharmacy { get; set; }
        public DbSet<Pharmacist> Pharmacist { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Medication> Medication { get; set; }
        public DbSet<MedicationStockOrder> MedicationStockOrder { get; set; }
        public DbSet<StockOrder> StockOrder { get; set; }
        public DbSet<DeletedActiveIngredient> DeletedActiveIngredients { get; set; }



    }

}

