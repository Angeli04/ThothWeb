using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Thoth.Web.Models.ViewModels
{
    public class UsuarioCrearViewModel
    {
        // ... (Nombre, Apellido, Rut, Email - sin cambios) ...
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El RUT es obligatorio")]
        public string Rut { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // --- INICIO: AÑADIR ESTE CAMPO ---
        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
        // --- FIN: AÑADIR ESTE CAMPO ---

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; } 
        
    }
}