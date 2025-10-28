using System.Reflection.Emit;
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
        public DbSet<BHBFC_Branch> BHBFC_Branch => Set<BHBFC_Branch>();
        public DbSet<MBLBranch> MBLBranch => Set<MBLBranch>();
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

            // Configure the BHBFC_Branch entity
            builder.Entity<BHBFC_Branch>(entity =>
            {
                // Table name
                entity.ToTable("BHBFC_Branch");

                // Primary key
                entity.HasKey(b => b.Id);

                // Column configurations
                entity.Property(b => b.BranchCode)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(b => b.BranchName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.IsActive)
                      .HasDefaultValue(true);

                // Optional: make BranchCode unique
                entity.HasIndex(b => b.BranchCode)
                      .IsUnique();
                
            });
            builder.Entity<MBLBranch>(entity =>
            {
                // Table name
                entity.ToTable("MBLBranch");

                // Primary key
                entity.HasKey(b => b.Id);

                // Column configurations
                entity.Property(b => b.BranchCode)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(b => b.BranchName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.IsActive)
                      .HasDefaultValue(true);

                // Optional: make BranchCode unique
                entity.HasIndex(b => b.BranchCode)
                      .IsUnique();

            });
        }
        // Add DbSets for your domain entities here, e.g.
        // public DbSet<Project> Projects { get; set; }
    }
}
