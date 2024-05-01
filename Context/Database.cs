using Microsoft.EntityFrameworkCore;
using StackUnderflow.Models;

namespace StackUnderflow.Context
{
    public class Database : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Image> Images { get; set; }
        public Database(DbContextOptions<Database> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
		}
    }
}
