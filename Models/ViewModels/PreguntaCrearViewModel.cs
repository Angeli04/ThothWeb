using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thoth.Web.Models.ViewModels
{
    public class PreguntaCrearViewModel
    {
        public int EvaluacionId { get; set; }

        [Required(ErrorMessage = "El enunciado de la pregunta es obligatorio")]
        [Display(Name = "Pregunta")]
        public string Enunciado { get; set; }

        [Required(ErrorMessage = "La opción 1 es obligatoria")]
        public string Opcion1 { get; set; }

        [Required(ErrorMessage = "La opción 2 es obligatoria")]
        public string Opcion2 { get; set; }

        public string Opcion3 { get; set; } 


        [Required(ErrorMessage = "Debes marcar cuál es la respuesta correcta")]
        public int RespuestaCorrectaIndex { get; set; }
    }
}