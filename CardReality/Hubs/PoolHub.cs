using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CardReality.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace CardReality.Hubs
{
    public class PoolHub : BaseHub
    {
        private const string RoomName = "Pool";

        public void Hello()
        {
            
            Clients.All.hello();
        }

        public void Subscribe()
        {
            Thread.Sleep(20);
            var player = this.Data.Players.Find(Context.User.Identity.GetUserId());
            player.BattleSubscribed = true;
            this.Data.SaveChanges();
            this.Connect();
        }

        public void Unsubscribe()
        {
            Thread.Sleep(20);
            var player = this.Data.Players.Find(Context.User.Identity.GetUserId());
            player.BattleSubscribed = false;
            this.Data.Pool.Delete(this.Data.Pool.All().FirstOrDefault(bp => bp.Player.Id == player.Id && bp.IsActive));
            this.Data.SaveChanges();
            this.Clients.User(player.UserName).detach();
        }

        public void Connect()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                return;
            }

            var player = this.Data.Players.Find(Context.User.Identity.GetUserId());
            if (!player.BattleSubscribed)
            {
                return;
            }

            if (this.Data.Pool.All().Any(bp => bp.Player.Id == player.Id && bp.IsActive))
            {
                return;
            }

            this.Data.Pool.Add(
                new BattlePool()
                {
                    IsActive = true,
                    JoinedOn = DateTime.Now,
                    Player = player
                });
            this.Groups.Add(player.UserName, RoomName);
            
            this.Data.SaveChanges();
            this.CheckAvailability(player.Id);
            //{
            //    return;
            //}
            //this.Clients.AllExcept(player.UserName).notify(player.Id);
        }

        public void CheckAvailability(string playerId)
        {
            var player = this.Data.Players.Find(Context.User.Identity.GetUserId());
            if (this.HasAvailablePlayers(player))
            {
                var availablePlayers = this.GetAvailablePlayers(player).ToList();
                var count = this.GetAvailablePlayers(player).Count();
                var index = new Random().Next(0, count);
                var opponent = availablePlayers[index];

                // start battle
                //----
                // remove opponent from pool

                foreach (var user in new[] { player, opponent })
                {
                    this.Data.Pool.Delete(
                        this.Data.Pool.All().FirstOrDefault(bp => bp.Player.Id == user.Id)
                        );
                    this.Clients.User(user.UserName).detach();
                    // add to room
                    // open room view
                    // check if player is in room (security)
                    user.BattleSubscribed = false;
                }
                var battle = new Battle()
                {
                    Attacker = player,
                    Defender = opponent,
                    IsActive = true
                };
                this.Data.SaveChanges();
                battle.InitFieldState();
                for (var card = 0; card < Player.MaxHandCards; card++)
                {
                    player.DrawCard(battle);
                    opponent.DrawCard(battle);
                }
                this.Data.Battles.Add(battle);
                this.Data.SaveChanges();
                this.Clients.User(player.UserName).startBattle(opponent.UserName, battle.Id);
                this.Clients.User(opponent.UserName).startBattle(player.UserName, battle.Id);
            }

        }

        private bool HasAvailablePlayers(Player player)
        {
            var deckCount = player.Deck.Count;
            return
                this.Data.Pool.All()
                    .Any(bp => Math.Abs(bp.Player.Deck.Count - deckCount) < 10 && bp.Player.Id != player.Id);
        }

        private IEnumerable<Player> GetAvailablePlayers(Player player)
        {
            var deckCount = player.Deck.Count;
            return
                this.Data.Pool.All()
                    .Where(bp => Math.Abs(bp.Player.Deck.Count - deckCount) < 10)
                    .Where(bp => bp.Player.Id != player.Id)
                    .Select(bp => bp.Player);
        }

        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}