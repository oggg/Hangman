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
        public GameController(IGameService games, IWordService words, ICategoryService categories)
        {
            this.games = games;
            this.words = words;
            this.categories = categories;
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
            return View(model);
        }
    }
}