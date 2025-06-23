using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class GameRepository(TournamentApiContext context): IGameRepository
    {
        public void Add(Core.Entities.Game game)
        {
            context.Game.Add(game);
        }
        public async Task<bool> AnyAsync(int id)
        {
            return await context.Game.AnyAsync(g => g.Id == id);
        }
        public async Task<IEnumerable<Core.Entities.Game>> GetAllAsync()
        {
            return await context.Game.ToListAsync();
        }
        public async Task<Core.Entities.Game> GetByIdAsync(int id)
        {
            return await context.Game.FindAsync(id) 
                ?? throw new KeyNotFoundException($"Game with ID {id} not found.");
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
