using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.DTOs;

public class CategoryCreateDTO
{
    /// <summary>
    /// The category name. Maximum length is 50 characters.
    /// </summary>
    [Required] 
    [StringLength(50)]
    public string Name { get; set; }
}