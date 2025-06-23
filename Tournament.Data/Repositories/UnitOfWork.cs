using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UnitOfWork(TournamentApiContext context) : IUnitOfWork
    {
        public ITournamentRepository TournamentRepository => new TournamentRepository(context);

        public IGameRepository GameRepository => new GameRepository(context);

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
