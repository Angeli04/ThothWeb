using System.Collections.Generic;

namespace Thoth.Web.Models
{
    public class Rol
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        // Propiedad de navegación:
        // Un Rol puede estar asociado a muchos Usuarios
        public List<Usuario> Usuarios { get; set; }
    }
}