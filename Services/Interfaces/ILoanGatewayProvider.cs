using HouseBuildingFinanceWebApp.Models.LoanGateway;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface ILoanGatewayProvider
    {
        Task<AuthToken?> GenerateTokenAsync();
        Task<ApiResponse<LoanAccountInfo>?> ValidateLoanAsync(string branchCode, string loanAC);
        Task<ApiResponse<List<PaymentTransaction>>?> PushDataAsync(List<PaymentTransaction> payments);
        Task<ApiResponse<List<PaymentReportItem>>?> GetPaymentsByDateAsync(string date);
    }
}
