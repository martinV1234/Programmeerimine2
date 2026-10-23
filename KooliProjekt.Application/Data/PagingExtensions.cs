using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public static class PagingExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            pageSize = Math.Min(pageSize, 100);

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync(cancellationToken)
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            // Use long arithmetic so an extreme page number cannot overflow Skip.
            var skip = (long)(page - 1) * pageSize;
            if (skip >= result.RowCount)
            {
                return result;
            }
            result.Results = await query
                .Skip((int)skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
