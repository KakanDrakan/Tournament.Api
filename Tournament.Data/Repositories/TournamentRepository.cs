using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Core.Entities.Tournament>> GetAllAsync(bool includeGames)
        {
            var query = context.Tournament.AsQueryable();
            if (includeGames)
            {
                query = query.Include(t => t.Games);
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
