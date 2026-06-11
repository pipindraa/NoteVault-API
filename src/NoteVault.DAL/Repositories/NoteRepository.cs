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
            var existingNote = await _context.Notes
                .FirstOrDefaultAsync(item => item.Id == note.Id, cancellationToken);

            if (existingNote is null)
            {
                return null;
            }

            existingNote.Name = note.Name;
            existingNote.Description = note.Description;
            existingNote.ImageUrls = note.ImageUrls;

            await _context.SaveChangesAsync(cancellationToken); 
            
            return existingNote;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var note =  await _context.Notes
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

            if (note is null)
            {
                return false;
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync(cancellationToken); 
            
            return true;
        }
    }
}
