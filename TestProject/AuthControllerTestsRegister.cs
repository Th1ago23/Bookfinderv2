using Bookfinder.Controllers;
using Bookfinder.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

public class AuthControllerTestsRegister
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _controller;
    public AuthControllerTestsRegister()
    {
        // Mock para IHttpContextAccessor
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        var authenticationSchemeProviderMock = new Mock<IAuthenticationSchemeProvider>();
        var userConfirmationMock = new Mock<IUserConfirmation<User>>();

        // Mock para UserManager
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        // Mock para SignInManager
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            httpContextAccessorMock.Object,
            userClaimsPrincipalFactoryMock.Object,
            optionsMock.Object,
            null,
            authenticationSchemeProviderMock.Object,
            userConfirmationMock.Object);

        // Mock para Logger
        _loggerMock = new Mock<ILogger<AuthController>>();

        // Inicializando o AuthController com os mocks
        _controller = new AuthController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Register_ReturnsViewResult()
    {
        var result = _controller.Register();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Register_ValidModel_ReturnsRedirectToActionResult()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.Register(model);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Book", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Register_InvalidModel_ReturnsViewResult()
    {
        // Arrange
        _controller.ModelState.AddModelError("Email", "Required");

        var model = new RegisterViewModel
        {
            Name = "Test User",
            Email = "",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        // Act
        var result = await _controller.Register(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
    }

}
