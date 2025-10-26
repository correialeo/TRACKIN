
using Microsoft.EntityFrameworkCore;
using Trackin.Domain.Entity;
using Trackin.Domain.Interfaces;
using Trackin.Infrastructure.Context;

namespace Trackin.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TrackinContext _context;

        public UsuarioRepository(TrackinContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetByIdAsync(long id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> CreateAsync(Usuario Usuario)
        {
            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<Usuario> UpdateAsync(Usuario Usuario)
        {
            _context.Usuarios.Update(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var Usuario = await GetByIdAsync(id);
            if (Usuario == null) return false;

            _context.Usuarios.Remove(Usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> ExistsAsync(string Username)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Nome == Username);
        }
    }
}