using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Thoth.Web.Models;

namespace Thoth.Web.Data
{
    public class ThothDbContext : DbContext
    {
        // Constructor: Pasa la configuración (como el connection string) a la clase base
        public ThothDbContext(DbContextOptions<ThothDbContext> options) : base(options)
        {
        }

        // Registra cada modelo como una "tabla" en el contexto
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Capacitacion> Capacitaciones { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        
        // Aquí es donde configuramos las relaciones complejas que EF no puede adivinar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configuración de Usuario ---
            modelBuilder.Entity<Usuario>(entity =>
            {
                // 1. Relación con Rol: Evitar borrado en cascada
                // Tu SQL no especificaba 'ON DELETE CASCADE' para Roles,
                // así que usamos 'Restrict' para que coincida (no se puede borrar un rol si tiene usuarios).
                entity.HasOne(u => u.Rol)
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(u => u.RolId)
                      .OnDelete(DeleteBehavior.Restrict); // Importante

                // 2. Email único (como en tu SQL)
                entity.HasIndex(u => u.Email).IsUnique();

                // 3. Relación Muchos-a-Muchos (Supervisor <-> Empleado)
                // Le decimos a EF que use tu tabla 'supervisorempleado'
                entity.HasMany(u => u.EmpleadosACargo)
                      .WithMany(u => u.Supervisores)
                      .UsingEntity<Dictionary<string, object>>(
                          "supervisorempleado", // Nombre exacto de tu tabla SQL
                          
                          // Columna izquierda (Supervisor)
                          l => l.HasOne<Usuario>().WithMany().HasForeignKey("SupervisorId"),
                          
                          // Columna derecha (Empleado)
                          r => r.HasOne<Usuario>().WithMany().HasForeignKey("EmpleadoId")
                      );
            });

            // --- Configuración de Evaluación (Relación 1-a-1) ---
            // Le decimos a EF que CapacitacionId es una clave única,
            // forzando la relación 1-a-1 que definiste en tu SQL.
            modelBuilder.Entity<Evaluacion>()
                .HasIndex(e => e.CapacitacionId)
                .IsUnique();

            // --- Configuración de Inscripción (Clave compuesta única) ---
            // Un usuario solo puede inscribirse una vez a una capacitación
            modelBuilder.Entity<Inscripcion>()
                .HasIndex(i => new { i.UsuarioId, i.CapacitacionId })
                .IsUnique();
        }
    }
}