using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Bookfinder.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
