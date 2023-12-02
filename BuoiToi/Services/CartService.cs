using BuoiToi.Data;
using BuoiToi.Models;
using BuoiToi.Models.Enums;
using BuoiToi.Services.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BuoiToi.Services
{
    public class CartService
    {
        private readonly SetupDatabase _db;

        private IHttpContextAccessor _contextAccessor;

        public CartService(SetupDatabase db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }

        public async Task<int> AddToCart(int productId, int quantity)
        {
            var cart = await GetCartOfCurrentUser();
            var idUser = await GetCurrentUserId(); 
            if (cart == null)
            {
                cart = new Bill()
                {
                    UserId = idUser,
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
                    BillId = cart.Id,
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
            var cart =  await GetCartOfCurrentUser();
            
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
            var cart =  await GetCartOfCurrentUser();
            
            if (cart == null) throw new Exception("Cart is not exist");
            
            cart.NameCustomer = bill.NameCustomer;
            cart.NumberPhone = bill.NumberPhone;
            cart.Address = bill.Address;
            cart.Note = bill.Note;
            cart.Status = StatusBill.DELIVERY;
            // update price name product cart Details 
            var billDetails = await _db.BillDetails.Where(bd => bd.BillId == cart.Id).ToListAsync();
            foreach (var billDetail in billDetails)
            {
                var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == billDetail.ProductId);
                billDetail.ProductName = product!.Name;
                billDetail.ProductPrice = product!.Price;
                _db.BillDetails.Update(billDetail);
            }
            _db.Bills.Update(cart);
            await _db.SaveChangesAsync();
        }
        public async Task<Bill?> GetCartOfCurrentUser()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await (from bill in _db.Bills
                          join user in _db.Users on bill.UserId equals user.Id
                          where bill.Status == StatusBill.CART && user.Username == username
                          select bill).FirstOrDefaultAsync();
        }

        public async Task<int> GetCurrentUserId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _db.Users.Where(user => user.Username == username).Select(user => user.Id).FirstOrDefaultAsync();

        }
        public async Task<User?> GetCurrentUser()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await _db.Users.FirstOrDefaultAsync(user => user.Username == username);

        }
    }

   
    
    
   
}
