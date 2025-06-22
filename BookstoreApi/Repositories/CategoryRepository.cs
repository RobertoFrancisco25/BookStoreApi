using BookstoreApi.Data;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }


    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }
}