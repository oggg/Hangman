using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
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

            var guessWord = GetGuessWord(randomWord.TheWord);

            //TODO: add cache to hold the data for the game
            return RedirectToAction("Play", new { newGame.Id });
        }

        [HttpGet]
        public ActionResult Play(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentPlayerScore = this.scores.GetById(currentUserId);
            return View(currentPlayerScore);
        }

        private string GetGuessWord(string dbWord)
        {
            var charArr = dbWord.ToCharArray();
            for (int i = 1; i < charArr.Length - 1; i++)
            {
                charArr[i] = '_';
            }

            return new string(charArr);
        }
    }
}