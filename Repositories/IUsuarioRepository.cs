using Thoth.Web.Models;
using System.Threading.Tasks;

namespace Thoth.Web.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> CreateUserAsync(Usuario usuario, string password);

        Task<Usuario> ValidateUserAsync(string email, string password);
    
        Task<bool> SoftDeleteUserAsync(int id);

        Task<IEnumerable<Usuario>> GetAllUsersAsync();

        Task<Usuario> GetUserByIdAsync(int id);

        Task<bool> ActivateUserAsync(int id);
        
        Task UpdateUserAsync(Usuario usuario);
    
    }
}