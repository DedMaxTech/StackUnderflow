namespace StackUnderflow.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
        public User Author { get; set; }
        //public List<Answer> Answers { get; set; } = new();
        //public List<Tag> Tags { get; set; } = new();
    }
}
