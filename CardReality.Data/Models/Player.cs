using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class Player : IdentityUser
    {
        [StringLength(1000)]
        [DisplayName("Full-size Image")]
        public string ImageURL { get; set; }

        [StringLength(1000)]
        [DisplayName("Thumbnail")]
        public string ThumbnailURL { get; set; }

        private static Random random = new Random();

        public const int MaxHandCards = 5;

        private uint money;

        public virtual ICollection<PlayerCard> Deck { get; set; }

        public virtual ICollection<BattleHand> Hand { get; set; } 

        public int Money
        {
            get 
            {
                checked
                {
                    return int.Parse(Convert.ToUInt32(this.money).ToString());
                } 
            }
            set
            {
                checked
                {
                    this.money = (uint) value;
                }
            }
        }

        public int Wins { get; set; }

        public int Loss { get; set; }

        public bool BattleSubscribed { get; set; }

        public Player()
        {
            this.Deck = new List<PlayerCard>();
            this.Hand = new List<BattleHand>();
            this.BattleSubscribed = false;
        }

        public void ReturnCardsToDeck(Battle battle)
        {
            if (battle.Attacker.Id != this.Id && battle.Defender.Id != this.Id)
            {
                return;
            }

            var hand = this.Hand
                .Where(h => h.Battle != null)
                .Where(h => h.Battle.Id == battle.Id)
                .Select(h => new PlayerCard()
                {
                    BoughtFor = 0,
                    Card = h.Card,
                    CardId = h.Card.Id,
                    Player = this,
                    PlayerId = this.Id
                });
            ((List<PlayerCard>)this.Deck).AddRange(hand);
            var handBattle = this.Hand.Where(h => h.Battle.Id == battle.Id);
            var context = ApplicationDbContext.Create();
            context.BattleHands.Where(h => h.Battle.Id == battle.Id)
               .ToList().ForEach(h => context.BattleHands.Remove(h));
        }

        public Card DrawCard(Battle battle)
        {
            if (battle.Attacker.Id != this.Id && battle.Defender.Id != this.Id)
            {
                return null;
            }

            if (battle.Hands.Count(h => h.Owner.Id == this.Id) >= Player.MaxHandCards)
            {
                return null;
            }

            if (!this.Deck.Any())
            {
                return null;
            }
            
            
            var randomCard = this.Deck.OrderBy(d => random.Next()).FirstOrDefault();
            this.Hand.Add(new BattleHand()
            {
                Battle = battle,
                Card = randomCard.Card,
            });
            Card cardResult = randomCard.Card;
            this.Deck.Remove(randomCard);
            ApplicationDbContext.Create().PlayerCards.Remove(randomCard);

            return cardResult;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Player> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
