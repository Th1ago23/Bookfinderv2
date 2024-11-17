using Bookfinder.Data;
using Bookfinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookfinder.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Bookfinder.Service;


namespace Bookfinder.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly MyContext _context;
        protected internal readonly OpenLibraryService _service;

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(bookKey))
            {
                return BadRequest();
            }

            var existingBook = await _context.Books
                .Where(b => b.UserId == userId)
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
                    UserId = userId,
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoriteBooks = await _context.Books
                .Where(b => b.IsFavorited)
                .Where(b => b.UserId == userId)
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
    }


}

