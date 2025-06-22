using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreApi.Models;

public class Book
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int NumberOfPages { get; set; }
    public decimal Price { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public int InStock { get; set; }
    public string? ImageUrl { get; set; } 
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

}