using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class Player : IdentityUser
    {
        private uint money;

        public virtual ICollection<PlayerCard> Deck { get; set; }

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

        public Player()
        {
            this.Deck = new List<PlayerCard>();
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
