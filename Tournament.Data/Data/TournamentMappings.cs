using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Data.Data
{
    public class TournamentMappings: Profile
    {
        public TournamentMappings()
        {
            CreateMap<Core.Entities.Tournament, Core.Dto.TournamentDto>();
            CreateMap<Core.Dto.TournamentDto, Core.Entities.Tournament>();

            CreateMap<Core.Entities.Game, Core.Dto.GameDto>();
            CreateMap<Core.Dto.GameDto, Core.Entities.Game>();
        }
    }
}
