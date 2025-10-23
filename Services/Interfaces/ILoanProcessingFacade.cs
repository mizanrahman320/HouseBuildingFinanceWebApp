using HouseBuildingFinanceWebApp.Models.LoanGateway;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface ILoanProcessingFacade
    {
        Task<ApiResponse<LoanAccountInfo>?> ValidateLoanAsync(string branchCode, string loanAC);
        Task<ApiResponse<List<PaymentTransaction>>?> ProcessTransactionAsync(PaymentTransaction txn, string authId, string authBranch);
        Task<ApiResponse<List<PaymentReportItem>>?> GetReportByDateAsync(string date);
    }
}
