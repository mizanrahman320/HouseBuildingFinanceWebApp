using HouseBuildingFinanceWebApp.Data;
using HouseBuildingFinanceWebApp.Models;
using HouseBuildingFinanceWebApp.Repositories;
using HouseBuildingFinanceWebApp.Services;
using HouseBuildingFinanceWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Repositories / UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// earlier in Program.cs, after builder creation
builder.Services.AddHttpClient<LoanGatewayProvider>(); // named via type
builder.Services.AddScoped<ILoanGatewayProvider, LoanGatewayProvider>(sp =>
{
    // HttpClientFactory will inject HttpClient into the implementation via constructor
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var client = clientFactory.CreateClient(nameof(LoanGatewayProvider));
    var config = sp.GetRequiredService<IConfiguration>();
    return new LoanGatewayProvider(client, config);
});

// Local transaction service
builder.Services.AddScoped<ILocalTransactionService, LocalTransactionService>();
// Facade
builder.Services.AddScoped<ILoanProcessingFacade, LoanProcessingFacade>();
// If you used LoanGatewayProvider type injection earlier, ensure correct namespaces
// Also register HttpClient for LoanGatewayProvider with base url (optional)
builder.Services.AddHttpClient<LoanGatewayProvider>(client =>
{
    var cfg = builder.Configuration;
    var baseUrl = cfg["LoanGatewayApi:BaseUrl"]?.TrimEnd('/');
    if (!string.IsNullOrEmpty(baseUrl))
        client.BaseAddress = new Uri(baseUrl);
});

// Services (Business logic)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
