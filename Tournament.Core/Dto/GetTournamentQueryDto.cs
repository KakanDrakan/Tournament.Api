using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GetTournamentQueryDto
    {
        public string? Title { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public bool OrderByTitle { get; set; } = false;
        public bool Descending { get; set; } = false;
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
        public bool IncludeGames { get; set; } = false;
    }
}
