using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bookfinder.Models;

namespace Bookfinder.Data
{
    public class MyContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Identity para ID do tipo int
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

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
        }
    }
}
