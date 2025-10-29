using HouseBuildingFinanceWebApp.Models.ViewModels;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HouseBuildingFinanceWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMBLBranchService _mblBranchService;

        public AccountController(IUserService userService, IMBLBranchService mblBranchService)
        {
            _userService = userService;
            _mblBranchService = mblBranchService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                BranchList = (await _mblBranchService.GetMBLBranchesAsync())
                    .Select(b => new SelectListItem
                    {
                        Value = b.BranchCode,
                        Text = $"{b.BranchCode} - {b.BranchName}"
                    })
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.BranchList = (await _mblBranchService.GetMBLBranchesAsync())
                    .Select(b => new SelectListItem
                    {
                        Value = b.BranchCode,
                        Text = $"{b.BranchCode} - {b.BranchName}"
                    });

                return View(model);
            }

            var result = await _userService.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful! Welcome.";
                // sign in or redirect to login
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userService.PasswordSignInAsync(model);
            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
