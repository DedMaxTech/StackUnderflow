using Microsoft.AspNetCore.Mvc;

namespace StackUnderflow.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
