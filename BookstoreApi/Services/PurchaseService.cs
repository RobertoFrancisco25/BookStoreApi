using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Exceptions;
using BookstoreApi.Models;
using BookstoreApi.Services.Interfaces;

namespace BookstoreApi.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper  _mapper;
    private readonly IStockService _stockService;

    public PurchaseService(IUnitOfWork uof, IMapper mapper, IStockService stockService)
    {
        _uof = uof;
        _mapper = mapper;
        _stockService = stockService;
    }
    
    public async Task<PurchaseResponseDTO> MakePurchaseAsync(PurchaseCreateDTO purchaseDTO)
    {
        var purchaseItems = new List<PurchaseItem>();
        decimal total = 0;
        var bookId = 0;
        var stock = 0;
        foreach (var item in purchaseDTO.Items)
        {
            var book = await _uof.Books.GetAsync(b => b.Id == item.BookId);
            if (book is null)
            {
                throw new NotFoundException("Book not found");
            }
            total += book.Price * item.Quantity;
            purchaseItems.Add(new PurchaseItem
            {
                BookId = item.BookId,
                BookName = book.Name,
                Quantity = item.Quantity,
                UnitPrice = book.Price, 
            });
          bookId = item.BookId;
          stock = item.Quantity;

        }

        var purchase = new Purchase
        {
            PurchaseDate = DateTime.UtcNow,
            Total = total,
            Items = purchaseItems,
        };
        _uof.Purchases.Create(purchase);
        await _stockService.UpdateStockAsync(bookId, stock);
        await _uof.CommitAsync();
        var purchaseItemDTO= _mapper.Map<PurchaseResponseDTO>(purchase); 
        return purchaseItemDTO;

    }

    public async Task<PurchaseResponseDTO> GetPurchaseByIdAsync(int id)
    {
        var purchase = await _uof.Purchases.GetPurchaseAsync(id);
        if (purchase is null)
        {
            throw new NotFoundException("Purchase not found");
        }
        var purchaseDTO = _mapper.Map<PurchaseResponseDTO>(purchase);
        return purchaseDTO;
    
    }

    public async Task<bool> DeletePurchaseAsync(int id)
    {
        var purchase = await _uof.Purchases.GetPurchaseAsync(id);
        if (purchase is null)
        {
            throw new NotFoundException("Purchase not found");
        }
        await _uof.Purchases.DeletePurchaseAsync(purchase);
        await _uof.CommitAsync();
        return true;
    }

      
    

    
    
}