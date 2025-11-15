namespace Thoth.Web.Models
{
    public class Opcion
    {
        public int Id { get; set; }
        public string TextoOpcion { get; set; } // El texto de la respuesta
        
        // Mapea a 'tinyint(1)' en tu SQL. 
        // true (1) si esta es la respuesta correcta, false (0) si no.
        public bool EsCorrecta { get; set; } 

        // --- Relaciones ---

        // 1. Relación con Pregunta (Muchos a Uno)
        // La clave foránea
        public int PreguntaId { get; set; }
        public Pregunta Pregunta { get; set; }
    }
}