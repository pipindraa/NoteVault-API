using Microsoft.EntityFrameworkCore;
using NoteVault.DAL.Data;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Extensions;
using NoteVault.DAL.Interfaces;

namespace NoteVault.DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Tag> _tags;

        public TagRepository(AppDbContext context)
        {
            _context = context;
            _tags = context.Tags;
        }

        public async Task<List<Tag>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _tags
                .AsNoTracking()
                .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
        }

        public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _tags
                .AsNoTracking()
                .FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);
        }

        public async Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _tags
                .AsNoTracking()
                .Where(tag => ids.Contains(tag.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Tag> AddAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            await _tags.AddAsync(tag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return tag;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedCount = await _tags
                .Where(tag => tag.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deletedCount != default;
        }
    }
}
