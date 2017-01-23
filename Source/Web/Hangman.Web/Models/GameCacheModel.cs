namespace Hangman.Web.Models
{
    using Hangman.Models;

    public class GameCacheModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GameState State { get; set; }

        public string Word { get; set; }

        public string ConvertedWordToPlay { get; set; }
    }
}