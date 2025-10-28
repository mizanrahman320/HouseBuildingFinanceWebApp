using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Services.Interfaces;

namespace HouseBuildingFinanceWebApp.Services
{
    public class BranchFacade : IBranchFacade
    {
        private readonly IBHFCBranchService _bhbfcBranchService;
        private readonly IMBLBranchService _mblBranchService;

        public BranchFacade(IBHFCBranchService bhbfcBranchService, IMBLBranchService mblBranchService)
        {
            _bhbfcBranchService = bhbfcBranchService;
            _mblBranchService = mblBranchService;
        }

        public async Task<List<BHBFC_Branch>> GetBHBFCBranchesAsync()
        {
            return await _bhbfcBranchService.GetBHBFCBranchesAsync();
        }

        public async Task<List<MBLBranch>> GetMBLBranchesAsync()
        {
            return await _mblBranchService.GetMBLBranchesAsync();
        }
    }
}
