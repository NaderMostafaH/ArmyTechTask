using Account.DomainModels.Models;

namespace Account.DataAccess.IRepository
{
    public interface ICashierRepository : IRepositoryAsync<Cashier>
    {
        void update(Cashier entity);
    }
}
