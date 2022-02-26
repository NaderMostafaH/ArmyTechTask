using Account.DomainModels.Models;

namespace Account.DataAccess.IRepository
{
    public interface ICityRepository : IRepositoryAsync<City>
    {
        void update(City entity);
    }
}
