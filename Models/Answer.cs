namespace StackUnderflow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public DateTime Timestamp { get; set; }
        public User Author { get; set; }
    }
}
