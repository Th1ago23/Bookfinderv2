using Bookfinder.Models;

namespace Bookfinder.Service
{
    public interface InterfaceAPI
    {
        Task<List<Book>> GetBooksAsync();  // Método para obter uma lista de livros
        Task<Book> GetBookDetailsAsync(string bookKey);  // Método para obter os detalhes de um livro específico
    }
}
