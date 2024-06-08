using System.ComponentModel.DataAnnotations;

namespace API.Models.Dto
{
    public class OrderDetailDTO
    {
        public string ApplicationUserId { get; set; }
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
