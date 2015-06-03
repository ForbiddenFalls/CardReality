using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class BattlePool
    {
        public int Id { get; set; }

        public virtual Player Player { get; set; }

        public DateTime JoinedOn { get; set; }

        public bool IsActive { get; set; }

        public BattlePool()
        {
            this.IsActive = false;
        }
    }
}
