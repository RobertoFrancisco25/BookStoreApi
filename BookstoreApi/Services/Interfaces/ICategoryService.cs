using BookstoreApi.DTOs;
using BookstoreApi.Models;
using BookstoreApi.Parameters;

namespace BookstoreApi.Services.Interfaces;

public interface ICategoryService
{
    public Task<PagedResult<CategoryDTO>> GetAllAsync(CategoryParameters parameters);
    public Task<CategoryDTO> GetByIdAsync(int id);
    public Task<Category> CreateAsync(CategoryCreateDTO categoryDTO);
    public Task UpdateAsync(int id, CategoryCreateDTO categoryDTO);
    public Task DeleteAsync(int id);
}