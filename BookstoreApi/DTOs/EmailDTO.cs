using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs;

public class EmailDTO
{
    /// <summary>
    /// The user's email address. Must be a valid email format.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }
}