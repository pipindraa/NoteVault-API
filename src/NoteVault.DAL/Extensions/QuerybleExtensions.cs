using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace NoteVault.DAL.Extensions
{
    public static class QuerybleExtensions
    {
        private const int FirstpageNumber = 1;

        public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> query, int  pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await query
                .Skip((pageNumber - FirstpageNumber) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
