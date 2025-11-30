# Inventory Management System - Backend API Documentation
## Overview

The **Inventory Management System (IMS)** is a production-ready, enterprise-grade RESTful API built with **.NET 9 Web API**. It provides comprehensive functionality for managing inventory operations across multiple warehouses with features like real-time stock tracking, order management, and role-based access control.

### Key Features

- **Product Management**: Complete CRUD operations for products with categories and suppliers
- **Multi-Warehouse Support**: Track inventory across multiple warehouse locations
- **Order Processing**: Handle sales and purchase orders with automatic stock updates
- **User Management**: Role-based access control with four permission levels
- **Advanced Filtering**: Pagination, filtering, sorting, and searching capabilities
- **Validation**: Robust input validation using FluentValidation
- **Documentation**: Interactive API documentation with Scalar

### Business Capabilities

- Real-time inventory tracking
- Low stock alerts and notifications
- Purchase order management
- Sales order processing
- Supplier management
- Multi-warehouse operations
- User role management
- Transaction audit logging

---

## Tech Stack

### Core Technologies

| Component | Technology | Version |
|-----------|-----------|---------|
| **Framework** | .NET Web API | 9.0 |
| **ORM** | Entity Framework Core | 9.0.9 |
| **Database** | Microsoft SQL Server | 2019+ |

### Libraries and Tools

| Purpose | Library | Description |
|---------|---------|-------------|
| **Authentication** | JWT Bearer | Token-based authentication |
| **Validation** | FluentValidation | Request validation |
| **Logging** | Serilog | Structured logging |
| **Documentation** | Swagger/OpenAPI | API documentation |
| **Documentation UI** | Scalar | Modern API documentation interface |
| **Rate Limiting** | ASP.NET Core Rate Limiting | Request throttling |
| **Caching** | IMemoryCache | In-memory caching |
---

## Project Structure

```
inventory_management_system/
│
├── Controllers/                    # API Controllers
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── OrdersController.cs
│   └── ...
│
├── Data/                          # Database Context
│   └── ApplicationDBContext.cs
│
├── DTOs/                          # Data Transfer Objects
│   ├── Requests/                 # Request DTOs
│   │   ├── CreateProductRequest.cs
│   │   ├── UpdateProductRequest.cs
│   │   └── ...
│   └── Responses/                # Response DTOs
│       ├── ProductResponse.cs
│       ├── OrderResponse.cs
│       └── ...
│
├── Enums/                         # Enumeration Types
│   ├── OrderStatus.cs
│   ├── OrderType.cs
│   ├── UserRole.cs
│   └── ...
│
├── Exceptions/                    # Custom Exceptions
│   ├── NotFoundException.cs
│   ├── UnauthorizedException.cs
│   └── ...
│
├── Extensions/                    # Extension Methods
│   ├── ServiceExtensions.cs
│   └── JwtExtensions.cs
│
├── Filters/                       # Action Filters
│   └── RolePermissionAttribute.cs
│
├── Helpers/                       # Helper Classes
│   ├── ProblemDetailHelper.cs
│   └── ...
│
├── Middleware/                    # Custom Middleware
│   └── GlobalExceptionMiddleware.cs
│
├── Migrations/                    # EF Core Migrations
│   └── [Timestamp]_InitialCreate.cs
│
├── Models/                        # Domain Entities
│   ├── User.cs
│   ├── Product.cs
│   ├── Category.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── Inventory.cs
│   ├── Warehouse.cs
│   └── ...
│
├── Repository/                    # Data Access Layer
│   ├── Implementations/
│   │   ├── ProductRepository.cs
│   │   ├── OrderRepository.cs
│   │   └── ...
│   └── Interfaces/
│       ├── IProductRepository.cs
│       ├── IOrderRepository.cs
│       └── ...
│
├── Security/                      # Security Utilities
│   └── PasswordHasher.cs
│
├── Services/                      # Business Logic Layer
│   ├── Implementations/
│   │   ├── AuthService.cs
│   │   ├── ProductService.cs
│   │   ├── OrderService.cs
│   │   └── ...
│   └── Interfaces/
│       ├── IAuthService.cs
│       ├── IProductService.cs
│       ├── IOrderService.cs
│       └── ...
│
├── Validations/                   # FluentValidation Validators
│   ├── CreateProductValidator.cs
│   ├── LoginRequestValidator.cs
│   └── ...
│
├── logs/                          # Application Logs
│   └── log-YYYYMMDD.txt
│
├── appsettings.json              # Configuration
├── appsettings.Development.json  # Development Config
├── appsettings.Production.json   # Production Config
├── Program.cs                    # Application Entry Point
└── inventory_management_system.csproj
```

---

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8 SDK** or later ([Download](https://dotnet.microsoft.com/download))
- **SQL Server** (LocalDB, Express, or Full Edition)
- **Visual Studio 2022** / **VS Code** / **JetBrains Rider**
- **Git** (for version control)

### Installation

#### Step 1: Clone the Repository

```bash
git clone <repository-url>
cd inventory_management_system
```

#### Step 2: Restore Dependencies

```bash
dotnet restore
```

#### Step 3: Update Configuration

Edit `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-super-secret-key-minimum-32-characters-long",
    "Issuer": "InventoryManagementSystem",
    "Audience": "IMSUsers",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Database Setup

#### Step 1: Create Database Migration

```bash
dotnet ef migrations add InitialCreate
```
#### Step 2: Apply Migration to Database

```bash
dotnet ef database update
```

### Running the Application
#### Development Mode

```bash
dotnet run
```

Or with watch mode (auto-restart on file changes):

```bash
dotnet watch run
```
