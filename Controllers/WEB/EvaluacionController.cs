using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Thoth.Web.Repositories;
using Thoth.Web.Models;
using Thoth.Web.Models.ViewModels;

namespace Thoth.Web.Controllers.Web
{
    [Authorize(Roles = "Administrador")]
    public class EvaluacionesController : Controller
    {
        private readonly IEvaluacionRepository _evaluacionRepo;
        private readonly ICapacitacionRepository _capacitacionRepo;

        public EvaluacionesController(IEvaluacionRepository evaluacionRepo, ICapacitacionRepository capacitacionRepo)
        {
            _evaluacionRepo = evaluacionRepo;
            _capacitacionRepo = capacitacionRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Crear(int capacitacionId)
        {
            var capacitacion = await _capacitacionRepo.GetByIdAsync(capacitacionId);
            if (capacitacion == null) return NotFound();

            // Verificamos si YA tiene evaluación (Relación 1 a 1)
            if (capacitacion.Evaluacion != null)
            {
                // Si ya tiene, redirigimos a editar o detalles de esa evaluación
                // Por ahora, volvemos a la capacitación con un aviso (opcional)
                return RedirectToAction("Details", "Capacitaciones", new { id = capacitacionId });
            }

            var model = new EvaluacionCrearViewModel
            {
                CapacitacionId = capacitacion.Id,
                CapacitacionTitulo = capacitacion.Titulo,
                NotaMinima = 4.0m // Valor por defecto
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(EvaluacionCrearViewModel model)
        {
            if (ModelState.IsValid)
            {
                var evaluacion = new Evaluacion
                {
                    Titulo = model.Titulo,
                    NotaMinima = model.NotaMinima,
                    CapacitacionId = model.CapacitacionId
                };

                await _evaluacionRepo.AddAsync(evaluacion);

                return RedirectToAction("Details", "Capacitaciones", new { id = model.CapacitacionId });
            }

            return View(model);
        }

    [HttpGet]
    public IActionResult AgregarPregunta(int evaluacionId)
    {
        return View(new PreguntaCrearViewModel { EvaluacionId = evaluacionId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarPregunta(PreguntaCrearViewModel model)
    {
        if (ModelState.IsValid)
        {
            var pregunta = new Pregunta
            {
                EvaluacionId = model.EvaluacionId,
                Enunciado = model.Enunciado,
                Opciones = new List<Opcion>()
            };

            void AddOpcion(string texto, int index)
            {
                if (!string.IsNullOrWhiteSpace(texto))
                {
                    pregunta.Opciones.Add(new Opcion
                    {
                        TextoOpcion = texto,
                        EsCorrecta = (model.RespuestaCorrectaIndex == index)
                    });
                }
            }

            AddOpcion(model.Opcion1, 0);  
            AddOpcion(model.Opcion2, 1); 
            AddOpcion(model.Opcion3, 2); 


            await _evaluacionRepo.AddPreguntaAsync(pregunta);

            return RedirectToAction("Details", "Capacitaciones", new { id = await GetCapacitacionIdByEvaluacion(model.EvaluacionId) });
        }
        return View(model);
    }

    private async Task<int> GetCapacitacionIdByEvaluacion(int evaluacionId)
    {
        var evaluacion = await _evaluacionRepo.GetByIdAsync(evaluacionId);
        return evaluacion.CapacitacionId;
    }















    }
}