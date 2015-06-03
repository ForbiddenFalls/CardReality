using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CardReality.Areas.Battles.Controllers;
using CardReality.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CardReality.Hubs
{
    public class BattleHub : BaseHub
    {
        private Timer timer = null;
        private bool hasStarted;

        public override Task OnConnected()
        {
            //var player = this.Data.Players.Find(Context.User.Identity.GetUserId());
            return base.OnConnected();
           
        }

        private List<dynamic> GetBattleInfo(int battleId)
        {
            var player = this.Data.Players.Find(Context.User.Identity.GetUserId());
            var battle = this.Data.Battles.Find(battleId);
            if (player.Id != battle.Attacker.Id && player.Id != battle.Defender.Id)
            {
                throw new HubException("You do not belong to that battle");
            }

            if (!battle.IsActive)
            {
                throw new HubException("Battle has ended");
            }

            return new List<dynamic>() {battle, player};
        }

        public void AttackPlayer(int battleId, int cardId)
        {
            var info = this.GetBattleInfo(battleId);
            var player = info[1] as Player;
            var battle = info[0] as Battle;
            var opponent = battle.Attacker.Id == player.Id ? battle.Defender : battle.Attacker;

            var attackerCard = battle.FieldState
                .Where(h => h.Card != null && h.Owner.Id == player.Id)
                .FirstOrDefault(h => h.Card.Id == cardId);

            var defenderHasCards = battle.FieldState
                .Where(h => h.Card != null)
                .Where(h => h.Card.IsSpecial == false)
                .Where(h => h.Owner != null)
                .Any(h => h.Owner.Id == opponent.Id);

            if (defenderHasCards)
            {
                throw new HubException("");
            }

            if (attackerCard == null)
            {
                throw new HubException("");
            }

            if (player.Id != battle.CurrentPlayer.Id)
            {
                throw new HubException("");
            }

            var pointsToLower = 0;
            if (opponent.Id == battle.Attacker.Id)
            {
                pointsToLower = (battle.AttackerLifePoints -= attackerCard.Card.AttackPoints);
            }
            else
            {
                pointsToLower = (battle.DefenderLifePoints -= attackerCard.Card.AttackPoints);
            }

            if (pointsToLower <= 0)
            {
                this.BattleEnd(battle, player);
            }

            this.Data.SaveChanges();
            this.Clients.All.notifyLifePoints(new
            {
                Attacker = battle.AttackerLifePoints,
                Defender = battle.DefenderLifePoints
            });
        }

        public void AttackCard(int battleId, int attackerCardId, int defenderCardId)
        {
            var info = this.GetBattleInfo(battleId);
            var player = info[1] as Player;
            var battle = info[0] as Battle;
            var opponent = battle.Attacker.Id == player.Id ? battle.Defender : battle.Attacker;

            var attackerCard = battle.FieldState
                .Where(h => h.Card != null && h.Owner.Id == player.Id)
                .FirstOrDefault(h => h.Card.Id == attackerCardId);

            var defenderCard = battle.FieldState
                .Where(h => h.Card != null && h.Owner.Id == opponent.Id)
                .FirstOrDefault(h => h.Card.Id == defenderCardId && h.Owner.Id == opponent.Id);

            if (attackerCard == null || defenderCard == null)
            {
                throw new HubException("");
            }

            if (player.Id != battle.CurrentPlayer.Id)
            {
                throw new HubException("");
            }

            if (attackerCard.Card.AttackPoints >= defenderCard.Card.DefensePoints)
            {
                var pointsToLower = attackerCard.Card.AttackPoints - defenderCard.Card.DefensePoints;
                battle.FieldState.Remove(defenderCard);
                var result = 0;
                if (opponent.Id == battle.Defender.Id)
                {
                    result = (battle.DefenderLifePoints -= pointsToLower);
                }
                else
                {
                    result = (battle.AttackerLifePoints -= pointsToLower);
                }

                if (result <= 0)
                {
                    this.BattleEnd(battle, player);
                }
            }
            else
            {
                var pointsToLower = defenderCard.Card.DefensePoints - attackerCard.Card.AttackPoints;
                var result = 0;
                if (player.Id == battle.Defender.Id)
                {
                    result = (battle.DefenderLifePoints -= pointsToLower);
                }
                else
                {
                    result = (battle.AttackerLifePoints -= pointsToLower);
                }

                if (result <= 0)
                {
                    this.BattleEnd(battle, opponent);
                }
            }
            this.Data.SaveChanges();
            this.Clients.All.notifyLifePoints(new
            {
                Attacker = battle.AttackerLifePoints,
                Defender = battle.DefenderLifePoints
            });
        }

        public void TimeExceeded(int battleId)
        {
            var info = this.GetBattleInfo(battleId);
            var player = info[1] as Player;
            var battle = info[0] as Battle;
            var now = DateTime.Now.AddSeconds(-(BattleController.TurnDelay - 1));
            if (now < battle.TurnStartedOn)
            {
                throw new HubException("Wrong method call");
            }

            var winner = battle.CurrentPlayer.Id == battle.Attacker.Id ? battle.Defender : battle.Attacker;
            this.BattleEnd(battle, winner);
        }

        private void BattleEnd(Battle battle, Player winner)
        {
            var looser = battle.Attacker.Id == winner.Id ? battle.Defender : battle.Attacker;
            battle.Winner = winner;
            battle.IsActive = false;
            battle.Attacker.ReturnCardsToDeck(battle);
            battle.Defender.ReturnCardsToDeck(battle);

            winner.Wins++;
            looser.Loss++;

            this.Data.SaveChanges();
            this.Clients.All.battleEnded(new
            {
                Id = winner.Id,
                Name = winner.UserName
            });
        }

        public void SetCard(int battleId, int cardId)
        {
            var info = this.GetBattleInfo(battleId);
            var player = info[1] as Player;
            var battle = info[0] as Battle;
            var card = player.Hand.FirstOrDefault(h => h.Card.Id == cardId);

            if (card == null)
            {
                throw new HubException("");    
            }
           
            if (player.Id != battle.CurrentPlayer.Id)
            {
                throw new HubException("");
            }

            var freeFields = battle.FieldState.Where(fs => fs.Owner.Id == player.Id && fs.Card == null);
            if (!freeFields.Any())
            {
                throw new HubException("");
            }

            var field = freeFields.FirstOrDefault();
            field.Card = card.Card;
            battle.Hands.Remove(card);
            this.Data.SaveChanges();
            this.Clients.All.setCard(new
            {
                field.Row,
                field.Col,
                CardId = field.Card.Id,
                CardName = field.Card.Name
            });
            this.Clients.Caller.removeCard(new
            {
                Id = field.Card.Id,
                Name = field.Card.Name
            });
        }

        public void EndTurn(int battleId)
        {
            var info = this.GetBattleInfo(battleId);
            var player = info[1] as Player;
            var battle = info[0] as Battle;

            if (player.Id != battle.CurrentPlayer.Id)
            {
                throw new HubException("Cannot end opponent's turn");
            }

            var opponent = battle.Attacker.Id == player.Id ? battle.Defender : battle.Attacker;
            battle.CurrentPlayer = opponent;
            Card card = opponent.DrawCard(battle);
            this.Clients.All.changeTurn(opponent.Id);
            if (card != null)
            {
                this.Clients.Caller.pushCard(new
                {
                    Name = card.Name,
                    Id = card.Id
                });
            }
            battle.TurnStartedOn = DateTime.Now;
        }
    }
}