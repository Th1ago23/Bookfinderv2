using Bookfinder.Controllers;
using Bookfinder.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;  // Adicionado
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;  // Alias de IdentitySignInResult
using MvcSignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

public class AuthControllerTestsLogin
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _controller;

    public AuthControllerTestsLogin()
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
    public void Login_ReturnsViewResult()
    {
        // Act
        var result = _controller.Login();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsRedirectToActionResult()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var user = new User { Email = model.Email };

        _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(user);

        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(
            user, model.Password, false, false))
            .ReturnsAsync(IdentitySignInResult.Success);

        // Act
        var result = await _controller.Login(model);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Book", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsViewResultWithError()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync((User)null);

        // Act
        var result = await _controller.Login(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True(_controller.ModelState.ContainsKey(string.Empty));
        Assert.Contains("Login inválido.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsViewResultWithError()
    {
        // Arrange
        var model = new LoginViewModel
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        var user = new User { Email = model.Email };

        _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(user);

        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(
            user, model.Password, false, false))
            .ReturnsAsync(IdentitySignInResult.Failed);

        // Act
        var result = await _controller.Login(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True(_controller.ModelState.ContainsKey(string.Empty));
        Assert.Contains("Login inválido.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
    }
}
