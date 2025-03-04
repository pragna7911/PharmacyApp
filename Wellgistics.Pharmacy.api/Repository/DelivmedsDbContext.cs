using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.Repository
{
    public class DelivmedsDbContext:DbContext
    {
        public DbSet<RxOptions> RxOptions { get; set; }

        private readonly IConfiguration _configuration;
        public DbSet<PharmacyCreationResponse> RuleCreationResponses { get; set; }
        public DelivmedsDbContext(DbContextOptions<DelivmedsDbContext> options, IConfiguration configuration) : base(options)
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
                var connectionString = _configuration.GetConnectionString("EinsteinDBConnection");
                optionsBuilder.UseSqlServer(connectionString ?? "");  // Replace with your MySQL version
            }
        }
    }
}
