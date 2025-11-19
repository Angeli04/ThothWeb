using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Thoth.Web.Models.ViewModels
{
    public class InscripcionViewModel
    {
        public int CapacitacionId { get; set; }
        public string CapacitacionTitulo { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un empleado")]
        [Display(Name = "Empleado a Inscribir")]
        public int UsuarioIdSeleccionado { get; set; }

    }
}