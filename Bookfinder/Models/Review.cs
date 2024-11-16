using System.ComponentModel.DataAnnotations;

namespace Bookfinder.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
