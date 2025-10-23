using HouseBuildingFinanceWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        // expose identity managers so services can orchestrate identity operations via UoW
        UserManager<ApplicationUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }

        // Example: repositories for domain entities
        // IRepository<Project> Projects { get; }
    }
}
