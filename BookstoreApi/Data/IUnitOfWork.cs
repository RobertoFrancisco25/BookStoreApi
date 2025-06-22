using BookstoreApi.Repositories.Interfaces;

namespace BookstoreApi.Data;

public interface IUnitOfWork
{
    public ICategoryRepository Categories { get;}
    public IBookRepository Books { get;}
    public IPurchaseRepository Purchases { get;}
    public Task CommitAsync();
    
}