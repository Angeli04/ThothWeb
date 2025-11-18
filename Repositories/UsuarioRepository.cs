using Microsoft.EntityFrameworkCore;
using Thoth.Web.Data;
using Thoth.Web.Models;
using System.Threading.Tasks;
using BCrypt.Net; // Recuerda instalar el paquete NuGet: BCrypt.Net-Next

namespace Thoth.Web.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ThothDbContext _context;

        public UsuarioRepository(ThothDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario> CreateUserAsync(Usuario usuario, string password)
        {
            // --- A. Validar que el email no exista ---
            if (await _context.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.Email == usuario.Email))
            {
                throw new System.Exception("El email ya está registrado.");
            }

            // --- B. Hashear el password ---
            // Nunca se guarda la contraseña en texto plano
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);


            // --- C. Guardar en la BD ---
            // 1. Agregar el usuario al "área de espera" de EF Core
            await _context.Usuarios.AddAsync(usuario);
            
            // 2. Ejecutar el "INSERT" en la base de datos
            await _context.SaveChangesAsync();

            // 3. Devolver el objeto Usuario (que ahora ya tiene su Id)
            return usuario;
        }

        public async Task<Usuario> ValidateUserAsync(string email, string password)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            bool esPasswordValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);

            if (esPasswordValido)
            {
                // ¡Éxito! Devolvemos el objeto Usuario completo
                return usuario;
            }

            // Si el password no es válido
            return null;

        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
        // 1. Encontrar al usuario
        // (No necesitamos .Include(), solo queremos actualizarlo)
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            // El usuario no existe (o ya estaba borrado y el 
            // filtro global lo ocultó, aunque FindAsync debería saltar eso)
            return false;
        }

        // 2. ESTA ES LA BAJA LÓGICA
        // En lugar de: _context.Usuarios.Remove(usuario);
        usuario.EstaActivo = false;

        // 3. Guardar el UPDATE en la BD
        await _context.SaveChangesAsync();
        return true;
        }
    }
}