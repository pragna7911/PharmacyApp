using Microsoft.EntityFrameworkCore;
using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.Repository
{
    public class PharmacyDbContext:DbContext
    {
        public DbSet<PharmacyInstance> PharmacyInstances { get; set; }
        public DbSet<PharmacyCreationResponse> RuleCreationResponses { get; set; }
        public DbSet<ConfigurationType> ConfigurationTypes { get; set; }
        public DbSet<PrescriptionStausCount> PrescriptionStausCount { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PharmacyEmployee> PharmacyEmployees { get; set; }
        public DbSet<PrescriptionUpdateStatus> prescriptionUpdateStatuses { get; set; }
        public DbSet<RxPharmacy> RxPharmacy { get; set; }
        public DbSet<RxOptions> RxOptions { get; set; }

        private readonly IConfiguration _configuration;
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PharmacyInstance>()
            .ToTable("pharmaciesinfo_stage");
            
            // Ignore the RuleSetInfo navigation property in RuleSetParameterInfo
            //modelBuilder.Entity<RuleSetParameterInfo>()
            //    .Ignore(r => r.RuleSetInfo);

            // Alternatively, configure the foreign key relationship explicitly
            //modelBuilder.Entity<RuleSetParameterInfo>()
            //    .HasOne<RuleSetInfo>()
            //    .WithMany() // or .WithOne() depending on your relationship
            //    .HasForeignKey(rp => rp.RuleSetId)
            //    .OnDelete(DeleteBehavior.Cascade);  // Add cascade delete or specify the behavior you need
        }

        // Override OnConfiguring to set up MySQL configuration and connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("PharmacyDBConnection");
                optionsBuilder.UseSqlServer(connectionString ?? "");  // Replace with your MySQL version
            }
        }
    }
}
