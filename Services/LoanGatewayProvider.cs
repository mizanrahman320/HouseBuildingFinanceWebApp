using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HouseBuildingFinanceWebApp.Models.LoanGateway;
using HouseBuildingFinanceWebApp.Services.Interfaces;

namespace HouseBuildingFinanceWebApp.Services
{
    public class LoanGatewayProvider : ILoanGatewayProvider
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private AuthToken? _token;

        public LoanGatewayProvider(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
            var baseUrl = _config["LoanGatewayApi:BaseUrl"]?.TrimEnd('/') ?? throw new InvalidOperationException("LoanGatewayApi:BaseUrl missing");
            _client.BaseAddress = new Uri(baseUrl);
        }

        public async Task<AuthToken?> GenerateTokenAsync()
        {
            var credentials = new { userName = _config["LoanGatewayApi:UserName"], password = _config["LoanGatewayApi:Password"] };
            var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
            AddHeaderToContent(content);

            var resp = await _client.PostAsync("/api/v1/auth/GenerateToken", content);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync();
            var tokenResp = JsonSerializer.Deserialize<AuthToken>(json, _jsonOptions);
            if (tokenResp != null)
            {
                // Use server TTL or config
                tokenResp.ExpiresAt = DateTime.UtcNow.AddMinutes(int.TryParse(_config["LoanGatewayApi:TokenLifetimeMinutes"], out var ttl) ? ttl : 25);
                _token = tokenResp;
            }
            return _token;
        }

        private void AddHeaderToContent(HttpContent content)
        {
            if (!content.Headers.Contains("apiKey"))
                content.Headers.Add("apiKey", _config["LoanGatewayApi:ApiKey"]);

            if (!content.Headers.Contains("bankId"))
                content.Headers.Add("bankId", _config["LoanGatewayApi:BankId"]);
        }

        private void EnsureAuthorizationHeader()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("apiKey", _config["LoanGatewayApi:ApiKey"]);
            _client.DefaultRequestHeaders.Add("bankId", _config["LoanGatewayApi:BankId"]);
            if (_token != null && !string.IsNullOrEmpty(_token.Token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
        }

        private async Task EnsureAuthenticatedAsync()
        {
            if (_token != null && _token.ExpiresAt > DateTime.UtcNow.AddSeconds(30)) return;
            await GenerateTokenAsync();
        }

        public async Task<ApiResponse<LoanAccountInfo>?> ValidateLoanAsync(string branchCode, string loanAC)
        {
            await EnsureAuthenticatedAsync();
            EnsureAuthorizationHeader();

            var body = new { branchCode, loanAC };
            var resp = await _client.PostAsync("/api/v1/gateway/GetLoanInfo",
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<LoanAccountInfo>>(json, _jsonOptions);
        }

        public async Task<ApiResponse<List<PaymentTransaction>>?> PushDataAsync(List<PaymentTransaction> payments)
        {
            await EnsureAuthenticatedAsync();
            EnsureAuthorizationHeader();
            var payload = payments.Select(x => new PaymentTransactionDto
            {
                bankId = x.BankId,
                transactionId = x.TransactionId,
                paymentDate = x.PaymentDate.ToString("yyyy-MM-dd"),
                loanAC = x.LoanAC,
                branchCode = x.BranchCode,
                purpose = x.Purpose,
                paymentAmount = x.PaymentAmount.ToString(),
                vatAmount = x.VatAmount.ToString(),
                memoNumber = x.MemoNumber,
                mobileNo = x.MobileNo,
                paymentMode = x.PaymentMode
            }).ToList();

            var jsonBody = JsonSerializer.Serialize(payload, _jsonOptions);

            Console.WriteLine("=== OUTGOING REQUEST ===");
            Console.WriteLine($"POST: {_client.BaseAddress}api/v1/gateway/PushData");
            Console.WriteLine("Headers:");
            foreach (var header in _client.DefaultRequestHeaders)
            {
                Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine("Body:");
            Console.WriteLine(jsonBody);
            Console.WriteLine("========================");

            var content = new StringContent(JsonSerializer.Serialize(payload, _jsonOptions), Encoding.UTF8, "application/json");
            var resp = await _client.PostAsync("/api/v1/gateway/PushData", content);

            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<PaymentTransaction>>>(json, _jsonOptions);
        }

        public async Task<ApiResponse<List<PaymentReportItem>>?> GetPaymentsByDateAsync(string date)
        {
            await EnsureAuthenticatedAsync();
            EnsureAuthorizationHeader();

            var body = new { bankId = _config["LoanGatewayApi:BankId"], paymentDate = date };
            var resp = await _client.PostAsync("/api/v1/gateway/GetPaymentDetails",
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode) return null;
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<List<PaymentReportItem>>>(json, _jsonOptions);
        }
    }
}
