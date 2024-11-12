using System.ComponentModel.DataAnnotations;

namespace Bookfinder.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Nova coleção para relacionamento n:n com Book
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>(); // Adiciona a coleção de Reviews
    }
}
