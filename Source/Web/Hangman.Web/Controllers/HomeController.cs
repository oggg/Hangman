﻿using System.Linq;
using System.Web.Mvc;
using Hangman.Services.Contracts;

namespace Hangman.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScoreService scores;
        public HomeController(IScoreService scores)
        {
            this.scores = scores;
        }
        public ActionResult Index()
        {
            var topPlayers = this.scores.GetAll().OrderBy(t => t.Won).Take(10).ToList();
            return View(topPlayers);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}