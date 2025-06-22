using BookstoreApi.DTOs;
using BookstoreApi.Models;

namespace BookstoreApi.Services.Interfaces;

public interface IPurchaseService
{
    public Task<PurchaseResponseDTO> MakePurchaseAsync(PurchaseCreateDTO purchase);
    public Task<PurchaseResponseDTO> GetPurchaseByIdAsync(int id);

    public Task<bool> DeletePurchaseAsync(int id);


}