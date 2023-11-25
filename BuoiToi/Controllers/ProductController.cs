using Microsoft.AspNetCore.Mvc;

namespace BuoiToi.Controllers
{
    [Route("/Demo")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
