using Account.DomainModels.Models;

namespace Account.DataAccess.IRepository
{
    public interface IBranchRepository : IRepositoryAsync<Branch>
    {
        void update(Branch entity);
    }
}
