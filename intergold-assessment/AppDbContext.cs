using InterGoldAssessment.Entity;
using Microsoft.EntityFrameworkCore;

namespace InterGoldAssessment
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
