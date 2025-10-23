using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<bool> RoleExistsAsync(string roleName);
    }
}
