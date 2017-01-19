namespace Hangman.Models
{
    public class Word
    {
        public int Id { get; set; }

        public string TheWord { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}