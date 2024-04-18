namespace StackUnderflow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
        public User Author { get; set; }
    }
}
