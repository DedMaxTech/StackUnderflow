using System.ComponentModel.DataAnnotations;

namespace StackUnderflow.Models
{
    public class User
    {
        public int Id { get; set; }
		public string Login { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Image { get; set; } = "/images/avatar.jpg";

		public int Reputation { get; set; } = 0;
        public List<Question> Questions { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public List<Answer> Answers { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Vote> Votes { get; set; } = new();
	}

    public record SignupUser(
        [Required(ErrorMessage = "No login provided")]
		[StringLength(18, MinimumLength = 3, ErrorMessage = "Login length should be from 3 to 18")]
        string Login,

		[EmailAddress]
		string Email, 

        [StringLength(18, MinimumLength = 8, ErrorMessage = "Password legnth should be from 8 to 18")]
        string Password
    );
	public record LoginUser(
		[Required(ErrorMessage = "No login provided")]
		[StringLength(18, MinimumLength = 3, ErrorMessage = "Login length should be from 3 to 18")]
		string Login,

		[StringLength(18, MinimumLength = 8, ErrorMessage = "Password legnth should be from 8 to 18")]
		string Password
	);
}
