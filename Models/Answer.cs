using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderflow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime Timestamp { get; set; } = DateTime.Now;

        [DeleteBehavior(DeleteBehavior.ClientCascade)]
        public User Author { get; set; } = null!;
		public List<Vote> Votes { get; set; } = new();
		public Question Question { get; set; } = null!;
		public List<Comment> Comments { get; set; } = new();
		public int Rating
		{
			get
			{
				int sum = 0;
				foreach (var vote in Votes)
					sum += vote.IsUp ? 1 : -1;
				return sum;
			}
		}
	}
}
