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
                return BadRequest();
            }

            var existingBook = await _context.Books
                .SingleOrDefaultAsync(b => b.Key == bookKey);

            if (existingBook == null)
            {
                var bookDetails = await _service.GetBookDetailsAsync(bookKey);

                var book = new Book
                {
                    Key = bookKey,
                    Title = bookDetails.Title,
                    Author = bookDetails.Author,
                    Cover = bookDetails.Cover,
                    IsFavorited = true
                };

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

            var books = await _service.GetBooksAsync();
            return View("Index", books);
        }

        public async Task<IActionResult> FavoriteBooks()
        {
            var favoriteBooks = await _context.Books
                .Where(b => b.IsFavorited)
                .ToListAsync();

            return View(favoriteBooks);
        }

        public async Task<IActionResult> DeleteFavorite(string bookKey)
        {
            if (string.IsNullOrEmpty(bookKey))
            {
                return BadRequest();
            }

            var book = await _context.Books
                .SingleOrDefaultAsync(b => b.Key == bookKey);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Livro removido dos favoritos com sucesso!";
            }
            else
            {
                TempData["Message"] = "Livro não encontrado nos favoritos.";
            }

            return RedirectToAction("FavoriteBooks");
        }

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
                    return View(review);
                }

                review.CreatedAt = DateTime.Now;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Resenha adicionada com sucesso!";
                return RedirectToAction("FavoriteBooks");
            }

            return View(review);
        }
    }


}

