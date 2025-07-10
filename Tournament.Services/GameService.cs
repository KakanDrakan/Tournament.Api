using AutoMapper;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Api.Exceptions;
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

        public async Task DeleteAsync(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new GameIdNotFoundException(id);
            }
            UoW.GameRepository.Remove(game);
            await UoW.SaveChangesAsync();

        }

        public async Task<PagedResultDto<GameDto>> GetAllAsync(int tournamentId,GetGameQueryDto dto)
        {
            var result = await UoW.GameRepository.GetAllAsync(tournamentId, dto);
            return new PagedResultDto<GameDto>
            {
                Items = result.Items.Select(g => mapper.Map<GameDto>(g)),
                TotalCount = result.TotalCount,
                PageSize = dto.PageSize ?? 20,
                Page = dto.Page ?? 1
            };
        }

        public async Task<GameDto> GetByInputAsync(bool byTitle, string input)
        {
            if (byTitle)
            {
                var game = await GetByTitleAsync(input);
                return mapper.Map<GameDto>(game);
            }
            else if (int.TryParse(input, out int id))
            {
                var game = await GetByIdAsync(id);
                return mapper.Map<GameDto>(game);
            }
            else
            {
                throw new ArgumentException("Please input only integers when byTitle is set to false");
            }
        }
        public async Task<GameDto> GetByIdAsync(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id) ?? throw new GameIdNotFoundException(id);
            return mapper.Map<GameDto>(game);
        }
        public async Task<GameDto> GetByTitleAsync(string title)
        {
            var game = await UoW.GameRepository.GetByTitleAsync(title) ?? throw new GameTitleNotFoundException(title);
            return mapper.Map<GameDto>(game);
        }

        public async Task<GameUpdateDto> MapToUpdateDtoAsync(int id)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new GameIdNotFoundException(id);
            }
            return mapper.Map<GameUpdateDto>(game);
        }

        public async Task PatchAsync(int id, GameUpdateDto patchDto)
        {
            var game = await UoW.GameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new GameIdNotFoundException(id);
            }
            mapper.Map(patchDto, game);
            UoW.GameRepository.Update(game);
            await UoW.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameUpdateDto dto)
        {
            var game = await UoW.GameRepository.GetByIdAsync(dto.Id);
            if (game == null)
            {
                throw new GameIdNotFoundException(dto.Id); //should probably throw something else because if game is null something more serious has happened
            }
            mapper.Map(dto, game);
            UoW.GameRepository.Update(game);
            await UoW.SaveChangesAsync();
        }
    }
}
