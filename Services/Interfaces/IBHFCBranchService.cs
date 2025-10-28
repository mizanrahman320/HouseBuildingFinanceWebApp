using HouseBuildingFinanceWebApp.Models;

namespace HouseBuildingFinanceWebApp.Services.Interfaces
{
    public interface IBHFCBranchService
    {
        Task<List<BHBFC_Branch>> GetBHBFCBranchesAsync();
    }
}
