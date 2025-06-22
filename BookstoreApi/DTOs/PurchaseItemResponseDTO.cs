using System.Text.Json.Serialization;
using BookstoreApi.Converters;

namespace BookstoreApi.DTOs;

public class PurchaseItemResponseDTO
{
    public int BookId { get; set; }
    public string BookName { get; set; }
    public int Quantity { get; set; }
    [JsonConverter(typeof(JsonDecimalConverter))]
    public decimal UnitPrice { get; set; }
}