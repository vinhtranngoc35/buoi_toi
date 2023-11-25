using System.ComponentModel.DataAnnotations;

namespace BuoiToi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int Price { get; set; }

        public string Brand { get; set; } = string.Empty;
        
        public int MonthWarranty { get; set; }
        
        public int RefundDay { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Category? Category { get; set; }

        public int CategoryId { get; set; }
    }
}
