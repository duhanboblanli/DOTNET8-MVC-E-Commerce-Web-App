using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace API.Models
{
    public class Product
    {
        [Key]
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

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
    }

    public class MonthsAgoAttribute : ValidationAttribute
    {
        private readonly int _months;

        public MonthsAgoAttribute(int months)
        {
            _months = months;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                // Belirtilen ay kadar önceki tarihi hesapla
                DateTime minDate = DateTime.Now.AddMonths(-_months);

                // Geçerli tarih, minimum tarihten önce mi kontrol et
                if (date < minDate)
                {
                    return new ValidationResult($"The field {validationContext.DisplayName} cannot be more than {_months} months ago.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
