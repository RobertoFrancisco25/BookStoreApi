using BookstoreApi.Data;
using BookstoreApi.Services.Interfaces;

namespace BookstoreApi.Services;

public class StockService : IStockService
{
    private readonly IUnitOfWork _ufo;

    public StockService(IUnitOfWork ufo)
    {
        _ufo = ufo;
    }
    public async Task<bool> UpdateStockAsync(int bookId,int quantity)
    {
        var book = await _ufo.Books.GetAsync(b=> b.Id ==  bookId);
        if (book.InStock < quantity)
        {
            throw new Exception("Quantity exceeds stock");
        }
        book.InStock -= quantity;
        _ufo.Books.Update(book);
        await _ufo.CommitAsync();
        return true;
    }
}