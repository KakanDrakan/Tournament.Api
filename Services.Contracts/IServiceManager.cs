using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IServiceManager
    {
        public ITournamentService TournamentService { get; }
        public IGameService GameService { get; }
    }
}
