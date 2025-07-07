using AutoMapper;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class GameService(IUnitOfWork UoW, IMapper mapper) : IGameService
    {
        public async Task<(int id, GameDto createdGame)> AddAsync(int tournamentId, GameCreateDto dto)
        {
            var game = mapper.Map<Game>(dto);
            game.TournamentId = tournamentId;
            UoW.GameRepository.Add(game);
            await UoW.SaveChangesAsync();

            var createdGame = mapper.Map<GameDto>(game);
            return (game.Id, createdGame);

        }

        public async Task<bool> AnyAsync(int id)
        {
            return await UoW.GameRepository.AnyAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            if(game == null)
            {
                return false; // Game not found
            }
            UoW.GameRepository.Remove(game);
            await UoW.SaveChangesAsync();
            return true; // Game deleted successfully

        }

        public async Task<IEnumerable<GameDto>> GetAllAsync(int tournamentId,GetGameQueryDto dto)
        {
            return mapper.Map<IEnumerable<GameDto>>(await UoW.GameRepository.GetAllAsync(tournamentId, dto));
        }

        public async Task<GameDto?> GetByInputAsync(bool byTitle, string input)
        {
            if (byTitle)
            {
                var game = await UoW.GameRepository.GetByTitleAsync(input);
                return game == null ? null : mapper.Map<GameDto>(game);
            }
            else if (int.TryParse(input, out int id))
            {
                var game = await UoW.GameRepository.GetByIdAsync(id);
                return game == null ? null : mapper.Map<GameDto>(game);
            }
            else
            {
                return null;
            }
        }

        public async Task<GameUpdateDto> MapToUpdateDtoAsync(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new KeyNotFoundException($"Game with ID {id} not found.");
            }
            return mapper.Map<GameUpdateDto>(game);
        }

        public async Task PatchAsync(int id, GameUpdateDto patchDto)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            mapper.Map(patchDto, game);
            UoW.GameRepository.Update(game);
            await UoW.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(GameUpdateDto dto)
        {
            var game = await UoW.GameRepository.GetByIdAsync(dto.Id);
            if (game == null)
            {
                return false; // Game not found
            }
            mapper.Map(dto, game);
            UoW.GameRepository.Update(game);
            await UoW.SaveChangesAsync();
            return true; // Game updated successfully
        }
    }
}
