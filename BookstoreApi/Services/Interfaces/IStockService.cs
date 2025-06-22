namespace BookstoreApi.Services.Interfaces;

public interface IStockService
{
    public Task<bool> UpdateStockAsync(int bookId,int quantity);
}