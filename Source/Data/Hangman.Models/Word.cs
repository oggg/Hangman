using System.Collections.Generic;

namespace Hangman.Models
{
    public class Word
    {
        private ICollection<Game> games;
        public Word()
        {
            this.games = new HashSet<Game>();
        }

        public int Id { get; set; }

        public string TheWord { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Game> Games { get { return this.games; } set { this.games = value; } }
    }
}