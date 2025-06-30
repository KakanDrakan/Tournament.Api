using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GetGameQueryDto
    {
        public string? Title { get; set; }
        public bool OrderByTitle { get; set; } = false;
        public bool Descending { get; set; } = false;
        public int? PageSize { get; set; } = 0;
        public int? SkipFirstEntities { get; set; } = 0;
    }
}
