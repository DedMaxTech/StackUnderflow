namespace StackUnderflow.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Author { get; set; }
        //public List<Question> Questions { get; set; } = new();
    }
}
