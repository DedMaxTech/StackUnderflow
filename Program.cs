using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Context;
using StackUnderflow.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

builder.Services.AddDbContext<Database>(o => o.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<Auth, Auth>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(option => { option.LoginPath = "/Auth/Login";
                          option.LogoutPath = ""; });
builder.Services.AddAuthorization();




var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute("default",
    "{controller=Main}/{action=Index}/{id?}");

app.Run();



public class Auth
{
    public User? User { set; get; } = null;
    public Auth(IHttpContextAccessor ctx, Database db)
    {
        if (ctx.HttpContext?.User.Identity != null)
        {
            User = db.Users.Where(o => o.Login == ctx.HttpContext.User.Identity.Name).FirstOrDefault();
        }
    }

}