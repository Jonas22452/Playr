using Playr.Models;
using Playr.Repositories;

namespace Playr.Services
{
    public class BasicRecommender
    {

        private readonly GameRepository _gameRepository;

        public BasicRecommender(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public List<Game> GetMostPlayed()
        {
            return _gameRepository
                .GetAll()
                .OrderByDescending(game => game.AveragePlaytime)
                .Take(10)
                .ToList(); //geef 10 meest gespeelde games terug op basis van average gametime
        }

        public List<Game> GetHighestRated()
        {
            return _gameRepository
                .GetAll()
                .Where(game => game.Positive + game.Negative >= 1000)
                .OrderByDescending(game => (double)game.Positive / (game.Positive + game.Negative))
                .Take(10)
                .ToList(); //geef 10 hoogst rated games terug

        }
    }
}
