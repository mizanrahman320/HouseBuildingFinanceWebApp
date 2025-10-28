using HouseBuildingFinanceWebApp.Data;
using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseBuildingFinanceWebApp.Services
{
    public class MBLBranchService : IMBLBranchService
    {
        private readonly ApplicationDbContext _db;

        public MBLBranchService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<MBLBranch>> GetMBLBranchesAsync()
        {
            return await _db.MBLBranch
                .Where(b => b.IsActive)
                .OrderBy(b => b.BranchName)
                .ToListAsync();
        }
    }
}
