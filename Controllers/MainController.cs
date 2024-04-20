using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Context;

namespace StackUnderflow.Controllers
{
    public class MainController : Controller
    {
		Database db;
		public MainController(Database db)
		{
			this.db = db;
		}
		public IActionResult Index()
        {
			var q = db.Questions.Include(q => q.Author).Include(q => q.Answers).ToList();
			return View(q);
        }
    }
}
