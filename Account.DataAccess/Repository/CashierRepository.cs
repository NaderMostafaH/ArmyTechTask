using Account.DataAccess.IRepository;
using Account.DomainModels.Models;

namespace Account.DataAccess.Repository
{
    public class CashierRepository : RepositoryAsync<Cashier>, ICashierRepository
    {
        private readonly ArmyTechTaskContext _db;
        public CashierRepository(ArmyTechTaskContext db) : base(db)
        {
            _db = db;
        }

        public void update(Cashier entity)
        {
            _db.Cashiers.Update(entity);
        }
    }
}
