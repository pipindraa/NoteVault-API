using Microsoft.EntityFrameworkCore;

namespace NoteVault.DAL.Extensions
{
    public static class QuerybleExtensions
    {
        public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> query, int  pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
