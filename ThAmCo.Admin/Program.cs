using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.Security.Claims;
using ThAmCo.Admin.Data;
using ThAmCo.Admin.Services;

var builder = WebApplication.CreateBuilder(args);
var coreServiceBaseUrl = Environment.GetEnvironmentVariable("CoreServiceBaseUrl");
DotNetEnv.Env.Load();
// Add OpenAPI
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpClient<CoreService>(client =>
{
    if (!string.IsNullOrEmpty(coreServiceBaseUrl))
    {
        client.BaseAddress = new Uri(coreServiceBaseUrl);
    }
    else
    {
        throw new Exception("CoreServiceBaseUrl is not configured in the .env file.");
    }
});

// Configure Database Context with Isolation
builder.Services.AddDbContext<AdminDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // SQLite for local development
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "adminservice.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        // Azure SQL for production
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString, sqlServerOptions =>
        {
            sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null
            );
        });
    }
});

// Configure Services with Isolation
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IAdminService, FakeAdminService>();
}
else
{
    builder.Services.AddHttpClient<IAdminService, AdminService>()
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
}

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();