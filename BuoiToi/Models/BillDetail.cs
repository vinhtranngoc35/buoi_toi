using System.ComponentModel.DataAnnotations;

namespace BuoiToi.Models
{
    public class BillDetail
    {
        [Key]
        public int Id { get; set; }

        public int BillId { get; set; }

        public Bill? Bill { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}
