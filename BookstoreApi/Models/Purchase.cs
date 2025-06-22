using BookstoreApi.DTOs;

namespace BookstoreApi.Models;

public class Purchase
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal Total { get; set; }
    
    public ICollection<PurchaseItem>  Items { get; set; }
    
    
}