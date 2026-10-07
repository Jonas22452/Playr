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

        public List<Game> GetMostPopular()
        {
            return _gameRepository
                .GetAll()
                .OrderByDescending(game => game.Positive + game.Negative)
                .Take(10)
                .ToList(); // Geef 10 populairste games terug op basis van aantal reviews
        }
    }
}
