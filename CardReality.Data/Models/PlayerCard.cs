using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class PlayerCard
    {
        public int Id { get; set; }

        public virtual Card Card { get; set; }
        public int CardId { get; set; }

        public virtual Player Player { get; set; }
        public string PlayerId { get; set; }
    }
}
