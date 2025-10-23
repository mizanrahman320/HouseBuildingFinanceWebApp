using HouseBuildingFinanceWebApp.Data;
using HouseBuildingFinanceWebApp.Models.LoanGateway;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseBuildingFinanceWebApp.Services
{
    public class LocalTransactionService : ILocalTransactionService
    {
        private readonly ApplicationDbContext _db;
        public LocalTransactionService(ApplicationDbContext db) => _db = db;

        public async Task SaveTransactionAsync(PaymentTransaction txn)
        {
            _db.PaymentTransactions.Add(txn);
            await _db.SaveChangesAsync();
        }

        public async Task<List<PaymentTransaction>> GetTransactionsAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            return await _db.PaymentTransactions
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<PaymentTransaction>> GetTransactionsByDateAsync(DateTime date)
        {
            return await _db.PaymentTransactions
                .Where(t => t.PaymentDate.Date == date.Date)
                .OrderByDescending(t => t.PaymentDate)
                .ToListAsync();
        }
    }
}
