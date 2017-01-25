using System.Linq;
using Hangman.Models;

namespace Hangman.Services.Contracts
{
    public interface IScoreService
    {
        Score Add(Score score);

        IQueryable<Score> GetAll();

        Score GetById(string id);

        Score UpdateById(string id, Score updateScore);
    }
}
