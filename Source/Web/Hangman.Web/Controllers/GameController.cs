using System;
using System.Collections.Generic;
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
                ImageId = 0,
                Word = randomWord.TheWord,
                ConvertedWordToPlay = guessWord,
                UsedLetters = new List<string>(),
                MovesLeft = HangmanConstants.InitialMoves
            };

            ScoreCacheModel scorePerGame = new ScoreCacheModel()
            {
                Id = User.Identity.GetUserId(),
                LettersGussed = 0
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

            if (this.HttpContext.Cache[currentPlayerScore.Id] == null)
            {
                this.HttpContext.Cache.Insert(
                    currentPlayerScore.Id,
                    currentPlayerScore,
                    null,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);
            }

            StatisticsPlayGameModel model = new StatisticsPlayGameModel();
            model.UserScore = currentPlayerScore;

            GamePlayStateModel gps = new GamePlayStateModel()
            {
                CurrentWordState = game.ConvertedWordToPlay,
                UsedLetters = new List<string>(),
                MovesLeft = HangmanConstants.InitialMoves,
                ImageUrl = string.Format("{0}{1}{2}",
                                                HangmanConstants.ImagesContentFolder,
                                                HangmanConstants.ImageStart,
                                                HangmanConstants.ImagesFileExtension)
            };

            model.GamePlayState = gps;

            return View(model);
        }

        public ActionResult Guess(int gameId, string letters)
        {
            GameCacheModel currentGame = (GameCacheModel)this.HttpContext.Cache[gameId.ToString()];
            var currentUserId = User.Identity.GetUserId();
            Score currentScore = (Score)this.HttpContext.Cache[currentUserId];
            Score latestScore;
            Game game = new Game();
            GamePlayStateModel gps = new GamePlayStateModel();
            gps.MovesLeft = currentGame.MovesLeft;
            // win/loose game with a whole word
            if (letters.Length > 1)
            {
                this.HttpContext.Cache.Remove(currentUserId);
                this.HttpContext.Cache.Remove(gameId.ToString());
                gps.MovesLeft--;

                //just added
                gps.CurrentWordState = new string[letters.Length];
                if (string.Compare(currentGame.Word, letters, true) == 0)
                {
                    currentScore.WordsGussed++;
                    currentScore.Won++;

                    for (int i = 0; i < currentGame.Word.Length; i++)
                    {
                        gps.CurrentWordState[i] = currentGame.Word[i].ToString();
                    }
                    gps.ImageUrl = string.Format("{0}{1}{2}",
                        HangmanConstants.ImagesContentFolder,
                        HangmanConstants.ImageWinWholeWord,
                        HangmanConstants.ImagesFileExtension);
                    gps.UsedLetters = currentGame.UsedLetters;
                }
                else
                {
                    currentScore.Lost++;
                    gps.ImageUrl = string.Format("{0}{1}{2}",
                        HangmanConstants.ImagesContentFolder,
                        HangmanConstants.ImageLoose,
                        HangmanConstants.ImagesFileExtension);
                    gps.UsedLetters = currentGame.UsedLetters;
                }

                latestScore = this.scores.UpdateById(currentUserId, currentScore);
                game = this.games.UpdateState(currentGame.Id, GameState.Ended);

                return this.PartialView("_GameVisualization", gps);
            }
            // check current conditions when only one letter is used
            else
            {
                var indexesOfGuessing = HangmanHelpers.GetIndexesOfGuessing(currentGame.Word, letters);
                string[] constructedWord;

                if (indexesOfGuessing.Count != 0)
                {
                    constructedWord = HangmanHelpers.ConstructUpdatedWord(currentGame.ConvertedWordToPlay, indexesOfGuessing, letters);
                }
                else
                {
                    constructedWord = currentGame.ConvertedWordToPlay;
                }

                bool wordGuessed = string.Compare(currentGame.Word.ToLower(), string.Join("", constructedWord).ToLower()) == 0;

                if (currentGame.MovesLeft > 0)
                {
                    currentGame.MovesLeft--;
                    currentGame.UsedLetters.Add(letters);

                    if (wordGuessed)
                    {
                        currentScore.LettersGussed++;
                        currentScore.Won++;

                        latestScore = this.scores.UpdateById(currentUserId, currentScore);
                        this.HttpContext.Cache.Remove(currentUserId);
                        this.HttpContext.Cache.Remove(gameId.ToString());

                        game = this.games.UpdateState(currentGame.Id, GameState.Ended);
                        gps.ImageUrl = string.Format("{0}{1}{2}",
                                            HangmanConstants.ImagesContentFolder,
                                            HangmanConstants.ImageWin,
                                            HangmanConstants.ImagesFileExtension);
                        gps.UsedLetters = currentGame.UsedLetters;
                        gps.CurrentWordState = new string[currentGame.Word.Length];
                        for (int i = 0; i < currentGame.Word.Length; i++)
                        {
                            gps.CurrentWordState[i] = currentGame.Word[i].ToString();
                        }
                    }
                    else
                    {
                        if (currentGame.ConvertedWordToPlay.SequenceEqual(constructedWord))
                        {
                            gps.ImageUrl = string.Format("{0}{1}{2}",
                                           HangmanConstants.ImagesContentFolder,
                                           ++currentGame.ImageId,
                                           HangmanConstants.ImagesFileExtension);
                        }
                        else
                        {
                            gps.ImageUrl = string.Format("{0}{1}{2}",
                                           HangmanConstants.ImagesContentFolder,
                                           currentGame.ImageId,
                                           HangmanConstants.ImagesFileExtension);
                        }
                        gps.CurrentWordState = constructedWord;
                        gps.MovesLeft = currentGame.MovesLeft;
                        currentGame.ConvertedWordToPlay = constructedWord;
                        gps.UsedLetters = currentGame.UsedLetters;

                        this.HttpContext.Cache.Insert(
                                    currentGame.Id.ToString(),
                                    currentGame,
                                    null,
                                    DateTime.Now.AddYears(1),
                                    TimeSpan.Zero,
                                    CacheItemPriority.Default,
                                    null);
                        this.HttpContext.Cache.Insert(
                                    currentScore.Id,
                                    currentScore,
                                    null,
                                    DateTime.Now.AddYears(1),
                                    TimeSpan.Zero,
                                    CacheItemPriority.Default,
                                    null);
                    }
                }
                else
                {
                    currentScore.Lost++;
                    gps.ImageUrl = string.Format("{0}{1}{2}",
                        HangmanConstants.ImagesContentFolder,
                        HangmanConstants.ImageLoose,
                        HangmanConstants.ImagesFileExtension);
                    gps.UsedLetters = currentGame.UsedLetters;
                    latestScore = this.scores.UpdateById(currentUserId, currentScore);
                    this.HttpContext.Cache.Remove(currentUserId);
                    this.HttpContext.Cache.Remove(gameId.ToString());

                    game = this.games.UpdateState(currentGame.Id, GameState.Ended);
                }
                return this.PartialView("_GameVisualization", gps);
            }
        }

    }
}