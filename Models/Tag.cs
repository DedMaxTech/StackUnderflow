using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderflow.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
		public string Description { get; set; } = "";


		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime Timestamp { get; set; } = DateTime.Now;
        [DeleteBehavior(DeleteBehavior.ClientCascade)]
        public User Author { get; set; } = null!;
		public List<Question> Questions { get; set; } = new();
    }
}
