using CsvHelper;
using Playr.Data;
using Playr.Models;
using System.Globalization;

namespace Playr.Repositories
{
    public class GameRepository
    {
        private readonly List<Game> _games; //we can't change the list --- _games indicates it's a private field

        public GameRepository(IWebHostEnvironment environment)
        {
            var csvPath = Path.Combine(environment.ContentRootPath, "Data", "games.csv");
            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException(
                    "Data/games.csv ontbreekt. Zet de lokale dataset in de Data-map naast Playr.csproj.",
                    csvPath);
            }

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<GameMap>(); //uses GameMap to map the CSV columns to the Game properties

            _games = csv.GetRecords<Game>().ToList(); //reads all games from the CSV and stores them in a list
        }

        public List<Game> GetAll()
        {
            return _games;
        }
    }
}
