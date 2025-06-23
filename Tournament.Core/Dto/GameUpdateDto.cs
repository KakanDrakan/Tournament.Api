using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GameUpdateDto : GameInputDto
    {
        public int Id { get; set; }
    }
}
