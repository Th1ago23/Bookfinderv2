using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookfinder.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Key { get; set; }
        public string? Cover { get; set; }
        public bool IsFavorited { get; set; } // Para marcar se o livro está favoritado

        // Nova coleção para relacionamento n:n com User
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
