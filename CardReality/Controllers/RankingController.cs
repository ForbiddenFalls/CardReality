using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Data.Data;

namespace CardReality.Controllers
{
    public class RankingController : BaseController
    {

        public RankingController(IApplicationData data) : base(data)
        {
        }

        // GET: Ranking
        public ActionResult Index()
        {
            var players = this.Data.Players.All().OrderByDescending(p => p.Wins);
            return View(players);
        }
    }
}