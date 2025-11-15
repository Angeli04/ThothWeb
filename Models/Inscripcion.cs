using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thoth.Web.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        
        // Mapea a 'timestamp'
        public DateTime FechaInscripcion { get; set; } 
        
        public string EstadoCapacitacion { get; set; } // Ej: 'Pendiente', 'Completado'
        
        // Mapea a 'decimal(4,2)' y permite nulos
        [Column(TypeName = "decimal(4,2)")]
        public decimal? CalificacionFinal { get; set; }
        
        public string EstadoEvaluacion { get; set; } // Ej: 'No Realizada', 'Aprobada'

        // --- Relaciones (Claves Foráneas) ---

        // 1. Relación con Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // 2. Relación con Capacitacion
        public int CapacitacionId { get; set; }
        public Capacitacion Capacitacion { get; set; }
    }
}