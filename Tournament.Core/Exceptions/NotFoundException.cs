using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Api.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        public string Title { get; set; }

        protected NotFoundException(string message, string title = "Not found") : base(message)
        {
            Title = title;
        }
    }

    public class TournamentNotFoundException(int id) : NotFoundException($"The tournament with id {id} was not found")
    {
    }

    public class GameIdNotFoundException(int id) : NotFoundException($"The game with id {id} was not found")
    {
    }
    public class GameTitleNotFoundException(string title) : NotFoundException($"The game with title '{title}' was not found")
    {
    }

}
