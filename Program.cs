using FluentValidation.AspNetCore;
using inventory_management_system.Data;
using inventory_management_system.Extensions;
using inventory_management_system.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;


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
                "https://localhost:4200",  
                "http://localhost:4200", 
                "http://localhost:7024",   
                "https://localhost:7024",  
                "http://localhost:5267",  
                "https://localhost:5267"   
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// -----------------------
// Build App
// -----------------------
var app = builder.Build();

// expose the OpenAPI JSON 
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
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
