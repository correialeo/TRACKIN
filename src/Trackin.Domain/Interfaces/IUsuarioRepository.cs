using Trackin.Domain.Entity;

namespace Trackin.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario?> GetByIdAsync(long id);
        Task<Usuario> CreateAsync(Usuario Usuario);
        Task<Usuario> UpdateAsync(Usuario Usuario);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> ExistsAsync(string Usuarioname);
    }
}