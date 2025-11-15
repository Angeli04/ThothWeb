using System;
using System.Collections.Generic;

namespace Thoth.Web.Models
{
    public class Capacitacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string ContenidoUrl { get; set; }
        
        // Mapea a 'timestamp' que por defecto tiene 'current_timestamp()'
        public DateTime FechaCreacion { get; set; } 

        // --- Relaciones ---

        // 1. Relación con Inscripcion (Uno a Muchos)
        // Una capacitación puede tener muchas inscripciones
        public List<Inscripcion> Inscripciones { get; set; }

        // 2. Relación con Evaluacion (Uno a Uno)
        // Una capacitación tiene una sola evaluación asociada
        // (La ? indica que puede ser nula si no se ha creado)
        public Evaluacion? Evaluacion { get; set; } 
    }
}