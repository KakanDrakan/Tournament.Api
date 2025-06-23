using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentMappings: Profile
    {
        public TournamentMappings()
        {
            CreateMap<Core.Entities.Tournament, TournamentDto>().ReverseMap();
            CreateMap<TournamentUpdateDto, Core.Entities.Tournament>().ReverseMap();
            CreateMap<TournamentCreateDto, Core.Entities.Tournament>().ReverseMap();

            CreateMap<Game, GameDto>().ReverseMap();
            CreateMap<GameUpdateDto, Game>().ReverseMap();
            CreateMap<GameCreateDto, Game>().ReverseMap();
        }
    }
}
