using Hangman.Models;

namespace Hangman.Services.Contracts
{
    public interface IWordService
    {
        Word GetById(int id, int categoryId);
    }
}
