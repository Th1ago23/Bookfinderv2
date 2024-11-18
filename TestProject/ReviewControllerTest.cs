using Bookfinder.Controllers;
using Bookfinder.Data;
using Bookfinder.Models; // Altere para o namespace dos seus modelos
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using NuGet.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class ReviewControllerTests
    {


        [Fact]
        public async Task Index_ReturnsViewWithReviews_WhenBookExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using var context = new MyContext(options);

            // Dados simulados
            var user = new User { Id = 1, Name = "Test User" };
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                Key = "12345", // ou qualquer valor válido
                UserId = "1",
                Reviews = new List<Review>
        {
            new Review { Id = 1, Content = "Great book!", User = user },
            new Review { Id = 2, Content = "Not bad.", User = user }
        }
            };

            context.Users.Add(user);
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var controller = new ReviewController(context);

            // Act
            var result = await controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Review>>(viewResult.Model);
            Assert.Equal(2, model.Count()); // Valida que retornou 2 reviews
        }

    }
}
