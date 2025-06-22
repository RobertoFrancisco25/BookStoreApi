using System.Linq.Expressions;
using BookstoreApi.Data;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Repositories;

public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
{
    public PurchaseRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Purchase?> GetPurchaseAsync(int id)
    {
        return await _context.Purchases.AsNoTracking().Include(p => p.Items)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<bool> DeletePurchaseAsync(Purchase purchase)
    {
        _context.Purchases.Remove(purchase);
        var purchaseItems = await _context.PurchaseItems
            .FirstOrDefaultAsync(p => p.PurchaseID == purchase.Id);
        _context.PurchaseItems.Remove(purchaseItems);

        return true;
    }
}