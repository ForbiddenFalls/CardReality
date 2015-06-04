using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Controllers;
using CardReality.Data.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace CardReality.Areas.Battles.Controllers
{
    public class BattleController : BaseController
    {
        public const int TurnDelay = 60;

        // GET: Battles/Battle
        public BattleController(IApplicationData data) : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Duel(int id)
        {
            var player = this.Data.Players.Find(User.Identity.GetUserId());
            //if (
            //    !this.Data.Battles.All()
            //        .Any(b => b.Id == id && (b.Attacker.Id == player.Id || b.Defender.Id == player.Id) && b.IsActive))
            //{
            //    throw new HubException("You do not belong to that duel");
            //}

            var battle = this.Data.Battles.Find(id);
            var currentDate = DateTime.Now;
            var turnDate = battle.TurnStartedOn;
            var left = turnDate.Subtract(currentDate);
            //if (left.Milliseconds >= 30*1000)
            //{
            //    battle.IsActive = false;
            //    throw new HubException("Battle has ended");
            //}

            return View(battle);
        }
    }
}