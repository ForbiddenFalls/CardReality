using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int AttackPoints { get; set; }

        public int DefensePoints { get; set; }

        [Required]
        public bool IsSpecial { get; set; }

        public virtual SpecialEffect SpecialEffect { get; set; }

        public virtual ICollection<PlayerCard> Owners { get; set; }

        public Card()
        {
            this.Owners = new HashSet<PlayerCard>();
        }
    }
}
