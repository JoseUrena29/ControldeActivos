// Controllers/LoginController.cs
using Proyecto_ControldeActivos.Models;
using System.Web.Mvc;

namespace Proyecto_ControldeActivos.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Lógica para verificar las credenciales (esto es solo un ejemplo)
                if (model.Username == "admin" && model.Password == "admin")
                {
                    // Redirigir al usuario a la página de inicio u otra página
                    return RedirectToAction("Index", "Activos");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                }
            }

            return View(model);
        }

        // GET: Login/Logout
        public ActionResult Logout()
        {
            // Aquí puedes agregar lógica para limpiar la sesión, cookies, etc.
            Session.Clear();
            Session.Abandon();

            // Redirigir a la página de login después de cerrar sesión
            return RedirectToAction("Login", "Login");
        }
    }
}
