using Microsoft.EntityFrameworkCore;
using NoteVault.DAL.Data;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;
using System.Linq.Expressions;

namespace NoteVault.DAL.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Note>> GetAllAsync(
            Expression<Func<Note, object>> orderBy,
            bool descending = true,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Notes.AsNoTracking();

            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(note => note.Id == id, cancellationToken);
        }

        public async Task<Note> AddAsync(Note note, CancellationToken cancellationToken = default)
        {
            await _context.Notes.AddAsync(note, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return note;
        }

        public async Task<Note?> UpdateAsync(Note note, CancellationToken cancellationToken = default)
        {
            var exists = await _context.Notes
                .AnyAsync(item => item.Id == note.Id, cancellationToken);

            if (!exists)
            {
                return null;
            }

            _context.Notes.Update(note);
            await _context.SaveChangesAsync(cancellationToken); 
            
            return note;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedCount = await _context.Notes
                .Where(note => note.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deletedCount > 0;
        }
    }
}
