using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository(TournamentApiContext context) : ITournamentRepository
    {
        public void Add(Core.Entities.Tournament tournament)
        {
            context.Tournament.Add(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await context.Tournament.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Core.Entities.Tournament>> GetAllAsync(GetTournamentQueryDto dto)
        {
            var query = context.Tournament.AsQueryable();
            if (dto.IncludeGames)
            {
                query = query.Include(t => t.Games);
            }
            if (!string.IsNullOrEmpty(dto.Title))
            {
                query = query.Where(t => t.Title.Contains(dto.Title));
            }
            if (dto.FromYear.HasValue)
            {
                query = query.Where(t => t.StartDate.Year >= dto.FromYear);
            }
            if (dto.ToYear.HasValue)
            {
                query = query.Where(t => t.StartDate.Year <= dto.ToYear);
            }
            if (dto.OrderByTitle)
            {
                query = dto.Descending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title);
            }
            if (dto.PageSize.HasValue && dto.PageSize > 0)
            {
                query = query.Skip(dto.SkipFirstEntities ?? 0).Take(dto.PageSize.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<Core.Entities.Tournament> GetByIdAsync(int id)
        {
            return await context.Tournament
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id == id)
                ?? throw new KeyNotFoundException($"Tournament with ID {id} not found.");
        }

        public void Remove(Core.Entities.Tournament tournament)
        {
            context.Tournament.Remove(tournament);
        }

        public void Update(Core.Entities.Tournament tournament)
        {
            context.Entry(tournament).State = EntityState.Modified;
        }
    }
}
