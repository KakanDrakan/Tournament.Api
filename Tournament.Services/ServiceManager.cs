using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Services
{
    public class ServiceManager(Lazy<ITournamentService> lazyTournamentService, Lazy<IGameService> lazyGameService): IServiceManager
    {
        public ITournamentService TournamentService => lazyTournamentService.Value;
        public IGameService GameService => lazyGameService.Value;
    }
}
