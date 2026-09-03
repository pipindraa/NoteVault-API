using Microsoft.EntityFrameworkCore;
using NoteVault.DAL.Data;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;

namespace NoteVault.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _users = context.Users;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Username == username, cancellationToken);
        }

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
    }
}
