using System.Linq.Expressions;
using BookstoreApi.Models;

namespace BookstoreApi.Repositories.Interfaces;

public interface IPurchaseRepository : IGenericRepository<Purchase>
{
    public Task<Purchase> GetPurchaseAsync(int id);
    public Task<bool> DeletePurchaseAsync(Purchase id);
}