using FluentValidation.AspNetCore;
using inventory_management_system.Data;
using inventory_management_system.Extensions;
using inventory_management_system.Helpers;
using inventory_management_system.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// -------------------------
// Configure Serilog
// -------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14)
    .CreateLogger();

builder.Host.UseSerilog();

// -----------------------
// Add Services
// -----------------------

// Register memory cache
builder.Services.AddMemoryCache();



builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.AutomaticValidationEnabled = false);

builder.Services.AddValidators();
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // Add API Info
        document.Info = new OpenApiInfo
        {
            Title = "Inventory Management System API",
            Version = "v1.0",
            Description = "API for managing inventory, products, orders, and authentication"
        };

        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes["BearerAuth"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Enter a valid JWT token"
        };

        // Apply the security requirement globally
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth" }
                },
                new string[] {}
            }
        });

        return Task.CompletedTask;
    });
});

// -----------------------
// JWT Authentication
// -----------------------
builder.Services.AddJwtAuthentication(builder.Configuration);

// -----------------------
// Database Connection
// -----------------------
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------
// CORS Configuration
// -----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendAndScalar",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:9091", //local testing
                "http://192.168.1.68:9091",     // ← Angular from external
                "http://localhost:9091",       
                "http://localhost:9090",
                "http://localhost:4200"
                


                ) // Angular Frontend
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// -----------------------
// In Memory RATE LIMITING
// -----------------------


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    options.AddFixedWindowLimiter("LoginLimiter", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(5);
        opt.QueueLimit = 0;
    });
    options.OnRejected = async (context, token) =>
    {
        var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var ra)
            ? (int)ra.TotalSeconds
            : 900; 

        var error = ProblemDetailHelper.CreateErrorResponse(
            errorKey: "rate-limit-exceeded",
            title: "Too many login attempts",
            status: 429,
            detail: "You have exceeded the maximum number of login attempts. Please try again later.",
            httpContext: context.HttpContext
        );

        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "application/problem+json";
        await context.HttpContext.Response.WriteAsJsonAsync(error, cancellationToken: token);
    };
});



// -----------------------
// Build App
// -----------------------
var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("IMS Api")
               .WithTheme(ScalarTheme.BluePlanet)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
               .AddPreferredSecuritySchemes("BearerAuth")
               .AddHttpAuthentication("BearerAuth", auth =>
               {
                   auth.Token = ""; 
               });
    });
}

// -----------------------
// Middleware
// -----------------------
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("AllowFrontendAndScalar");
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.MapScalarApiReference();
}
app.UseRateLimiter();   // Must be before Authh        
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
