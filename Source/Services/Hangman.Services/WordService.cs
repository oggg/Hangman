namespace Hangman.Services
{
    using System;
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

        public Word GetRandom(int categoryId)
        {
            var randomWord = this.words.All()
                                        .Where(w => w.CategoryId == categoryId)
                                        .OrderBy(w => Guid.NewGuid())
                                        .Take(1)
                                        .FirstOrDefault();
            return randomWord;
        }
    }
}
