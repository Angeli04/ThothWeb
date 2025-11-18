using System.Collections.Generic;

namespace Thoth.Web.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rut { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Avatar { get; set; }

        public bool EstaActivo { get; set; } = true;



        // --- Relaciones ---

        // 1. Relación con Rol (Muchos a Uno)
        // Un Usuario tiene un solo Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        // 2. Relación con Inscripcion (Uno a Muchos)
        // Un Usuario puede tener muchas inscripciones a capacitaciones
        public List<Inscripcion> Inscripciones { get; set; }

        // 3. Relación Supervisor <-> Empleado (Muchos a Muchos consigo mismo)
        // Esta configuración se completará en el DbContext,
        // aquí solo definimos las listas de navegación.

        // Lista de empleados que este usuario supervisa
        public List<Usuario> EmpleadosACargo { get; set; } = new List<Usuario>();

        // Lista de supervisores que este usuario tiene
        public List<Usuario> Supervisores { get; set; } = new List<Usuario>();
    }
}