using System.Linq.Expressions;
using AutoMapper;
using BookstoreApi.Data;
using Microsoft.EntityFrameworkCore;
namespace BookstoreApi.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAllQueryable()
    {
        return  _context.Set<T>().AsNoTracking().AsQueryable();
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public T Create(T entity)
    {
         _context.Set<T>().Add(entity);
        return entity;
    }

    public  T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
         _context.Set<T>().Remove(entity);
        return entity;
    }
}