namespace HouseBuildingFinanceWebApp.Models.LoanGateway
{
    public class PaymentTransactionDto
    {
        public string bankId { get; set; }
        public string transactionId { get; set; }
        public string paymentDate { get; set; }  // Keep as string if API expects that
        public string loanAC { get; set; }
        public string branchCode { get; set; }
        public string purpose { get; set; }
        public string paymentAmount { get; set; }
        public string vatAmount { get; set; }
        public string memoNumber { get; set; }
        public string mobileNo { get; set; }
        public string paymentMode { get; set; }
    }

}
