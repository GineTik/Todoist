using Microsoft.EntityFrameworkCore;
using Todoist.Data.Models.Base;
using Todoist.Data.Models.Page;

namespace Todoist.Data.EF.Extensions
{
    public static class QuerableExtension
    {
        public static async Task<Page<TModel>> PaginateAsync<TModel>(this IQueryable<TModel> query, int page, int pageSize)
            where TModel : class, IBaseModel
        {
            var totalPages = (int)Math.Ceiling(await query.CountAsync() / (float)pageSize);

            if (page < 1)
                page = 1;
            else if (page > totalPages)
                page = totalPages;

            var entities = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Page<TModel>
            {
                PageMetadata = new PageMetadata
                {
                    Current = page,
                    Size = pageSize,
                    TotalPages = totalPages
                },
                Content = entities,
            };
        }
    }
}
