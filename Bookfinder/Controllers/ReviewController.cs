using Bookfinder.Data;
using Bookfinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bookfinder.Controllers
{
    public class ReviewController : Controller
    {

        private readonly MyContext _context;

        public ReviewController (MyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int bookId)
        {
            Console.WriteLine("Index Review");
            ViewData["BookId"] = bookId;

            var book = await _context.Books
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.Id ==  bookId);

            if (book == null) {
                return NotFound("Livro não encontrado");
            }

            ViewData["BookTitle"] = book.Title;

            return View(book.Reviews);
        }


        [HttpPost]
        public async Task<IActionResult> Create(int bookId, string content)
        {
            Console.WriteLine($"Bucetinha: {bookId}");
            if (string.IsNullOrEmpty(content))
            {
                ModelState.AddModelError("", "O conteúdo da resenha não pode ser vazio.");
                return RedirectToAction("Index", new { bookId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound("Livro não encontrado.");
            }

            var review = new Review
            {
                Content = content,
                BookId = bookId,
                UserId = int.Parse(userId),
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Resenha adicionada com sucesso!";
            return RedirectToAction("Index", new { bookId });
        }
    }
}
