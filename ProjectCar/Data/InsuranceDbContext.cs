using Microsoft.EntityFrameworkCore;
using ProjectCar.Models;

namespace ProjectCar.Data
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Insuree> Insurees { get; set; }
    }
} 