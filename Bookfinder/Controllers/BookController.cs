using Bookfinder.Data;
using Bookfinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeuProjeto.Services;
using Microsoft.AspNetCore.Authorization;


namespace Bookfinder.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly MyContext _context;
        private readonly OpenLibraryService _service;

        public BookController(MyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _service = new OpenLibraryService();
        }

        public async Task<IActionResult> Index()
        {
            var books = await _service.GetBooksAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Books.SingleOrDefaultAsync(i => i.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public async Task<IActionResult> Favorite(string bookKey)
        {
            if (string.IsNullOrEmpty(bookKey))
            {
                return BadRequest(); // Trata caso de chave vazia
            }

            // Verifica se o livro já existe no banco de dados
            var existingBook = await _context.Books
                .SingleOrDefaultAsync(b => b.Key == bookKey);

            if (existingBook == null)
            {
                // Se o livro não existir, pegue seus detalhes da API
                var bookDetails = await _service.GetBookDetailsAsync(bookKey);

                // Crie um novo livro com os detalhes
                var book = new Book
                {
                    Key = bookKey,
                    Title = bookDetails.Title,
                    Author = bookDetails.Author,
                    Cover = bookDetails.Cover,
                    IsFavorited = true // Define que o livro está favoritado
                };

                // Adiciona o livro ao contexto
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                ViewBag.Message = "Livro favoritado com sucesso!";
                ViewBag.Style = "alert alert-success";
            }
            else
            {
                ViewBag.Message = "Este livro já está na sua lista de favoritos.";
                ViewBag.Style = "alert alert-danger";
            }

            // Obtém a lista de livros da API para mostrar novamente na view
            var books = await _service.GetBooksAsync();
            return View("Index", books);
        }

        public async Task<IActionResult> FavoriteBooks()
        {
            // Obtém todos os livros que estão no banco de dados
            var favoriteBooks = await _context.Books
                .Where(b => b.IsFavorited) // Filtra livros favoritados
                .ToListAsync();

            return View(favoriteBooks); // Retorna a view com a lista de livros favoritos
        }

        public async Task<IActionResult> DeleteFavorite(string bookKey)
        {
            if (string.IsNullOrEmpty(bookKey))
            {
                return BadRequest(); // Trata caso de chave vazia
            }

            // Procura o livro na tabela
            var book = await _context.Books
                .SingleOrDefaultAsync(b => b.Key == bookKey);

            if (book != null)
            {
                // Remove o livro do contexto
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Livro removido dos favoritos com sucesso!";
            }
            else
            {
                TempData["Message"] = "Livro não encontrado nos favoritos.";
            }

            // Redireciona de volta para a lista de livros favoritos
            return RedirectToAction("FavoriteBooks");
        }

        // GET: Formulário para adicionar resenha
        public IActionResult AddReview(int bookId)
        {
            ViewBag.BookId = bookId;
            ViewBag.BookTitle = _context.Books.FirstOrDefault(b => b.Id == bookId)?.Title;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                var bookExists = await _context.Books.AnyAsync(b => b.Id == review.BookId);
                if (!bookExists)
                {
                    ModelState.AddModelError("", "O livro associado à resenha não existe.");
                    return View(review); // Retorna à view se o livro não existir
                }

                review.CreatedAt = DateTime.Now; // Data e hora da criação
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Resenha adicionada com sucesso!";
                return RedirectToAction("FavoriteBooks"); // Redireciona após adicionar
            }

            return View(review); // Retorna à view em caso de erro no modelo
        }
    }


}

