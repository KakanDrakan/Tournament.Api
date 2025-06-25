using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface IGameRepository
    {
        void Add(Game game);
        Task<bool> AnyAsync(int id);
        Task<IEnumerable<Game>> GetAllAsync(int? tournamentId);
        Task<Game> GetByIdAsync(int id);
        void Remove(Game game);
        void Update(Game game);
    }
}
