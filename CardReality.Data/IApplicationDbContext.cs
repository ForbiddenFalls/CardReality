namespace CardReality.Data
{
    using System.Data.Entity;

    using CardReality.Data.Models;

    public interface IApplicationDbContext
    {
        IDbSet<Card> Cards { get; set; }
        IDbSet<Letter> Letters { get; set; }
        IDbSet<Market> Offers { get; set; }
        IDbSet<PlayerCard> PlayerCards { get; set; }

        int SaveChanges();
    }
}
