using System.ComponentModel.DataAnnotations.Schema;

namespace Hangman.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string User1Id { get; set; }

        [ForeignKey("User1Id")]
        public virtual User FirstPlayer { get; set; }


        public string User2Id { get; set; }

        [ForeignKey("User2Id")]
        public virtual User SecondPlayer { get; set; }

        public GameState State { get; set; }

        public int WordId { get; set; }

        public virtual Word Word { get; set; }
    }
}
