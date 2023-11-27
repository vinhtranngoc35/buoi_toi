using BuoiToi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuoiToi.Controllers
{
    [Route("/Cart")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{productId}/{quantity}")]
        public async Task<IActionResult> AddToCart(int productId, int quantity,  [FromQuery] bool isCheckout)
        {
            
            var total = await _cartService.AddToCart(productId, quantity);
            if (isCheckout)
            {
                return Redirect("/Cart/Checkout/?total=" +total);
            }
            return Redirect("/Product/View/" + productId + "?total=" +total);
        }
        
        [HttpGet("CheckOut")]
        public async Task<IActionResult> CheckOut([FromQuery] string? total)
        {
            
            ViewBag.Cart = await _cartService.GetCartDetail();
            if (total != null)
            {
                ViewBag.Total = total;
            }
            
            return  View();
        } 
        [HttpGet("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            
            int total =  await _cartService.RemoveBilDetail(id);
            return  RedirectToAction("CheckOut", new { total = total.ToString() });
        } 
    }
}
