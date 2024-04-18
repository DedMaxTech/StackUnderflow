namespace StackUnderflow.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
        public User Author { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
