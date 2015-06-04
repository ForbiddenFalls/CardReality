using System.Threading.Tasks;

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
        IRepository<BattlePool> Pool { get; }
        IRepository<Battle> Battles { get; }
        IRepository<FieldState> FieldStates { get; }
        IRepository<BattleHand> BattleHands { get; }
        IRepository<Contact> Contacts { get; } 
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
