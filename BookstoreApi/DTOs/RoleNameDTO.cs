using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs;

public class RoleNameDTO
{
    /// <summary>
    /// The name of the entity. This field is required.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
}