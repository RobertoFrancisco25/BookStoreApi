using BookstoreApi.DTOs;

namespace BookstoreApi.Models;

public class PurchaseItem
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookName { get; set; }
    public Book Book { get; set; }
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public int PurchaseID { get; set; }
    public Purchase Purchase { get; set; }
}