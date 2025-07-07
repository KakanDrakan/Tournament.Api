using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
        public int TotalCount { get; init; } = 0;
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
