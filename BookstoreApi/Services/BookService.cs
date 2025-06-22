using AutoMapper;
using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Exceptions;
using BookstoreApi.Helpers;
using BookstoreApi.Models;
using BookstoreApi.Parameters;
using BookstoreApi.Services.Interfaces;
using Newtonsoft.Json;

namespace BookstoreApi.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _ufo;
    private readonly IMapper _mapper;

    public BookService(IUnitOfWork ufo, IMapper mapper)
    {
        _ufo = ufo;
        _mapper = mapper;
    }

    public async Task<PagedResult<BookDTO>> GetAllAsync(BookParameters parameters)
    {
        var query = _ufo.Books.GetAllQueryable();
        if (!query.Any())
        {
            throw new InternalServerErrorException("No book found");
        }
        var pagedList = await query.ToPagedList(parameters.PageNumber, parameters.PageSize);
        var booksDTO = _mapper.Map<List<BookDTO>>(pagedList);
        var metadata = PaginationMetadata.FromPagedList(pagedList);
        return new PagedResult<BookDTO>
        {
            Items = booksDTO,
            Metadata = metadata,
        };
    }

    public async Task<PagedResult<BookDTO>> GetByCategoryId(int id, BookParameters parameters)
    {
        var existsCategory = await _ufo.Categories.ExistsAsync(id);
        if (!existsCategory)
        {
            throw new NotFoundException("No Category found for the database.");
        }

        var books = _ufo.Books.GetByCategory(id);
        var pagedList = await books.ToPagedList(parameters.PageNumber, parameters.PageSize);
        if (pagedList.TotalCount == 0)
        {
            throw new NotFoundException("No book found for this category.");
        }

        var metadata = PaginationMetadata.FromPagedList(pagedList);
        var booksDTO = _mapper.Map<List<BookDTO>>(pagedList);

        return new PagedResult<BookDTO>
        {
            Items = booksDTO,
            Metadata = metadata,
        };
    }

    public async Task<BookDTO> GetByIdAsync(int id)
    {
        var book = await _ufo.Books.GetAsync(b => b.Id == id);
        if (book is null)
        {
            throw new NotFoundException("No book found for this Id.");
        }

        var bookDTO = _mapper.Map<BookDTO>(book);
        return bookDTO;
    }

    public async Task<Book> CreateAsync(BookCreateDTO bookDTO)
    {
        var exists = await _ufo.Categories.ExistsAsync(bookDTO.CategoryId);
        if (!exists)
        {
            throw new NotFoundException("Category not found.");
        }

        var book = _mapper.Map<Book>(bookDTO);
        _ufo.Books.Create(book);
        await _ufo.CommitAsync();
        return book;
    }

    public async Task UpdateAsync(int id, BookCreateDTO bookDTO)
    {
        var existingBook = await _ufo.Books.GetAsync(b => b.Id == id);
        if (existingBook is null)
        {
            throw new NotFoundException("Book not found for the given ID.");
        }

        _mapper.Map(bookDTO, existingBook);
        await _ufo.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bookToDelete = await _ufo.Books.GetAsync(b => b.Id == id);
        if (bookToDelete is null)
        {
            throw new NotFoundException("Book not found for the given ID.");
        }

        _ufo.Books.Delete(bookToDelete);
        await _ufo.CommitAsync();
    }
}