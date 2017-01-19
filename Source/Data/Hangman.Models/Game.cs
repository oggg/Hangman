using System.ComponentModel.DataAnnotations.Schema;

namespace Hangman.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string User1Id { get; set; }

        public string User2Id { get; set; }

        [Index]
        public GameState State { get; set; }
    }
}
