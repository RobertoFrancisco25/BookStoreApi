using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs;

public class RegisterDTO
{
    /// <summary>
    /// The email address of the user. This field is required.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    /// <summary>
    /// The password of the user. This field is required.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

}