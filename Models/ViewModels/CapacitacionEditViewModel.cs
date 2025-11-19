using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Thoth.Web.Models.ViewModels
{
    public class CapacitacionEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Display(Name = "Reemplazar Archivo (Opcional)")]
        public IFormFile? ArchivoMaterial { get; set; }

        public string? ContenidoUrlActual { get; set; }
    }
}