namespace Hangman.Services
{
    using System.Linq;
    using Contracts;
    using Data.Repositories;
    using Models;

    public class WordService : IWordService
    {
        IRepository<Word> words;

        public WordService(IRepository<Word> words)
        {
            this.words = words;
        }

        public Word GetById(int id, int categoryId)
        {
            Word word = this.words.All().FirstOrDefault(w => w.Id == id && w.CategoryId == categoryId);
            return word;
        }
    }
}
