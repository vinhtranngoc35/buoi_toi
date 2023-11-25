using BuoiToi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BuoiToi.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        
        public string NameCustomer { get; set; } = string.Empty;

        public string NumberPhone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public StatusBill? Status { get; set; } = StatusBill.CART;
    }
}
