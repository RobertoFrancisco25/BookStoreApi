using AutoMapper;
using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Exceptions;
using BookstoreApi.Helpers;
using BookstoreApi.Models;
using BookstoreApi.Parameters;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;
using Newtonsoft.Json;

namespace BookstoreApi.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _ufo;
    private readonly IMapper _mapper;
    
    public CategoryService(IUnitOfWork ufo, IMapper mapper)
    {
        _ufo = ufo;
        _mapper = mapper;
    }

    public async Task<PagedResult<CategoryDTO>> GetAllAsync(CategoryParameters parameters)
    {
        var query = _ufo.Categories.GetAllQueryable();
        var pagedList = await query.ToPagedList(parameters.PageNumber, parameters.PageSize);
        var metadatas = PaginationMetadata.FromPagedList(pagedList);
        var categoriesDTO = _mapper.Map<List<CategoryDTO>>(pagedList);
        
        return new PagedResult<CategoryDTO>
        {
            Items = categoriesDTO,
            Metadata = metadatas
        };
    }

    public async Task<CategoryDTO> GetByIdAsync(int id)
    {
        var category = await _ufo.Categories.GetAsync(c => c.Id == id);
        if (category is null)
        {
            throw new NotFoundException("Category does not exist in the database.");
        }

        var categoryDTO = _mapper.Map<CategoryDTO>(category);
        return categoryDTO;
    }

    public async Task<Category> CreateAsync(CategoryCreateDTO categoryDTO)
    {
        var category = _mapper.Map<Category>(categoryDTO);
        _ufo.Categories.Create(category);
        await _ufo.CommitAsync();
        return category;
    }

    public async Task UpdateAsync(int id, CategoryCreateDTO categoryDTO)
    {
        var existingCategory = await _ufo.Categories.GetAsync(c => c.Id == id);
        if (existingCategory is null)
        {
           throw new NotFoundException("Category does not exist in the database.");
        }
        _mapper.Map(categoryDTO, existingCategory);
        await _ufo.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var categoryToDelete = await _ufo.Categories.GetAsync(c => c.Id == id);
        if (categoryToDelete is null)
        {
            throw new NotFoundException("The category does not exist in the database.");
        }
        _ufo.Categories.Delete(categoryToDelete);
        await _ufo.CommitAsync();
    }
}