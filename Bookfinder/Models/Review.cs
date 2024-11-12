using System.ComponentModel.DataAnnotations;

namespace Bookfinder.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; } // Conteúdo da resenha

        public int BookId { get; set; } // ID do livro associado
        public virtual Book Book { get; set; } // Relacionamento com Book

        public int UserId { get; set; } // ID do usuário que escreveu a resenha
        public virtual User User { get; set; } // Relacionamento com User

        public DateTime CreatedAt { get; set; } // Data e hora de criação
    }
}
