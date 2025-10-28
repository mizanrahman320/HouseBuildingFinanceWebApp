using System.ComponentModel.DataAnnotations;

namespace HouseBuildingFinanceWebApp.Models
{
    public class BHBFC_Branch
    {
        public int Id { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
