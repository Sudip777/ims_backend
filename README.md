# Inventory Management System - Backend API Documentation

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
- [Architecture](#architecture)
  - [Clean Architecture](#clean-architecture)
  - [Repository Pattern](#repository-pattern)
  - [Service Layer](#service-layer)
- [Authentication and Authorization](#authentication-and-authorization)
  - [JWT Authentication](#jwt-authentication)
  - [Role-Based Authorization](#role-based-authorization)
  - [Authentication Flow](#authentication-flow)
- [API Endpoints](#api-endpoints)
  - [Authentication Endpoints](#authentication-endpoints)
  - [Product Endpoints](#product-endpoints)
  - [Category Endpoints](#category-endpoints)
  - [Supplier Endpoints](#supplier-endpoints)
  - [Inventory Endpoints](#inventory-endpoints)
  - [Order Endpoints](#order-endpoints)
  - [Warehouse Endpoints](#warehouse-endpoints)
  - [User Management Endpoints](#user-management-endpoints)
- [Request and Response Formats](#request-and-response-formats)
  - [Pagination](#pagination)
  - [Filtering and Sorting](#filtering-and-sorting)
  - [Standard Response Format](#standard-response-format)
- [Database Schema](#database-schema)
  - [Entity Relationships](#entity-relationships)
  - [Core Tables](#core-tables)
- [Configuration](#configuration)
  - [Connection Strings](#connection-strings)
  - [JWT Configuration](#jwt-configuration)
  - [CORS Configuration](#cors-configuration)
  - [Logging Configuration](#logging-configuration)
- [Error Handling](#error-handling)
  - [Global Exception Middleware](#global-exception-middleware)
  - [HTTP Status Codes](#http-status-codes)
  - [Error Response Format](#error-response-format)
- [Rate Limiting](#rate-limiting)
  - [Login Rate Limiting](#login-rate-limiting)
  - [Configuration](#rate-limiting-configuration)
- [Logging](#logging)
  - [Serilog Setup](#serilog-setup)
  - [Log Levels](#log-levels)
  - [Log Storage](#log-storage)
- [Validation](#validation)
  - [FluentValidation](#fluentvalidation)
  - [Custom Validators](#custom-validators)
- [Security Best Practices](#security-best-practices)
- [Performance Optimization](#performance-optimization)
- [Testing](#testing)
- [Deployment](#deployment)
  - [Production Checklist](#production-checklist)
  - [IIS Deployment](#iis-deployment)
  - [Docker Deployment](#docker-deployment)
- [Troubleshooting](#troubleshooting)
- [API Documentation](#api-documentation)

---

## Overview

The **Inventory Management System (IMS)** is a production-ready, enterprise-grade RESTful API built with **.NET 8 Web API**. It provides comprehensive functionality for managing inventory operations across multiple warehouses with advanced features like real-time stock tracking, order management, and role-based access control.

### Key Features

- **Product Management**: Complete CRUD operations for products with categories and suppliers
- **Multi-Warehouse Support**: Track inventory across multiple warehouse locations
- **Order Processing**: Handle sales and purchase orders with automatic stock updates
- **Audit Trail**: Comprehensive transaction history for all inventory movements
- **User Management**: Role-based access control with four permission levels
- **API Versioning**: Structured API versioning for backward compatibility
- **Advanced Filtering**: Pagination, filtering, sorting, and searching capabilities
- **Validation**: Robust input validation using FluentValidation
- **Documentation**: Interactive API documentation with Swagger and Scalar

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
| **Framework** | .NET Web API | 8.0 |
| **Language** | C# | 12.0 |
| **ORM** | Entity Framework Core | 8.0 |
| **Database** | Microsoft SQL Server | 2019+ |

### Libraries and Tools

| Purpose | Library | Description |
|---------|---------|-------------|
| **Authentication** | JWT Bearer | Token-based authentication |
| **Validation** | FluentValidation | Request validation |
| **Logging** | Serilog | Structured logging |
| **Mapping** | AutoMapper | Object-to-object mapping |
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

#### Step 3: Seed Initial Data (Optional)

Create a database seeder or manually insert an admin user:

```sql
INSERT INTO Users (Email, PasswordHash, FirstName, LastName, Role, IsActive, CreatedAt)
VALUES ('admin@ims.com', '[BCrypt_Hash]', 'Admin', 'User', 'SuperAdmin', 1, GETDATE());
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

#### Access the API

- **API Base URL**: `https://localhost:5001/api/v1`
- **Swagger UI**: `https://localhost:5001/swagger`
- **Scalar UI**: `https://localhost:5001/scalar/v1`

---

## Architecture

### Clean Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│         (Controllers)                       │
├─────────────────────────────────────────────┤
│         Application Layer                   │
│         (Services, DTOs, Validators)        │
├─────────────────────────────────────────────┤
│         Infrastructure Layer                │
│         (Repository, Data Access)           │
├─────────────────────────────────────────────┤
│         Domain Layer                        │
│         (Models, Entities, Enums)           │
└─────────────────────────────────────────────┘
```

#### Layer Responsibilities

**Presentation Layer (Controllers)**
- Handle HTTP requests and responses
- Input validation routing
- Response formatting
- Authentication/Authorization enforcement

**Application Layer (Services)**
- Business logic implementation
- Transaction coordination
- DTO mapping
- Business rule validation

**Infrastructure Layer (Repository)**
- Database access
- Data persistence
- Query optimization
- Transaction management

**Domain Layer (Models)**
- Core business entities
- Domain logic
- Entity relationships

### Repository Pattern

All database operations are abstracted through repositories:

#### Generic Repository Interface

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

#### Specific Repository Example

```csharp
public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<Product> GetBySkuAsync(string sku);
    Task<IEnumerable<Product>> GetLowStockProductsAsync();
}
```

### Service Layer

Business logic is encapsulated in service classes:

#### Service Interface Example

```csharp
public interface IProductService
{
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
    Task<ProductResponse> GetProductByIdAsync(int id);
    Task<PagedResponse<ProductResponse>> GetProductsAsync(ProductQueryParameters parameters);
    Task<ProductResponse> UpdateProductAsync(int id, UpdateProductRequest request);
    Task DeleteProductAsync(int id);
}
```

#### Benefits

- **Testability**: Easy to mock for unit testing
- **Maintainability**: Clear separation of concerns
- **Reusability**: Services can be used across multiple controllers
- **Flexibility**: Easy to swap implementations

---

## Authentication and Authorization

### JWT Authentication

The API uses **JSON Web Tokens (JWT)** for stateless authentication.

#### Token Types

**Access Token**
- **Lifetime**: 15 minutes
- **Purpose**: Authenticate API requests
- **Storage**: Client-side (memory or secure storage)
- **Claims**: UserId, Email, Role, Expiration

**Refresh Token**
- **Lifetime**: 7 days
- **Purpose**: Obtain new access tokens
- **Storage**: Database (hashed)
- **Security**: Single-use, rotation on refresh

### Role-Based Authorization

#### Available Roles

| Role | Level | Permissions |
|------|-------|-------------|
| **SuperAdmin** | 4 | Full system access, user management, system configuration |
| **Manager** | 3 | Manage inventory, orders, products, view reports |
| **Staff** | 2 | Process orders, update stock, view products |
| **Supplier** | 1 | View assigned products, manage supply orders |

#### Role Hierarchy

```
SuperAdmin
    └── Manager
        └── Staff
            └── Supplier
```

Higher roles inherit permissions from lower roles.

#### Using Roles in Controllers

```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    [HttpGet]
    [RolePermission("Staff", "Manager", "SuperAdmin")]
    public async Task<IActionResult> GetProducts() { }

    [HttpPost]
    [RolePermission("Manager", "SuperAdmin")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request) { }

    [HttpDelete("{id}")]
    [RolePermission("SuperAdmin")]
    public async Task<IActionResult> DeleteProduct(int id) { }
}
```

### Authentication Flow

#### 1. User Login

```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response:**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "3d4f5g6h7j8k9l0m1n2p3q...",
  "expiresIn": 900,
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Manager"
  }
}
```

#### 2. Making Authenticated Requests

```http
GET /api/v1/products
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

#### 3. Token Refresh

When access token expires:

```http
POST /api/v1/auth/refresh
Content-Type: application/json

{
  "refreshToken": "3d4f5g6h7j8k9l0m1n2p3q..."
}
```

**Response:**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "9z8y7x6w5v4u3t2s1r0q9p...",
  "expiresIn": 900
}
```

#### 4. Logout

```http
POST /api/v1/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
Content-Type: application/json

{
  "refreshToken": "3d4f5g6h7j8k9l0m1n2p3q..."
}
```

---

## API Endpoints

### Base URL

```
Production: https://api.yourcompany.com/api/v1
Development: https://localhost:5001/api/v1
```

### Authentication Endpoints

#### Login

```http
POST /api/v1/auth/login
```

**Request Body:**

```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response:** `200 OK`

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "3d4f5g6h7j8k9l0m...",
  "expiresIn": 900,
  "user": {
    "id": 1,
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Manager"
  }
}
```

#### Register

```http
POST /api/v1/auth/register
```

**Request Body:**

```json
{
  "email": "newuser@example.com",
  "password": "Password123!",
  "firstName": "Jane",
  "lastName": "Smith",
  "role": "Staff"
}
```

**Response:** `201 Created`

#### Refresh Token

```http
POST /api/v1/auth/refresh
```

**Request Body:**

```json
{
  "refreshToken": "3d4f5g6h7j8k9l0m..."
}
```

**Response:** `200 OK`

#### Logout

```http
POST /api/v1/auth/logout
Authorization: Bearer {accessToken}
```

**Request Body:**

```json
{
  "refreshToken": "3d4f5g6h7j8k9l0m..."
}
```

**Response:** `204 No Content`

### Product Endpoints

#### Get All Products

```http
GET /api/v1/products?pageNumber=1&pageSize=10&searchTerm=laptop&sortBy=name&sortOrder=asc
Authorization: Bearer {accessToken}
```

**Query Parameters:**

- `pageNumber` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)
- `searchTerm` (optional): Search in name, SKU, description
- `categoryId` (optional): Filter by category
- `supplierId` (optional): Filter by supplier
- `sortBy` (optional): Sort field (name, price, sku)
- `sortOrder` (optional): asc or desc

**Response:** `200 OK`

```json
{
  "items": [
    {
      "id": 1,
      "name": "Laptop Dell XPS 15",
      "sku": "DELL-XPS-15-2024",
      "description": "High-performance laptop",
      "categoryId": 2,
      "categoryName": "Electronics",
      "supplierId": 5,
      "supplierName": "Dell Inc.",
      "unitPrice": 1299.99,
      "reorderLevel": 10,
      "totalStock": 45,
      "createdAt": "2024-01-15T10:30:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 150,
  "totalPages": 15,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

#### Get Product by ID

```http
GET /api/v1/products/{id}
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "id": 1,
  "name": "Laptop Dell XPS 15",
  "sku": "DELL-XPS-15-2024",
  "description": "High-performance laptop",
  "categoryId": 2,
  "categoryName": "Electronics",
  "supplierId": 5,
  "supplierName": "Dell Inc.",
  "unitPrice": 1299.99,
  "reorderLevel": 10,
  "inventoryByWarehouse": [
    {
      "warehouseId": 1,
      "warehouseName": "Main Warehouse",
      "quantity": 25
    },
    {
      "warehouseId": 2,
      "warehouseName": "Secondary Warehouse",
      "quantity": 20
    }
  ],
  "totalStock": 45,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-11-10T14:22:00Z"
}
```

#### Create Product

```http
POST /api/v1/products
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

**Request Body:**

```json
{
  "name": "Laptop HP Pavilion",
  "sku": "HP-PAV-15-2024",
  "description": "Mid-range laptop for everyday use",
  "categoryId": 2,
  "supplierId": 6,
  "unitPrice": 899.99,
  "reorderLevel": 15
}
```

**Response:** `201 Created`

#### Update Product

```http
PUT /api/v1/products/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

**Request Body:**

```json
{
  "name": "Laptop HP Pavilion 15",
  "description": "Updated description",
  "unitPrice": 849.99,
  "reorderLevel": 12
}
```

**Response:** `200 OK`

#### Delete Product

```http
DELETE /api/v1/products/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** SuperAdmin

**Response:** `204 No Content`

### Category Endpoints

#### Get All Categories

```http
GET /api/v1/categories
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "items": [
    {
      "id": 1,
      "name": "Electronics",
      "description": "Electronic devices and accessories",
      "productCount": 45,
      "createdAt": "2024-01-10T09:00:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 8,
  "totalPages": 1
}
```

#### Get Category by ID

```http
GET /api/v1/categories/{id}
Authorization: Bearer {accessToken}
```

#### Create Category

```http
POST /api/v1/categories
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

**Request Body:**

```json
{
  "name": "Office Supplies",
  "description": "Stationery and office equipment"
}
```

#### Update Category

```http
PUT /api/v1/categories/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

#### Delete Category

```http
DELETE /api/v1/categories/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** SuperAdmin

### Supplier Endpoints

#### Get All Suppliers

```http
GET /api/v1/suppliers?pageNumber=1&pageSize=10
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "items": [
    {
      "id": 1,
      "name": "Dell Inc.",
      "contactPerson": "John Anderson",
      "email": "john@dell.com",
      "phone": "+1-555-0123",
      "address": "123 Tech Street, Austin, TX",
      "productCount": 15,
      "isActive": true,
      "createdAt": "2024-01-05T08:00:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 12,
  "totalPages": 2
}
```

#### Get Supplier by ID

```http
GET /api/v1/suppliers/{id}
Authorization: Bearer {accessToken}
```

#### Create Supplier

```http
POST /api/v1/suppliers
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

**Request Body:**

```json
{
  "name": "HP Inc.",
  "contactPerson": "Sarah Wilson",
  "email": "sarah@hp.com",
  "phone": "+1-555-0456",
  "address": "456 Innovation Drive, Palo Alto, CA"
}
```

#### Update Supplier

```http
PUT /api/v1/suppliers/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

#### Delete Supplier

```http
DELETE /api/v1/suppliers/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** SuperAdmin

### Inventory Endpoints

#### Get Inventory by Warehouse

```http
GET /api/v1/inventory?warehouseId=1&pageNumber=1&pageSize=20
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "items": [
    {
      "productId": 1,
      "productName": "Laptop Dell XPS 15",
      "sku": "DELL-XPS-15-2024",
      "warehouseId": 1,
      "warehouseName": "Main Warehouse",
      "quantity": 25,
      "minimumStock": 10,
      "status": "Adequate",
      "lastUpdated": "2024-11-15T14:30:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 150
}
```

#### Get Product Inventory

```http
GET /api/v1/inventory/product/{productId}
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "productId": 1,
  "productName": "Laptop Dell XPS 15",
  "sku": "DELL-XPS-15-2024",
  "totalStock": 45,
  "reorderLevel": 10,
  "stockByWarehouse": [
    {
      "warehouseId": 1,
      "warehouseName": "Main Warehouse",
      "quantity": 25,
      "minimumStock": 10
    },
    {
      "warehouseId": 2,
      "warehouseName": "Secondary Warehouse",
      "quantity": 20,
      "minimumStock": 5
    }
  ]
}
```

#### Adjust Stock

```http
POST /api/v1/inventory/adjust
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

**Request Body:**

```json
{
  "productId": 1,
  "warehouseId": 1,
  "quantity": 10,
  "adjustmentType": "Addition",
  "reason": "Stock replenishment from supplier",
  "reference": "PO-2024-0123"
}
```

**Response:** `200 OK`

#### Get Low Stock Products

```http
GET /api/v1/inventory/low-stock
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "items": [
    {
      "productId": 15,
      "productName": "Wireless Mouse",
      "sku": "MOUSE-WL-001",
      "currentStock": 5,
      "reorderLevel": 20,
      "deficit": 15,
      "supplierId": 3,
      "supplierName": "Logitech"
    }
  ]
}
```

### Order Endpoints

#### Get All Orders

```http
GET /api/v1/orders?pageNumber=1&pageSize=10&orderType=Sale&status=Pending
Authorization: Bearer {accessToken}
```

**Query Parameters:**

- `orderType` (optional): Sale or Purchase
- `status` (optional): Pending, Completed, Cancelled
- `startDate` (optional): Filter by date range
- `endDate` (optional): Filter by date range

**Response:** `200 OK`

```json
{
  "items": [
    {
      "id": 1,
      "orderNumber": "ORD-2024-0001",
      "orderType": "Sale",
      "status": "Pending",
      "userId": 5,
      "userName": "John Doe",
      "totalAmount": 2599.98,
      "itemCount": 2,
      "orderDate": "2024-11-18T10:00:00Z",
      "completedDate": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 45,
  "totalPages": 5
}
```

#### Get Order by ID

```http
GET /api/v1/orders/{id}
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "id": 1,
  "orderNumber": "ORD-2024-0001",
  "orderType": "Sale",
  "status": "Pending",
  "userId": 5,
  "userName": "John Doe",
  "totalAmount": 2599.98,
  "orderDate": "2024-11-18T10:00:00Z",
  "items": [
    {
      "productId": 1,
      "productName": "Laptop Dell XPS 15",
      "sku": "DELL-XPS-15-2024",
      "quantity": 2,
      "unitPrice": 1299.99,
      "totalPrice": 2599.98
    }
  ]
}
```

#### Create Order

```http
POST /api/v1/orders
Authorization: Bearer {accessToken}
```

**Required Role:** Staff, Manager, SuperAdmin

**Request Body:**

```json
{
  "orderType": "Sale",
  "warehouseId": 1,
  "items": [
    {
      "productId": 1,
      "quantity": 2,
      "unitPrice": 1299.99
    },
    {
      "productId": 5,
      "quantity": 1,
      "unitPrice": 499.99
    }
  ]
}
```

**Response:** `201 Created`

#### Update Order Status

```http
PUT /api/v1/orders/{id}/status
Authorization: Bearer {accessToken}
```

**Required Role:** Staff, Manager, SuperAdmin

**Request Body:**

```json
{
  "status": "Completed"
}
```

**Response:** `200 OK`

#### Cancel Order

```http
DELETE /api/v1/orders/{id}
Authorization: Bearer {accessToken}
```

**Required Role:** Manager, SuperAdmin

**Response:** `204 No Content`

### Warehouse Endpoints

#### Get All Warehouses

```http
GET /api/v1/warehouses
Authorization: Bearer {accessToken}
```

**Response:** `200 OK`

```json
{
  "items": [
    {
      "
