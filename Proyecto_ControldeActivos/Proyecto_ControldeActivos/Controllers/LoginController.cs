// Controllers/LoginController.cs
using Proyecto_ControldeActivos.Models;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_ControldeActivos.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login/Login
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Activos");
            }
            return View();
        }

        // POST: Login/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //// Lógica para verificar las credenciales (esto es solo un ejemplo)
                //if (model.Username == "admin" && model.Password == "admin")
                //{
                //    // Redirigir al usuario a la página de inicio u otra página
                //    return RedirectToAction("Index", "Activos");
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                //}

                using (DbModels context = new DbModels())
                {
                    // Find the user by username
                    var user = context.Usuarios.SingleOrDefault(u => u.Usuario == model.Username);


                    if (user != null)
                    {
                        // Validate the password

                        if (user.Contrasena.Trim() == model.Password)
                        {
                            // Set session or cookie for authentication
                            Session["UserId"] = user.ID;
                            Session["Username"] = user.Usuario.Trim();
                            Session["Rol"] = user.Rol.Trim();

                            //ViewBag.username = Session["Username"];
                            //ViewBag.rol = Session["Rol"];

                            TempData["username"] = Session["Username"];
                            TempData["rol"] = Session["Rol"];


                            return RedirectToAction("Index", "Activos");
                        }
                    }
                }
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");

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

        public ActionResult Registrar()
        {
            return View();
        }

        // POST: RegistrarUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarUsuario(UserModel model)
        {
            if (ModelState.IsValid)
            {
                using (DbModels context = new DbModels())
                {
                    // Create a new user entity based on the model
                    var nuevoUsuario = new Usuarios
                    {
                        Usuario = model.Usuario,
                        Contrasena = model.Contrasena,
                        Rol = model.Rol
                    };

                    // Save the new user to the database
                    context.Usuarios.Add(nuevoUsuario);
                    context.SaveChanges();
                }

                // Optionally, set a success message
                TempData["SuccessMessage"] = "Usuario registrado exitosamente.";

                // Redirect to an appropriate page (e.g., list of users)
                return RedirectToAction("Registrar", "Login");
            }

            // If the model state is invalid, return the view with errors
            return View(model);
        }






    }
}
