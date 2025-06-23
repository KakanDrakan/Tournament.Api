using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static void SeedTournamentData(TournamentApiContext db)
        {
            var tournaments = new List<Core.Entities.Tournament>
            {
                new Core.Entities.Tournament
                {
                    Title = "Spring Invitational 2025",
                    StartDate = new DateTime(2025, 3, 15),
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Title = "Quarterfinal Match 1",
                            StartDate = new DateTime(2025, 3, 16)
                        },
                        new Game
                        {
                            Title = "Quarterfinal Match 2",
                            StartDate = new DateTime(2025, 3, 17)
                        }
                    }
                },
                new Core.Entities.Tournament
                {
                    Title = "Summer Cup 2025",
                    StartDate = new DateTime(2025, 6, 10),
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Title = "Opening Game",
                            StartDate = new DateTime(2025, 6, 11)
                        },
                        new Game
                        {
                            Title = "Semi-Final",
                            StartDate = new DateTime(2025, 6, 15)
                        }
                    }
                }
            };

            db.Tournament.AddRange(tournaments);
        }
    }
}
