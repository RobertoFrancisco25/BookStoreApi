using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs;

public class PurchaseItemDTO
{
    /// <summary>
    /// The ID of the book.
    /// </summary>
    [Required]
    public int BookId { get; set; }

    /// <summary>
    /// The quantity of books to purchase. Must be at least 1.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

}