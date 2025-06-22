using BookstoreApi.DTOs;
using BookstoreApi.Models;
using BookstoreApi.Parameters;

namespace BookstoreApi.Services.Interfaces;

public interface IBookService
{ 
    public Task<PagedResult<BookDTO>> GetAllAsync(BookParameters parameters);
    public Task<PagedResult<BookDTO>> GetByCategoryId(int id, BookParameters parameters);
    public Task<BookDTO> GetByIdAsync(int id);
    public Task<Book> CreateAsync(BookCreateDTO bookDTO);
    public Task UpdateAsync(int id,BookCreateDTO bookDTO);
    public Task DeleteAsync(int id);
}