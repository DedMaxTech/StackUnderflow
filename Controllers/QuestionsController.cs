using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Context;
using StackUnderflow.Models;
using StackUnderflow.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace StackUnderflow.Controllers
{
    public class QuestionsController : Controller
    {
		Database db;
		Auth auth;
        IWebHostEnvironment env;
        public QuestionsController(Database db, Auth auth, IWebHostEnvironment webHostEnvironment)
		{
			this.db = db;
			this.auth = auth;
            this.env = webHostEnvironment;
		}

		[Authorize()]
		[Route("questions/new")]
		public async Task<IActionResult> New()
        {
			return View(new QuestionViewModel() { AvailableTags = await db.Tags.Select(o=>o.Name).ToListAsync()});
        }

        [Authorize()]
        [HttpPost]
		[Route("questions/new")]
        public async Task<IActionResult> New(QuestionViewModel form)
        {
            if (ModelState.IsValid)
            {

                var q = new Question() {
                    Title = form.Title,
                    Body = form.Body,
                    Author = auth.User,
                    Tags = form.SelectedTags.Split(',')
                            .Select(i => db.Tags.FirstOrDefault(o => o.Name == i)==null? 
                                new Tag { Author = auth.User, Name = i }:
                                db.Tags.First(o => o.Name == i))
                            .ToList() };
                db.Questions.Add(q);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Detail), new { title = q.Title });
            }
            form.AvailableTags = db.Tags.Select(o => o.Name).ToList();
            return View(form);
        }

        [Route("questions/{title}")]
        public async Task<IActionResult> Detail(string title)
        {
			var q = db.Questions
                        .Include(q => q.Tags)
                        .Include(q => q.Author)
                        .Include(q => q.Answers).ThenInclude(a => a.Author)
                        .Include(q => q.Answers).ThenInclude(a => a.Comments).ThenInclude(c => c.Author)
                        .FirstOrDefault(q => q.Title == title);
            if (q == null) return NotFound();
            return View(q);
        }
    }
}
