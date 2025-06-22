using BookstoreApi.Data;
using BookstoreApi.Repositories.Interfaces;

namespace BookstoreApi.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _disposed = false;

    public ICategoryRepository Categories { get; }
    public IBookRepository Books { get; }
    public IPurchaseRepository Purchases { get; }


    public UnitOfWork(AppDbContext context, ICategoryRepository categoryRepo, IBookRepository bookRepo,
        IPurchaseRepository purchaseRepo)
    {
        _context = context;
        Categories = categoryRepo;
        Books = bookRepo;
        Purchases = purchaseRepo;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}