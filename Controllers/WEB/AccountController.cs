using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Thoth.Web.Models;
using Thoth.Web.Models.ViewModels;
using Thoth.Web.Repositories;

namespace Thoth.Web.Controllers.Web
{
    // [Route("[controller]")] // <-- 1. ¡ELIMINA ESTA LÍNEA!
    public class AccountController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public AccountController(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public IActionResult Registro()
        {

            return View(); 
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Rut = model.Rut,
                    Email = model.Email,
                    Avatar = "default.png",
                    RolId = 3 
                };

                try
                {
                    await _usuarioRepo.CreateUserAsync(usuario, model.Password);
                    TempData["Exito"] = "¡Usuario registrado exitosamente!";
                    return RedirectToAction("Login"); 
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _usuarioRepo.ValidateUserAsync(model.Email, model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authPropoerties = new AuthenticationProperties
            {
              AllowRefresh = true,
              IsPersistent = model.RememberMe  
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authPropoerties);

            return RedirectToAction("Index","Dashboard");
        

        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Valida el token del formulario
        public async Task<IActionResult> Logout()
        {
            // 1. Llama al método de .NET para borrar la cookie de autenticación
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Redirige al usuario a la página de inicio (Login)
            return RedirectToAction("Login", "Account");
        }

    }
}