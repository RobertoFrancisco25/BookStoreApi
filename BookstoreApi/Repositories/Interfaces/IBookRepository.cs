using System.Linq.Expressions;
using BookstoreApi.DTOs;
using BookstoreApi.Models;

namespace BookstoreApi.Repositories.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
   public IQueryable<Book> GetByCategory(int id);
}