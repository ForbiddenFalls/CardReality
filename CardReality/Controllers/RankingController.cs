using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Data.Data;
using CardReality.Models;
using WebGrease.Css.Extensions;

namespace CardReality.Controllers
{
    public class RankingController : BaseController
    {

        public RankingController(IApplicationData data) : base(data)
        {
        }

        // GET: Ranking
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.RatioSortParm = String.IsNullOrEmpty(sortOrder) ? "ratio" : "";
            ViewBag.WinsSortParm = sortOrder == "wins" ? " " : "wins";
            ViewBag.LossSortParm = sortOrder == "loss" ? " " : "loss";

            var players = this.Data.Players.All()
                .Select(p => new RankingViewModel
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    Wins = p.Wins,
                    Loss = p.Loss,
                    Ratio =
                        p.Wins + p.Loss == 0 ? 0 : Math.Round(p.Wins/(double) (p.Wins + p.Loss), 4)*100
                }).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.UserName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ratio":
                    players = players.OrderByDescending(p => p.Ratio).ThenBy(p => p.Id);
                    break;
                case "wins":
                    players = players.OrderByDescending(p => p.Wins).ThenBy(p => p.Id);
                    break;
                case "loss":
                    players = players.OrderByDescending(p => p.Loss).ThenBy(p => p.Id);
                    break;
                default:
                    players = players.OrderByDescending(p => p.Ratio).ThenBy(p => p.Id);
                    break;
            }

            var pl = players.ToList();
            for (int i = 0; i < pl.Count; i++)
            {
                int rank;
                int wins = pl[i].Wins;
                int loss = pl[i].Loss;
                int games = wins + loss;
                double ratio = games == 0 ? 0 : wins / (double)games;
                switch (sortOrder)
                {
                    case "ratio":
                        rank = this.Data.Players.All().Count(p => (p.Wins + p.Loss == 0 ? 0 : p.Wins / (double)(p.Wins + p.Loss))>= ratio);
                        break;
                    case "wins":
                        rank = this.Data.Players.All().Count(p => p.Wins >= wins);
                        break;
                    case "loss":
                        rank = this.Data.Players.All().Count(p => p.Loss >= loss);
                        break;
                    default:
                        rank = this.Data.Players.All().Count(p => (p.Wins + p.Loss == 0 ? 0 : p.Wins / (double)(p.Wins + p.Loss))>= ratio);
                        break;
                }

                pl[i].Rank = rank;
            }

            return View(pl);
        }

    }
}