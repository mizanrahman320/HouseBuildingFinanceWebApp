using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Models.ViewModels;
using HouseBuildingFinanceWebApp.Repositories;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork uow,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _uow = uow;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                Branch = model.Branch,
                Phone = model.Phone,
                Designation = model.Designation,
                Status = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return result;

            if (await _uow.RoleManager.RoleExistsAsync("User"))
            {
                await _uow.UserManager.AddToRoleAsync(user, "User");
            }

            await _uow.SaveChangesAsync();
            return result;
        }

        public async Task<SignInResult> PasswordSignInAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task SignOutAsync() => await _signInManager.SignOutAsync();
    }
}
