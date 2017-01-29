namespace Hangman.Web.Models
{
    using System.Collections.Generic;
    using Hangman.Models;
    using Infrastructure.Mapping;

    public class GameCacheModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GameState State { get; set; }

        public int ImageId { get; set; }

        public string Word { get; set; }

        public IList<string> UsedLetters { get; set; }

        public int MovesLeft { get; set; }

        public string[] ConvertedWordToPlay { get; set; }
    }
}