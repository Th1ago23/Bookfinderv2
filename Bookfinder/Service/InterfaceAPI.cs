using Bookfinder.Models;

namespace Bookfinder.Service
{
    public interface InterfaceAPI
    {
        Task<List<Book>> GetBooksAsync(); 
        Task<Book> GetBookDetailsAsync(string bookKey); 
    }
}
