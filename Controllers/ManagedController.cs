using ExamenLibrosMvcCore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExamenLibrosMvcCore.Repositories;
using ExamenLibrosMvcCore.Filters;

namespace ExamenLibrosMvcCore.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryUsuarios repo;

        public ManagedController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = await this.repo.GetUserByEmailPasswordAsync(email, password);
            if (usuario != null)
            {
                // Seguridad
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);

                // Creamos el claim para el nombre (apellido)
                //Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                //identity.AddClaim(claimName);

                //Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                //identity.AddClaim(claimId);

                //Claim claimEmail = new Claim(ClaimTypes.Role, usuario.Email);
                //identity.AddClaim(claimEmail);

                Claim claimId = new Claim("id", usuario.IdUsuario.ToString());
                identity.AddClaim(claimId);

                Claim claimEmail = new Claim("Email", usuario.Email.ToString());
                identity.AddClaim(claimEmail);

                Claim claimNombre = new Claim("Nombre", usuario.Nombre.ToString());
                identity.AddClaim(claimNombre);

                Claim claimApellidos = new Claim("Apellidos", usuario.Apellidos.ToString());
                identity.AddClaim(claimApellidos);

                Claim claimFoto = new Claim("Foto", usuario.Foto.ToString());
                identity.AddClaim(claimFoto);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);

                // Lo vamos a llevar a una vista con la informacion
                // Que nos devuelve el filter en tempdata
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                if (TempData["id"] != null)
                {
                    string id = "";
                    id = TempData["id"].ToString();
                    return RedirectToAction(action, controller, new { id = id });
                }
                else
                {
                    return RedirectToAction(action, controller);
                }
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Libros");
        }

        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            return View();
        }
    }
}