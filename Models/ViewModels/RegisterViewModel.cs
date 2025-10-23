using System.ComponentModel.DataAnnotations;

namespace HouseBuildingFinanceWebApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; } = default!;

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;

        [Required, StringLength(100)]
        public string FullName { get; set; } = default!;

        [StringLength(100)]
        public string? Branch { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Designation { get; set; }

        public bool Status { get; set; } = true;
    }
}
