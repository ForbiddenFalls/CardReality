using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CardRealityConsoleTest
{
    public class Battle
    {
        private readonly Player attacker;
        private readonly Player defender;

        public Player CurrentPlayer { get; private set; }

        private Dictionary<Position, Card> field; 

        public Battle(Player attacker, Player defender)
        {
            this.attacker = attacker;
            this.defender = defender;

            this.CurrentPlayer = attacker;

            this.field = new Dictionary<Position, Card>();
            for (var row = 0; row < 4; row++)
            {
                for (var col = 0; col < 5; col++)
                {
                    var position = new Position(row, col);
                    this.field.Add(position, null);
                }
            }
        }

        public void ChangePlayer()
        {
            this.CurrentPlayer = this.CurrentPlayer.Id == this.attacker.Id ? this.defender : this.attacker;
        }

        public Player GetOpponent()
        {
            return this.CurrentPlayer.Id == this.attacker.Id ? this.defender : this.attacker;
        }

        public void SetFieldCard(Card card)
        {
            var availableField = new KeyValuePair<Position, Card>();
            var hasAvailableFields = false;
            foreach (var field in this.GetOwnFields())
            {
                if (field.Value == null)
                {
                    availableField = field;
                    hasAvailableFields = true;
                    break;
                }
            }

            if (!hasAvailableFields)
            {
                throw new Exception("No available fields");
            }

            var square = this.GetSquare(availableField.Key);
            this.field[square.Key] = card;
            this.CurrentPlayer.Deck.Remove(card);
        }

        public void DirectAttack()
        {
            var opponentCards = this.field.Where(f => f.Value.Owner.Id == this.GetOpponent().Id && f.Value.IsSpecial == false);
            if (opponentCards.Count() > 0)
            {
                throw new Exception("Direct attack is only allowed when opponent has no battle cards");
            }

            this.GetOpponent().LifePoints -= this.CurrentPlayer.CurrentSelection.Value.AttackPoints;
        }

        public void CardAttack(Position fieldPosition)
        {
            var opponentCard = this.GetSquare(fieldPosition).Value;
            if (opponentCard == null)
            {
                throw new Exception("There's no opponent card there");
            }

            if (opponentCard.Owner.Id != this.GetOpponent().Id)
            {
                throw new Exception("Wrong card owner");
            }

            if (this.CurrentPlayer.CurrentSelection.Value.AttackPoints >= opponentCard.DefensePoints)
            {
                this.GetOpponent().LifePoints -= (this.CurrentPlayer.CurrentSelection.Value.AttackPoints -
                                                  opponentCard.DefensePoints);
                var position = this.GetSquare(fieldPosition).Key;
                this.field[position] = null;
            }
            else
            {
                this.CurrentPlayer.LifePoints -= (opponentCard.DefensePoints -
                                                  this.CurrentPlayer.CurrentSelection.Value.AttackPoints);
            }
        }

        private KeyValuePair<Position, Card> GetSquare(Position fieldPosition)
        {
            return this.field.FirstOrDefault(f => f.Key.X == fieldPosition.X && f.Key.Y == fieldPosition.Y);
        }

        private KeyValuePair<Position, Card> GetSquare(int x, int y)
        {
            return this.field.FirstOrDefault(f => f.Key.X == x && f.Key.Y == y);
        }

        private IEnumerable<KeyValuePair<Position, Card>> GetOwnFields()
        {
            if (this.CurrentPlayer.Id == this.defender.Id)
            {
                return this.field.Where(f => f.Key.X <= 1);
            }

            return this.field.Where(f => f.Key.X > 1);
        }

        static void Main()
        {

            var player1 = new Player()
            {
                LifePoints = 4000,
                Id = 1,
                Name = "Player edno"
            };
            var player1Deck = new List<Card>()
            {
                new Card()
                {
                    AttackPoints = 800,
                    DefensePoints = 450,
                    Id = 1,
                    IsSpecial = false,
                    Name = "KUF",
                    Owner = player1,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 800,
                    DefensePoints = 450,
                    Id = 1,
                    IsSpecial = false,
                    Name = "KUF",
                    Owner = player1,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 900,
                    DefensePoints = 800,
                    Id = 2,
                    IsSpecial = false,
                    Name = "LQF",
                    Owner = player1,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 100,
                    DefensePoints = 1200,
                    Id = 3,
                    IsSpecial = false,
                    Name = "HUM",
                    Owner = player1,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 800,
                    DefensePoints = 800,
                    Id = 4,
                    IsSpecial = false,
                    Name = "GOL",
                    Owner = player1,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 1500,
                    DefensePoints = 100,
                    Id  = 5,
                    IsSpecial = false,
                    Name = "EXO",
                    Owner = player1,
                    SpecialEffect = 0
                }
            };
            player1.Deck = player1Deck;

            var player2 = new Player()
            {
                Id = 2,
                LifePoints = 4000,
                Name = "Vtori player"
            };
            var player2Deck = new List<Card>()
            {
                new Card()
                {
                    AttackPoints = 300,
                    DefensePoints = 450,
                    Id = 5,
                    IsSpecial = false,
                    Name = "FLO",
                    Owner = player2,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 1800,
                    DefensePoints = 600,
                    Id = 6,
                    IsSpecial = false,
                    Name = "MOF",
                    Owner = player2,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 900,
                    DefensePoints = 800,
                    Id = 2,
                    IsSpecial = false,
                    Name = "LQF",
                    Owner = player2,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 100,
                    DefensePoints = 1200,
                    Id = 3,
                    IsSpecial = false,
                    Name = "HUM",
                    Owner = player2,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 800,
                    DefensePoints = 800,
                    Id = 4,
                    IsSpecial = false,
                    Name = "GOL",
                    Owner = player2,
                    SpecialEffect = 0
                },
                new Card()
                {
                    AttackPoints = 1300,
                    DefensePoints = 40,
                    Id  = 7,
                    IsSpecial = false,
                    Name = "VGZ",
                    Owner = player2,
                    SpecialEffect = 0
                }
            };
            player1.Deck = player1Deck;
            player2.Deck = player2Deck;

            var battle = new Battle(player1, player2);

            while (player1.LifePoints > 0 || player2.LifePoints > 0)
            {
                var cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "SetCard":
                        battle.SetFieldCard(
                            battle.CurrentPlayer.Deck[
                                (new Random().Next(0, battle.CurrentPlayer.Deck.Count))
                                ]
                            );
                        break;
                }
                battle.ChangePlayer();
            }
        }
    }

    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttackPoints { get; set; }
        public int DefensePoints { get; set; }
        public bool IsSpecial { get; set; }
        public SpecialEffect SpecialEffect { get; set; }
        public Player Owner { get; set; }

    }

    public enum SpecialEffect
    {
        ReturnCards,
        DevastateField
    }

    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Card> Deck { get; set; }

        public KeyValuePair<Position, Card> CurrentSelection { get; set; }

        public int LifePoints { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
