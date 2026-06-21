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
        private readonly DbSet<Note> _notes;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
            _notes = context.Notes;
        }

        public async Task<List<Note>> GetAllAsync(
            Expression<Func<Note, object>> orderBy,
            bool descending = true,
            CancellationToken cancellationToken = default)
        {
            var query = _notes.Include(note => note.Tags).AsNoTracking();

            if (descending)
            {
                query = query.OrderByDescending(orderBy);
            }
            else
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _notes
                .Include(note => note.Tags)
                .AsNoTracking()
                .FirstOrDefaultAsync(note => note.Id == id, cancellationToken);
        }

        public async Task<Note> AddAsync(Note note, CancellationToken cancellationToken = default)
        {
            await _notes.AddAsync(note, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return note;
        }

        public async Task<Note?> UpdateAsync(Note note, CancellationToken cancellationToken = default)
        {
            var existingNote = await _notes.FindAsync(new object[] { note.Id });

            if (existingNote is null)
            {
                return null;
            }

            _context.Entry(existingNote).CurrentValues.SetValues(note);

            await _context.SaveChangesAsync(cancellationToken);
            return note;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedCount = await _notes
                .Where(note => note.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deletedCount != default;
        }
    }
}
