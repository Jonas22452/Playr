using CsvHelper;
using Playr.Data;
using Playr.Models;
using System.Globalization;

namespace Playr.Repositories
{
    public class GameRepository
    {
        private readonly List<Game> _games; //we can't change the list --- _games indicates it's a private field

        public GameRepository()
        {
            using var reader = new StreamReader("Data/games.csv");
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
