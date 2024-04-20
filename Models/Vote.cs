using Microsoft.EntityFrameworkCore;

namespace StackUnderflow.Models
{
	public class Vote
	{
		public int Id { get; set; }
		public bool IsUp {  get; set; }
		[DeleteBehavior(DeleteBehavior.ClientCascade)]
		public User Author { get; set; } = null!;
		public int AuthorId { get; set; }

		public int? QuestionId { get; set; }
		public Question? Question { get; set; }

		public int? AnswerId { get; set; }
		public Answer? Answer { get; set; }
	}
}
