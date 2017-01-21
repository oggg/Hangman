using System.Collections.Generic;

namespace Hangman.Models
{
    public class Category
    {
        private ICollection<Word> words;

        public Category()
        {
            this.words = new HashSet<Word>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Word> Words { get { return this.words; } set { this.words = value; } }
    }
}
