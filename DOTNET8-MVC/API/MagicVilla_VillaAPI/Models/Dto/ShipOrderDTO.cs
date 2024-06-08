using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class ShipOrderDTO
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? PaymentIntentId { get; set; }
      
    }
}
