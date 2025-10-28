using HouseBuildingFinanceWebApp.Models;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface IBranchFacade
    {
        Task<List<BHBFC_Branch>> GetBHBFCBranchesAsync();
        Task<List<MBLBranch>> GetMBLBranchesAsync();
    }
}
