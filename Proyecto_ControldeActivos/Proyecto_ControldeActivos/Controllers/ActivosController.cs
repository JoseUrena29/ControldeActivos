using Antlr.Runtime.Misc;
using Proyecto_ControldeActivos.Helpers;
using Proyecto_ControldeActivos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_ControldeActivos.Controllers
{
    public class ActivosController : Controller
    {

        private readonly string _uploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "uploads");


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

        public ActionResult UploadImage(int id)
        {
            var result = CheckRol();
            if (result != null) return result;

            FileUploadActivoViewModel imageUploadViewModel = new FileUploadActivoViewModel();
            imageUploadViewModel.IdActivo = id;
            return View(imageUploadViewModel);
        }


        [HttpPost]
        public ActionResult UploadImage(FileUploadActivoViewModel model)
        {
            

            ViewBag.MessageError = null;
            ViewBag.MessageSuccess = null;
            ViewBag.FilePath = null;

            string filePath =  null;
            try
            {
                if (model.IdActivo == 0)
                {
                    ViewBag.MessageError = "No hay activo seleccionado.";
                    return View();
                }
                if (model.FileB64 == null || model.FileB64.Length < 100) {
                    ViewBag.MessageError = "Se debe seleccionar el archivo!";
                    return View();
                }

                //  Crea el directorio si no existe
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }
                string fileName = Guid.NewGuid().ToString() + '.' + model.FileExt;
                filePath = Path.Combine(_uploadPath, fileName);
            
                FileHelper.SaveBase64ToFile(model.FileB64, filePath);
                string finalPathInServer = filePath.Replace(AppDomain.CurrentDomain.BaseDirectory, "/");

                ActivoArchivos achivoActivo = new ActivoArchivos();
                achivoActivo.IdActivo = model.IdActivo;
                achivoActivo.FechaCreacion = DateTime.Now;
                achivoActivo.NombreArchivo = model.FileName;
                achivoActivo.TamanoArchivo = model.FileSize;
                achivoActivo.TipoArchivo = model.FileMimeType;
                achivoActivo.RutaArchivo = finalPathInServer;
                achivoActivo.ExtArchivo = model.FileExt;

                using (DbModels context = new DbModels())
                {
                    context.ActivoArchivos.Add(achivoActivo);
                    context.SaveChanges();
                }
                ViewBag.FilePath = finalPathInServer;
                ViewBag.MessageSuccess = "Se ha subido el archivo!";
            }
            catch (Exception ex)
            {
                // Si sucede algún error se elimina el archivo
                if (filePath != null)
                {
                    FileHelper.DeleteFile(filePath);
                }
                ViewBag.MessageError = ex.Message;
            }
            return View();
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
            //var result = CheckRol();
            //if (result != null) return result;

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
            //var result = CheckRol();
            //if (result != null) return result;

            ViewBag.Images = new List<ActivoArchivos>();
            using (DbModels context = new DbModels())
            {
                ViewBag.Images = context.ActivoArchivos.Where(x => x.IdActivo == id).ToList();
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
