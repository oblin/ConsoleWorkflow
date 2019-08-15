using AbhCare.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AbhCare.Identity.Data
{
    public class RbIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicUsers> ClinicUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=RbIdentity;Trusted_Connection=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
