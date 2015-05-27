namespace CardReality.Data.Data
{
    using CardReality.Data.Models;
    using CardReality.Data.Repositories;

    public interface IApplicationData
    {
        IRepository<Player> Players { get; }
        IRepository<Card> Cards { get; }
        IRepository<Letter> Letters { get; }
        IRepository<Market> Offers { get; }
        IRepository<PlayerCard> PlayerCards { get; }

        int SaveChanges();
    }
}
