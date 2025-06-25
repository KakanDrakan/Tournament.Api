using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GameDto
    {
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
    }
}
