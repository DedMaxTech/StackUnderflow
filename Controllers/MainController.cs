using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Context;
using StackUnderflow.ViewModels;

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
			var q = db.Questions.Include(q => q.Author).Include(q => q.Answers).Include(q=>q.Tags).Include(q=>q.Votes).ToList();
			return View(q);
        }
		[Route("tags/{name}")]
		public IActionResult Tag(string name)
		{
			var t = db.Tags
				.Include(t=>t.Questions).ThenInclude(q => q.Author)
				.Include(t => t.Questions).ThenInclude(q => q.Answers)
				.Include(t => t.Questions).ThenInclude(q => q.Tags)
				.Include(t => t.Questions).ThenInclude(q => q.Votes)
				.FirstOrDefault(t=>t.Name == name);
			if (t==null) return NotFound();
			return View(t);
		}
	}
}
