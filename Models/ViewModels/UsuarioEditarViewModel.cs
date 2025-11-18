using System.ComponentModel.DataAnnotations;

namespace Thoth.Web.Models.ViewModels
{
    public class UsuarioEditarViewModel
    {
        public int Id { get; set; } // Necesario para saber a quién actualizar

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El RUT es obligatorio")]
        public string Rut { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }
    }
}