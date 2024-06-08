using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Stock Quantity")]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [MonthsAgo(12, ErrorMessage = "Opening Date cannot be more than 12 months ago.")]
        public DateTime OpeningDate { get; set; } // Tarih bilgisi

        public List<ProductImageDTO> ProductImages { get; set; }
    }

    public class ProductImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
}
