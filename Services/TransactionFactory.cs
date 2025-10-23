using HouseBuildingFinanceWebApp.Models.LoanGateway;

namespace HouseBuildingFinanceWebApp.Services
{
    public static class TransactionFactory
    {
        public static PaymentTransaction CreateLocalFromApi(PaymentTransaction apiTxn, string authId, string authBranch)
        {
            return new PaymentTransaction
            {
                BankId = apiTxn.BankId,
                TransactionId = apiTxn.TransactionId,
                LoanAC = apiTxn.LoanAC,
                BranchCode = apiTxn.BranchCode,
                PaymentDate = apiTxn.PaymentDate,
                Purpose = apiTxn.Purpose,
                PaymentAmount = apiTxn.PaymentAmount,
                VatAmount = apiTxn.VatAmount,
                MemoNumber = apiTxn.MemoNumber,
                MobileNo = apiTxn.MobileNo,
                PaymentMode = apiTxn.PaymentMode,
                IsReceived = apiTxn.IsReceived,
                AuthId = authId,
                AuthBranch = authBranch,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
