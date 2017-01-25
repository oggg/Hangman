namespace Hangman.Web.Models
{
    using Hangman.Models;
    using Infrastructure.Mapping;

    public class ScoreCacheModel : IMapFrom<Score>
    {
        public string Id { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int LettersGussed { get; set; }

        public int WordsGussed { get; set; }
    }
}