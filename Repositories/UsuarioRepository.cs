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
            if (await _context.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.Email == usuario.Email))
            {
                throw new System.Exception("El email ya está registrado.");
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
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

        public async Task<IEnumerable<Usuario>> GetAllUsersAsync()
        {
            return await _context.Usuarios.IgnoreQueryFilters()
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public async Task<Usuario> GetUserByIdAsync(int id)
        {
            if (id == 0)
            {
                return null;
            }else{
            return await _context.Usuarios
                        .Include(u => u.Rol)
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(u => u.Id == id);
            }
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            var usuario = await _context.Usuarios.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return false;
            }

            usuario.EstaActivo = true;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task UpdateUserAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        

    }
}