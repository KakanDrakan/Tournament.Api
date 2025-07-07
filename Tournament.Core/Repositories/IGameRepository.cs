using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface IGameRepository
    {
        void Add(Game game);
        Task<bool> AnyAsync(int id);
        Task<PagedResultDto<Game>> GetAllAsync(int? tournamentId, GetGameQueryDto dto);
        Task<Game> GetByIdAsync(int id);
        Task<Game> GetByTitleAsync(string title);
        void Remove(Game game);
        void Update(Game game);
    }
}
