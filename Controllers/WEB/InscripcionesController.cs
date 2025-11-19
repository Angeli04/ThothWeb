using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering; // Para SelectListItem
using System.Threading.Tasks;
using System.Linq; // Para Select
using System; // Para DateTime
using Thoth.Web.Repositories;
using Thoth.Web.Models;
using Thoth.Web.Models.ViewModels;

namespace Thoth.Web.Controllers.Web
{
    [Authorize(Roles = "Administrador")]
    public class InscripcionesController : Controller
    {
        private readonly IInscripcionRepository _inscripcionRepo;
        private readonly ICapacitacionRepository _capacitacionRepo;

        public InscripcionesController(IInscripcionRepository inscripcionRepo, ICapacitacionRepository capacitacionRepo)
        {
            _inscripcionRepo = inscripcionRepo;
            _capacitacionRepo = capacitacionRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Crear(int capacitacionId)
        {
            var curso = await _capacitacionRepo.GetByIdAsync(capacitacionId);
            if (curso == null) return NotFound();

            var empleados = await _inscripcionRepo.GetUsuariosDisponiblesParaCurso(capacitacionId);

            if (!empleados.Any())
            {
                TempData["Info"] = "Todos los empleados activos ya están inscritos en este curso.";
                return RedirectToAction("Details", "Capacitaciones", new { id = capacitacionId });
            }

            ViewData["Empleados"] = empleados.Select(u => new SelectListItem
            {
                Text = $"{u.Nombre} {u.Apellido} ({u.Rut})",
                Value = u.Id.ToString()
            });

            var model = new InscripcionViewModel
            {
                CapacitacionId = curso.Id,
                CapacitacionTitulo = curso.Titulo
            };

            return View(model);
        }

        // POST: Guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(InscripcionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nuevaInscripcion = new Inscripcion
                {
                    CapacitacionId = model.CapacitacionId,
                    UsuarioId = model.UsuarioIdSeleccionado,
                    FechaInscripcion = DateTime.Now,
                    EstadoCapacitacion = "Pendiente",
                    EstadoEvaluacion = "No Realizada"
                };

                await _inscripcionRepo.InscribirUsuario(nuevaInscripcion);
                TempData["Exito"] = "Empleado inscrito correctamente.";
                return RedirectToAction("Details", "Capacitaciones", new { id = model.CapacitacionId });
            }

            var empleados = await _inscripcionRepo.GetUsuariosDisponiblesParaCurso(model.CapacitacionId);
            
            ViewData["Empleados"] = empleados.Select(u => new SelectListItem
            {
                Text = $"{u.Nombre} {u.Apellido} ({u.Rut})",
                Value = u.Id.ToString()
            });

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id, int capacitacionId)
        {
            await _inscripcionRepo.EliminarInscripcion(id);
            return RedirectToAction("Details", "Capacitaciones", new { id = capacitacionId });
        }
    }
}