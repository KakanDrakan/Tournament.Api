using AutoMapper;
using Services.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class TournamentService(IUnitOfWork UoW, IMapper mapper) : ITournamentService
    {
        public async Task<PagedResultDto<TournamentDto>> GetAllAsync(GetTournamentQueryDto dto)
        {
            var result = await UoW.TournamentRepository.GetAllAsync(dto);
            return new PagedResultDto<TournamentDto>
                {
                Items = result.Items.Select(t => mapper.Map<TournamentDto>(t)),
                TotalCount = result.TotalCount,
                PageSize = dto.PageSize ?? 20,
                Page = dto.Page ?? 1
            };
        }

        public async Task<TournamentDto?> GetByIdAsync(int id)
        {
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
        public async Task<bool> UpdateAsync(TournamentUpdateDto dto)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(dto.Id);
            if (tournament == null)
            {
                return false; // Tournament not found
            }

            mapper.Map(dto, tournament);
            UoW.TournamentRepository.Update(tournament);

            await UoW.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var tournament = await UoW.TournamentRepository.GetByIdAsync(id);
            if (tournament == null)
            {
                return false; // Tournament not found
            }

            UoW.TournamentRepository.Remove(tournament);

            await UoW.SaveChangesAsync();
            return true;
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
