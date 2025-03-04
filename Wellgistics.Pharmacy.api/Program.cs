using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NLog.Web;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.IRepositries;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;
using Wellgistics.Pharmacy.api.Repository;
using Wellgistics.Pharmacy.api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Makes property name matching case-insensitive
}); ;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()  // Allow all origins, can be restricted for production
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<PharmacyDbContext>(options =>
{
    // DbContext configuration is now handled inside the RoleDbContext class (OnConfiguring)
});
builder.Services.AddDbContext<DelivmedsDbContext>(options =>
{
    // DbContext configuration is now handled inside the RoleDbContext class (OnConfiguring)
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:DomainName"] + "/";
    options.Audience = builder.Configuration["Auth0:ClientId"];
});
// Register repository and service layers
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IPharmacyService, PharmacyService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IStackyonService, StackyonService>();
builder.Services.AddScoped<IServiceLogService, ServiceLogService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient and DopplerService
builder.Services.AddHttpClient();
builder.Services.AddSingleton<DopplerService,DopplerService>();

// Register HttpClient and HttpClientHelper
builder.Services.AddHttpClient<HttpClientHelper>();

builder.Services.AddCustomAuthorizationPolicies();
// Register NLog as the logging provider
builder.Logging.ClearProviders();
builder.Logging.AddNLog("nlog.config"); // Adds NLog as a logging provider and loads from nlog.config

// Set up NLog configuration directly from appsettings.json
//builder.Host.ConfigureLogging(logging =>
//{
//    logging.ClearProviders();
//    logging.AddNLog("nlog.config");
//});
var app = builder.Build();

app.UseCors("AllowAll");
//builder.Host.UseNLog();
// Use middleware for request logging
app.UseMiddleware<RequestLoggingMiddleware>(); // You can define this middleware for logging API requests and responses


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Pharmacy API is running");

app.Run();
