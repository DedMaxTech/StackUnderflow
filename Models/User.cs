using System.ComponentModel.DataAnnotations;

namespace StackUnderflow.Models
{
    public class User
    {
        public int Id { get; set; }
		public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Reputation { get; set; } = 0;
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
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
