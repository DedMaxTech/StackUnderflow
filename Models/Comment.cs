using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderflow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime Timestamp { get; set; } = DateTime.Now;

        [DeleteBehavior(DeleteBehavior.ClientCascade)]
		public User Author { get; set; } = null!;

		public Answer Answer { get; set; } = null!;
	}
}
