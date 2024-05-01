using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Models;
using System.Security.Claims;
using StackUnderflow.Context;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace StackUnderflow.Controllers
{
    public class AuthController : Controller
    {
        Database db;
        Auth auth;
        IWebHostEnvironment env;
        public AuthController(Database db, Auth auth, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.auth = auth;
            this.env = webHostEnvironment;
        }

        private async Task Auth(string login)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim> { new Claim(ClaimTypes.Name, login) }, 
                    "Cookies"
                )));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid) return View(user);

            using SHA256 hash = SHA256.Create();
            var dbUser = db.Users.Where(q => q.Login == user.Login && q.Password == Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(user.Password)))).FirstOrDefault();
            if (dbUser != null)
            {
                await Auth(dbUser.Login);
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError("", "Wrong login/password pair");
                return View(user);
            }
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupUser user)
        {
            if (ModelState.IsValid)
            {
                using SHA256 hash = SHA256.Create();
                var u = new User()
				{
					Login = user.Login,
					Email = user.Email,
					Password = Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(user.Password))),
				};
                db.Users.Add(u);
                await db.SaveChangesAsync();

                await Auth(user.Login);
                return Redirect("/");
            }
            else return View(user);
        }
        [Authorize()]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
		[Route("users/{login}")]
		public IActionResult UserDetail(string login)
		{
			var u = db.Users.Include(u=>u.Answers).Include(u => u.Questions).Where(o => o.Login == login).FirstOrDefault();
			if (u == null)
				return NotFound();
			return View(u);
		}

        string[] permittedExtensions = { ".png", ".jpeg", ".jpg" };
        [HttpPost]
        [Route("uploadImage")]
        [RequestSizeLimit(4_000_000)]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null) return BadRequest();

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return StatusCode(StatusCodes.Status415UnsupportedMediaType);

            string path = $"/Media/{auth.User.Id}";
            if (!Directory.Exists(env.WebRootPath + path)) 
                Directory.CreateDirectory(env.WebRootPath + path);


            path = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(image.FileName));
            using (var fileStream = new FileStream(env.WebRootPath+path, FileMode.Create))
                await image.CopyToAsync(fileStream);

            db.Images.Add(new() { Owner = auth.User, Path = path });
            await db.SaveChangesAsync();
            return Ok(new { data = new { filePath = path } });
        }
    }
}
