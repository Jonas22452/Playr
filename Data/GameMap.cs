using CsvHelper.Configuration;
using Playr.Models;

namespace Playr.Data
{
    public class GameMap : ClassMap<Game> // Inherits from ClassMap<Game> to map CSV columns to Game properties
    {
        public GameMap()
        {
            Map(g => g.AppId).Name("AppID");
            Map(g => g.Name).Name("Name");
            Map(g => g.ReleaseDate).Name("Release date");
            Map(g => g.EstimatedOwners).Name("Estimated owners");
            Map(g => g.PeakCcu).Name("Peak CCU");
            Map(g => g.Price).Name("Price");
            Map(g => g.HeaderImage).Name("Header image");
            Map(g => g.MetacriticScore).Name("Metacritic score");
            Map(g => g.Positive).Name("Positive");
            Map(g => g.Negative).Name("Negative");
            Map(g => g.Recommendations).Name("Recommendations");
            Map(g => g.AveragePlaytime).Name("Average playtime forever");
            Map(g => g.Developers).Name("Developers");
            Map(g => g.Publishers).Name("Publishers");
            Map(g => g.Genres).Name("Genres");
            Map(g => g.Tags).Name("Tags");
        }
    }
}
