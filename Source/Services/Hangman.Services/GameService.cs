namespace Hangman.Services
{
    using System.Linq;
    using Contracts;
    using Data.Repositories;
    using Models;

    public class GameService : IGameService
    {
        IRepository<Game> games;

        public GameService(IRepository<Game> games)
        {
            this.games = games;
        }

        public Game Add(Game game)
        {
            this.games.Add(game);
            this.games.SaveChanges();
            return game;
        }

        public Game GetById(int id)
        {
            return this.games.All().FirstOrDefault(s => s.Id == id);
        }

        public Game UpdateState(int id, GameState state)
        {
            var gameToUpdate = this.games.GetById(id);
            gameToUpdate.State = state;

            this.games.SaveChanges();

            return gameToUpdate;
        }
    }
}
