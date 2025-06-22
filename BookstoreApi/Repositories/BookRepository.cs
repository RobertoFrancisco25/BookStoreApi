using System.Linq.Expressions;
using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(AppDbContext context) : base(context)
    {
    }

    public IQueryable<Book> GetByCategory(int id)
    {
        var books = _context.Books.Where(b => b.CategoryId == id).AsNoTracking();
        return books;
    }
}

