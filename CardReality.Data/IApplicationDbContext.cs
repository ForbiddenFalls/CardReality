using System.Threading.Tasks;

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
        IDbSet<BattlePool> BattlePool { get; set; }
        IDbSet<Battle> Battles { get; set; }
        IDbSet<FieldState> FieldState { get; set; }
        IDbSet<BattleHand> BattleHands { get; set; }
        IDbSet<Contact> Contacts { get; set; } 

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
