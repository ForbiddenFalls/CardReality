using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CardReality.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using CardReality.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<Player>, IApplicationDbContext
    {
        private const string AdminRole = "Administrator";
        private const string AdminUserName = "admin";
        private const string AdminEmail = "admin@admin.com";
        private const string AdminPassword = "#Lo6omie";


        public static readonly List<Card> InitialCards = new List<Card>()
        {
            new Card() {AttackPoints = 1000, DefensePoints = 200, IsSpecial = false, Name = "DM", SpecialEffect = 0},
            new Card() {AttackPoints = 800, DefensePoints = 300, IsSpecial = false, Name = "WD", SpecialEffect = 0},
            new Card() {AttackPoints = 1200, DefensePoints = 1000, IsSpecial = false, Name = "KUF", SpecialEffect = 0}
        };

        private static ApplicationDbContext instance = null;

        private ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrationStrategy());
            
        }

        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Letter> Letters { get; set; }
        public IDbSet<Market> Offers { get; set; }
        public IDbSet<PlayerCard> PlayerCards { get; set; }
        public IDbSet<BattlePool> BattlePool { get; set; }
        public IDbSet<Battle> Battles { get; set; }
        public IDbSet<FieldState> FieldState { get; set; }
        public IDbSet<BattleHand> BattleHands { get; set; } 

        private static object syncRoot = new Object();

        public static ApplicationDbContext Create()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ApplicationDbContext();
                    }   
                }
            }

            return instance;
        }

        public static ApplicationDbContext CreateNew()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerCard>()
                .HasKey(pc => pc.Id);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.Deck)
                .WithRequired()
                .HasForeignKey(pc => pc.PlayerId);

            modelBuilder.Entity<Card>()
                .HasMany(c => c.Owners)
                .WithRequired()
                .HasForeignKey(pc => pc.CardId);

             base.OnModelCreating(modelBuilder);
         }

        public void Seed()
        {
            List<Letter> letters = new List<Letter>()
            {
               new Letter() 
               {
                   Char = "a",
                   Weight = 0
               },
               new Letter() 
               {
                   Char = "b",
                   Weight = 4
               },
               new Letter() 
               {
                   Char = "c",
                   Weight = 2
               },
               new Letter() 
               {
                   Char = "d",
                   Weight = 4
               },
               new Letter() 
               {
                   Char = "e",
                   Weight = 1
               },
               new Letter() 
               {
                   Char = "f",
                   Weight = 4
               },
               new Letter() 
               {
                   Char = "g",
                   Weight = 6
               },
               new Letter() 
               {
                   Char = "h",
                   Weight = 2
               },
               new Letter() 
               {
                   Char = "i",
                   Weight = 2
               },
               new Letter() 
               {
                   Char = "j",
                   Weight = 10
               },
               new Letter() 
               {
                   Char = "k",
                   Weight = 7
               },
               new Letter() 
               {
                   Char = "l",
                   Weight = 6
               },
               new Letter() 
               {
                   Char = "m",
                   Weight = 2
               },
               new Letter() 
               {
                   Char = "n",
                   Weight = 3
               },
               new Letter() 
               {
                   Char = "o",
                   Weight = 1
               },
               new Letter() 
               {
                   Char = "p",
                   Weight = 3
               },
               new Letter() 
               {
                   Char = "q",
                   Weight = 11
               },
               new Letter() 
               {
                   Char = "r",
                   Weight = 4
               },
               new Letter() 
               {
                   Char = "s",
                   Weight = 4
               },
               new Letter() 
               {
                   Char = "t",
                   Weight = 3
               },
               new Letter() 
               {
                   Char = "u",
                   Weight = 8
               },
               new Letter() 
               {
                   Char = "v",
                   Weight = 6
               },
               new Letter() 
               {
                   Char = "w",
                   Weight = 11
               },
               new Letter() 
               {
                   Char = "x",
                   Weight = 10
               },
               new Letter() 
               {
                   Char = "y",
                   Weight = 8
               },
               new Letter() 
               {
                   Char = "z",
                   Weight = 4
               }
            };

            ((DbSet<Letter>)this.Letters).AddRange(letters);
            ((DbSet<Card>)this.Cards).AddRange(InitialCards);
            this.CreateUser(AdminRole, AdminUserName, AdminEmail, AdminPassword);
        }

        public void CreateUser(string role, string userName, string email, string password)
        {
            var user = new Player
            {
                UserName = userName,
                Email = email
            };

            var userStore = new UserStore<Player>(this);
            var userManager = new UserManager<Player>(userStore);
            var createResult = userManager.Create(user, password);
            if (!createResult.Succeeded)
            {
                throw new Exception(string.Join(",", createResult.Errors));

            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(this));
            if (roleManager.FindByName(role) == null)
            {
                var roleCreateResult = roleManager.Create(new IdentityRole(role));

                if (!roleCreateResult.Succeeded)
                {
                    throw new Exception(string.Join(",", roleCreateResult.Errors));
                }
            }

            var roleResult = userManager.AddToRole(user.Id, role);
            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(",", roleResult.Errors));
            }

            foreach (var card in InitialCards)
            {
                var dbCard = this.Cards.FirstOrDefault(c => c.Name == card.Name);
                user.Deck.Add(new PlayerCard()
                {
                    BoughtFor = 0,
                    Card = dbCard
                });
            }
        }
    }
}
