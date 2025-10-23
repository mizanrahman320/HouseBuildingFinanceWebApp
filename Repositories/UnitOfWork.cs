using HouseBuildingFinanceWebApp.Data;
using HouseBuildingFinanceWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public UnitOfWork(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() => _context.Dispose();
    }
}
