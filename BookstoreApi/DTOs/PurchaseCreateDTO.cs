using BookstoreApi.Models;

namespace BookstoreApi.DTOs;

public class PurchaseCreateDTO
{
    /// <summary>
    /// Collection of purchased book items included in this purchase.
    /// </summary>
    public ICollection<PurchaseItemDTO> Items { get; set; }
}