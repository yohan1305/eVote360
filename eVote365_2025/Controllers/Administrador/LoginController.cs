using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Login;
using Evote366.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eVote365_2025.Controllers.Administrador
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var usuario = await _usuarioService.LoginAsync(vm.Usuario, vm.Contrasena);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Los datos de acceso son inválidos.");
                return View(vm);
            }

            if (usuario.Estado != EstadoEntidad.Activo)
            {
                ModelState.AddModelError(string.Empty, "El usuario está inactivo.");
                return View(vm);
            }

            if (usuario.Rol == RolUsuario.DirigentePolitico && usuario.PartidoAsignadoId == null)
            {
                ModelState.AddModelError(string.Empty, "No tiene un partido político asignado, por lo tanto no puede iniciar sesión. Por favor, póngase en contacto con un administrador.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim("Id", usuario.Id.ToString()),
                new Claim("Email", usuario.Email),
                new Claim("Rol", usuario.Rol.ToString()),
                new Claim("PartidoId", usuario.PartidoAsignadoId?.ToString() ?? "0"),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return usuario.Rol switch
            {
                RolUsuario.Administrador => RedirectToAction("Admin", "Home"),
                RolUsuario.DirigentePolitico => RedirectToAction("Dirigente", "Home"),
                _ => RedirectToAction("Index")
            };
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
