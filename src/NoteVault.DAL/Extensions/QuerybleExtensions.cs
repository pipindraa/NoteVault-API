using Microsoft.EntityFrameworkCore;

namespace NoteVault.DAL.Extensions
{
    public static class QuerybleExtensions
    {
        private const int FirstpageNumber = 1;

        public static async Task<(List<T> Items, int TotalCount)> ToPagedListAsync<T>(this IQueryable<T> query, int  pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - FirstpageNumber) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
