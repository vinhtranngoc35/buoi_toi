namespace BuoiToi.Services.DTO
{
    public class CartResponse
    {
        public int Id { get; set; }

        public int Price { get; set; }

        public List<CartDetailResponse> Details { get; set; }   = new List<CartDetailResponse>();
    }
    public class CartDetailResponse
    {
        public int BillDetailId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public int ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}
