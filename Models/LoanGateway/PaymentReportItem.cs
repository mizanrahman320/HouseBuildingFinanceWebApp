namespace HouseBuildingFinanceWebApp.Models.LoanGateway
{
    public class PaymentReportItem
    {
        public string BankId { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string LoanAC { get; set; } = string.Empty;
        public string? MobileNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string MemoNumber { get; set; } = string.Empty;
        public decimal PaymentAmount { get; set; }
        public decimal VatAmount { get; set; }
    }
}
