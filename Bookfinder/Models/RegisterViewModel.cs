using System.ComponentModel.DataAnnotations;

namespace Bookfinder.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "O nome de usuário deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não são iguais.")]
        public string ConfirmPassword { get; set; }
    }
}
