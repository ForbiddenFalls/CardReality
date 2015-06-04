using System.Threading.Tasks;

namespace CardReality.Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using CardReality.Data.Models;
    using CardReality.Data.Repositories;

    public class ApplicationData : IApplicationData
    {
        public IApplicationDbContext context;

        private IDictionary<Type, object> repositories;

        public ApplicationData(IApplicationDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<Player> Players
        {
            get { return this.GetRepository<Player>(); }
        }

        public IRepository<Card> Cards
        {
            get { return this.GetRepository<Card>(); }
        }

        public IRepository<Letter> Letters
        {
            get { return this.GetRepository<Letter>(); }
        }

        public IRepository<Market> Offers
        {
            get { return this.GetRepository<Market>(); }
        }

        public IRepository<PlayerCard> PlayerCards
        {
            get { return this.GetRepository<PlayerCard>(); }
        }

        public IRepository<BattlePool> Pool
        {
            get { return this.GetRepository<BattlePool>();  }
        }

        public IRepository<Battle> Battles
        {
            get { return this.GetRepository<Battle>(); }
        }

        public IRepository<FieldState> FieldStates
        {
            get { return this.GetRepository<FieldState>(); }
        }

        public IRepository<BattleHand> BattleHands
        {
            get { return this.GetRepository<BattleHand>(); }
        }

        public IRepository<Contact> Contacts
        {
            get { return this.GetRepository<Contact>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof(GenericRepository<T>);

                var repository = Activator.CreateInstance(typeOfRepository, this.context);
                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type];
        }

    }
}
