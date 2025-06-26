using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<Tournament.Core.Entities.Tournament>> GetAllAsync(GetTournamentQueryDto dto);
        Task<Tournament.Core.Entities.Tournament> GetByIdAsync(int id);
        Task<bool>AnyAsync(int id);
        void Add(Tournament.Core.Entities.Tournament tournament);
        void Update(Tournament.Core.Entities.Tournament tournament);
        void Remove(Tournament.Core.Entities.Tournament tournament);

    }
}
