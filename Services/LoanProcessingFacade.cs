using HouseBuildingFinanceWebApp.Models.LoanGateway;
using HouseBuildingFinanceWebApp.Services;
using HouseBuildingFinanceWebApp.Services.Interfaces;

namespace HouseBuildingFinanceWebApp.Services
{
    public class LoanProcessingFacade : ILoanProcessingFacade
    {
        private readonly ILoanGatewayProvider _gateway;
        private readonly ILocalTransactionService _localService;

        public LoanProcessingFacade(ILoanGatewayProvider gateway, ILocalTransactionService localService)
        {
            _gateway = gateway;
            _localService = localService;
        }

        public async Task<ApiResponse<LoanAccountInfo>?> ValidateLoanAsync(string branchCode, string loanAC)
        {
            return await _gateway.ValidateLoanAsync(branchCode, loanAC);
        }

        public async Task<ApiResponse<List<PaymentTransaction>>?> ProcessTransactionAsync(PaymentTransaction txn, string authId, string authBranch)
        {
            // push to external gateway
            var pushResp = await _gateway.PushDataAsync(new List<PaymentTransaction> { txn });
            if (pushResp?.Data != null)
            {
                // map remote response(s) to local records and save each
                foreach (var remote in pushResp.Data)
                {
                    if (remote.IsReceived == "Y")
                    {

                        // remote may contain IsReceived status; create local copy using factory
                        var local = TransactionFactory.CreateLocalFromApi(remote, authId, authBranch);
                        await _localService.SaveTransactionAsync(local);
                    }
                    else
                    {
                        // handle failed transaction case if needed
                        
                    }

                }
            }

            return pushResp;
        }

        public async Task<ApiResponse<List<PaymentReportItem>>?> GetReportByDateAsync(string date)
        {
            return await _gateway.GetPaymentsByDateAsync(date);
        }
    }
}
