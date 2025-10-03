using inventory_management_system.Data;
using inventory_management_system.Models;
using inventory_management_system.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace inventory_management_system.Repository.Implementations
{
    public class InventoryTransactionHistoryRepository : IInventoryTransactionHistoryRepository
    {
        private readonly ApplicationDBContext _context;

        public InventoryTransactionHistoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<InventoryTransactionHistory> CreateInvenotryTransactionHistoryAsync(InventoryTransactionHistory transaction)
        {
            _context.InventoryTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<InventoryTransactionHistory>> GetAllInventoryTransactionHistoriesAsync()
        {
            return await _context.InventoryTransactions
                .Include(t => t.TransactionType)
                .Include(t => t.Product)
                .Include(t => t.Warehouse)
                .Include(t => t.User)
                .ToListAsync();
        }
    }
}
