using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public record GameInputDto
    {
        [Required(ErrorMessage = "Title is a required field for games")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "StartTime is a required field for games")]
        public DateTime StartDate { get; set; }
    }
}
