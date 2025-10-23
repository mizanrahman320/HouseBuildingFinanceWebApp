using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Models.LoanGateway;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBuildingFinanceWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configure PaymentTransaction if you want explicit constraints / indexes
            builder.Entity<PaymentTransaction>(e =>
            {
                e.HasIndex(x => new { x.TransactionId, x.BankId }).IsUnique(false);
                e.Property(x => x.PaymentAmount).HasColumnType("decimal(18,2)");
                e.Property(x => x.VatAmount).HasColumnType("decimal(18,2)");
            });
        }
        // Add DbSets for your domain entities here, e.g.
        // public DbSet<Project> Projects { get; set; }
    }
}
