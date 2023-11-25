using BuoiToi.Models;
using BuoiToi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuoiToi.Controllers
{
    [Route("Todo")]
    public class TodoController : Controller
    {

        private readonly TodoService _todoService;

       public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }
       
        public IActionResult Index([FromQuery]string Message)
        {
            ViewBag.Todos = _todoService.FindAll();
            
            ViewBag.Message = Message;
            return View();
        }

        [HttpGet("Status/{id}")]
        public IActionResult Status(int id)
        {
            _todoService.UpdateStatus(id);
            return RedirectToAction("Index", new { Message = "Update status success" });
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.Categories = _todoService.GetCategories();
            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(Todo todo)
        {
            _todoService.AddTodo(todo);
            return RedirectToAction("Index",new {Message = "Created"});
        }
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _todoService.GetCategories();
            ViewBag.Todo = _todoService.FindById(id);
            return View();
        }
        [HttpPost("Edit/{id}")]
        public IActionResult Edit(int id, Todo todo)
        {

            _todoService.EditTodo(id, todo);
            return RedirectToAction("Index", new { Message = "Edited" });
        }

    }
}
