using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseBuildingFinanceWebApp.Models.LoanGateway
{
    public class PaymentTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BankId { get; set; } = string.Empty;

        [Required]
        public string TransactionId { get; set; } = string.Empty;

        [Required]
        public string LoanAC { get; set; } = string.Empty;

        [Required]
        public string BranchCode { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string Purpose { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatAmount { get; set; }

        public string? MemoNumber { get; set; }
        public string? MobileNo { get; set; }
        public string? PaymentMode { get; set; }

        [MaxLength(1)]
        public string IsReceived { get; set; } = "N"; // Y, N, R

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // NEW fields for audit
        [MaxLength(50)]
        public string? AuthId { get; set; }

        [MaxLength(50)]
        public string? AuthBranch { get; set; }
    }
}
