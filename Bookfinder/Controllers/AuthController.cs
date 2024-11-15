using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookfinder.Models;
using System.Threading.Tasks;

namespace Bookfinder.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Register()
        {
            _logger.LogInformation("Carregando a página de registro.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation("Tentativa de registro com email: {Email}", model.Email);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model válido. Criando o usuário.");

                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário criado com sucesso. Realizando login.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Book");
                }
                else
                {
                    _logger.LogWarning("Falha ao criar o usuário. Erros: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                _logger.LogWarning("Modelo inválido ao tentar registrar o usuário.");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            _logger.LogInformation("Carregando a página de login.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Book");
                    }
                    else
                    {
                        _logger.LogWarning("Falha ao tentar logar o usuário com e-mail: {Email}. Motivo: Credenciais inválidas.", model.Email);
                        ModelState.AddModelError(string.Empty, "Login inválido.");
                    }
                }
                else
                {
                    _logger.LogWarning("Falha ao tentar logar. Usuário não encontrado com e-mail: {Email}.", model.Email);
                    ModelState.AddModelError(string.Empty, "Login inválido.");
                }
            }
            else
            {
                _logger.LogWarning("Falha na validação do formulário de login para o e-mail: {Email}.", model.Email);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                _logger.LogInformation("Usuário {UserName} tentando realizar logout.", User.Identity.Name);

                await _signInManager.SignOutAsync();

                _logger.LogInformation("Usuário {UserName} saiu com sucesso.", User.Identity.Name);
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar realizar logout para o usuário {UserName}.", User.Identity.Name);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
