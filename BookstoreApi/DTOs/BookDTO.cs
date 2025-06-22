

using System.Text.Json.Serialization;
using BookstoreApi.Converters;

namespace BookstoreApi.DTOs;

public class BookDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int NumberOfPages { get; set; }
    [JsonConverter(typeof(JsonDecimalConverter))]
    public decimal Price { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public int InStock { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
}