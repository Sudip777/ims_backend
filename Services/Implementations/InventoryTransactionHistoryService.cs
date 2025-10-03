using inventory_management_system.Data;
using inventory_management_system.DTOs.Requests;
using inventory_management_system.DTOs.Responses;
using inventory_management_system.Models;
using inventory_management_system.Repository.Implementations;
using inventory_management_system.Repository.Interfaces;
using inventory_management_system.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Services.Implementations
{
    public class InventoryTransactionHistoryService : IInventoryTransactionHistoryService
    {
        private readonly IUserService _userService;
        private readonly IInventoryTransactionHistoryRepository _inventoryTransactionRepository;
        private readonly ApplicationDBContext _context;

        public InventoryTransactionHistoryService(IUserService userService, IInventoryTransactionHistoryRepository inventoryTransactionRepository, ApplicationDBContext context)
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _userService = userService;
            _context = context;
           
        }
        public async Task<InventoryTransactionHistoryResponse> CreateInventoryTransactionHistoryAsync(InventoryTransactionHistoryDto transaction)
        {
            var response = transaction.MappedInventoryTransactionHistory();
            response.UserId = _userService.GetCurrentUserId();

            var userExists = await _context.Users.AnyAsync(u => u.UserId == _userService.GetCurrentUserId());
            if (!userExists) throw new InvalidOperationException($"UserId {_userService.GetCurrentUserId()} does not exist.");

            var res = await _inventoryTransactionRepository.CreateInventoryTransactionHistoryAsync(response);
            return new InventoryTransactionHistoryResponse
            {
                TransactionId = res.TransactionId,
                ProductId = res.ProductId,
                WarehouseId = res.WarehouseId,
                QuantityChange = res.QuantityChange,
                TransactionTypeId = res.TransactionTypeId,
                OrderId = res.OrderId,
                PurchaseOrderId = null,
                Details = res.Details,
                UserId = res.UserId,
                TransactionDate = res.TransactionDate,
            };
        }

        public async Task<IEnumerable<InventoryTransactionHistoryResponse>> GetAllInventoryTransactionHistoriesAsync()
        {
            var data = await _inventoryTransactionRepository.GetAllInventoryTransactionHistoriesAsync();
            if (data == null) throw new KeyNotFoundException("No Categories Found.");

            return data.Select(res => new InventoryTransactionHistoryResponse
            {
                TransactionId = res.TransactionId,
                ProductId = res.ProductId,
                WarehouseId = res.WarehouseId,
                QuantityChange = res.QuantityChange,
                TransactionTypeId = res.TransactionTypeId,
                OrderId = res.OrderId,
                PurchaseOrderId = res.PurchaseOrderId,
                Details = res.Details,
                UserId = res.UserId,
                TransactionDate = res.TransactionDate,
            }).ToList();
        }
    }
}
