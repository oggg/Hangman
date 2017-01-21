using System.ComponentModel.DataAnnotations.Schema;

namespace Hangman.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("FirstPlayer")]
        public string User1Id { get; set; }

        [ForeignKey("SecondPlayer")]
        public string User2Id { get; set; }

        public virtual User FirstPlayer { get; set; }

        public virtual User SecondPlayer { get; set; }

        [Index]
        public GameState State { get; set; }
    }
}
