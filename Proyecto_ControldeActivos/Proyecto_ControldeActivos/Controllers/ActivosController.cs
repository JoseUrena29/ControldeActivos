using Proyecto_ControldeActivos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_ControldeActivos.Controllers
{
    public class ActivosController : Controller
    {
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
        public ActionResult Index(string searchString)
        {
            using (DbModels context = new DbModels())
            {
                // Obtén todos los activos
                var activos = context.Activos.AsQueryable();

                // Si searchString no es nulo o vacío, filtra los activos por nombre, descripción o estado exactos
                if (!String.IsNullOrEmpty(searchString))
                {
                    activos = activos.Where(a => a.Nombre == searchString
                                               || a.Descripcion == searchString
                                               || a.Estado == searchString);
                }

                // Pasa el valor de búsqueda a la vista
                ViewBag.SearchString = searchString;

                // Devuelve la vista con los activos filtrados o todos los activos si no se pasa un filtro
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
            return View();
        }

        // POST: Activos/Create
        [HttpPost]
        public ActionResult Create(Activos activos)
        {
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
            using (DbModels context = new DbModels())
            {
                return View(context.Activos.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Activos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Activos activos)
        {
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
            using (DbModels context = new DbModels())
            {
                return View(context.Activos.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        // POST: Activos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
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
