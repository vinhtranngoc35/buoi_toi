using BuoiToi.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var total = await _cartService.AddToCart(productId, quantity);
            return Redirect("/Product/View/" + productId + "?total=" +total);
        }
    }
}
