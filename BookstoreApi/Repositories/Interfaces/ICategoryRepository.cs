using BookstoreApi.Models;

namespace BookstoreApi.Repositories.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    public Task<bool> ExistsAsync(int id);
}