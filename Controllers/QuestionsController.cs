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
    [Route("questions")]
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
        [HttpGet("new")]
		public async Task<IActionResult> New()
        {
			return View(new QuestionViewModel() { AvailableTags = await db.Tags.Select(o=>o.Name).ToListAsync()});
        }

        [Authorize()]
        [HttpPost("new")]
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
                return RedirectToAction(nameof(Detail), new { name = q.Title });
            }
            form.AvailableTags = db.Tags.Select(o => o.Name).ToList();
            return View(form);
        }
        [Route("{name}")]
        public async Task<IActionResult> Detail(string name)
        {
			var q = db.Questions
                        .Include(q => q.Tags)
                        .Include(q => q.Author)
                        .Include(q => q.Answers).ThenInclude(a => a.Author)
                        .Include(q => q.Votes)
                        .Include(q => q.Answers).ThenInclude(a => a.Votes)
                        .Include(q => q.Answers).ThenInclude(a => a.Comments).ThenInclude(c => c.Author)
                        .FirstOrDefault(q => q.Title == name);
            if (q == null) return NotFound();
            return View(q);
        }
        [Authorize()]
        [Route("{name}/vote/{vote}")]
        public async Task<IActionResult> Vote(string name,string vote)
        {
            if (vote != "up" &&  vote != "down") return BadRequest();
            var q = await db.Questions
                        .Include(q => q.Votes).ThenInclude(q=>q.Author)
                        .FirstOrDefaultAsync(q => q.Title == name);
            if (q == null) return NotFound();
            var v = q.Votes.FirstOrDefault(v => v.AuthorId == auth.User.Id);
            if (v==null)
                db.Votes.Add(new Vote { Author = auth.User, Question = q, IsUp = vote == "up" });
             else if (v.IsUp != (vote=="up"))
                db.Votes.Remove(v);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), new { name = q.Title });
        }

        [Authorize()]
        [Route("{name}/answer/new")]
        public async Task<IActionResult> NewAnswer(string name)
        {
            if (db.Questions.FirstOrDefault(q => q.Title == name) == null) return NotFound();
            return View();
        }
        [Authorize()]
        [HttpPost("{name}/answer/new")]
        public async Task<IActionResult> NewAnswer(string name, Answer answer)
        {
            var q = await db.Questions.FirstOrDefaultAsync(q => q.Title == name);
            if (q == null) return NotFound();
            if (answer.Body.Length > 15)
            {
                db.Answers.Add(new Answer { Body = answer.Body, Question = q, Author = auth.User });
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Detail), new { name = q.Title });
            }
            else
                ModelState.AddModelError(nameof(answer.Body), "Body is to short");
            return View(answer);
        }

        [Authorize()]
        [Route("{name}/answer/{answerId:int}/vote/{vote}")]
        public async Task<IActionResult> VoteAnswer(string name,int answerId, string vote)
        {
            if (vote != "up" && vote != "down") return BadRequest();
            var a = await db.Answers
                        .Include(a=>a.Question)
                        .Include(a => a.Votes).ThenInclude(q => q.Author)
                        .FirstOrDefaultAsync(c => c.Id==answerId && c.Question.Title==name);
            if (a == null) return NotFound();
            var v = a.Votes.FirstOrDefault(v => v.AuthorId == auth.User.Id);
            if (v == null)
                db.Votes.Add(new Vote { Author = auth.User, Answer = a, IsUp = vote == "up" });
            else if (v.IsUp != (vote == "up"))
                db.Votes.Remove(v);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), new { name = a.Question.Title });
        }
    }
}
