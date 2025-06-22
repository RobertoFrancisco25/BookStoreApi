using System.Linq.Expressions;

namespace BookstoreApi.Repositories;

public interface IGenericRepository<T>
{
    public IQueryable<T> GetAllQueryable();
    public Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    public T Create(T entity);
    public T Update(T entity);
    public T Delete(T entity);

}