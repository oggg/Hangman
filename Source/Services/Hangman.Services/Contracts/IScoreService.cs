using System.Linq;
using Hangman.Models;

namespace Hangman.Services.Contracts
{
    interface IScoreService
    {
        Score Add(Score score);

        IQueryable<Score> GetAll();

        Score GetById(string id);
    }
}
