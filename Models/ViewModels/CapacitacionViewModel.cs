using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Thoth.Web.Models.ViewModels
{
    public class CapacitacionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe subir un archivo de material")]
        [Display(Name = "Archivo de Material (PDF, Word, Excel)")]
        public IFormFile ArchivoMaterial { get; set; }
    }
}