using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Protov4.DAO;
using System.Security.Claims;
using Protov4.DTO;

namespace Protov4.Controllers
{
    public class AccesoController : Controller
    {
        private readonly UsuariosDAO _usuariosDAO;

        public AccesoController(UsuariosDAO usuariosDAO)
        {
            _usuariosDAO = usuariosDAO;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuariosDTO user)
        {
            try
            {
                ((int id_usuario, int id_rol_user), int id_cliente) = _usuariosDAO.ValidarUsuario(user);
                if (id_usuario != 0)
                {
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, user.correo_elec),
                    new Claim("id_usuario", id_usuario.ToString()),
                    new Claim("id_cliente", id_cliente.ToString()),
                    new Claim("id_rol_user", id_rol_user.ToString())

                    };

                    if (id_rol_user == 1)
                    {
                        claims.Add(new Claim("id_rol_user", "1")); // Administrador
                    }
                    else
                    {
                        claims.Add(new Claim("id_rol_user", "2")); // Usuario normal
                    }

                    //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (id_rol_user == 1)
                    {
                        return RedirectToAction("Admin", "Administrador");
                    }
                    else
                    {

                        _usuariosDAO.RegistrarAuditoria(id_usuario, DateTime.Now, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.Error = "Credenciales incorrectas";
                }
                return View();
            }
            catch (System.Exception)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            ClaimsPrincipal c = HttpContext.User; 
            if (c.Identity != null && c.Identity.IsAuthenticated)
            {
                var idUsuarioClaim = c.FindFirstValue("id_usuario");
                if (int.TryParse(idUsuarioClaim, out int id_usuario))
                {
                    bool esInicioSesion = false; // Indicar que es un cierre de sesión
                    _usuariosDAO.RegistrarAuditoria(id_usuario, DateTime.Now, esInicioSesion);
                }
            }
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");

        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(ClientesDTO nclient)
        {
            if (!ModelState.IsValid)
            {
                // El modelo no es válido, regresar a la vista con mensajes de error
                return View();
            }

            if (nclient.contrasena_nueva != nclient.confirmar_contrasena)
            {
                ModelState.AddModelError("confirmar_contrasena", "Las contraseñas no coinciden");
                return View(nclient);
            }

            bool registrado = _usuariosDAO.Registrar(nclient);

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = "Credenciales incorrectas";
            }

            return View();
        }
    }
}
