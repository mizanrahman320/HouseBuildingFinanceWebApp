using HouseBuildingFinanceWebApp.Models;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface IMBLBranchService
    {
        Task<List<MBLBranch>> GetMBLBranchesAsync();
    }
}
