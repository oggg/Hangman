namespace Hangman.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class GameCreateViewModel
    {
        [Required]
        [Display(Name = "Name of the game")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Select category")]
        public int CategoryId { get; set; }

        [Display(Name = "Multiplayer")]
        public bool Multiplayer { get; set; }
    }
}