using Bogus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public static class SeedData
{
    public static void SeedTournamentData(TournamentApiContext db)
    {
        var textInfo = CultureInfo.CurrentCulture.TextInfo;


        var tournamentFaker = new Faker<Core.Entities.Tournament>()
            .RuleFor(t => t.Title, f => textInfo.ToTitleCase($"{f.Commerce.Color()} {f.PickRandom(tournamentTypes)}"))
            .RuleFor(t => t.StartDate, f => f.Date.Future())
            .RuleFor(t => t.Games, (f, t) =>
            {
                var gameFaker = new Faker<Game>()
                    .RuleFor(g => g.Title, f => $"Game: {f.Address.City()} vs {f.Address.City()}")
                    .RuleFor(g => g.StartDate, f => f.Date.Between(t.StartDate, t.StartDate.AddMonths(3)));
                return gameFaker.Generate(f.Random.Int(2, 6));
            });

        var tournaments = tournamentFaker.Generate(15);

        db.Tournament.AddRange(tournaments);
    }
    private static string[] tournamentTypes = {
            "Tournament", "Cup", "Showdown", "Invitational", "Competition", "League", "Event", "Contest", "Series", "Clash"
        };

}

