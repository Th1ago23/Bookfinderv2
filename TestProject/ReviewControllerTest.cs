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


        //

        [Fact]
        public async Task Create_ReturnsRedirectToActionResult_WhenContentIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyContext>()
                .UseInMemoryDatabase(databaseName: "BookfinderTestDb")
                .Options;

            // Criar o contexto com uma base de dados em memória
            using var context = new MyContext(options);

            // Criar o livro fictício e adicionar ao banco de dados
            var book = new Book { Id = 1, Title = "Livro Teste", Author = "Autor Teste", Key = "sdfsdfsdf", UserId = "1" };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            // Mockar o UserManager e o SignInManager
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var controller = new ReviewController(context);

            var userId = "123"; // Mockar o userId
            var bookId = 1;
            var content = "Excelente livro!";


            // Mockar o contexto do usuário autenticado
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var identity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(identity);
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var result = await controller.Create(bookId, content);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(bookId, redirectResult.RouteValues["bookId"]);
        }





    }
}
