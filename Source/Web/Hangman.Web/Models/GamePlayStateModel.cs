using System.Collections.Generic;

namespace Hangman.Web.Models
{
    public class GamePlayStateModel
    {
        public string[] CurrentWordState { get; set; }

        public string ImageUrl { get; set; }

        public IList<string> UsedLetters { get; set; }

        public int MovesLeft { get; set; }
    }
}