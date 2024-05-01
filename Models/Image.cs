using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderflow.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public User Owner { get; set; } = null!;
    }
}
