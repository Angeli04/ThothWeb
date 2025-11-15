using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thoth.Web.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        // Mapea a 'decimal(4,2)' en tu SQL
        [Column(TypeName = "decimal(4,2)")]
        public decimal NotaMinima { get; set; }

        // --- Relaciones ---

        // 1. Relación con Capacitacion (Uno a Uno)
        // La clave foránea que define la relación 1 a 1
        public int CapacitacionId { get; set; }
        public Capacitacion Capacitacion { get; set; }

        // 2. Relación con Pregunta (Uno a Muchos)
        // Una evaluación tiene muchas preguntas
        public List<Pregunta> Preguntas { get; set; }
    }
}