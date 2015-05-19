using CardReality.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReality.Data
{
    public class ApplicationDbContext : IdentityDbContext<Player>
    {
        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Letter> Letters { get; set; }
        public IDbSet<Market> Offers { get; set; }
        public IDbSet<PlayerCard> PlayerCards { get; set; } 

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrationStrategy());
        }

        public static ApplicationDbContext Create()
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
    }
}
