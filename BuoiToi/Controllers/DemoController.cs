using BuoiToi.Data;
using BuoiToi.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.Helpers;
using BuoiToi.Services.Uploads;
using BuoiToi.Services;

namespace BuoiToi.Controllers
{
    [Route("/Product")]
    public class DemoController : Controller
    {
        private readonly SetupDatabase _db;


        private readonly UploadService _uploadService;


        public DemoController(SetupDatabase db, UploadService uploadService)
        {
            _db = db;
            _uploadService = uploadService;
        }


        public async Task<IActionResult> Index([FromQuery] string search = "")
        {
            search ??= string.Empty;

            //gửi qua 1 list _Products với tên biến là Products qua trang Index
            var products = from p in _db.Products
                           join c in _db.Categories on p.CategoryId equals c.Id
                           where
                                c.Name.ToLower().Contains(search.ToLower())
                                || p.Name.ToLower().Contains(search.ToLower())
                                || p.Description.ToLower().Contains(search.ToLower())
                           select new Product()
                           {
                               Id = p.Id,
                               Description = p.Description,
                               Name = p.Name,
                               Price = p.Price,
                               ImageUrl = p.ImageUrl,
                               Category = c
                           };
            ViewBag.Products = await products.ToListAsync();
            ViewBag.Search = search;
            return View("Index");
        }
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View("Create");
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Product product, [FromForm] IFormFile avatar)
        {
            var result = await _uploadService.UploadImage(avatar);
            product.ImageUrl = result?.Uri?.ToString();
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _db.Products.Remove(new Product
            {
                Id=id
            });
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Product = await _db.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(Product product, int id)
        {
           product.Id = id;
            _db.Products.Update(product);
           await _db.SaveChangesAsync(true);
           return RedirectToAction("Index");
        }
        
        [HttpGet("View/{id}")]
        public async Task<IActionResult> View([FromQuery] string total, int id)
        {
            ViewBag.Product = await _db.Products.FirstOrDefaultAsync(e => e.Id == id);
            ViewBag.Total = total;
            return View();
        }
    }
}
