using System.Text.Json.Serialization;
using BookstoreApi.Converters;

namespace BookstoreApi.DTOs;

public class PurchaseResponseDTO
{
    
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal Total { get; set; }
        public List<PurchaseItemResponseDTO> Items { get; set; }
    
}