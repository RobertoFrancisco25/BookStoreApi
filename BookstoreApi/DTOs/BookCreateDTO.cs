using System.ComponentModel.DataAnnotations;

using BookstoreApi.Models;

namespace BookstoreApi.DTOs;

public class BookCreateDTO
{
        /// <summary>
        /// The title of the book. Maximum length is 50 characters.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// A detailed description of the book. Maximum length is 1000 characters.
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// The total number of pages in the book. Must be at least 1.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of pages must be at least 1")]
        public int NumberOfPages { get; set; }

        /// <summary>
        /// The price of the book. Must be a positive value.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        /// <summary>
        /// The author of the book. Maximum length is 20 characters.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string? Author { get; set; }

        /// <summary>
        /// The ISBN (International Standard Book Number) of the book.
        /// </summary>
        [Required]
        public string ISBN { get; set; }

        /// <summary>
        /// The number of copies available in stock. Must be a positive integer.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive")]
        public int InStock { get; set; }

        /// <summary>
        /// The URL of the book cover image. Maximum length is 400 characters.
        /// </summary>
        [Required]
        [StringLength(400)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The ID of the category to which the book belongs.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

}
   