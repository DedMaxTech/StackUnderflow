using Microsoft.EntityFrameworkCore;
using StackUnderflow.Models;

namespace StackUnderflow.Context
{
    public class Auth
    {
        public User? User { set; get; } = null;
        public Auth(IHttpContextAccessor ctx, Database db)
        {
            if (ctx.HttpContext?.User.Identity != null)
            {
                User = db.Users
                    .Include(u=>u.Votes)
                    .Where(o => o.Login == ctx.HttpContext.User.Identity.Name)
                    .FirstOrDefault();
            }
        }
    }
}
