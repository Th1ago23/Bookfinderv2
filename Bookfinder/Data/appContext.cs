using Microsoft.EntityFrameworkCore;
using Bookfinder.Models;

namespace Bookfinder.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        // DbSet para Books
        public DbSet<Book> Books { get; set; }

        // DbSet para Users
        public DbSet<User> Users { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do relacionamento de muitos para muitos entre User e Book
            modelBuilder.Entity<User>()
                .HasMany(u => u.Books)
                .WithMany(b => b.Users);

            // Configuração do relacionamento de um para muitos entre Book e Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId); // Chave estrangeira no Review

            // Configuração do relacionamento de um para muitos entre User e Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId); // Chave estrangeira no Review

            base.OnModelCreating(modelBuilder);
        }
    }
}
