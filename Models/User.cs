using System.ComponentModel.DataAnnotations;

namespace StackUnderflow.Models
{
    public class User
    {
        public int Id { get; set; }
		public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; } = "https://media.istockphoto.com/id/1495088043/vector/user-profile-icon-avatar-or-person-icon-profile-picture-portrait-symbol-default-portrait.jpg?s=612x612&w=0&k=20&c=dhV2p1JwmloBTOaGAtaA3AW1KSnjsdMt7-U_3EZElZ0=";

		public int Reputation { get; set; } = 0;
        public List<Question> Questions { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public List<Answer> Answers { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
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
