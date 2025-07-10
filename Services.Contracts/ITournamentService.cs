using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Services.Contracts
{
    public interface ITournamentService
    {
        public Task<PagedResultDto<TournamentDto>> GetAllAsync(GetTournamentQueryDto dto);
        public Task<TournamentDto> GetByIdAsync(int id);
        public Task<bool> AnyAsync(int id);
        public Task<(int id, TournamentDto createdTournament)> AddAsync(TournamentCreateDto dto);
        public Task UpdateAsync(TournamentUpdateDto dto);
        public Task DeleteAsync(int id);
        public Task<TournamentUpdateDto> MapToUpdateDtoAsync(int id);
        public Task PatchAsync(int id, TournamentUpdateDto patchDto);
    }
}
