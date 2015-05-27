namespace CardReality.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using CardReality.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<Player>, IApplicationDbContext
    {
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

        public static ApplicationDbContext Create()
        {
            if (instance == null)
            {
                instance = new ApplicationDbContext();
            }
            return instance;
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
