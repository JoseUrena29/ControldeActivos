using Proyecto_ControldeActivos.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto_ControldeActivos.Controllers
{
    public class ActivosController : Controller
    {
        public ActionResult CheckRol()
        {
            var username = Session["Username"];
            var rol = Session["Rol"];

            // Verifica la sessión
            if (username == null || rol == null)
            {
                return Redirect("/");
            }

            if (Session["rol"] == null || Session["rol"].ToString() != "admin")
            {
                TempData["ErrorMessage"] = "No tienes permiso para realizar esta acción.";
                return RedirectToAction("Index", "Activos");
            }

            return null;
        }


        public ActionResult GraficoActivos()
            {
                using (DbModels context = new DbModels())
                {
                    // Obtener los activos agrupados por estado
                    var activosPorEstado = context.Activos
                        .GroupBy(a => a.Estado)
                        .Select(g => new
                        {
                            Estado = g.Key,
                            Cantidad = g.Count()
                        }).ToList();

                    // Pasar los datos a la vista
                    ViewBag.Estados = activosPorEstado.Select(a => a.Estado).ToArray();
                    ViewBag.Cantidades = activosPorEstado.Select(a => a.Cantidad).ToArray();
                }

                return View();
            }

        // GET: Activos
        public ActionResult Index(string searchString, string status, DateTime? startDate, DateTime? endDate)
        {
            var result = CheckRol();
            if (result != null) return result;

            using (DbModels context = new DbModels())
            {
                // Obtén todos los activos
                var activos = context.Activos.AsQueryable();

                // Filtrar por nombre, descripción o estado si searchString no está vacío
                if (!string.IsNullOrEmpty(searchString))
                {
                    activos = activos.Where(a => a.Nombre.Contains(searchString)
                                               || a.Descripcion.Contains(searchString));
                }

                // Filtrar por estado si se selecciona uno
                if (!string.IsNullOrEmpty(status))
                {
                    activos = activos.Where(a => a.Estado == status);
                }

                // Filtrar por fecha de adquisición si las fechas están proporcionadas
                if (startDate.HasValue)
                {
                    activos = activos.Where(a => a.FechaAdquisicion >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    activos = activos.Where(a => a.FechaAdquisicion <= endDate.Value);
                }

                // Pasar valores a la vista
                ViewBag.SearchString = searchString;
                ViewBag.Status = status;
                ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

                return View(activos.ToList());
            }
        }

        // GET: Activos/Details/5
        public ActionResult Details(int id)
        {
            using (DbModels context = new DbModels())
            {
                return View(context.Activos.Where(x=>x.Id == id).FirstOrDefault());
            }
        }

        // GET: Activos/Create
        public ActionResult Create()
        {
            var result = CheckRol();
            if (result != null) return result;

            return View();
        }

        // POST: Activos/Create
        [HttpPost]
        public ActionResult Create(Activos activos)
        {
            var result = CheckRol();
            if (result != null) return result;

            try
            {
                using (DbModels context = new DbModels())
                {
                    context.Activos.Add(activos);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Activos/Edit/5
        public ActionResult Edit(int id)
        {

            var result = CheckRol();
            if (result != null) return result;

            using (DbModels context = new DbModels())
            {
                return View(context.Activos.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Activos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Activos activos)
        {

            var result = CheckRol();
            if (result != null) return result;

            try
            {
                using (DbModels context = new DbModels())
                {
                    context.Entry(activos).State = System.Data.EntityState.Modified;
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Activos/Delete/5
        public ActionResult Delete(int id)
        {
            var result = CheckRol();
            if (result != null) return result;

            using (DbModels context = new DbModels())
            {
                return View(context.Activos.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Activos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var result = CheckRol();
            if (result != null) return result;

            try
            {
                using (DbModels context = new DbModels())
                {
                    Activos activo = context.Activos.Where(x =>x.Id == id).FirstOrDefault();
                    context.Activos.Remove(activo);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
