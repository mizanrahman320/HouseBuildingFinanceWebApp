using HouseBuildingFinanceWebApp.Data;
using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseBuildingFinanceWebApp.Services
{
    public class BHBFCBranchService : IBHFCBranchService
    {
        private readonly ApplicationDbContext _db;

        public BHBFCBranchService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<BHBFC_Branch>> GetBHBFCBranchesAsync()
        {
            return await _db.BHBFC_Branch
                .Where(b => b.IsActive)
                .OrderBy(b => b.BranchName)
                .ToListAsync();
        }
    }
}
