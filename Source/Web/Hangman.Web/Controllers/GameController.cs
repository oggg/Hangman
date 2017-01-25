using System;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Hangman.Common;
using Hangman.Models;
using Hangman.Services.Contracts;
using Hangman.Web.Models;
using Microsoft.AspNet.Identity;

namespace Hangman.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameService games;
        private readonly IWordService words;
        private readonly ICategoryService categories;
        private readonly IScoreService scores;
        public GameController(IGameService games, IWordService words, ICategoryService categories, IScoreService scores)
        {
            this.games = games;
            this.words = words;
            this.categories = categories;
            this.scores = scores;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var allCategories = this.categories
                                        .GetAll()
                                        .Project()
                                        .To<CategoryViewModel>()
                                        .ToList();

            ViewBag.Categories = allCategories;
            //TODO: add cache for the dropdown
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GameCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var randomWord = this.words.GetRandom(model.CategoryId);

            var newGame = new Game()
            {
                Name = model.Name,
                State = model.Multiplayer ? GameState.Pending : GameState.Playing,
                User1Id = User.Identity.GetUserId(),
                WordId = randomWord.Id,
                User2Id = null
            };
            this.games.Add(newGame);

            var guessWord = HangmanHelpers.GetGuessWord(randomWord.TheWord);

            GameCacheModel playingGame = new GameCacheModel()
            {
                Id = newGame.Id,
                Name = newGame.Name,
                State = newGame.State,
                Word = randomWord.TheWord,
                ConvertedWordToPlay = guessWord
            };

            if (this.HttpContext.Cache[playingGame.Id.ToString()] == null)
            {
                this.HttpContext.Cache.Insert(
                    playingGame.Id.ToString(),
                    playingGame,
                    null,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);
            }

            return RedirectToAction("Play", new { newGame.Id });
        }

        [HttpGet]
        public ActionResult Play(int id)
        {
            var game = (GameCacheModel)this.HttpContext.Cache[id.ToString()];
            var currentPlayerScore = this.scores.GetById(User.Identity.GetUserId());

            StatisticsPlayGameModel model = new StatisticsPlayGameModel();
            model.UserScore = currentPlayerScore;

            GamePlayStateModel gps = new GamePlayStateModel()
            {
                CurrentWordState = game.ConvertedWordToPlay,
                UsedLetters = string.Empty,
                MovesLeft = HangmanConstants.InitialMoves,
                ImageUrl = string.Format("{0}{1}{2}",
                                                HangmanConstants.ImagesContentFolder,
                                                HangmanConstants.ImageStart,
                                                HangmanConstants.ImagesFileExtension)
            };

            model.GamePlayState = gps;

            return View(model);
        }

        public ActionResult Guess(int gameId, string letters, int movesLeft)
        {
            var currentGame = (GameCacheModel)this.HttpContext.Cache[gameId.ToString()];
            var indexesOfGuessing = HangmanHelpers.GetIndexesOfGuessing(currentGame.Word, letters);

            return this.PartialView("_GameVisualization");
        }
    }
}