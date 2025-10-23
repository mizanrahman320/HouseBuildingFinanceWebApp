using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HouseBuildingFinanceWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Branch { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Designation { get; set; }

        [Required]
        public bool Status { get; set; } = true; // Active by default
    }
}
