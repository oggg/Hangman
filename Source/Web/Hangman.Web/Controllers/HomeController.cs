﻿using System.Linq;
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