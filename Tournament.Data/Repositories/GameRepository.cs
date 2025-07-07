using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class GameRepository(TournamentApiContext context) : IGameRepository
    {
        public void Add(Core.Entities.Game game)
        {
            context.Game.Add(game);
        }
        public async Task<bool> AnyAsync(int id)
        {
            return await context.Game.AnyAsync(g => g.Id == id);
        }
        public async Task<PagedResultDto<Core.Entities.Game>> GetAllAsync(int? tournamentId, GetGameQueryDto dto)
        {
            var query = context.Game.AsQueryable();
            if (tournamentId.HasValue)
            {
                query = query.Where(g => g.TournamentId == tournamentId.Value);
            }
            if (!string.IsNullOrEmpty(dto.Title))
            {
                query = query.Where(t => t.Title.Contains(dto.Title));
            }
            if (dto.OrderByTitle)
            {
                query = dto.Descending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title);
            }

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                throw new KeyNotFoundException("No games found matching the specified criteria.");
            }

            int pageSize = dto.PageSize.HasValue && dto.PageSize > 0 ? dto.PageSize.Value : 20;
            int page = dto.Page.HasValue && dto.Page > 0 ? dto.Page.Value : 1;
            int skipAmount = (page - 1) * pageSize;
            query = query.Skip(skipAmount).Take(pageSize);

            var items = await query.ToListAsync();
            return new PagedResultDto<Core.Entities.Game>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page
            };
        }
        public async Task<Core.Entities.Game> GetByIdAsync(int id)
        {
            return await context.Game.FindAsync(id)
                ?? throw new KeyNotFoundException($"Game with ID {id} not found.");
        }
        public async Task<Core.Entities.Game> GetByTitleAsync(string title)
        {
            return await context.Game.FirstOrDefaultAsync(g => g.Title == title)
                ?? throw new KeyNotFoundException($"Game with title '{title}' not found.");
        }
        public void Remove(Core.Entities.Game game)
        {
            context.Game.Remove(game);
        }
        public void Update(Core.Entities.Game game)
        {
            context.Entry(game).State = EntityState.Modified;
        }
    }
}
