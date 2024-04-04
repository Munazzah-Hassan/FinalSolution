using Microsoft.AspNetCore.Mvc;

namespace FinalSolution.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
