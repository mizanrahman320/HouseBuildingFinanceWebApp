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
            // Push to external gateway
            var pushResp = await _gateway.PushDataAsync(new List<PaymentTransaction> { txn });

            if (pushResp?.Data == null || pushResp.Data.Count == 0)
                return pushResp; // nothing to process, gracefully exit

            // Prepare timezone (UTC+6)
            var bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dhaka");
            var paymentDateLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bdTimeZone);

            foreach (var remote in pushResp.Data)
            {
                if (remote.IsReceived == "Y")
                {
                    // Keep original data consistent and apply local timestamp
                    remote.BranchCode = txn.BranchCode;
                    remote.PaymentDate = paymentDateLocal; // <-- UTC+6
                    remote.Purpose = txn.Purpose;
                    remote.PaymentAmount = txn.PaymentAmount;
                    remote.VatAmount = txn.VatAmount;
                    remote.MemoNumber = txn.MemoNumber;
                    remote.MobileNo = txn.MobileNo;
                    remote.PaymentMode = txn.PaymentMode;

                    // Create local DB copy using factory and persist
                    var local = TransactionFactory.CreateLocalFromApi(remote, authId, authBranch);
                    await _localService.SaveTransactionAsync(local);
                }
                else
                {
                    // Optional: handle gateway rejection / partial success cases
                    Console.WriteLine($"Transaction not received by gateway: {txn.TransactionId}");
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
