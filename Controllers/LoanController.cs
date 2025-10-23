using System.Security.Claims;
using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Models.LoanGateway;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseBuildingFinanceWebApp.Controllers
{
    [Authorize] // optional: require Identity user for auditing; remove if this flow is anonymous
    public class LoanController : Controller
    {
        private readonly ILoanProcessingFacade _facade;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoanController(ILoanProcessingFacade facade, UserManager<ApplicationUser> userManager)
        {
            _facade = facade;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify(string branchCode, string loanAC)
        {
            if (string.IsNullOrWhiteSpace(branchCode) || string.IsNullOrWhiteSpace(loanAC))
            {
                ModelState.AddModelError(string.Empty, "Branch code and loan account are required.");
                return View();
            }

            var resp = await _facade.ValidateLoanAsync(branchCode, loanAC);
            if (resp == null || resp.Status != 200 || resp.Data == null)
            {
                ModelState.AddModelError(string.Empty, resp?.Message ?? "Validation failed.");
                return View();
            }

            // pass LoanAccountInfo to the transaction form
            return View("TransactionForm", resp.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTransaction(LoanAccountInfo loanModel, decimal paymentAmount, string paymentMode, string mobileNumber)
        {
            // find current user for audit info
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userId != null ? await _userManager.FindByIdAsync(userId) : null;
            var authId = user?.UserName ?? "system";
            var authBranch = user?.Branch ?? loanModel.BranchCode;

            var txn = new PaymentTransaction
            {
                BankId = /*HttpContext.Request.Headers["bankId"].ToString() ??*/ "MBL",
                TransactionId = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
                LoanAC = loanModel.LoanAC,
                BranchCode = loanModel.BranchCode,
                PaymentDate = DateTime.UtcNow,
                Purpose = "11",
                PaymentAmount = paymentAmount,
                VatAmount = 0,
                MemoNumber = DateTime.Now.ToString("yyMMdd") + new Random().Next(1000, 9999),
                MobileNo = mobileNumber,
                PaymentMode = paymentMode
            };

            var resp = await _facade.ProcessTransactionAsync(txn, authId, authBranch);

            if (resp == null)
            {
                TempData["Toast"] = "Transaction failed: no response from gateway.";
                return View("TransactionResult", resp);
            }

            TempData["Toast"] = resp.Message;
            return View("TransactionResult", resp);
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Report(string date)
        {
            if (!DateTime.TryParse(date, out var dt))
            {
                ModelState.AddModelError(string.Empty, "Invalid date format.");
                return View();
            }

            var resp = await _facade.GetReportByDateAsync(date);
            return View(resp);
        }
    }
}
