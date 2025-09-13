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
    
        public DbSet<DeletedDoctor> DeletedDoctors { get; set; }
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
        public DbSet<ApprovalLog> ApprovalLogs { get; set; }
        public DbSet<ApprovedOrder> ApprovedOrders { get; set; }
        public DbSet<ApprovedMedicationItem> ApprovedMedicationItems { get; set; }
        public DbSet<UserAllergy> UserAllergies { get; set; }

        //Customer Subsystem Tables
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MedicationItem> MedicationItems { get; set; }
        public DbSet<PrescriptionOrders> PrescriptionOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserAllergy>()
                .HasKey(ua => new { ua.UserId, ua.ActiveIngredientId });

            builder.Entity<UserAllergy>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Allergies)
                .HasForeignKey(ua => ua.UserId);

            builder.Entity<UserAllergy>()
                .HasOne(ua => ua.ActiveIngredient)
                .WithMany(ai => ai.UserAllergies)
                .HasForeignKey(ua => ua.ActiveIngredientId);

            builder.Entity<Prescription>()
               .HasMany(p => p.MedicationItems)
               .WithOne(mi => mi.Prescription)
               .HasForeignKey(mi => mi.PrescriptionId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MedicationItem>()
                   .HasOne(mi => mi.Medication)
                   .WithMany()  // Medication itself is a catalog; doesn't need navigation back
                   .HasForeignKey(mi => mi.MedicationId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Medication if used in prescription
        }
    }
}

