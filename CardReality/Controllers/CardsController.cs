using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Data.Data;
using CardReality.Data.Models;
using Microsoft.AspNet.Identity;

namespace CardReality.Controllers
{
    public class CardsController : BaseController
    {
        public CardsController(IApplicationData data)
            :base(data)
        {
        }

        // GET: Cards
        [Authorize]
        public ActionResult Index()
        {
            Player player = this.Data.Players.Find(User.Identity.GetUserId());
            var playerCards = player.Deck.Select(c => c.Card);
            return View(playerCards);
        }
    }
}