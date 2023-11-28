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
                                     where b.Id == cart.Id
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
        public async Task<int> RemoveBilDetail(int id)
        {
            var bilDetail = await _db.BillDetails.FirstOrDefaultAsync(e => e.Id == id);
            var quantityOld = await (from b in _db.Bills
                                     join bd in _db.BillDetails on b.Id equals bd.BillId
                                     where b.Id == bilDetail!.BillId
                                     select bd.Quantity)
                                       .SumAsync();
            _db.BillDetails.Remove(bilDetail!);
            await _db.SaveChangesAsync();
            return quantityOld - bilDetail!.Quantity;
        }
        public async Task<CartResponse> GetCartDetail()
        {
            var cart =  await _db.Bills.FirstOrDefaultAsync(e => e.Status == StatusBill.CART);
            
            if (cart == null) return new CartResponse();
            
            var cartDetails = await (from bd in _db.BillDetails
                join p in _db.Products on bd.ProductId equals p.Id
                where bd.BillId == cart!.Id
                select new CartDetailResponse()
                {
                    ProductId = p.Id,
                    ProductImage = p.ImageUrl,
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    Quantity = bd.Quantity,
                    BillDetailId = bd.Id
                }).ToListAsync();
            var totalPrice = cartDetails.Select(e => e.Quantity * e.ProductPrice).Sum();
            return new CartResponse()
            {
                Details = cartDetails,
                Id = cart!.Id,
                Price = totalPrice
            };
        }
        public async Task CheckOut(Bill bill)
        {
            var cart =  await _db.Bills.FirstOrDefaultAsync(e => e.Status == StatusBill.CART);
            
            if (cart == null) throw new Exception("Cart is not exist");
            
            cart.NameCustomer = bill.NameCustomer;
            cart.NumberPhone = bill.NumberPhone;
            cart.Address = bill.Address;
            cart.Note = bill.Note;
            cart.Status = StatusBill.DELIVERY;
            _db.Bills.Update(cart);
            await _db.SaveChangesAsync();
        }
    }
    
    
   
}
