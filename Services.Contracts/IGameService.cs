using Tournament.Core.Dto;

namespace Services.Contracts
{
    public interface IGameService
    {
        public Task<PagedResultDto<GameDto>> GetAllAsync(int tournamentId, GetGameQueryDto dto);
        public Task<GameDto?> GetByInputAsync(bool byTitle, string input);
        public Task<bool> AnyAsync(int id);
        public Task<(int id, GameDto createdGame)> AddAsync(int tournamentId, GameCreateDto dto);
        public Task<bool> UpdateAsync(GameUpdateDto dto);
        public Task<bool> DeleteAsync(int id);
        public Task<GameUpdateDto> MapToUpdateDtoAsync(int id);
        public Task PatchAsync(int id, GameUpdateDto patchDto);
    }
}
