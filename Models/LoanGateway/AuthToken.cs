namespace HouseBuildingFinanceWebApp.Models.LoanGateway
{
    public class AuthToken
    {
        public string Token { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
