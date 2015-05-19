using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardReality.Data.Models;
using CardReality.Models;
using Microsoft.AspNet.Identity;

namespace CardReality.Controllers
{
    public class MarketController : BaseController
    {
        // GET: Market
        public ActionResult Index()
        {
            var offers = this.Data.Offers;

            return View(offers);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Player player = this.Data.Users.Find(User.Identity.GetUserId());
            var cards = player.Deck;

            return View(cards);
        }

        [HttpPost]
        public ActionResult AddOffer()
        {
            Player player = this.Data.Users.Find(User.Identity.GetUserId());
            var cardId = int.Parse(this.Request.Form.Get("card"));
            var price = int.Parse(this.Request.Form.Get("price"));

            PlayerCard card = player.Deck.FirstOrDefault(c => c.Card.Id == cardId);

            if (card == null)
            {
                throw new Exception("You do not own this card");
            }
 
            Market offer = new Market()
            {
                CardName = card.Card.Name,
                Owner = player,
                Price = price,
                SoldOn = DateTime.Now
            };
            this.Data.PlayerCards.Remove(card);

            this.Data.Offers.Add(offer);

            if (this.Data.SaveChanges() > 0)
            {
                return RedirectToAction("Add");
            }
            throw new Exception("Card has not been sold");
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            var data = this.Data.Offers.Find(id);

            if (data == null)
            {
                throw new Exception("Invalid offer");
            }

            Player player = this.Data.Users.Find(User.Identity.GetUserId());

            if (player.Money < data.Price)
            {
                throw new Exception("Not enough money");
            }

            if (player.Id == data.Owner.Id)
            {
                throw new Exception("Cannot buy your own offer");
            }

            player.Money -= data.Price;
            data.Owner.Money += data.Price;

            Card card = this.Data.Cards.FirstOrDefault(c => c.Name == data.CardName);
            if (card == null)
            {
                bool hasSpecialEffect = this.IsSpecial();

                card = new Card()
                {
                    Name = data.CardName,
                    AttackPoints = this.CalculateAttackPoints(data.CardName),
                    DefensePoints = this.CalculateDefensePoints(data.CardName),
                    IsSpecial = hasSpecialEffect
                };
                if (hasSpecialEffect)
                {
                    card.SpecialEffect = this.GetSpecialEffect();
                }
                this.Data.Cards.Add(card);            
            }

            PlayerCard cardPosession = new PlayerCard()
            {
                Card = card,
                Player = player
            };

            player.Deck.Add(cardPosession);
            this.Data.PlayerCards.Add(cardPosession);

            this.Data.Offers.Remove(data);

            if (this.Data.SaveChanges() > 0)
            {
                return this.RedirectToAction("index");
            }
            throw new Exception("Cannot buy offer");
        }

        private int CalculateAttackPoints(string cardName)
        {
            double sum = 0;
            var letters = cardName.ToCharArray().Select(c => c.ToString());
            foreach (string letter in letters)
            {
                var letterData = this.Data.Letters.FirstOrDefault(l => l.Char.ToLower() == letter);
                if (letterData == null) continue;

                sum += letterData.Weight;
            }

            return (int) sum;
        }

        private int CalculateDefensePoints(string cardName)
        {
            Random rnd = new Random();
            double sum = 0;
            var letters = cardName.ToCharArray().Select(c => c.ToString());
            foreach (string letter in letters)
            {
                var letterData = this.Data.Letters.FirstOrDefault(l => l.Char.ToLower() == letter);
                if (letterData == null) continue;

                sum += letterData.Weight*((double)rnd.Next(3, 16)/10);
            }

            return (int)sum;
        }

        private bool IsSpecial()
        {
            Random rnd = new Random();
            int result = rnd.Next(1, 101);

            return result <= 20;
        }

        private SpecialEffect GetSpecialEffect()
        {
            Random rnd = new Random();
            List<SpecialEffect> effects = Enum.GetValues(typeof (SpecialEffect)).Cast<SpecialEffect>().ToList();

            return effects[rnd.Next(0, effects.Count)];
        }
    }
}