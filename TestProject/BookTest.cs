using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Bookfinder.Controllers;
using Bookfinder.Models;
using Bookfinder.Service;
using Bookfinder.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestBookController;
public class BookControllerTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly Mock<InterfaceAPI> _mockService;

    public BookControllerTests()
    {
        // Configuração do mock para o serviço
        _mockService = new Mock<InterfaceAPI>();

        // Cria o container de serviços
        var serviceCollection = new ServiceCollection();

        // Adiciona o contexto em memória ao DI
        serviceCollection.AddDbContext<MyContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));

        // Registra o serviço mockado
        serviceCollection.AddSingleton(_mockService.Object);

        // Registra o BookController no contêiner de DI
        serviceCollection.AddScoped<BookController>();

        // Cria o provider de serviços
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithBooks()
    {
        // Arrange: Mocka a resposta do serviço para retornar uma lista de 15 livros
        var books = new List<Book>();
        for (int i = 1; i <= 15; i++)
        {
            books.Add(new Book { Id = i, Title = $"Book {i}", Author = $"Author {i}" });
        }

        // Configura o mock para retornar a lista de 15 livros
        _mockService.Setup(service => service.GetBooksAsync()).ReturnsAsync(books);

        // Obtém o controlador a partir do container de DI
        var controller = _serviceProvider.GetRequiredService<BookController>();

        // Act: Executa o método Index
        var result = await controller.Index();

        // Assert: Verifica se o retorno foi um ViewResult
        var viewResult = Assert.IsType<ViewResult>(result);

        // Verifica se o modelo da view é uma lista de livros
        var model = Assert.IsAssignableFrom<List<Book>>(viewResult.Model);

        // Verifica se a quantidade de livros é a esperada (15 livros mockados)
        Assert.Equal(15, model.Count);
    }
}
