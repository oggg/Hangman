using Hangman.Models;

namespace Hangman.Services.Contracts
{
    public interface IGameService
    {
        Game Add(Game game);

        Game GetById(int id);

        Game UpdateState(int id, GameState state);
    }
}
