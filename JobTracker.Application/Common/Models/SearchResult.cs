using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Models
{
    public sealed record SearchResult<T>(
        List<T> Items,
        int TotalCount,
        int Page,
        int PageSize)
    {
        public int TotalPage => PageSize > 0
            ? (int)Math.Ceiling((double)TotalCount / PageSize)
            : 0;

        public bool HasNextPage => Page < TotalPage;

        public bool HasPreviousPage => Page > 1;
    }
}
