using CoodeshPharmaIncAPI.Models.Pagination;
using System;
using System.Linq;

namespace CoodeshPharmaIncAPI.Models.Extensions
{
    public static class UserPaginationExtension
    {
        public static IQueryable<User> Page(this IQueryable<User> query, UserPagination pagintaion)
        {
            int total = query.Count();

            if(pagintaion == null || pagintaion.Size <= 0)
            {
                return query;
            }

            int pageSize = pagintaion.Size;

            int totalPages = (int)Math.Ceiling((double)total / pageSize);

            int PageNumber = Math.Clamp(pagintaion.Page, 1, totalPages);

            int min = (PageNumber - 1) * pageSize;

            IQueryable<User> result = query.Skip(min).Take(pageSize);

            return result;

        }
    }
}
