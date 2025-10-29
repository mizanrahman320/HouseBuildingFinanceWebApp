using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Models.LoanGateway;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseBuildingFinanceWebApp.Controllers
{
    public class BranchReportController : Controller
    {
        private readonly ILocalTransactionService _txnService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BranchReportController(ILocalTransactionService txnService, UserManager<ApplicationUser> userManager)
        {
            _txnService = txnService;
            _userManager = userManager;
        }
        // GET: BranchReportController
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                startDate = DateTime.Today;
                endDate = DateTime.Today;
            }

            var user = await _userManager.GetUserAsync(User); // Current logged-in user
            var roles = await _userManager.GetRolesAsync(user); // List<string>
            var role = roles.FirstOrDefault(); // assuming single role

            List<PaymentTransaction> transactions;

            if (role == "User")
            {
                // User can only see their own branch
                transactions = await _txnService.GetTransactionsByDateAsync(startDate.Value, endDate.Value, user.Branch);
            }
            else
            {
                // Admin can see all
                transactions = await _txnService.GetTransactionsByDateAsync(startDate.Value, endDate.Value);
            }

            return View(transactions);
        }

        // GET: BranchReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BranchReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BranchReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BranchReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BranchReportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BranchReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BranchReportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
