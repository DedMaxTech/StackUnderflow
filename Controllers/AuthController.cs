using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Models;
using System.Security.Claims;
using StackUnderflow.Context;
using System.Text;
using System.Security.Cryptography;

namespace StackUnderflow.Controllers
{
    public class AuthController : Controller
    {
        Database db;
        public AuthController(Database db)
        {
            this.db = db;
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
        [Route("users/{login:string}")]
		public async Task<IActionResult> UserDetail(string login)
		{
            var u = db.Users.Where(o=>o.Login == login).FirstOrDefault();
            if (u == null)
                return NotFound();
			return View(u);
		}
	}
}
