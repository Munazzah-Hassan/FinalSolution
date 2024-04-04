using FinalSolution.Data;
using FinalSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace FinalSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var toDoItems = _context.ToDoItems.ToList();
            if (toDoItems != null)
            {
                return View(toDoItems);
            }
            else
            {
                return View(new List<ToDo>());
            }
        }

        [HttpPost]
        public IActionResult AddToDoItem(string description, string priority)
        {
            var toDoItem = new ToDo
            {
                Description = description,
                Priority = priority,
                CreatedAt = DateTime.Now
            };

            _context.ToDoItems.Add(toDoItem);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteToDoItem(int id)
        {
            var toDoItem = _context.ToDoItems.Find(id);

            if (toDoItem != null)
            {
                _context.ToDoItems.Remove(toDoItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
