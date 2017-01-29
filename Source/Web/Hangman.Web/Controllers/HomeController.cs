using System.Linq;
using System.Web.Mvc;
using Hangman.Services.Contracts;

namespace Hangman.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IScoreService scores)
            : base(scores)
        {
        }

        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Create", "Game");
            }

            var topPlayers = this.scores
                                .GetAll()
                                .OrderByDescending(t => t.Won)
                                .Take(10)
                                .ToList();
            return View(topPlayers);

        }
    }
}