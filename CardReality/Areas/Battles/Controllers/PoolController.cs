using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Controllers;
using CardReality.Data.Data;
using CardReality.Data.Models;
using Microsoft.AspNet.Identity;

namespace CardReality.Areas.Battles.Controllers
{
    public class PoolController : BaseController
    {
        public PoolController(IApplicationData data) : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Subscribe()
        {
            var user = this.Data.Players.Find(this.User.Identity.GetUserId());
            if (this.Data.Pool.All().Any(bp => bp.Player.Id == user.Id && bp.IsActive))
            {
                return this.RedirectToRoute("home/index");
            }

            var deckCount = user.Deck.Count;
            var availablePlayers =
                this.Data.Pool.All().Where(bp => Math.Abs(bp.Player.Deck.Count - deckCount) < 10)
                    .Select(bp => bp.Player);
            if (availablePlayers.Count() > 0)
            {
                var opponent = availablePlayers.ToList()[new Random().Next(0, availablePlayers.Count())];
                // start battle
                //----
                // remove opponent from pool
                this.Data.Pool.Delete(
                    this.Data.Pool.All().FirstOrDefault(bp => bp.Player.Id == opponent.Id));
                this.Data.SaveChanges();

                return this.RedirectToRoute("battle/index");
            }

            this.Data.Pool.Add(
                new BattlePool()
                {
                    IsActive = true,
                    JoinedOn = DateTime.Now,
                    Player = user
                }
                );
            this.Data.SaveChanges();
            return this.RedirectToRoute("account/cards");
        }
    }
}