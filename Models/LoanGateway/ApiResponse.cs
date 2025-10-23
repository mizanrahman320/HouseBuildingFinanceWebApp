namespace HouseBuildingFinanceWebApp.Models.LoanGateway
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
