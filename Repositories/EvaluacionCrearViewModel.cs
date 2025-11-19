using System.ComponentModel.DataAnnotations;

namespace Thoth.Web.Models.ViewModels
{
    public class EvaluacionCrearViewModel
    {
        public int CapacitacionId { get; set; } 
        public string CapacitacionTitulo { get; set; } 

        [Required(ErrorMessage = "El título de la evaluación es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La nota mínima es obligatoria")]
        [Range(1.0, 7.0, ErrorMessage = "La nota debe estar entre 1.0 y 7.0")]
        [Display(Name = "Nota Mínima de Aprobación")]
        public decimal NotaMinima { get; set; }
    }
}