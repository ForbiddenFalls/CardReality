using CardReality.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CardReality.Data
{
    public class MigrationStrategy : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        public static readonly List<Card> InitialCards = new List<Card>()
            {
                new Card() {AttackPoints = 1000, DefensePoints = 200, IsSpecial = false, Name = "DM", SpecialEffect = 0},
                new Card() {AttackPoints = 800, DefensePoints = 300, IsSpecial = false, Name = "WD", SpecialEffect = 0},
                new Card() {AttackPoints = 1200, DefensePoints = 1000, IsSpecial = false, Name = "KUF", SpecialEffect = 0}
            };

        protected override void Seed(ApplicationDbContext context)
        {
            var adminPassword = "#Lo6omie";
            var adminUser = new Player
            {
                UserName = "administrator",
                Email = "admin@admin.com"
            };

            var userStore = new UserStore<Player>(context);
            var userManager = new UserManager<Player>(userStore);
            var adminCreateResult = userManager.Create(adminUser, adminPassword);
            if (!adminCreateResult.Succeeded)
            {
                throw new Exception(string.Join(",", adminCreateResult.Errors));
                
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var adminRoleCreateResult = roleManager.Create(new IdentityRole("Administrator"));
            if (!adminRoleCreateResult.Succeeded)
            {
                throw new Exception(string.Join(",", adminRoleCreateResult.Errors));
            }

            var adminRoleResult = userManager.AddToRole(adminUser.Id, "Administrator");
            if (!adminRoleResult.Succeeded)
            {
                throw new Exception(string.Join(",", adminRoleResult.Errors));
            }

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

            //var hasher = new PasswordHasher();
            //Player player = new Player()
            //{
            //    UserName = "sdfdssdf@sddsfs.com",
            //    Email = "sdfdssdf@sddsfs.com",
            //    PasswordHash = hasher.HashPassword("#Lo6omie"),
            //    Money = 6666
            //};

            //context.Users.Add(player);

            //List<PlayerCard> pcards = new List<PlayerCard>()
            //{
            //    new PlayerCard()
            //    {
            //        Id = 1,
            //        Card = cards[0],
            //        Player = player
            //    },
            //    new PlayerCard()
            //    {
            //        Id = 2,
            //        Card = cards[2],
            //        Player = player
            //    },
            //    new PlayerCard()
            //    {
            //        Id = 3,
            //        Card = cards[0],
            //        Player = player
            //    }
            //};
            


            ((DbSet<Letter>)context.Letters).AddRange(letters);
            ((DbSet<Card>) context.Cards).AddRange(InitialCards);
           // ((DbSet<PlayerCard>)context.PlayerCards).AddRange(pcards);
        }
    }
}
