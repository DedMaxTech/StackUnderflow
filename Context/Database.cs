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
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Tag>()
        //            .HasMany(t => t.Questions)
        //            .WithMany(q => q.Tags).OnDelete(DeleteBehavior.Restrict)
        //            .UsingEntity(qt => qt.On);
        //}
        public Database(DbContextOptions<Database> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
			//Users.Load();
			//Questions.Load();
			//Answers.Load();
			//Comments.Load();
		}
    }
}
