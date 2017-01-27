using System.Web.Mvc;
using Hangman.Services.Contracts;

namespace Hangman.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IScoreService scores;

        public BaseController(IScoreService scores)
        {
            this.scores = scores;
        }

    }
}