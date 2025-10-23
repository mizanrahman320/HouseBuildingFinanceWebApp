using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentException("Role name required", nameof(roleName));
            if (await _roleManager.RoleExistsAsync(roleName)) return IdentityResult.Success;
            return await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            // Filter out nulls to ensure IEnumerable<string> is returned, not IEnumerable<string?>
            return await Task.FromResult(_roleManager.Roles
                .Select(r => r.Name)
                .Where(name => name != null)
                .Cast<string>()
                .ToList());
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
