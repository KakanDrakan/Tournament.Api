using AutoMapper;
using Services.Contracts;
using Tournament.Api.Exceptions;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class TournamentService(IUnitOfWork UoW, IMapper mapper) : ITournamentService
    {
        public async Task<PagedResultDto<TournamentDto>> GetAllAsync(GetTournamentQueryDto dto)
        {
            var result = await UoW.TournamentRepository.GetAllAsync(dto);
            if (result.TotalCount == 0)
            {
                throw new KeyNotFoundException("No tournaments found matching the specified criteria.");
            }
            return new PagedResultDto<TournamentDto>
                {
                Items = result.Items.Select(t => mapper.Map<TournamentDto>(t)),
                TotalCount = result.TotalCount,
                PageSize = dto.PageSize ?? 20,
                Page = dto.Page ?? 1
            };
        }

        public async Task<TournamentDto> GetByIdAsync(int id)
        {
            var exists = await UoW.TournamentRepository.AnyAsync(id);
            if (!exists)
            {
                throw new TournamentNotFoundException(id);
            }
            return mapper.Map<TournamentDto>(await UoW.TournamentRepository.GetByIdAsync(id));
        }
        public Task<bool> AnyAsync(int id)
        {
            return UoW.TournamentRepository.AnyAsync(id);
        }
        public async Task<(int id, TournamentDto createdTournament)> AddAsync(TournamentCreateDto dto)
        {
            var tournament = mapper.Map<Core.Entities.Tournament>(dto);
            UoW.TournamentRepository.Add(tournament);
            await UoW.SaveChangesAsync();

            var createdTournament = mapper.Map<TournamentDto>(tournament);
            return (tournament.Id, createdTournament);
        }
        public async Task UpdateAsync(TournamentUpdateDto dto)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(dto.Id);

            mapper.Map(dto, tournament);
            UoW.TournamentRepository.Update(tournament);

            await UoW.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(id);

            UoW.TournamentRepository.Remove(tournament);

            await UoW.SaveChangesAsync();
        }
        public async Task<TournamentUpdateDto> MapToUpdateDtoAsync(int id)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(id);
            return mapper.Map<TournamentUpdateDto>(tournament);
        }
        public async Task PatchAsync(int id, TournamentUpdateDto patchDto)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(id);
            mapper.Map(patchDto, tournament);
            UoW.TournamentRepository.Update(tournament);
            await UoW.SaveChangesAsync();
        }
    }
}
