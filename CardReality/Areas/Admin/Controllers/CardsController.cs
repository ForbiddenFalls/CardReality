using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CardReality.Controllers;
using CardReality.Data.Data;
using CardReality.Data.Models;
using CardReality.Services;

namespace CardReality.Areas.Admin
{
    [Authorize(Roles = "Administrator")]
    public class CardsController : BaseController
    {
        // GET: Admin/Cards
        public CardsController(IApplicationData data) : base(data)
        {
        }

        public ActionResult Index()
        {
            var cards = this.Data.Cards.All();
            return View(cards);
        }

        // GET: Admin/Cards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {
                CardsService cardsService = new CardsService(this.Data);
                string cardName = collection[1];
                int attackPoints = 0;
                int defencePoints = 0;
                if (string.IsNullOrEmpty(collection[2]) || int.Parse(collection[2]) == 0)
                {
                    attackPoints = cardsService.CalculateAttackPoints(cardName);
                }
                else
                {
                    attackPoints = int.Parse(collection[2]);
                }

                if (string.IsNullOrEmpty(collection[3]) || int.Parse(collection[3]) == 0)
                {
                    defencePoints = cardsService.CalculateDefensePoints(cardName);
                }
                else
                {
                    defencePoints = int.Parse(collection[3]);
                }

                var card = new Card
                {
                    Name = cardName,
                    AttackPoints = attackPoints,
                    DefensePoints = defencePoints
                };

                var admin = this.Data.Players.All().FirstOrDefault(p => p.UserName == "admin");
                var playerCard = new PlayerCard
                {
                    Card = card,
                    BoughtFor = 0
                };
                admin.Deck.Add(playerCard);
                this.Data.Cards.Add(card);
                await this.Data.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Cards/Edit/5
        public ActionResult Edit(int id)
        {
            var card = this.Data.Cards.All().FirstOrDefault(c => c.Id == id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Admin/Cards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var card = this.Data.Cards.All().FirstOrDefault(c => c.Id == id);
                if (card == null)
                {
                    return HttpNotFound();
                }

                CardsService cardsService = new CardsService(this.Data);
                string cardName = collection[1];
                int attackPoints = 0;
                int defencePoints = 0;
                if (string.IsNullOrEmpty(collection[2]) || int.Parse(collection[2]) == 0)
                {
                    attackPoints = cardsService.CalculateAttackPoints(cardName);
                }
                else
                {
                    attackPoints = int.Parse(collection[2]);
                }

                if (string.IsNullOrEmpty(collection[3]) || int.Parse(collection[3]) == 0)
                {
                    defencePoints = cardsService.CalculateDefensePoints(cardName);
                }
                else
                {
                    defencePoints = int.Parse(collection[3]);
                }

                card.Name = cardName;
                card.AttackPoints = attackPoints;
                card.DefensePoints = defencePoints;

                this.Data.Cards.Update(card);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Cards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var card = this.Data.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Admin/Cards/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var card = this.Data.Cards.Find(id);
            this.Data.Cards.Delete(card);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
