using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<SignInResult> PasswordSignInAsync(LoginViewModel model);
        Task SignOutAsync();
        Task<ApplicationUser?> FindByEmailAsync(string email);
    }
}
