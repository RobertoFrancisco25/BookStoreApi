using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs;

public class UserRoleDTO
{
    /// <summary>
    /// The user's email address. Must be a valid email format.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    /// <summary>
    /// The name of the role to assign to the user. This field is required.
    /// </summary>
    [Required(ErrorMessage = "Role name is required")]
    public string? RoleName { get; set; }

}