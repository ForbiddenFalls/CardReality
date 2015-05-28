using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CardReality.Data.Data;
using CardReality.Data.Models;
using CardReality.Enums;
using CardReality.Services;
using Microsoft.AspNet.Identity;

namespace CardReality.Controllers
{
    public class MarketController : BaseController
    {
        
        public MarketController(IApplicationData data) : base(data)
        {
        }

        // GET: Market
        public ActionResult Index()
        {
            var offers = this.Data.Offers.All();

            return View(offers);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Player player = this.Data.Players.Find(User.Identity.GetUserId());
            var cards = player.Deck;

            return View(cards);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOffer()
        {
            Player player = this.Data.Players.Find(User.Identity.GetUserId());
            int cardId = int.Parse(this.Request.Form.Get("card"));
            int price = int.Parse(this.Request.Form.Get("price"));

            PlayerCard card = player.Deck.FirstOrDefault(c => c.Card.Id == cardId);

            if (card == null)
            {
                throw new Exception(LocalizationService.Translate(Message.CardNotOwner));
            }

            Market offer = new Market()
            {
                CardName = card.Card.Name,
                Owner = player,
                Price = price,
                SoldOn = DateTime.Now
            };

            this.Data.PlayerCards.Delete(card);
            this.Data.Offers.Add(offer);

            if (this.Data.SaveChanges() > 0)
            {
                return RedirectToAction("Add");
            }

            throw new Exception(LocalizationService.Translate(Message.CardNotSold));
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            CardsService cardsService = new CardsService(this.Data);
            Market data = this.Data.Offers.Find(id);

            if (data == null)
            {
                throw new Exception(LocalizationService.Translate(Message.CardInvalidOffer));
            }

            Player player = this.Data.Players.Find(User.Identity.GetUserId());

            if (player.Money < data.Price)
            {
                throw new Exception(LocalizationService.Translate(Message.NotEnoughMoney));
            }

            if (player.Id == data.Owner.Id)
            {
                throw new Exception(LocalizationService.Translate(Message.CardSameOwner));
            }

            player.Money -= data.Price;
            data.Owner.Money += data.Price;

            Card card = this.Data.Cards.All().FirstOrDefault(c => c.Name == data.CardName);
            if (card == null)
            {
                bool hasSpecialEffect = cardsService.IsSpecial();

                card = new Card()
                {
                    Name = data.CardName,
                    AttackPoints = cardsService.CalculateAttackPoints(data.CardName),
                    DefensePoints = cardsService.CalculateDefensePoints(data.CardName),
                    IsSpecial = hasSpecialEffect
                };
                if (hasSpecialEffect)
                {
                    card.SpecialEffect = cardsService.GetSpecialEffect();
                }
                this.Data.Cards.Add(card);
            }

            PlayerCard cardPosession = new PlayerCard()
            {
                Card = card,
                Player = player,
                BoughtFor = data.Price
            };

            player.Deck.Add(cardPosession);
            this.Data.PlayerCards.Add(cardPosession);
            this.Data.Offers.Delete(data);

            if (this.Data.SaveChanges() > 0)
            {
                return this.RedirectToAction("index");
            }

            throw new Exception(LocalizationService.Translate(Message.OfferNotBought));
        }

    }
}