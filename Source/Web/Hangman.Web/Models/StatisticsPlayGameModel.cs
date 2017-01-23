using Hangman.Models;

namespace Hangman.Web.Models
{
    public class StatisticsPlayGameModel
    {
        public Score UserScore { get; set; }

        public GamePlayStateModel GamePlayState { get; set; }
    }
}