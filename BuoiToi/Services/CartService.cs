using BuoiToi.Data;
using BuoiToi.Models;
using BuoiToi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BuoiToi.Services
{
    public class CartService
    {
        private readonly SetupDatabase _db;

        public CartService(SetupDatabase db)
        {
            _db = db;
        }

        public async Task<int> AddToCart(int productId, int quantity)
        {
            var cart = await _db.Bills.FirstOrDefaultAsync(b => b.Status == StatusBill.CART);
            if (cart == null)
            {
                cart = new Bill()
                {
                    Status = StatusBill.CART,
                };
                 _db.Bills.Add(cart);
                _db.SaveChanges();
                var billDetail = new BillDetail()
                {
                    Quantity = quantity,
                    ProductId = productId,
                    BillId = cart.Id
                };
                _db.BillDetails.Add(billDetail);
                await _db.SaveChangesAsync();
                return quantity;
            }
            var billDetailHasProduct = await _db.BillDetails.FirstOrDefaultAsync(bd => bd.ProductId == productId);

            var quantityOld = await (from b in _db.Bills
                                     join bd in _db.BillDetails on b.Id equals bd.BillId
                                     select bd.Quantity)
                                       .SumAsync();
            if (billDetailHasProduct == null)
            {
                var billDetail = new BillDetail()
                {
                    Quantity = quantity,
                    ProductId = productId,
                    BillId = cart.Id
                };
                _db.BillDetails.Add(billDetail);

                
               
                await _db.SaveChangesAsync();
                return quantityOld + quantity;
            }
            billDetailHasProduct.Quantity += quantity;
            await _db.SaveChangesAsync();

            return quantityOld + quantity;
        }
    }
}
