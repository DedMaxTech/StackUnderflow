using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Context;
using StackUnderflow.Models;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMarkdown();
builder.Services.AddMvc();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

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

app.UseMarkdown();
app.UseStaticFiles();

app.MapControllerRoute("default",
    "{controller=Main}/{action=Index}/{id:int?}");
//app.MapControllerRoute("detail",
//	"{controller}/{name}", defaults: new { action = "Detail" });

app.Run();



