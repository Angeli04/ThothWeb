using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // ¡Importante para [Authorize]!
using Thoth.Web.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Thoth.Web.Models;

namespace Thoth.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CursosApiController : ControllerBase
    {
        private readonly IInscripcionRepository _inscripcionRepo;

        private readonly ICapacitacionRepository _capacitacionRepo;


        public CursosApiController(IInscripcionRepository inscripcionRepo, ICapacitacionRepository capacitacionRepo)
        {
            _inscripcionRepo = inscripcionRepo;
            _capacitacionRepo = capacitacionRepo;
        }

        // GET: api/CursosApi/MisCursos
        [HttpGet("MisCursos")]
        public async Task<IActionResult> GetMisCursos()
        {
            // 1. Leer quién es el usuario desde el Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null) return Unauthorized();
            
            int userId = int.Parse(userIdClaim.Value);

            // 2. Pedir los cursos a la BD
            var inscripciones = await _inscripcionRepo.GetInscripcionesPorUsuario(userId);

            // 3. Crear un JSON limpio (DTO) para el celular
            var lista = inscripciones.Select(i => new 
            {
                Id = i.CapacitacionId, // ID del Curso
                Titulo = i.Capacitacion.Titulo,
                Descripcion = i.Capacitacion.Descripcion,
                Estado = i.EstadoCapacitacion, // "Pendiente" o "Completado"
                FechaAsignacion = i.FechaInscripcion.ToString("dd/MM/yyyy")
            });

            return Ok(lista);
        }

        // GET: api/CursosApi/Detalle/{id}
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> GetDetalle(int id)
        {
            // A. Obtenemos la capacitación completa (con examen y preguntas)
            // El repositorio ya hace los .Include necesarios.
            var curso = await _capacitacionRepo.GetByIdAsync(id);

            if (curso == null) return NotFound(new { message = "Curso no encontrado" });

            // B. Construimos el JSON (DTO)
            // Aquí decidimos qué datos enviar al celular.
            var detalle = new 
            {
                curso.Id,
                curso.Titulo,
                curso.Descripcion,
                // Convertimos la ruta relativa en absoluta si es necesario, 
                // o la app la completa (ej: https://tudominio.com/uploads/...)
                MaterialUrl = curso.ContenidoUrl, 
                
                // Datos del Examen (si existe)
                TieneExamen = curso.Evaluacion != null,
                Examen = curso.Evaluacion == null ? null : new 
                {
                    curso.Evaluacion.Id,
                    curso.Evaluacion.Titulo,
                    curso.Evaluacion.NotaMinima,
                    
                    // Lista de Preguntas
                    Preguntas = curso.Evaluacion.Preguntas.Select(p => new 
                    {
                        p.Id,
                        p.Enunciado,
                        // Opciones de respuesta
                        Opciones = p.Opciones.Select(o => new 
                        {
                            o.Id,
                            Texto = o.TextoOpcion
                        })
                    })
                }
            };

            return Ok(detalle);
        }

        [HttpPost("RendirExamen")]
        public async Task<IActionResult> RendirExamen([FromBody] RespuestaExamenDto intento)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var curso = await _capacitacionRepo.GetByIdAsync(intento.CapacitacionId);
            if(curso?.Evaluacion == null) return NotFound("Examen no encontrado");

            int totalPreguntas = curso.Evaluacion.Preguntas.Count();
            int respuestasCorrectas = 0;

            foreach(var respuestaUsuario in intento.Respuestas)
            {
                var preguntaReal = curso.Evaluacion.Preguntas.FirstOrDefault(p => p.Id == respuestaUsuario.PreguntaId);
                if(preguntaReal != null)
                {
                    var opcionCorrecta = preguntaReal.Opciones.FirstOrDefault(o => o.EsCorrecta);
                    if(opcionCorrecta != null && opcionCorrecta.Id == respuestaUsuario.OpcionIdSeleccionada)
                    {
                        respuestasCorrectas++;
                    }
                }
            }

            decimal nota = 0;

            if(totalPreguntas>0)
            {
                nota = ((decimal)respuestasCorrectas / (decimal)totalPreguntas) * 6 + 1;
            }
            nota = Math.Round(nota, 1);

            bool aprobado = nota >= curso.Evaluacion.NotaMinima;

            await _inscripcionRepo.ActualizarCalificacionAsync(userId, intento.CapacitacionId, nota, aprobado);

            return Ok(new
            {
                Nota = nota,
                Aprobado = aprobado,
                TotalPreguntas = totalPreguntas,
                RespuestasCorrectas = respuestasCorrectas,
                Mensaje = aprobado ? "Aprobado" : "Reprobado"
            });
        }
        
         
        

    















    }
}