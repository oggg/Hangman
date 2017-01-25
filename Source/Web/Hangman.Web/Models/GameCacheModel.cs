namespace Hangman.Web.Models
{
    using System.Collections.Generic;
    using Hangman.Models;

    public class GameCacheModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GameState State { get; set; }

        public string Word { get; set; }

        public IList<string> UsedLetters { get; set; }

        public int MovesLeft { get; set; }

        public string[] ConvertedWordToPlay { get; set; }
    }
}