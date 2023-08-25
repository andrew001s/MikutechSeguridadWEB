using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Protov4.Models;
using Protov4.DAO;
using System.Security.Claims;

namespace Protov4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UsuariosDAO _usuariosDAO;

        public HomeController(UsuariosDAO usuariosDAO)
        {
            _usuariosDAO = usuariosDAO;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MiCuenta()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null && c.Identity.IsAuthenticated)
            {
                if (int.TryParse(c.FindFirstValue("id_cliente"), out int idCliente))
                {
                    var Lista = _usuariosDAO.ListarPedidos(idCliente);
                    return View(Lista);
                }
            }
            return RedirectToAction("Index", "Home"); // Por ejemplo, redirige a la página principal en caso de error
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}