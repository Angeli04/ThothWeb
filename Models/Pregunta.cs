using System.Collections.Generic;

namespace Thoth.Web.Models
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Enunciado { get; set; } // El texto de la pregunta

        // --- Relaciones ---

        // 1. Relación con Evaluacion (Muchos a Uno)
        // La clave foránea
        public int EvaluacionId { get; set; }
        public Evaluacion Evaluacion { get; set; }

        // 2. Relación con Opcion (Uno a Muchos)
        // Una pregunta tiene varias opciones de respuesta
        public List<Opcion> Opciones { get; set; }
    }
}