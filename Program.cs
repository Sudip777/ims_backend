using FluentValidation;
using FluentValidation.AspNetCore;
using inventory_management_system.Data;
using inventory_management_system.Exceptions;
using inventory_management_system.Repository.Implementations;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Implementations;
using inventory_management_system.Services.Interfaces;
using inventory_management_system.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -----------------------
// Add Services
// -----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

// -----------------------
// JWT Authentication
// -----------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = builder.Configuration["Jwt:Key"];
    if (string.IsNullOrEmpty(key))
        throw new ArgumentException("Jwt:Key is not configured in appsettings.json");

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = signingKey,
    };
});

// -----------------------
// Database Connection
// -----------------------
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------
// Dependency Injection
// -----------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductSupplierRepository, ProductSupplierRepository>();
builder.Services.AddScoped<IProductSupplierService, ProductSupplierService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddHttpContextAccessor();

// -----------------------
// FluentValidation Setup
// -----------------------
builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProductSupplierValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CategoriesValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// -----------------------
// Build App
// -----------------------
var app = builder.Build();

// -----------------------
// Swagger
// -----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API v1");
    });
}

// -----------------------
// Global Exception Handling
// -----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception != null)
            {
                context.Response.ContentType = "application/json";

                var errorResponse = ExceptionHandler.ErrorHandler.HandleException(exception, context);

                context.Response.StatusCode = errorResponse.Status;

                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(exception, "Unhandled exception caught by global handler.");

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        });
    });
}
else
{
    app.UseDeveloperExceptionPage();
}

// -----------------------
// Middleware
// -----------------------
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
