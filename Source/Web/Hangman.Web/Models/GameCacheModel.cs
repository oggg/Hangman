namespace Hangman.Web.Models
{
    using Hangman.Models;

    public class GameCacheModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string User1Id { get; set; }

        public string User2Id { get; set; }

        public GameState State { get; set; }

        public int WordId { get; set; }

        public string ConvertedWordToPlay { get; set; }
    }
}