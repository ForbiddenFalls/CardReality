using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data.Models
{
    public class Battle
    {
        public const int InitialLifePoints = 100;
        public const int FieldRows = 4;
        public const int FieldCols = 5;

        public static readonly int[] AttackerRows = {2, 3};
        public static readonly int[] DefenderRows = {0, 1};

        public Battle()
        {
            this.AttackerLifePoints = this.DefenderLifePoints = Battle.InitialLifePoints;
            this.IsActive = false;
            this.FieldState = new HashSet<FieldState>();
            this.Hands = new List<BattleHand>();
            this.TurnStartedOn = DateTime.Now;
        }

        public int Id { get; set; }

        public virtual Player Attacker { get; set; }

        public virtual Player Defender { get; set; }

        public int AttackerLifePoints { get; set; }

        public int DefenderLifePoints { get; set; }

        public bool IsActive { get; set; }

        public virtual Player Winner { get; set; }

        public virtual ICollection<FieldState> FieldState { get; set; }

        public virtual ICollection<BattleHand> Hands { get; set; }

        public virtual Player CurrentPlayer { get; set; }

        public DateTime TurnStartedOn { get; set; }

        public void InitFieldState()
        {
            for (var row = 0; row < Battle.FieldRows; row++)
            {
                for (var col = 0; col < Battle.FieldCols; col++)
                {
                    var state = new FieldState()
                    {
                        Col = col,
                        Row = row,
                        Owner = Battle.AttackerRows.Contains(row) ? this.Attacker : this.Defender
                    };
                    this.FieldState.Add(state);
                    ApplicationDbContext.Create().FieldState.Add(state);
                }
            }
            this.CurrentPlayer = Attacker;
        }
    }
}
