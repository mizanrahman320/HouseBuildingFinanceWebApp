using HouseBuildingFinanceWebApp.Models.LoanGateway;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface ILocalTransactionService
    {
        Task SaveTransactionAsync(PaymentTransaction txn);
        Task<List<PaymentTransaction>> GetTransactionsAsync(int page = 1, int pageSize = 50);
        Task<List<PaymentTransaction>> GetTransactionsByDateAsync(DateTime date);
    }
}
