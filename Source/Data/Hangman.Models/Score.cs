using System.ComponentModel.DataAnnotations.Schema;

namespace Hangman.Models
{
    public class Score
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int LettersGussed { get; set; }

        public int WordsGussed { get; set; }
    }
}
